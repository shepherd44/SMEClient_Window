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
    public partial class SMEServerForm : Form
    {
        public SMEServerForm()
        {
            InitializeComponent();
            SMEServer.Start("127.0.0.1", 3000);
            richTB_Main.Text += "Server Start";
        }

        private void SMEServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SMEServer.Close();
        }

        public void AddText(string text)
        {
            richTB_Main.Text = string.Format("{0}\n{1}", richTB_Main.Text, text);
        }
        
    }
}
