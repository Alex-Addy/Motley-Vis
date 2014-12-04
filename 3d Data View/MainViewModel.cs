using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace _3d_Data_View
{
    class MainViewModel
    {
        public MainViewModel(IEnumerable<List<double>> rows, int xAxis, int yAxis, int zAxis)
        {
            var modelGroup = new Model3DGroup();

            PointsVisual3D points = new PointsVisual3D
            {
                Color = Colors.Blue,
                Points = new List<Point3D>(rows.Select(row => new Point3D(row[xAxis], row[yAxis], row[zAxis])))
            };

            modelGroup.Children.Add(points.Content);
            Model = modelGroup;
        }

        public Model3D Model { get; set; }
    }
}
