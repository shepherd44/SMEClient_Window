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

namespace SMETestServer
{
    public partial class SMEServerForm : Form
    {
        public SMEServerForm()
        {
            InitializeComponent();
            SMEServer.Start();
        }

        private void SMEServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SMEServer.Close();
        }

        
    }
}
