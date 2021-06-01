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

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for MovePMD.xaml
    /// </summary>
    public partial class MovePMD : Page
    {
        public MovePMD()
        {
            InitializeComponent();
            Init();
            
        }

        private void Init()
        { 
            pmdinitializecmd = Properties.Settings.Default.pmdinitializecmd;
            pmdpositioncmd = Properties.Settings.Default.pmdpositioncmd;
            pmdspeedcmd = Properties.Settings.Default.pmdspeedcmd;
            pmdgotoabscmd = Properties.Settings.Default.pmdgotoabscmd;
            pmdgotorelcmd = Properties.Settings.Default.pmdgotorelcmd;
            pmddriveoutcmd = Properties.Settings.Default.pmddriveoutcmd;
            pmddriveincmd = Properties.Settings.Default.pmddriveincmd;

            this.min232updtime = Convert.ToDouble(Properties.Settings.Default.minRS232updtime);


        }

        string pmdinitializecmd;
        string pmdpositioncmd;
        string pmdspeedcmd;
        string pmdgotoabscmd;
        string pmdgotorelcmd;
        string pmddriveoutcmd;
        string pmddriveincmd;

        double min232updtime = Convert.ToDouble(Properties.Settings.Default.minRS232updtime);


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.pmdsport = Properties.Settings.Default.pmdserobj;

            string pmdspeed = this.pmdspeed.Text;
            string pmdmovetoby = this.pmdmovetoby.Text;
            string movemode = this.pmdmovemode.Text;

            string pmdinitializecmd = Properties.Settings.Default.pmdinitializecmd;
            string pmdpositioncmd = Properties.Settings.Default.pmdpositioncmd;
            string pmdspeedcmd = Properties.Settings.Default.pmdspeedcmd;
            string pmdgotoabscmd = Properties.Settings.Default.pmdgotoabscmd;
            string pmdgotorelcmd = Properties.Settings.Default.pmdgotorelcmd;
            string pmddriveoutcmd = Properties.Settings.Default.pmddriveoutcmd;
            string pmddriveincmd = Properties.Settings.Default.pmddriveincmd;

            Thread.Sleep((int) min232updtime);
            pmdsport.Write(pmdspeedcmd + pmdspeed + "\r\n");
            Thread.Sleep((int)min232updtime);


            if (movemode == "Absolute")
            {
                pmdsport.Write(pmdgotoabscmd + pmdmovetoby + "\r\n");
            }
            else if (movemode == "Relative")
            {
                pmdsport.Write(pmdgotorelcmd + pmdmovetoby + "\r\n");
            }
            else if (movemode == "Drive Out")
            {
                pmdsport.Write(pmddriveoutcmd + "\r\n");
            }
            else if (movemode == "Drive In")
            {
                pmdsport.Write(pmddriveincmd + "\r\n");
            }





        }

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

        private void pmdmovetoby_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.pmdsport = Properties.Settings.Default.pmdserobj;

            pmdsport.Write(pmdinitializecmd + "\r\n");
        }
    }
}
