using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParallelCoordinates
{
    /// <summary>
    /// Interaction logic for ParallelCoordinatesWindow.xaml
    /// </summary>
    public partial class ParallelCoordinatesWindow : Window
    {
        private readonly List<Axis> axes = new List<Axis>();
        private readonly List<Tuple<double, double>> ranges = new List<Tuple<double, double>>();
        private readonly List<PcLine> pcLines = new List<PcLine>();

        // Margin between the axes and the top and bot of the canvas
        private const double TopBotMargin = 30;
        private const double AxisStrokeThickness = 2;
        private const double LineStrokeThickness = 1;

        public ParallelCoordinatesWindow(IEnumerable<List<double>> rows, List<String> headers)
        {
            InitializeComponent();

            // Setup axes
            foreach (var header in headers)
            {
                var newLn = new Axis(header, this.Canvas);
                axes.Add(newLn);
            }

            // HACK: needed because the rows must be enumerated twice
            var hacky_rows = rows.ToList();

            // Setup min/max values for each axis
            var mins = new List<double>(Enumerable.Repeat(Double.PositiveInfinity, headers.Count));
            var maxs = new List<double>(Enumerable.Repeat(Double.NegativeInfinity, headers.Count));
            foreach (var doubles in hacky_rows)
            {
                foreach (var i in Enumerable.Range(0, headers.Count))
                {
                    if (doubles[i] > maxs[i])
                    {
                        maxs[i] = doubles[i];
                    }
                    if (doubles[i] < mins[i])
                    {
                        mins[i] = doubles[i];
                    }
                }
            }
            foreach (var i in Enumerable.Range(0, headers.Count))
            {
                ranges.Add(new Tuple<double, double>(mins[i], maxs[i]));
            }

            // Setup each PcLine
            pcLines.Capacity = hacky_rows.Count;
            foreach (var row in hacky_rows)
            {
                var percents = new List<double>(ranges.Count);
                foreach (var i in Enumerable.Range(0, ranges.Count))
                {
                    var range = ranges[i];
                    if (Math.Abs(range.Item1 - range.Item2) < Double.Epsilon)
                    {
                        percents.Add(0);
                    }
                    else
                    {
                        percents.Add(
                            (row[i] - Math.Min(range.Item1, range.Item2))/
                            Math.Abs(range.Item1 - range.Item2)
                            );
                    }
                }
                pcLines.Add(new PcLine(percents, this.Canvas));
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAxes();  // The axis draw MUST happen before the line redraw
            DrawLines(); // so that the lines redraw to the new locations and not the old
        }

        private void DrawLines()
        {
            foreach (var pcLine in pcLines)
            {
                pcLine.Draw(this.axes);
            }
        }

        private void DrawAxes()
        {
            // this refers to the width of the spaces between axes
            var spacing = Canvas.ActualWidth/(axes.Count + 1);

            foreach (var i in Enumerable.Range(0, axes.Count))
            {
                var axis = axes[i];
                axis.Draw(spacing*(i+1), this.Canvas.ActualHeight - TopBotMargin);
            }
        }
        
        private class Axis
        {
            private readonly Line mainLine;
            private readonly TextBlock nameBlock;
            private readonly Line topTick;
            private readonly Line botTick;

            public Axis(string label, Canvas drawCanvas)
            {
                // TODO
                //nameBlock = new TextBlock {Text = label};
                //drawCanvas.Children.Add(nameBlock);

                mainLine = new Line {Y1 = TopBotMargin, Stroke = Brushes.Black, StrokeThickness = AxisStrokeThickness};
                topTick = new Line {Stroke = Brushes.Black, StrokeThickness = 1.5, Y1 = mainLine.Y1, Y2 = mainLine.Y1};
                botTick = new Line {Stroke = Brushes.Black, StrokeThickness = 1.5};

                drawCanvas.Children.Add(mainLine);
                drawCanvas.Children.Add(topTick);
                drawCanvas.Children.Add(botTick);
            }

            public void Draw(double newX, double newY2)
            {
                const double tickSize = 5;

                mainLine.Y2 = newY2;

                mainLine.X1 = newX;
                mainLine.X2 = newX;

                // draw ticks
                topTick.X1 = newX - tickSize;
                topTick.X2 = newX + tickSize;

                botTick.Y1 = newY2;
                botTick.Y2 = newY2;
                botTick.X1 = newX - tickSize;
                botTick.X2 = newX + tickSize;
            }

            public double Top
            {
                get { return mainLine.Y1; }
            }

            public double Bot
            {
                get { return mainLine.Y2; }
            }

            public double HorizontalPosition
            {
                get { return mainLine.X1; }
            }
        }

        /// <summary>
        /// Handles a single Parallel Coordinates line
        /// </summary>
        private class PcLine
        {
            private readonly List<double> axisLocs;

            private readonly Polyline line;

            /// <summary>
            /// Creates a new PcLine
            /// </summary>
            /// <param name="percents">The location of the line on each axis in a percentage</param>
            /// <param name="drawCanvas">Canvas to place its lines on</param>
            public PcLine(IEnumerable<double> percents, Canvas drawCanvas)
            {
                axisLocs = new List<double>(percents);
                line = new Polyline
                {
                    Stroke = Brushes.SlateGray,
                    StrokeThickness = LineStrokeThickness,
                    Points = new PointCollection(axisLocs.Count)
                };

                foreach (var i in Enumerable.Range(0, axisLocs.Count))
                {
                    line.Points.Add(new Point());
                }

                drawCanvas.Children.Add(line);
            }

            /// <summary>
            /// Redraws component line given new axes positions
            /// </summary>
            /// <param name="axes"></param>
            public void Draw(List<Axis> axes)
            {
                foreach (var i in Enumerable.Range(0, line.Points.Count))
                {
                    var ax = axes[i];

                    var p = new Point {X = ax.HorizontalPosition, Y = ax.Top + Math.Abs(ax.Top - ax.Bot)*axisLocs[i]};

                    line.Points[i] = p;
                }
            }
        }
    }
}
