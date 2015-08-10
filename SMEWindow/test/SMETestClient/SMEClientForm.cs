using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;

using SME;

namespace SMETestClient
{
    public partial class SMEClientForm : Form
    {
        public SMEClientForm()
        {
            InitializeComponent();
        }

        private void nullReferenceErrorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(nullReferenceError));
            th.Start();
        }

        private void nullReferenceError()
        {
            throw new NullReferenceException();
        }

        private void filePathErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(filePathError));
            th.Start();
        }

        private void filePathError()
        {
            XElement xml = XElement.Load("F://");
            
        }

        private void nullRefErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nullReferenceError();
        }

        private void filePathErrorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            filePathError();
        }

        private void onlyCSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMEClient smeclient;
            smeclient = new SMEClient("SMEClientForm", new Version("0.0"), false, "Test Key");
        }

        private void withCPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMEClient smeclient;
            smeclient = new SMEClient("SMEClientForm", new Version("0.0"), false, "Test Key");
        }
    }
}
