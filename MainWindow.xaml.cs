using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TBIControl.Pages;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            CFrameMain.Content = new MainMenu();


            System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse();
            circle.Height = 25; //or some size
            circle.Width = 25; //height and width is the same for a circle
            circle.Fill = System.Windows.Media.Brushes.Green;
            circle.VerticalAlignment = VerticalAlignment.Center;
            //circle.Margin = 

            TextBlock statustxt = new TextBlock();
            statustxt.Text = "Status: ready";
            statustxt.FontSize = 14;
            statustxt.TextAlignment = TextAlignment.Center;
            statustxt.VerticalAlignment = VerticalAlignment.Center;
            statustxt.Margin = new Thickness(15, 0, 5, 0);

            
            programstatus.Children.Add(circle);
            programstatus.Children.Add(statustxt);


        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Options popup = new Options();
            popup.ShowDialog();

            popup.Close();
        }

        
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            CFrameMain.Content = new MainMenu();
            
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            CFrameMain.Content = new MovePMD();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            CFrameMain.Content = new ElectrometerRead();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            CFrameMain.Content = new StepandRecord();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            CFrameMain.Content = new WallScatter();
        }
    }
}
