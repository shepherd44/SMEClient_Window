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
            SMEClient smeclient = new SMEClient("SMETestClient",    // Project Name
                                                null,               // Project Version
                                                true,               // Native Exception Catch
                                                false,              // Handled Exception Catch
                                                "18",               // API Key
                                                "127.0.0.1",        // Server IP
                                                3000);              // Server Port

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SMEClientForm());
        }
    }
}
