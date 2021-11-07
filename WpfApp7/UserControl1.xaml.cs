using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1
    {
        private SerialPort _serialPort;

        public UserControl1()
        {
            InitializeComponent();
            for (int i = 17; i >= 0; i--)
            {
                ToggleSwitch btn = new ToggleSwitch
                {
                    Name = $"SW{i:d2}",
                    OnContent = $"SW{i:d2}",
                    OffContent = $"SW{i:d2}",
                    FontSize = 20,
                    Width = 120,
                    LayoutTransform = new RotateTransform()
                    {
                        Angle = -90
                    }
                };
                btn.Toggled += Btn_Toggled;

                _ = sp_left.Children.Add(btn);
            }

            var keyButtons = new List<RepeatButton>() { KEY00, KEY01, KEY02, KEY03 };
            foreach (RepeatButton keybtn in keyButtons)
            {
                keybtn.Click += Keybtn_Click;
            }
        }

        private void Keybtn_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton btn = (RepeatButton)sender;
            uint num = Convert.ToUInt32(btn.Name[^2..]);

            byte sendbyte = (byte)((0b11 << 6) + (byte)num);
            byte[] sendarray = new byte[] { sendbyte };
            _serialPort.Write(sendarray, 0, 1);
        }

        private void Btn_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch btn = (ToggleSwitch)sender;
            uint num = Convert.ToUInt32(btn.Name[^2..]);

            byte sendbyte = (byte)((Convert.ToByte(btn.IsOn) << 7) + (byte)num);
            byte[] sendarray = new byte[] { sendbyte };
            _serialPort.Write(sendarray, 0, 1);

            //Debug.Print($"btn.IsOn");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] portnames = SerialPort.GetPortNames();
            string portname = portnames[0];
            //var portname = "COM2";

            _serialPort = new SerialPort(portname, 115200, Parity.Even, 8, StopBits.One);
            try
            {
                _serialPort.Open();
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Не удалось открыть порт");
                throw;
            }
        }
    }
}