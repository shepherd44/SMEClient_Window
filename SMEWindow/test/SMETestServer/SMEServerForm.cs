using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SME.Server;
using DumpReader;

namespace SMETestServer
{
    public delegate void AddTextDele();

    public partial class SMEServerForm : Form
    {
        public AddTextDele Add;
        public static string IP { get; set; }
        public static string FileName { get; set; }
        public static string Time { get; set; }
        public SMEServerForm()
        {
            InitializeComponent();
            SMEServer.Start("127.0.0.1", 3000);
            richTB_Main.Text += "Server Start";
            SMEServer.AfterR += invokef;
            Add += AddText;
        }

        private void SMEServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SMEServer.Close();
        }

        public static void AddText()
        {
            System.Windows.Forms.MessageBox.Show("invoke2");
            richTB_Main.Text = string.Format("{0}\n[{1}:{2}]{3}", richTB_Main.Text, Time, IP, FileName);
        }
        public void invokef(string ip, string filename, string time)
        {
            System.Windows.Forms.MessageBox.Show("invoke");
            IP = ip;
            FileName = filename;
            Time = time;
            this.Invoke(Add);

        }
        
    }
}
