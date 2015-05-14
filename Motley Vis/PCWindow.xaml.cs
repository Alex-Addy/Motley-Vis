using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace ParallelCoordinates
{
    /// <summary>
    /// Interaction logic for ParallelCoordinatesWindow.xaml
    /// </summary>
    public partial class ParallelCoordinatesWindow : Window
    {
        private readonly List<Axis> axes = new List<Axis>();
        private readonly List<PcLine> pcLines = new List<PcLine>();

        // Margin between the axes and the top and bot of the canvas
        private const double TopBotMargin = 30;
        private const double AxisStrokeThickness = 2;
        private const double LineStrokeThickness = 1;
        private const double MinAxisSpacing = 50;
        private const int minHeight = 860;
        private const int minWidth = 1024;

        // Z-indexes for various items
        private const int TextZIndex = 1;

        public ParallelCoordinatesWindow(IEnumerable<List<double>> rows, List<String> headers)
        {
            InitializeComponent();

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
            var ranges = Enumerable.Range(0, headers.Count).Select(i => new Tuple<double, double>(mins[i], maxs[i])).ToList();
            // Setup axes
            foreach (var i in Enumerable.Range(0, headers.Count))
            {
                var newLn = new Axis(headers[i], ranges[i], this.Canvas);
                axes.Add(newLn);
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

            Canvas.Width = Math.Max(minWidth, MinAxisSpacing*axes.Count*2);
            Canvas.Height = minHeight;

            DrawAxes();  // The axis draw MUST happen before the line redraw
            DrawLines(); // so that the lines redraw to the new locations and not the old
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            base.MeasureOverride(availableSize);

            double width = Math.Max(minWidth, MinAxisSpacing*(axes.Count+2));
            double height = minHeight;

            return new Size(width, height);
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

        #region Axis

        private class Axis
        {
            private readonly Line mainLine;
            private readonly TextBlock nameBlock;
            private readonly TextBlock minBlock;
            private readonly TextBlock maxBlock;
            private readonly Line topTick;
            private readonly Line botTick;

            private readonly Tuple<double, double> range;

            public Axis(string label, Tuple<double, double> valueRange, Canvas drawCanvas)
            {
                range = valueRange;
                minBlock = new TextBlock {Text = range.Item1.ToString(), Foreground = Brushes.Black};
                maxBlock = new TextBlock {Text = range.Item2.ToString(), Foreground = Brushes.Black};
                drawCanvas.Children.Add(minBlock);
                drawCanvas.Children.Add(maxBlock);

                nameBlock = new TextBlock {Text = label, Foreground = Brushes.Black};
                drawCanvas.Children.Add(nameBlock);

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

                // draw axis labels
                // magic # is just to get the text off the top tick
                Canvas.SetTop(nameBlock, TopBotMargin - nameBlock.ActualHeight - 2);
                Canvas.SetLeft(nameBlock, newX - (nameBlock.ActualWidth/2));

                Canvas.SetLeft(minBlock, newX - minBlock.ActualWidth - 2);
                Canvas.SetTop(minBlock, TopBotMargin);
                
                Canvas.SetZIndex(minBlock, TextZIndex);

                Canvas.SetLeft(maxBlock, newX - maxBlock.ActualWidth - 2);
                Canvas.SetTop(maxBlock, newY2 - maxBlock.ActualHeight);
                Canvas.SetZIndex(maxBlock, TextZIndex);
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

        #endregion


        #region PCLine

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
                var newPoints = new PointCollection(line.Points.Count);
                foreach (var i in Enumerable.Range(0, line.Points.Count))
                {
                    var ax = axes[i];

                    var p = new Point { X = ax.HorizontalPosition, Y = ax.Top + Math.Abs(ax.Top - ax.Bot) * axisLocs[i] };
                    newPoints.Add(p);
                }
                line.Points = newPoints;
            }
        } 

        #endregion

        private void Save_Display(object sender, RoutedEventArgs e)
        {
            var selectDialog = new SaveFileDialog
            {
                FileName = "Parallel Coordinates Dump",
                DefaultExt = ".png",
                Filter = "PNG (.png)|*.png"
            };
            DialogResult result = selectDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var rtb = new RenderTargetBitmap((int)Canvas.RenderSize.Width, (int)Canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(Canvas);

                // encode as png
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                // save to memory stream
                var ms = new MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();

                File.WriteAllBytes(selectDialog.FileName, ms.ToArray());
            }
        }
    }
}
