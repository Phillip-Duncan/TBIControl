using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using MathNet.Numerics.Statistics;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for ElectrometerRead.xaml
    /// </summary>
    public partial class ElectrometerRead : Page
    {
        public ElectrometerRead()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.elecsport = Properties.Settings.Default.elecserobj;

            this.elecstartcmd = Properties.Settings.Default.elecstartcmd;
            this.elecholdcmd = Properties.Settings.Default.elecholdcmd;
            this.elecresetcmd = Properties.Settings.Default.elecresetcmd;
            this.elecnullcmd = Properties.Settings.Default.elecnullcmd;
            this.elecreadchratecmd = Properties.Settings.Default.elecreadchratecmd;
            this.elecreadchcmd = Properties.Settings.Default.elecreadchcmd;

        }


        string elecstartcmd = Properties.Settings.Default.elecstartcmd;
        string elecholdcmd = Properties.Settings.Default.elecholdcmd;
        string elecresetcmd = Properties.Settings.Default.elecresetcmd;
        string elecnullcmd = Properties.Settings.Default.elecnullcmd;
        string elecreadchratecmd = Properties.Settings.Default.elecreadchratecmd;
        string elecreadchcmd = Properties.Settings.Default.elecreadchcmd;


        public System.IO.Ports.SerialPort pmdsport;
        public System.IO.Ports.SerialPort elecsport;



        private void Readbtn_Click(object sender, RoutedEventArgs e)
        {

            //
            //   MessageBox.Show("Read in progress, please wait..", "Status");
            //});
            Task.Run(() =>
            {

                this.Dispatcher.Invoke(() =>
                {
                    var mw = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

                    mw.programstatus.Children.Clear();

                    System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse();
                    circle.Height = 25; //or some size
                    circle.Width = 25; //height and width is the same for a circle
                    circle.Fill = System.Windows.Media.Brushes.Red;
                    circle.VerticalAlignment = VerticalAlignment.Center;
                    //circle.Margin = 

                    TextBlock statustxt = new TextBlock();
                    statustxt.Text = "Status: recording electrometer...";
                    statustxt.FontSize = 14;
                    statustxt.TextAlignment = TextAlignment.Center;
                    statustxt.VerticalAlignment = VerticalAlignment.Center;
                    statustxt.Margin = new Thickness(15, 0, 5, 0);


                    mw.programstatus.Children.Add(circle);
                    mw.programstatus.Children.Add(statustxt);
                });


            });


            double Avgchargerate;
            double Stdchargerate;
            double StdErrorOfMean;
            var chargerates = new List<Double>();
            var chrate = 0.00;
            string[] chratedata = new string[] { };
            var Totcharge = 0.00;
            string[] Totchargedata = new string[] { };

            double electempres = Convert.ToDouble(Properties.Settings.Default.electempres);
            double elecdatacutoff = Convert.ToDouble(Properties.Settings.Default.elecdatacutoff);
            int elecdataindex = Convert.ToInt32(Properties.Settings.Default.elecdataindex);
            string elecdatadivchar = Properties.Settings.Default.elecdatadivchar;
            string elecdataid = Properties.Settings.Default.elecdataid;
            int elecdataidindex = Convert.ToInt32(Properties.Settings.Default.elecdataidindex);



            this.elecsport = Properties.Settings.Default.elecserobj;
            var recordperiod = Convert.ToDouble(this.readperiod.Text);
            ChargeRateOutput.Text = "";

            var timesincelastrec = DateTime.Now;

            Task.Run( async() =>
            {

                try
                {

                    //Double recordperiod = 0.0;

                    // Clear Everything


                    // NEED TO QUERY, PTW E Electrometer requires WRITE AND READ for ALL Commands
                    elecsport.Write(elecholdcmd + "\r\n");
                    await Task.Delay(55);
                    elecsport.ReadExisting();


                    elecsport.Write(elecresetcmd + "\r\n");
                    await Task.Delay(55);
                    elecsport.ReadExisting();


                    elecsport.Write(elecstartcmd + "\r\n");

                    DateTime dt = DateTime.Now;
                    DateTime delta = dt.AddSeconds(recordperiod);

                    await Task.Delay(55);
                    elecsport.ReadExisting();

                    await Task.Delay(1500);

                    try
                    {

                        while (DateTime.Now < delta)
                        {

                            //this.elecreadprogress.Value = ((int)((delta - DateTime.Now).Ticks / recordperiod) * 100);


                            var timeout = DateTime.Now.AddSeconds(electempres / 2);
                            while (DateTime.Now < timeout)
                            {
                                try
                                {
                                    elecsport.Write(elecreadchratecmd + "\r\n");

                                    //MessageBox.Show(elecsport.ReadExisting(), "test");

                                    // Needs a 0.05s delay on release version for RS232 processing delay
                                    await Task.Delay(55);

                                    var elecdataformatted = (elecsport.ReadExisting()).Replace(" ", String.Empty);


                                    chratedata = Regex.Split(elecdataformatted, elecdatadivchar);

                                    //MessageBox.Show(chratedata[0].ToString(), "test");

                                    chrate = Convert.ToDouble(chratedata[elecdataindex]);


                                    break;
                                }
                                catch
                                {
                                    continue;
                                }
                            }



                            var isD1 = chratedata[elecdataidindex];

                            if (chargerates.Count == 0 && isD1 == elecdataid && (Math.Abs(chrate) < Math.Abs(elecdatacutoff)))
                            {
                                chargerates.Add(chrate);
                                timesincelastrec = DateTime.Now;
                                await Dispatcher.BeginInvoke((Action)(() => ChargeRateOutput.AppendText(Convert.ToString(chrate) + ", ")));
                            }
                            else if (chargerates.Count > 0 && isD1 == elecdataid && (Math.Abs(chrate) < Math.Abs(elecdatacutoff)))
                            {
                                if (chrate != chargerates[chargerates.Count - 1] || ((DateTime.Now - timesincelastrec).TotalSeconds > electempres))
                                {
                                    chargerates.Add(chrate);
                                    timesincelastrec = DateTime.Now;
                                    await Dispatcher.BeginInvoke((Action)(() => ChargeRateOutput.AppendText(Convert.ToString(chrate) + ", ")));
                                }
                            }



                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }


                    var timeouttotch = DateTime.Now.AddSeconds(electempres / 2);
                    while (DateTime.Now < timeouttotch)
                    {
                        try
                        {
                            elecsport.Write(elecreadchcmd + "\r\n");
                            // Needs a 0.06s delay on release version for RS232 processing delay
                            await Task.Delay(60);

                            var totchargedataformatted = (elecsport.ReadExisting()).Replace(" ", String.Empty);

                            Totchargedata = Regex.Split(totchargedataformatted, elecdatadivchar);

                            Totcharge = Convert.ToDouble(Totchargedata[elecdataindex]);

                            break;
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    if (chargerates.Count == 1)
                    {
                        Avgchargerate = chargerates[0];
                        Stdchargerate = 0;
                        StdErrorOfMean = 0;
                    }
                    else if (chargerates.Count > 2)
                    {
                        Avgchargerate = chargerates.Average();
                        Stdchargerate = chargerates.StandardDeviation();
                        StdErrorOfMean = Stdchargerate / Math.Sqrt(chargerates.Count);
                    }
                    else
                    {
                        // Easy way to show an error has occurred
                        Avgchargerate = 999999999.999;
                        Stdchargerate = 999999999.999;
                        StdErrorOfMean = 999999999.999;
                    }

                    await Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (Avgchargerate == 999999999.999) {
                            this.AverageChargeRate.Text = "No data has been recorded, an error has likely occurred. Check Electrometer Upper Cutoff And/or other settings.";
                        }
                        else {
                            this.AverageChargeRate.Text = Convert.ToString(Avgchargerate);
                            this.StdChargeRate.Text = Convert.ToString(Stdchargerate);
                            this.StdErrorOfMean.Text = Convert.ToString(StdErrorOfMean);
                            this.AverageChargeRateInt.Text = Convert.ToString((Double)Avgchargerate * (Double)(recordperiod));
                            this.TotalCharge.Text = Convert.ToString(Totcharge);
                        }
                    }));

                }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }


                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    var mw = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

                    mw.programstatus.Children.Clear();

                    System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse();
                    circle.Height = 25; //or some size
                    circle.Width = 25; //height and width is the same for a circle
                    circle.Fill = System.Windows.Media.Brushes.Green;
                    circle.VerticalAlignment = VerticalAlignment.Center;
                    //circle.Margin = 

                    TextBlock statustxt = new TextBlock();
                    statustxt.Text = "Status: recording complete.";
                    statustxt.FontSize = 14;
                    statustxt.TextAlignment = TextAlignment.Center;
                    statustxt.VerticalAlignment = VerticalAlignment.Center;
                    statustxt.Margin = new Thickness(15, 0, 5, 0);


                    mw.programstatus.Children.Add(circle);
                    mw.programstatus.Children.Add(statustxt);
                }));




            });


            


        }
        
        private void Nullbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.elecsport = Properties.Settings.Default.elecserobj;

                elecsport.Write(elecstartcmd + "\r\n");
                elecsport.ReadExisting();
                Thread.Sleep(20000);
                elecsport.Write(elecnullcmd + "\r\n");
                elecsport.ReadExisting();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }
        }

        private void ReadSingle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                elecsport.Write(elecreadchratecmd + "\r\n");
                Thread.Sleep(50);
                var chratedata = elecsport.ReadExisting();

                MessageBox.Show(chratedata.ToString(), "Single Read Output");

                var cchhrate = Convert.ToDouble(Regex.Split(chratedata, ";")[5]);

                MessageBox.Show(cchhrate.ToString(), "Charge rate Read Output");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }

        }
    }
}
