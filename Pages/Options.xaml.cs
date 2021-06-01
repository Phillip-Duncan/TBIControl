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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.pmdbaudrate.Text = Convert.ToString(Properties.Settings.Default.pmdbaudrate);
            this.pmdparity.Text = Convert.ToString(Properties.Settings.Default.pmdparity);
            this.pmddatabits.Text = Convert.ToString(Properties.Settings.Default.pmddatabits);
            this.pmdstopbits.Text = Convert.ToString(Properties.Settings.Default.pmdstopbits);
            this.pmdflowcontrol.Text = Convert.ToString(Properties.Settings.Default.pmdflowcontrol);

            this.elecbaudrate.Text = Convert.ToString(Properties.Settings.Default.elecbaudrate);
            this.elecparity.Text = Convert.ToString(Properties.Settings.Default.elecparity);
            this.elecdatabits.Text = Convert.ToString(Properties.Settings.Default.elecdatabits);
            this.elecstopbits.Text = Convert.ToString(Properties.Settings.Default.elecstopbits);
            this.elecflowcontrol.Text = Convert.ToString(Properties.Settings.Default.elecflowcontrol);

            this.pmdinitializecmd.Text = Properties.Settings.Default.pmdinitializecmd;
            this.pmdpositioncmd.Text = Properties.Settings.Default.pmdpositioncmd;
            this.pmdspeedcmd.Text = Properties.Settings.Default.pmdspeedcmd;
            this.pmdgotoabscmd.Text = Properties.Settings.Default.pmdgotoabscmd;
            this.pmdgotorelcmd.Text = Properties.Settings.Default.pmdgotorelcmd;
            this.pmddriveoutcmd.Text = Properties.Settings.Default.pmddriveoutcmd;
            this.pmddriveincmd.Text = Properties.Settings.Default.pmddriveincmd;


            this.elecstartcmd.Text = Properties.Settings.Default.elecstartcmd;
            this.elecholdcmd.Text = Properties.Settings.Default.elecholdcmd;
            this.elecresetcmd.Text = Properties.Settings.Default.elecresetcmd;
            this.elecnullcmd.Text = Properties.Settings.Default.elecnullcmd;
            this.elecreadchratecmd.Text = Properties.Settings.Default.elecreadchratecmd;
            this.elecreadchcmd.Text = Properties.Settings.Default.elecreadchcmd;


            // Advanced Settings

            this.electempres.Text = Properties.Settings.Default.electempres;
            this.elecdatacutoff.Text = Properties.Settings.Default.elecdatacutoff;

            this.elecdataindex.Text = Properties.Settings.Default.elecdataindex;
            this.elecdatadivchar.Text = Properties.Settings.Default.elecdatadivchar;
            this.elecdataid.Text = Properties.Settings.Default.elecdataid;
            this.elecdataidindex.Text = Properties.Settings.Default.elecdataidindex;

            // Adv RS232 update rate
            this.minRS232updtime.Text = Convert.ToString(Properties.Settings.Default.minRS232updtime);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pmdbaudrate = Convert.ToInt32(pmdbaudrate.Text);
            Properties.Settings.Default.pmdparity = (Parity)Enum.Parse(typeof(Parity), pmdparity.Text);
            Properties.Settings.Default.pmddatabits = Convert.ToInt32(pmddatabits.Text);
            Properties.Settings.Default.pmdstopbits = (StopBits)Enum.Parse(typeof(StopBits), pmdstopbits.Text);
            Properties.Settings.Default.pmdflowcontrol = (Handshake)Enum.Parse(typeof(Handshake), pmdflowcontrol.Text);

            Properties.Settings.Default.elecbaudrate = Convert.ToInt32(elecbaudrate.Text);
            Properties.Settings.Default.elecparity = (Parity)Enum.Parse(typeof(Parity), elecparity.Text);
            Properties.Settings.Default.elecdatabits = Convert.ToInt32(elecdatabits.Text);
            Properties.Settings.Default.elecstopbits = (StopBits)Enum.Parse(typeof(StopBits), elecstopbits.Text);
            Properties.Settings.Default.elecflowcontrol = (Handshake)Enum.Parse(typeof(Handshake), elecflowcontrol.Text);


            Properties.Settings.Default.pmdinitializecmd = pmdinitializecmd.Text;
            Properties.Settings.Default.pmdpositioncmd = pmdpositioncmd.Text;
            Properties.Settings.Default.pmdspeedcmd = pmdspeedcmd.Text;
            Properties.Settings.Default.pmdgotoabscmd = pmdgotoabscmd.Text;
            Properties.Settings.Default.pmdgotorelcmd = pmdgotorelcmd.Text;
            Properties.Settings.Default.pmddriveoutcmd = pmddriveoutcmd.Text;
            Properties.Settings.Default.pmddriveincmd = pmddriveincmd.Text;


            Properties.Settings.Default.elecstartcmd = elecstartcmd.Text;
            Properties.Settings.Default.elecholdcmd = elecholdcmd.Text;
            Properties.Settings.Default.elecresetcmd = elecresetcmd.Text;
            Properties.Settings.Default.elecnullcmd = elecnullcmd.Text;
            Properties.Settings.Default.elecreadchratecmd = elecreadchratecmd.Text;
            Properties.Settings.Default.elecreadchcmd = elecreadchcmd.Text;

            // Adv settings

            Properties.Settings.Default.electempres = electempres.Text;
            Properties.Settings.Default.elecdatacutoff = elecdatacutoff.Text;
            Properties.Settings.Default.elecdataindex = elecdataindex.Text;
            Properties.Settings.Default.elecdatadivchar = elecdatadivchar.Text;
            Properties.Settings.Default.elecdataid= elecdataid.Text;
            Properties.Settings.Default.elecdataidindex = elecdataidindex.Text;

            // Adv RS232 update rate
            Properties.Settings.Default.minRS232updtime = Convert.ToDouble(minRS232updtime.Text);

            Properties.Settings.Default.Save();


            this.Close();
        }





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

        private void pmdbaudrate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }





        private void pmdbaudrate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minRS232updrate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void elecbaudrate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void electempres_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void elecdataindex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }

        private void elecdataidindex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowedNumOnly(e.Text);
        }
    }
}

