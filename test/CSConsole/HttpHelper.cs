using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SME
{
    class HttpHelper
    {
        public static void func()
        {
            // post 방식으로 httprq 생성
            //WebRequest request = WebRequest.Create("https://192.168.23.196");
            WebRequest request = WebRequest.Create("http://www.contoso.com/");
            request.Method = "POST";

            // 전송할 data 와 Content 타입 생성
            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "text/xml";
            request.ContentLength = byteArray.Length;
            
            // 
            using(Stream outStream = request.GetRequestStream())
            {
                outStream.Write(byteArray, 0, byteArray.Length);
            }
            
            // 응답 받기
            using(WebResponse response = request.GetResponse())
            {
                Console.WriteLine("SatausDescrption: " + ((HttpWebResponse)response).StatusDescription);
                using (Stream inStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(inStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        Console.WriteLine(responseFromServer);
                    }
                }
            }
        }
    }
}
