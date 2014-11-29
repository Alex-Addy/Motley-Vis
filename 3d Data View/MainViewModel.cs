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

            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddSphere(new Point3D(0, 0, 0), 0.1, 10, 5);

            // Create a mesh from the meshbuilder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);

            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);

            // Add points to model group
            foreach (var row in rows)
            {
                modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = blueMaterial, Transform = new TranslateTransform3D(row[xAxis], row[yAxis], row[zAxis])});
            }
        }

        public Model3D Model { get; set; }
    }
}
