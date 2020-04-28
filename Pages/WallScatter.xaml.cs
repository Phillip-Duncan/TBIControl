using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

namespace TBIControl.Pages
{
    /// <summary>
    /// Interaction logic for WallScatter.xaml
    /// </summary>
    public partial class WallScatter : Page
    {
        public WallScatter()
        {
            InitializeComponent();
        }




        private void CalculateWallScatter(object sender, RoutedEventArgs e)
        {
            // ALBEDO Values FROM NCRP Report No. 151
            var Energies = new List<double>() { 30,24,18,10,6,4,0.5,0.25};
            var Angles = new List<double>() { 0,30,45,60,75};
            Double[,] diffalbedos = new Double[8, 5] { {3.0, 2.7, 2.6, 2.2, 1.5 },{3.2, 3.2, 2.8, 2.3, 1.5 },{3.4, 3.4, 3.0, 2.5, 1.6 },{4.3, 4.1, 3.8, 3.1, 2.1 },{ 5.3, 5.2, 4.7, 4.0, 2.7 },{ 6.7, 6.4, 5.8, 4.9, 3.1 },{ 19.0, 17.0, 15.0, 13.0, 8.0 },{ 32.0, 28.0, 25.0, 22.0, 13.0 } };
            List<double> diffalbedoslist = diffalbedos.Cast<double>().ToList().Select(r => r * Math.Pow(10, -3)).ToList();

            Double Scatterval = 0;


            //Vector3D wallvec = new Vector3D(Convert.ToDouble(wallxlen.Text), 1, Convert.ToDouble(wallzheight.Text));

            Double Wallxlen = Convert.ToDouble(wallxlen.Text);
            Double Wallzheight = Convert.ToDouble(wallzheight.Text);
            Double Objres = Convert.ToDouble(ObjectResolution.Text);
            Double xw = 0;
            Double zh = 0;

            Double Wallarea = Wallxlen * Wallzheight;
            
            Double phantomlengthx = Convert.ToDouble(phantomxlength.Text);
            Double phantomwidthy = Convert.ToDouble(phantomywidth.Text);
            Double phantomheightz = Convert.ToDouble(phantomzheight.Text);


            Double BeamEnergy = Convert.ToDouble(beamenergy.Text);
            Double BeamX = Convert.ToDouble(beamx.Text);
            Double BeamY = Convert.ToDouble(beamy.Text);
            Double Beamtowalldist = Convert.ToDouble(SWD.Text);

            Double Beamareaonwall = ((BeamX * BeamY) / 10000) * (Math.Pow(Beamtowalldist, 2));
            Double beamxonwall = ((BeamX / 100) * (Math.Pow(Beamtowalldist, 2)));
            Double beamyonwall = ((BeamY / 100) * (Math.Pow(Beamtowalldist, 2)));



            Viewport3D myViewport = new Viewport3D();
            Model3DGroup myModel3dGroup = new Model3DGroup();
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            PerspectiveCamera myPCamera = new PerspectiveCamera();


            Vector3D measurementpointpos = new Vector3D(Wallxlen / 2, 10, Wallzheight / 2);

            Point3DCollection wall = new Point3DCollection();

            Vector3DCollection wall2 = new Vector3DCollection();


            GeometryModel3D wallmesh = new GeometryModel3D();
            
            myPCamera.Position = new Point3D(Wallxlen/2, 2, Wallzheight/2);
            myPCamera.LookDirection = new Vector3D(0, 0, 1);

            myPCamera.FieldOfView = 90;

            myViewport.Camera = myPCamera;

            while (zh < Wallzheight) {

                xw = 0;

                while (xw < Wallxlen)
                {
                    Vector3D wallscpoint = new Vector3D(xw, 1, zh);

                    Double Anglebetween = Vector3D.AngleBetween(wallscpoint, measurementpointpos);

                    // Area of each point in m^2
                    Double pointobjarea = Objres / 10000;

                    if (Anglebetween < Angles[Angles.Count -1])
                    {
                        
                        alglib.spline2dinterpolant s;

                        alglib.spline2dbuildbilinearv(Angles.ToArray(), Angles.Count, Energies.ToArray(), Energies.Count, diffalbedoslist.ToArray(), 1, out s);

                        Double Albedovalue = alglib.spline2dcalc(s, Anglebetween, BeamEnergy);

                        // Scatter Value, Incorporating that the Albedos are multiplied by 10^-3.
                        Scatterval += Albedovalue * pointobjarea;//Math.Pow(10,-3);


                        //wallmesh.Positions.Add(new Point3D(xw, 1, zh));

                    }

                    

                    wall2.Add(wallscpoint);
                    xw += Objres;
                }

                zh += Objres;
            }

            Vector3D vector1 = new Vector3D(20, 30, 40);


            Output.Text = Scatterval.ToString();



        }




    }
}
