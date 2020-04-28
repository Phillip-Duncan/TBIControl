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
using System.IO.Ports;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Shapes;
using System.IO;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();


            pmdClose.IsEnabled = false;
            elecClose.IsEnabled = false;

            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                pmdPort.Items.Add(s);
                elecPort.Items.Add(s);
            }
            Init();

        }


        private void Init()
        {

            //Create Data files for Step and Record and Continuous Mode.
            if (!File.Exists("srdata.json"))
            {
                using (StreamWriter sw = File.AppendText("srdata.json"))
                {
                    sw.WriteLine("[ ]");
                    sw.Close();
                }
            }

            if (!File.Exists("contdata.json"))
            {
                using (StreamWriter sw = File.AppendText("contdata.json"))
                {
                    sw.WriteLine("[ ]");
                    sw.Close();
                }
            }



            this.pmdPort.Text = Properties.Settings.Default.pmdport;
            this.pmdConnect.IsEnabled = !Properties.Settings.Default.pmdconnected;
            this.pmdClose.IsEnabled = Properties.Settings.Default.pmdconnected;
            this.pmdsport = Properties.Settings.Default.pmdserobj;

            this.elecPort.Text = Properties.Settings.Default.elecport;
            this.elecConnect.IsEnabled = !Properties.Settings.Default.elecconnected;
            this.elecClose.IsEnabled = Properties.Settings.Default.elecconnected;
            this.elecsport = Properties.Settings.Default.elecserobj;

            try
            {
                var pmdc = pmdsport.IsOpen;
            } catch { this.pmdConnect.IsEnabled = true; }
            try
            {
                var elecc = elecsport.IsOpen;
            } catch { this.elecConnect.IsEnabled = true; }



        }


        private void IsConnected()
        {
            Properties.Settings.Default.pmdport = pmdPort.Text;
            Properties.Settings.Default.pmdconnected = !pmdConnect.IsEnabled;
            Properties.Settings.Default.pmdserobj = pmdsport;

            Properties.Settings.Default.elecport = elecPort.Text;
            Properties.Settings.Default.elecconnected = !elecConnect.IsEnabled;
            Properties.Settings.Default.elecserobj = elecsport;

            Properties.Settings.Default.Save();
        }


        public System.IO.Ports.SerialPort pmdsport;
        public System.IO.Ports.SerialPort elecsport;

        public void pmdserialport_connect(String port, int baudrate, Parity parity, int databits, StopBits stopbits, Handshake flowcontrol)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            pmdsport = new System.IO.Ports.SerialPort(
            port, baudrate, parity, databits, stopbits);
            pmdsport.Handshake = flowcontrol;
            try
            {
                pmdsport.Open();
                pmdClose.IsEnabled = true;
                pmdConnect.IsEnabled = false;
                pmdtxtReceive.AppendText("[" + dtn + "] " + "Connected\n");
                pmdsport.DataReceived += new SerialDataReceivedEventHandler(pmdsport_DataReceived);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }
        }

        public void elecserialport_connect(String port, int baudrate, Parity parity, int databits, StopBits stopbits, Handshake flowcontrol)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            elecsport = new System.IO.Ports.SerialPort(
            port, baudrate, parity, databits, stopbits);
            elecsport.Handshake = flowcontrol;
            elecsport.ReadTimeout = 2000;
            elecsport.WriteTimeout = 2000;
            try
            {
                elecsport.Open();
                elecClose.IsEnabled = true;
                elecConnect.IsEnabled = false;
                var serialnum = "";

                try
                {
                    elecsport.Write("SER" + "\r\n");
                    Thread.Sleep(55);
                    serialnum = Convert.ToString(elecsport.ReadExisting());
                } catch { }

                electxtReceive.AppendText("[" + dtn + "] " + "Connected\n");
                electxtReceive.AppendText("[" + dtn + "] " + "Serial Number: "+serialnum+"\n");
                //elecsport.DataReceived += new SerialDataReceivedEventHandler(elecsport_DataReceived);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }
        }

        private void pmdsport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            this.Dispatcher.Invoke(() =>
            {
                pmdtxtReceive.AppendText("[" + dtn + "] " + "Received: " + pmdsport.ReadExisting() + "\n");
            });

        }
        /*
        private void elecsport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    electxtReceive.AppendText("[" + dtn + "] " + "Received: " + elecsport.ReadExisting() + "\n");
                });
            } catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }

        } */


        private void pmdConnect_Click(object sender, RoutedEventArgs e)
        {
            String port = pmdPort.Text;
            int baudrate = Properties.Settings.Default.pmdbaudrate;
            Parity parity = Properties.Settings.Default.pmdparity;
            int databits = Properties.Settings.Default.pmddatabits;
            StopBits stopbits = Properties.Settings.Default.pmdstopbits;
            Handshake flowcontrol = Properties.Settings.Default.pmdflowcontrol;

            pmdserialport_connect(port, baudrate, parity, databits, stopbits, flowcontrol);

            IsConnected();

        }

        private void pmdDataSendbtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();
            //String data = pmdDatatoSend.Text;
            //pmdsport.Write(data);
            //pmdtxtReceive.AppendText("[" + dtn + "] " + "Sent: " + data + "\n");
        }

        private void pmdClose_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            if (pmdsport.IsOpen)
            {
                pmdsport.Close();
                pmdClose.IsEnabled = false;
                pmdConnect.IsEnabled = true;
                pmdtxtReceive.AppendText("[" + dtn + "] " + "Disconnected\n");
                IsConnected();
            }
        }


        private void elecConnect_Click(object sender, RoutedEventArgs e)
        {
            String port = elecPort.Text;
            int baudrate = Properties.Settings.Default.elecbaudrate;
            Parity parity = Properties.Settings.Default.elecparity;
            int databits = Properties.Settings.Default.elecdatabits;
            StopBits stopbits = Properties.Settings.Default.elecstopbits;
            Handshake flowcontrol = Properties.Settings.Default.elecflowcontrol;

            elecserialport_connect(port, baudrate, parity, databits, stopbits, flowcontrol);

            IsConnected();

        }

        private void elecDataSendbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Now;
                String dtn = dt.ToShortTimeString();
                //String data = elecDatatoSend.Text;
                //elecsport.Write(data + "\r\n");
                Thread.Sleep(50);
                elecsport.ReadExisting();
                //electxtReceive.AppendText("[" + dtn + "] " + "Sent: " + data + "\n");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error"); }

        }

        private void elecClose_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            if (elecsport.IsOpen)
            {
                elecsport.Close();
                elecClose.IsEnabled = false;
                elecConnect.IsEnabled = true;
                electxtReceive.AppendText("[" + dtn + "] " + "Disconnected\n");
                IsConnected();
            }
        }



    }
}
