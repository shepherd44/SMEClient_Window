using System;
using System.Collections;
using System.Xml.Linq;

namespace SME.SMEXML
{
    public class XMLHelper
    {
        public static XElement FindElement(XElement parent, string child)
        {
            IEnumerable childnodes = parent.Nodes();
            foreach (XElement item in childnodes)
            {
                if (item.Name.ToString().Equals(child))
                    break;
            }
            return null;
        }
        public static void PrintChildElement(XElement parent)
        {
            foreach (XElement item in parent.Nodes())
            {
                Console.WriteLine(item.Name);
            }
        }
        public static void PrintAllElement(XElement root)
        {
        }
    }
    
}