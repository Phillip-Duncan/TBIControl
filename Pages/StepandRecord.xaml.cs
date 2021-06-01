using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO.Ports;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Globalization;
using MathNet.Numerics.Statistics;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Helpers;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for StepandRecord.xaml
    /// </summary>
    public partial class StepandRecord : Page
    {

        public StepandRecord()
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

            this.pmdsport = Properties.Settings.Default.pmdserobj;

            this.pmdinitializecmd = Properties.Settings.Default.pmdinitializecmd;
            this.pmdpositioncmd = Properties.Settings.Default.pmdpositioncmd;
            this.pmdspeedcmd = Properties.Settings.Default.pmdspeedcmd;
            this.pmdgotoabscmd = Properties.Settings.Default.pmdgotoabscmd;
            this.pmdgotorelcmd = Properties.Settings.Default.pmdgotorelcmd;
            this.pmddriveoutcmd = Properties.Settings.Default.pmddriveoutcmd;
            this.pmddriveincmd = Properties.Settings.Default.pmddriveincmd;


            // Import adv settings on reload
            this.electempres = Convert.ToDouble(Properties.Settings.Default.electempres);
            this.elecdataindex = Convert.ToInt32(Properties.Settings.Default.elecdataindex);
            this.elecdatadivchar = Properties.Settings.Default.elecdatadivchar;
            this.elecdataid = Properties.Settings.Default.elecdataid;
            this.elecdataidindex = Convert.ToInt32(Properties.Settings.Default.elecdataidindex);

            this.min232updtime = Convert.ToDouble(Properties.Settings.Default.minRS232updtime);


            updatesrdatalist();
        }


        string elecstartcmd = Properties.Settings.Default.elecstartcmd;
        string elecholdcmd = Properties.Settings.Default.elecholdcmd;
        string elecresetcmd = Properties.Settings.Default.elecresetcmd;
        string elecnullcmd = Properties.Settings.Default.elecnullcmd;
        string elecreadchratecmd = Properties.Settings.Default.elecreadchratecmd;
        string elecreadchcmd = Properties.Settings.Default.elecreadchcmd;


        //adv settings import
        double electempres = Convert.ToDouble(Properties.Settings.Default.electempres);
        int elecdataindex = Convert.ToInt32(Properties.Settings.Default.elecdataindex);
        string elecdatadivchar = Properties.Settings.Default.elecdatadivchar;
        string elecdataid = Properties.Settings.Default.elecdataid;
        int elecdataidindex = Convert.ToInt32(Properties.Settings.Default.elecdataidindex);

        double min232updtime = Convert.ToDouble(Properties.Settings.Default.minRS232updtime);


        string pmdinitializecmd;
        string pmdpositioncmd;
        string pmdspeedcmd;
        string pmdgotoabscmd;
        string pmdgotorelcmd;
        string pmddriveoutcmd;
        string pmddriveincmd;

        
        public System.IO.Ports.SerialPort pmdsport;
        public System.IO.Ports.SerialPort elecsport;

        private static readonly Regex _numregex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

        // Numeric Entry Only
        private static bool IsTextAllowedNumOnly(string text)
        {
            return _numregex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowedNumOnly(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }


        private void pmdspeed_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void readperiod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void startpos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void finalpos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void posstep_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }


        public static IEnumerable<double> Range(double min, double max, double step)
        {
            double i;
            for (i = min; i <= max; i += step)
                yield return i;

            if (i != max + step) // added only because you want max to be returned as last item
                yield return max;
        }


        static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0.0)
            {
                return 0.0;
            }
            else
            {
                double leftSideNumbers = Math.Floor(Math.Log10(Math.Abs(d))) + 1;
                double scale = Math.Pow(10, leftSideNumbers);
                double result = scale * Math.Round(d / scale, digits, MidpointRounding.AwayFromZero);

                // Clean possible precision error.
                if ((int)leftSideNumbers >= digits)
                {
                    return Math.Round(result, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return Math.Round(result, digits - (int)leftSideNumbers, MidpointRounding.AwayFromZero);
                }
            }
        }




        private int timeout(double speed, double gotopos)
        {

            this.pmdsport.Write(pmdpositioncmd + "\r\n");
            Thread.Sleep((int) min232updtime);
            var pos = Convert.ToDouble(Regex.Split(pmdsport.ReadExisting(), "E")[1]);

            var deltapos = Math.Abs(pos - gotopos);

            int calctimeout = (int)Math.Round(deltapos / speed) + 20;


            return calctimeout;
        }


        private void updatesrdatalist()
        {
            listsrdata.Items.Clear();

            JArray lstjarr;
            using (StreamReader sr = new StreamReader("srdata.json"))
            {
                string lstjson = sr.ReadToEnd();
                lstjarr = JArray.Parse(lstjson);
            }

            for(int jj=0;jj<lstjarr.Count();jj++)
            {
                 listsrdata.Items.Add((jj+1).ToString() +"  "+ lstjarr[jj][0][0]);
            }

            if (lstjarr.Count() > 0)
            {
                Excelexportbtn.IsEnabled = true;
                Datadeletebtn.IsEnabled = true;
            }

        }

        private int getsrdatalistsize()
        {
            JArray lstjarr;
            using (StreamReader sr = new StreamReader("srdata.json"))
            {
                string lstjson = sr.ReadToEnd();
                lstjarr = JArray.Parse(lstjson);
            }
            return lstjarr.Count();
        }



        // Helper functions for click and hold to select multiple checkboxes for TPR/OAD Graphs
        private void Checkbox_OnTPROADMouseEnter(object sender, MouseEventArgs e)
        {
            var checkbox = sender as CheckBox;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (checkbox != null)
                {
                    checkbox.IsChecked = !checkbox.IsChecked;
                }
            }
        }

        private void Checkbox_TPROADMouseCapture(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    checkbox.IsChecked = !checkbox.IsChecked;
                    checkbox.ReleaseMouseCapture();
                }
            }
        }
        // OAD/TPR Checkbox helper functions end


        // Function containing loops controlling the measurement of profiles & TPRs
        private void MeasureProfiles()
        {
            double electempres = Convert.ToDouble(Properties.Settings.Default.electempres);
            double elecdatacutoff = Convert.ToDouble(Properties.Settings.Default.elecdatacutoff);
            int elecdataindex = Convert.ToInt32(Properties.Settings.Default.elecdataindex);
            string elecdatadivchar = Properties.Settings.Default.elecdatadivchar;
            string elecdataid = Properties.Settings.Default.elecdataid;
            int elecdataidindex = Convert.ToInt32(Properties.Settings.Default.elecdataidindex);

            this.elecsport = Properties.Settings.Default.elecserobj;
            this.pmdsport = Properties.Settings.Default.pmdserobj;
            Double recordperiod = Convert.ToDouble(this.readperiod.Text);

            List<Double> depths = this.recorddepths.Text.Split(',').Select(Double.Parse).ToList();

            string pmdspeed = this.pmdspeed.Text;
            string pmdstartpos = this.startpos.Text;
            string pmdfinalpos = this.finalpos.Text;
            string pmdposstep = this.posstep.Text;

            string caxpostxt = this.caxpos.Text;

            if (caxpostxt == "")
            {
                caxpostxt = pmdstartpos;
            }

            double caxpos = Convert.ToDouble(caxpostxt);

            var OApositions = new List<double>();
            OApositions = Range(Convert.ToDouble(pmdstartpos), Convert.ToDouble(pmdfinalpos), Convert.ToDouble(pmdposstep)).ToList();

            Double[,] avgintchrates = new Double[depths.Count() + 1, OApositions.Count() * 3 + 1];

            // Set Depths
            for (int g = 0; g < depths.Count(); g++)
            {
                avgintchrates[g + 1, 0] = depths[g];
            }
            // Set OADs
            for (int h = 0; h < OApositions.Count(); h++)
            {
                avgintchrates[0, h + 1] = OApositions[h] - caxpos;
                avgintchrates[0, OApositions.Count() + h + 1] = OApositions[h] - caxpos;
                avgintchrates[0, 2 * OApositions.Count() + h + 1] = OApositions[h] - caxpos;
            }


            // Set some base variables for output files..
            string jout = "";
            var name = "SRsave-" + DateTime.Now.ToString("ddMMyyyyhhmmss");


            // Write JSON header... This will be replaced later anyway
            string jsondata = JsonConvert.SerializeObject(avgintchrates);
            JArray jarray = JArray.Parse(jsondata);
            jarray[0][0] = name;


            using (StreamReader sr = new StreamReader("srdata.json"))
            {
                string json = sr.ReadToEnd();
                JArray jarr = JArray.Parse(json);

                jarr.Add(jarray);

                jout = JsonConvert.SerializeObject(jarr);
            }
            File.WriteAllText("srdata.json", jout);

            updatesrdatalist();

            var jidx = getsrdatalistsize();



            for (int i = 0; i < depths.Count(); i++)
            {

                #region "Initialize to 0"
                this.pmdsport.Write(pmdspeedcmd + "20.0" + "\r\n");
                Thread.Sleep((int) min232updtime);
                this.pmdsport.Write(pmdinitializecmd + "\r\n");
                Thread.Sleep(1500);

                bool inposition1 = false;
                var timeout21 = DateTime.Now.AddSeconds(120);//DateTime.Now.AddSeconds(120);

                while (inposition1 == false && DateTime.Now < timeout21)
                {
                    try
                    {
                        //this.pmdsport.Write(pmdpositioncmd + "\r\n");
                        Thread.Sleep((int) min232updtime);
                        //var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];
                        var read = pmdsport.ReadExisting();

                        if (read.Contains("R"))
                        {
                            Thread.Sleep(1000);
                            this.pmdsport.Write(pmdpositioncmd + "\r\n");
                            Thread.Sleep((int) min232updtime);
                            var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];
                            if (Convert.ToDouble(pos) == 0)
                            {
                                inposition1 = true;
                                break;
                            }

                        }
                        else
                        {
                            continue;
                        }

                    }
                    catch { continue; }

                }

                if (DateTime.Now >= timeout21)
                {
                    this.pmdinit.IsEnabled = true;
                    MessageBox.Show("PMD Initialize timed out. Please try again", "Status");
                    return;
                }
                #endregion


                #region "Initialize to offset point"

                this.pmdsport.Write(pmdgotoabscmd + pmdstartpos + "\r\n");
                Thread.Sleep((int) min232updtime);
                var inposition2 = false;

                var timeout23 = DateTime.Now.AddSeconds(120);
                while (inposition2 == false && DateTime.Now < timeout23)
                {
                    try
                    {
                        Thread.Sleep((int) min232updtime);
                        var read = pmdsport.ReadExisting();

                        if (read.Contains("P"))
                        {
                            Thread.Sleep(1000);
                            this.pmdsport.Write(pmdpositioncmd + "\r\n");
                            Thread.Sleep((int) min232updtime);
                            var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];

                            if (Convert.ToDouble(pos) == Convert.ToDouble(pmdstartpos))
                            {
                                inposition2 = true;
                                break;
                            }

                        }
                        else
                        {
                            continue;
                        }

                    }
                    catch { continue; }
                }

                if (DateTime.Now >= timeout23)
                {
                    MessageBox.Show("PMD Initialize timed out. Please try again", "Status");
                    break;
                }
                #endregion


                // SET PMD SPEED POST INITIALIZATION
                this.pmdsport.Write(pmdspeedcmd + pmdspeed + "\r\n");
                Thread.Sleep((int) min232updtime);


                DoProfile:
                for (int j = 0; j < OApositions.Count(); j++)
                {
                    // MOVE PMD TO NEXT OA Position
                    var gotopos = OApositions[j].ToString();

                    this.pmdsport.Write(pmdgotoabscmd + gotopos + "\r\n");
                    Thread.Sleep((int) min232updtime);
                    var inpositionoa = false;

                    while (inpositionoa == false)
                    {
                        try
                        {
                            var read = pmdsport.ReadExisting();

                            if (read.Contains("P"))
                            {
                                Thread.Sleep(1000);
                                this.pmdsport.Write(pmdpositioncmd + "\r\n");
                                Thread.Sleep((int) min232updtime);
                                var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];

                                if (Convert.ToDouble(pos) == Convert.ToDouble(gotopos))
                                {
                                    inpositionoa = true;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                        }
                        catch { continue; }
                    }
                    // MOVE PMD END

                    elecsport.Write(elecholdcmd + "\r\n");
                    Thread.Sleep((int) min232updtime);
                    elecsport.ReadExisting();


                    elecsport.Write(elecresetcmd + "\r\n");
                    Thread.Sleep((int) min232updtime);
                    elecsport.ReadExisting();


                    elecsport.Write(elecstartcmd + "\r\n");

                    var chargerates = new List<Double>();
                    var chrate = 0.00;
                    string[] chratedata = new string[] { };
                    double Avgchargerate;
                    double Stdchargerate;
                    double StdErrorOfMean;

                    Thread.Sleep(1500);

                    //var timenow = DateTime.Now;
                    var timerecord = DateTime.Now.AddSeconds(Convert.ToDouble(recordperiod));

                    var timesincelastrec = DateTime.Now;

                    var timeout = DateTime.Now.AddSeconds(0.3);
                    while (DateTime.Now < timerecord)
                    {
                        timeout = DateTime.Now.AddSeconds(0.3);
                        while (DateTime.Now < timeout)
                        {
                            try
                            {
                                elecsport.Write(elecreadchratecmd + "\r\n");
                                Thread.Sleep((int) min232updtime);

                                var elecdataformatted = (elecsport.ReadExisting()).Replace(" ", String.Empty);

                                chratedata = Regex.Split(elecdataformatted, elecdatadivchar);


                                chrate = Convert.ToDouble(chratedata[elecdataindex]);
                                if ((Math.Abs(chrate) < Math.Abs(elecdatacutoff)))
                                {
                                    break;
                                }

                            }
                            catch { continue; }
                            break;
                        }
                        var isD1 = chratedata[elecdataidindex];//Regex.Replace(chratedata[elecdataidindex], @"\s+", "");

                        if (chargerates.Count == 0 && isD1 == elecdataid && (Math.Abs(chrate) < Math.Abs(elecdatacutoff)))
                        {
                            chargerates.Add(chrate);
                            timesincelastrec = DateTime.Now;
                        }
                        else if (chargerates.Count > 0 && isD1 == elecreadchratecmd && (Math.Abs(chrate) < Math.Abs(elecdatacutoff)))
                        {
                            // Add new measurement point if Buffer updates and value changes OR if it has been more than *TempRes/Buffer update time* seconds since last measurement
                            if ((chrate != chargerates[chargerates.Count - 1]) || ((DateTime.Now - timesincelastrec).TotalSeconds > electempres))
                            {
                                chargerates.Add(chrate);
                                timesincelastrec = DateTime.Now;
                            }
                        }

                    }

                    if (chargerates.Count == 1)
                    {
                        Avgchargerate = chargerates[0];
                        Stdchargerate = 0.00;
                        StdErrorOfMean = 0.00;
                    }
                    else if (chargerates.Count > 2)
                    {
                        Avgchargerate = chargerates.Average();
                        Stdchargerate = chargerates.StandardDeviation();
                        StdErrorOfMean = Stdchargerate / Math.Sqrt((chargerates.Count));

                    }
                    else
                    {
                        if (MessageBox.Show("Error has occurred as no charge rates recorded for depth: " + depths[i].ToString() + "cm , Off-Axis Position: " + ((OApositions[j] / 10).ToString()) + "cm" + " Do you want to continue or repeat the profile measurements for this depth? (YES: Continue, NO: Repeat Measurements)", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            //do no stuff
                            goto DoProfile;
                        }
                        // If continue set charge rates/std/etc all to -99999 as these have not been recorded and may need to be highlighted.
                        Avgchargerate = -99999;
                        Stdchargerate = -99999;
                        StdErrorOfMean = -99999;
                    }


                    avgintchrates[i + 1, j + 1] = Avgchargerate;
                    avgintchrates[i + 1, OApositions.Count() + j + 1] = Stdchargerate;
                    avgintchrates[i + 1, OApositions.Count() * 2 + j + 1] = StdErrorOfMean;
                }

                if (i < depths.Count() - 1)
                {

                    if (MessageBox.Show("Was the run successful?  (Selecting 'No' will cause repeat profile measurement for this depth)", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                        goto DoProfile;
                    }


                    jsondata = JsonConvert.SerializeObject(avgintchrates);
                    jarray = JArray.Parse(jsondata);
                    jarray[0][0] = name;

                    using (StreamReader sr = new StreamReader("srdata.json"))
                    {
                        string json = sr.ReadToEnd();
                        JArray jarr = JArray.Parse(json);

                        //jarr.Add(jarray);
                        jarr[jidx - 1] = jarray;

                        jout = JsonConvert.SerializeObject(jarr);
                    }
                    File.WriteAllText("srdata.json", jout);

                    updatesrdatalist();

                    this.pmdsport.Write(pmdgotoabscmd + caxpostxt + "\r\n");
                    Thread.Sleep((int) min232updtime);


                    inposition2 = false;

                    timeout23 = DateTime.Now.AddSeconds(timeout(Convert.ToDouble(pmdspeed), Convert.ToDouble(caxpos)));
                    while (inposition2 == false && DateTime.Now < timeout23)
                    {
                        try
                        {
                            Thread.Sleep((int) min232updtime);
                            var read = pmdsport.ReadExisting();

                            if (read.Contains("P"))
                            {
                                Thread.Sleep(1000);
                                this.pmdsport.Write(pmdpositioncmd + "\r\n");
                                Thread.Sleep((int) min232updtime);
                                var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];

                                if (Convert.ToDouble(pos) == caxpos)
                                {
                                    inposition2 = true;
                                    break;
                                }

                            }
                            else
                            {
                                continue;
                            }

                        }
                        catch { continue; }
                    }


                    elecsport.Write(elecholdcmd + "\r\n");
                    Thread.Sleep((int) min232updtime);
                    elecsport.ReadExisting();

                    //goto DoProfile;

                    elecsport.Write(elecresetcmd + "\r\n");
                    Thread.Sleep((int) min232updtime);
                    elecsport.ReadExisting();

                    MessageBox.Show("Depth: " + depths[i].ToString() + "cm completed, please move chamber to depth: " + depths[i + 1].ToString() + "cm and then hit 'OK' / close this prompt", "Info");
                }
                else
                {
                    if (MessageBox.Show("Was the run successful?  (Selecting 'No' will cause repeat profile measurement for this depth)", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                        goto DoProfile;
                    }

                    jsondata = JsonConvert.SerializeObject(avgintchrates);
                    //JArray jarray = JArray.Parse(jsondata);
                    jarray = JArray.Parse(jsondata);
                    jarray[0][0] = name;


                    using (StreamReader sr = new StreamReader("srdata.json"))
                    {
                        string json = sr.ReadToEnd();
                        JArray jarr = JArray.Parse(json);

                        //jarr.Add(jarray);
                        jarr[jidx - 1] = jarray;

                        jout = JsonConvert.SerializeObject(jarr);
                    }

                    File.WriteAllText("srdata.json", jout);

                    updatesrdatalist();

                    MessageBox.Show("Run has been completed and data saved");
                }

            }


        }


        //Start button
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            // Update UI to let user know profile acquisition is in progress
            await Task.Run(() =>
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
                    statustxt.Text = "Status: profile acquisition in progress...";
                    statustxt.FontSize = 14;
                    statustxt.TextAlignment = TextAlignment.Center;
                    statustxt.VerticalAlignment = VerticalAlignment.Center;
                    statustxt.Margin = new Thickness(15, 0, 5, 0);


                    mw.programstatus.Children.Add(circle);
                    mw.programstatus.Children.Add(statustxt);
                });


            });

            await Task.Delay(2000);


            MeasureProfiles();



            //Update UI Once profile acquisition fully complete.
            await Task.Run(() =>
            {

                this.Dispatcher.Invoke(() =>
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
                    statustxt.Text = "Status: acquisition complete.";
                    statustxt.FontSize = 14;
                    statustxt.TextAlignment = TextAlignment.Center;
                    statustxt.VerticalAlignment = VerticalAlignment.Center;
                    statustxt.Margin = new Thickness(15, 0, 5, 0);


                    mw.programstatus.Children.Add(circle);
                    mw.programstatus.Children.Add(statustxt);
                });


            });
            await Task.Delay(2000);


        }

        //Initialize
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                pmdinit.IsEnabled = false;

                this.pmdsport = Properties.Settings.Default.pmdserobj;
                string pmdspeed = this.pmdspeed.Text;
                string pmdstartpos = this.startpos.Text;

                this.pmdsport.Write(pmdspeedcmd + pmdspeed + "\r\n");
                Thread.Sleep((int) min232updtime);
                this.pmdsport.Write(pmdinitializecmd + "\r\n");
                Thread.Sleep(1500);

                bool inposition1 = false;
                bool inposition2 = false;
                var timeout = DateTime.Now.AddSeconds(120);

                while (inposition1 == false && DateTime.Now < timeout)
                {
                    try
                    {
                        Thread.Sleep((int) min232updtime);
                        var read = pmdsport.ReadExisting();

                        if (read.Contains("P"))
                        {
                            Thread.Sleep(1000);
                            this.pmdsport.Write(pmdpositioncmd + "\r\n");
                            Thread.Sleep((int) min232updtime);
                            var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];

                            if (Convert.ToDouble(pos) == 0)
                            {
                                inposition2 = true;
                                break;
                            }

                        }

                    }
                    catch { continue; }

                }

                if (DateTime.Now >= timeout)
                {
                    this.pmdinit.IsEnabled = true;
                    MessageBox.Show("PMD Initialize timed out. Please try again", "Status");
                    return;
                }

                this.pmdsport.Write(pmdgotoabscmd + pmdstartpos + "\r\n");
                Thread.Sleep((int) min232updtime);

                var timeout2 = DateTime.Now.AddSeconds(120);
                while (inposition2 == false && DateTime.Now < timeout2)
                {
                    try
                    {
                        Thread.Sleep((int) min232updtime);
                        var read = pmdsport.ReadExisting();

                        if (read.Contains("P"))
                        {
                            Thread.Sleep(1000);
                            this.pmdsport.Write(pmdpositioncmd + "\r\n");
                            Thread.Sleep((int) min232updtime);
                            var pos = Regex.Split(pmdsport.ReadExisting(), "E")[1];

                            if (Convert.ToDouble(pos) == Convert.ToDouble(pmdstartpos))
                            {
                                inposition2 = true;
                                break;
                            }

                        }

                    }
                    catch { continue; }
                }

                if (DateTime.Now >= timeout2)
                {
                    MessageBox.Show("PMD Initialize timed out. Please try again", "Status");
                    this.pmdinit.IsEnabled = true;
                }



            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }
        
        }



        //Export selected data
        private void Excelexportbtn_Click(object sender, RoutedEventArgs e)
        {
            var rgx = Regex.Split(listsrdata.SelectedItem.ToString(), "  ");

            string dtxl = DateTime.ParseExact(Regex.Split(rgx[1], "SRsave-")[1], "ddMMyyyyhhmmss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy-hh-mm");


            // get index of data
            int idx = Convert.ToInt32(rgx[0]) - 1;

            JArray exjarr;
            using (StreamReader sr = new StreamReader("srdata.json"))
            {
                string exjson = sr.ReadToEnd();
                exjarr = JArray.Parse(exjson);
            }

            var data = exjarr[idx];

            var dt = new DataTable();

            var rownum = data.Count();
            var colnum = data[0].Count();

            Double[,] datamat = new Double[rownum, colnum];

            var table = new DataTable();


            XLWorkbook wb = new XLWorkbook();

            wb.Worksheets.Add("Profile Data - " + dtxl);

            var ws = wb.Worksheet("Profile Data - " + dtxl);


            for (int rr=0;rr<rownum;rr++)
            {

                for (int cc=0;cc<colnum;cc++)
                {
                    if( (data[rr][cc].ToString()).Contains("SRsave-"))
                    {
                        datamat[rr,cc] = 0.00;
                        ws.Cell(rr+1, cc+1).Value = "Depth";
                    }
                    else
                    {
                        datamat[rr, cc] = Convert.ToDouble(data[rr][cc].ToString());
                        ws.Cell(rr+1,cc+1).Value = Convert.ToDouble(data[rr][cc].ToString());

                    }

                }

            }

            ws.Column((colnum - 1) / 3 +1).InsertColumnsAfter(1);
            ws.Column(2*(colnum - 1) / 3 + 2).InsertColumnsAfter(1);
            ws.Row(1).InsertRowsAbove(4);

            ws.Cell(1, 1).Value = "TBI Commissioning Data";
            ws.Cell(1, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;


            // Charge Rate Section
            ws.Cell(3, 1).Value = "Mean charge rate";
            ws.Cell(3, 1).Style.Font.Bold = true;

            ws.Cell(4, 1).Value = "Off-Axis Distance (mm)";

            // STD Section
            ws.Cell(3, (colnum - 1) / 3 + 3).Value = "Standard Deviation of Charge Rate";
            ws.Cell(3, (colnum - 1) / 3 + 3).Style.Font.Bold = true;

            ws.Cell(4, (colnum - 1) / 3 + 3).Value = "Off-Axis Distance (mm)";


            //SEM Section
            ws.Cell(3, 2*(colnum - 1) / 3 + 4).Value = "Standard Error of the Mean (SEM)";
            ws.Cell(3, 2 * (colnum - 1) / 3 + 4).Style.Font.Bold = true;

            ws.Cell(4, 2*(colnum - 1) / 3 + 4).Value = "Off-Axis Distance (mm)";



            wb.SaveAs("TBI_Commissioning_"+dtxl+".xlsx");

        }

        private void Datadeletebtn_Click(object sender, RoutedEventArgs e)
        {
            var selected = listsrdata.SelectedItem.ToString();
            
            if(selected != "")
            {
                try
                {


                    var rgx = Regex.Split(selected, "  ");

                    // get index of data
                    int idx = Convert.ToInt32(rgx[0]) - 1;

                    JArray exjarr;
                    using (StreamReader sr = new StreamReader("srdata.json"))
                    {
                        string exjson = sr.ReadToEnd();
                        exjarr = JArray.Parse(exjson);
                    }

                    if (MessageBox.Show("Are you sure you want to delete data " +selected + " ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                        return;
                    }
                    else
                    {
                        //do yes stuff
                        exjarr.RemoveAt(idx);
                        var jout = JsonConvert.SerializeObject(exjarr);
                        File.WriteAllText("srdata.json", jout);
                        updatesrdatalist();
                    }

                } catch { }
            }

            updatesrdatalist();
        }

        private void plotdatabtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SeriesCollection.Clear();
            }
            catch
            {
                SeriesCollection = new SeriesCollection
                {
                };
            }

            try
            {
                SeriesCollection2.Clear();
            }
            catch
            {
                SeriesCollection2 = new SeriesCollection
                {
                };
            }

            var selected = listsrdata.SelectedItem.ToString();

            var chkboxOADs = TPRplot.Children.OfType<CheckBox>();
            var chkboxdepths = Profileplot.Children.OfType<CheckBox>();
            
            var plotOADs = new List<int>();
            var plotOADsdb = new List<double>();
            var plotdepths = new List<int>();
            var plotdepthsdb = new List<double>();
            var OADlabels = new List<string> {};

            var rgx = Regex.Split(selected, "  ");

            // get index of data
            int idx = Convert.ToInt32(rgx[0]) - 1;

            JArray exjarr;
            using (StreamReader sr = new StreamReader("srdata.json"))
            {
                string exjson = sr.ReadToEnd();
                exjarr = JArray.Parse(exjson);
            }

            var data = exjarr[idx];

            var rownum = data.Count();
            var colnum = data[0].Count();




            // For the TPR Graph
            foreach (CheckBox chkoad in chkboxOADs)
            {
                if (chkoad.IsChecked.Value)
                {
                    var oadindex = Convert.ToInt32(Regex.Split(chkoad.Name, "_")[1]);

                    plotOADs.Add(oadindex);
                    plotOADsdb.Add(Convert.ToDouble(chkoad.Content));

                    
                }

            }

            // For the Profile Graph
            foreach (CheckBox chkdepth in chkboxdepths)
            {
                if (chkdepth.IsChecked.Value)
                {
                    var dpindex = Convert.ToInt32(Regex.Split(chkdepth.Name, "_")[1]);

                    plotdepths.Add(dpindex);
                    plotdepthsdb.Add(Convert.ToDouble(chkdepth.Content));
                }
            }


            //Plot TPR
            for (int f=1;f<plotOADs.Count()+1;f++)
            {
                var tprvals = new List<double>();
                var pltdepthlabels = new List<string> { };

                var pltoadindex = plotOADs[f - 1];

                for (int ff = 1; ff < rownum; ff++)
                {
                    if (Normalizebox.IsChecked.Value == true)
                    {
                        var Normval = Math.Abs(Convert.ToDouble(data[Normdepth.SelectedIndex + 1][Normoad.SelectedIndex + 1]));

                        tprvals.Add(RoundToSignificantDigits(Math.Abs(Convert.ToDouble(data[ff][pltoadindex]))/Normval,4) );

                    }
                    else
                    {
                        tprvals.Add(RoundToSignificantDigits(Math.Abs(Convert.ToDouble(data[ff][pltoadindex])),4));
                    }
                    pltdepthlabels.Add(data[ff][0].ToString());
                }

                

                var chvals = tprvals.ToArray<double>();

                //YFormatter = value => value.ToString("C");

                plottpr.AxisX.Clear();

                plottpr.AxisX.Add(
                    new Axis
                    {
                        Labels=pltdepthlabels.ToArray()
                    }
                    );
                //Labels = pltdepthlabels.ToArray();

                DataContext = this;

                SeriesCollection.Add(new LineSeries
                {
                    Title = "OAD - " + Convert.ToString(plotOADsdb[f-1]),
                    Values = chvals.AsChartValues(), //new ChartValues<double> {  },
                    DataLabels = true,
                    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                    PointGeometry = null,//Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                    PointGeometrySize = 50,
                    PointForeground = Brushes.Gray
                });


            }


            // Plot profile
            for (int g=1;g< plotdepths.Count()+1;g++)
            {
                var oadvals = new List<double>();
                var pltoadlabels = new List<string> { };

                var pltdpindex = plotdepths[g - 1];

                for (int gg=1;gg<colnum/3 +1;gg++)
                {
                    if (Normalizebox.IsChecked.Value == true)
                    {
                        var Normval = Math.Abs(Convert.ToDouble(data[Normdepth.SelectedIndex+1][Normoad.SelectedIndex+1]));

                        oadvals.Add(RoundToSignificantDigits(Math.Abs(Convert.ToDouble(data[pltdpindex][gg]))/Normval,4) );
                    }
                    else
                    {
                        oadvals.Add(RoundToSignificantDigits(Math.Abs(Convert.ToDouble(data[pltdpindex][gg])),4));
                    }

                    pltoadlabels.Add(data[0][gg].ToString());

                }

                var chvals = oadvals.ToArray<double>();

                plotprofile.AxisX.Clear();

                plotprofile.AxisX.Add(
                    new Axis
                    {
                        Labels = pltoadlabels.ToArray()
                    }
                    );


                DataContext = this;


                SeriesCollection2.Add(new LineSeries
                {
                    Title = "Depth - " + Convert.ToString(plotdepthsdb[g - 1]) + "cm",
                    Values = chvals.AsChartValues(), //new ChartValues<double> {  },
                    DataLabels = true,
                    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                    PointGeometry = null,//Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                    PointGeometrySize = 50,
                    PointForeground = Brushes.Gray
                });

                


            }

            



        }



        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> YFormatterprof { get; set; }



        private void listsrdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TPRplot.Children.Clear();
            Profileplot.Children.Clear();

            Normoad.Items.Clear();
            Normdepth.Items.Clear();

            TextBlock tprtitle = new TextBlock();
            tprtitle.Text = "TPR OADs: ";
            tprtitle.FontSize = 16;

            TextBlock oadtitle = new TextBlock();
            oadtitle.Text = "Profile Depths: ";
            oadtitle.FontSize = 16;

            TPRplot.Children.Add(tprtitle);
            Profileplot.Children.Add(oadtitle);

            if (listsrdata.SelectedIndex != -1)
            {

                var selected = listsrdata.SelectedItem.ToString();


                var rgx = Regex.Split(selected, "  ");

                // get index of data
                int idx = Convert.ToInt32(rgx[0]) - 1;

                JArray exjarr;
                using (StreamReader sr = new StreamReader("srdata.json"))
                {
                    string exjson = sr.ReadToEnd();
                    exjarr = JArray.Parse(exjson);
                }

                var data = exjarr[idx];

                var rownum = data.Count();
                var colnum = data[0].Count();

                var OApositions = new List<string>();


                for (int h = 1; h < (colnum / 3) + 1; h++)
                {
                    var OApos = (Convert.ToString(data[0][h]));
                    var OAposname = (Convert.ToString(Math.Abs(Convert.ToDouble(data[0][h]))));

                    OApositions.Add(OApos);

                    CheckBox bb = new CheckBox();
                    bb.Name = "_" + Convert.ToString(h) + "_OAD" + OAposname;
                    bb.Content = OApos;
                    bb.MouseEnter += new MouseEventHandler(Checkbox_OnTPROADMouseEnter);
                    bb.GotMouseCapture += new MouseEventHandler(Checkbox_TPROADMouseCapture);
                    bb.Margin = new Thickness(2.5, 0, 2.5, 0);

                    TPRplot.Children.Add(bb);

                    Normoad.Items.Add(OApos);

                }

                for (int ii = 1; ii < rownum; ii++)
                {
                    var tprdepth = data[ii][0].ToString(); ;

                    CheckBox cc = new CheckBox();
                    cc.Name = "_" + Convert.ToString(ii) + "_depth" + tprdepth;
                    cc.Content = data[ii][0].ToString();
                    cc.Margin = new Thickness(2.5, 0, 2.5, 0);
                    cc.MouseEnter += new MouseEventHandler(Checkbox_OnTPROADMouseEnter);
                    cc.GotMouseCapture += new MouseEventHandler(Checkbox_TPROADMouseCapture);

                    Profileplot.Children.Add(cc);

                    Normdepth.Items.Add(tprdepth);

                }

                Normdepth.SelectedIndex = 2;
                Normoad.SelectedIndex = 0;


            }


        }








    }
}
