using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace IPDetector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        Thread myTherad = null;

        public void scan (string subnet)
        {
            Ping ping;
            PingReply reply;
            IPAddress address;
            IPHostEntry host;

            for (int i = 1; i < 255; i++)
            {
                string subnetn = "." + i.ToString();
                ping = new Ping();
                reply = ping.Send(subnet + subnetn);

                if (reply.Status == IPStatus.Success)
                {
                    try
                    {
                        address = IPAddress.Parse(subnet + subnetn);
                        host = Dns.GetHostEntry(address);

                        IPBox.AppendText(subnet + "\t" + subnetn + "\t" + host.HostName.ToString() + "\t" + "UP\n");
                    }
                    catch
                    {
                    }
                }
            }
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            myTherad = new Thread(() => scan(SubnetBox.Text));
            myTherad.Start();

            if (myTherad.IsAlive)
            {
                StopButton.Enabled = true;
                StartButton.Enabled = false;
                SubnetBox.Enabled = false;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            myTherad.Suspend();
            StartButton.Enabled = true;
            StopButton.Enabled = false;
            SubnetBox.Enabled = true;
        }
    }
}
