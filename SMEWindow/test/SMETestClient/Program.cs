using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SME.Client;

namespace SMETestClient
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SMEClient smeclient = new SMEClient("SMETestClient", null, true, false, "10", "127.0.0.1", 3000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SMEClientForm());
        }
    }
}
