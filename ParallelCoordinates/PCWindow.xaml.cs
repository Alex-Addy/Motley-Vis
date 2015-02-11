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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Axis> axes = new List<Axis>();
        private readonly List<Tuple<double, double>> ranges = new List<Tuple<double, double>>();
        private readonly List<PcLine> pcLines = new List<PcLine>();

        // Margin between the axes and the top and bot of the canvas
        private const double TopBotMargin = 30;
        private const double AxisStrokeThickness = 2;
        private const double LineStrokeThickness = 1;

        public MainWindow(IEnumerable<List<double>> rows, List<String> headers)
        {
            InitializeComponent();

            // Setup axes
            foreach (var header in headers)
            {
                var newLn = new Axis(this.Canvas);
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

            public Axis(Canvas drawCanvas)
            {
                mainLine = new Line {Y1 = TopBotMargin, Stroke = Brushes.Black, StrokeThickness = AxisStrokeThickness};
                drawCanvas.Children.Add(mainLine);
            }

            public void Draw(double newX, double newY2)
            {
                mainLine.Y2 = newY2;

                mainLine.X1 = newX;
                mainLine.X2 = newX;
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
            private readonly List<Line> lines;

            /// <summary>
            /// Creates a new PcLine
            /// </summary>
            /// <param name="percents">The location of the line on each axis in a percentage</param>
            /// <param name="drawCanvas">Canvas to place its lines on</param>
            public PcLine(IEnumerable<double> percents, Canvas drawCanvas)
            {
                axisLocs = new List<double>(percents);
                lines = new List<Line>(axisLocs.Count-1);

                foreach (var _ in Enumerable.Range(0, axisLocs.Count-1))
                {
                    var newLn = new Line { Stroke = Brushes.DarkCyan, StrokeThickness = LineStrokeThickness };
                    lines.Add(newLn);
                    drawCanvas.Children.Add(newLn);
                }
            }

            /// <summary>
            /// Redraws component line given new axes positions
            /// </summary>
            /// <param name="axes"></param>
            public void Draw(List<Axis> axes)
            {
                foreach (var i in Enumerable.Range(0, lines.Count))
                {
                    var line = lines[i];
                    var left_axis = axes[i];
                    var right_axis = axes[i+1];

                    line.X1 = left_axis.HorizontalPosition;
                    line.X2 = right_axis.HorizontalPosition;

                    var offset1 = Math.Abs(left_axis.Top - left_axis.Bot)*axisLocs[i];
                    var offset2 = Math.Abs(right_axis.Top - right_axis.Bot)*axisLocs[i+1];
                    line.Y1 = left_axis.Top + offset1;
                    line.Y2 = right_axis.Top + offset2;
                }
            }
        }
    }
}
