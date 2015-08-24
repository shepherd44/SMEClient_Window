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

using SME.Client;
using NetLib;

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
            //SMEClient smeclient;
            //smeclient = new SMEClient("SMEClientForm", new Version("0.0"), false, "Test Key");
        }

        private void withCPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SMEClient smeclient;
            //smeclient = new SMEClient("SMEClientForm", new Version("0.0"), false, "Test Key");
        }

        private void fileSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TCPSender sendertemp = new TCPSender("127.0.0.1", 3000, "C:\\Dumps\\CS\\SMETestClient-0.0-2015-08-18-16-48-37.xml", "SMETestClient-0.0-2015-08-18-16-48-37.xml", 10);
            sendertemp.FileSend();
        }

        private void nativeErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                managedCal.AddCalWrap add = new managedCal.AddCalWrap();
                add.Add(2, 1);
            }
            catch(Win32Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch
            {
                MessageBox.Show("Error");
            }
            
        }

        private void handledExceptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("My Excetpion");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
