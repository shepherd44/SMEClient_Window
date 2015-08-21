using System;
using System.Xml.Linq;

namespace SME.SMECollect.Data
{
    //현재 system의 정보 수집
    [Serializable]
    public class SMESystemInformation
    {
        #region Members
        public PlatformID PlatformID { get; protected internal set; }
        public string ServicePack { get; protected internal set; }
        public Version OSVersion { get; protected internal set; }
        public Version CLRVersion { get; protected internal set; }
        public bool Is64bitOS { get; protected internal set; }
        public bool Is64bitProcess { get; protected internal set; }
        #endregion

        #region Constructor
        public SMESystemInformation()
        {
            OperatingSystem os = Environment.OSVersion;

            PlatformID = os.Platform;
            OSVersion = os.Version;
            CLRVersion = Environment.Version;
            ServicePack = os.ServicePack;
            Is64bitOS = Environment.Is64BitOperatingSystem;
            Is64bitProcess = Environment.Is64BitProcess;
        }

        public SMESystemInformation(XElement xelement)
        {
            LoadFromXElement(xelement);
        }
        #endregion

        #region Functions
        public XElement ToXElement() 
        {
            XElement xmldoc = new XElement("SystemInformation",
                                new XElement("PlatformID", PlatformID.ToString()),
                                new XElement("ServicePack", ServicePack),
                                new XElement("OSVersion", OSVersion.ToString()),
                                new XElement("CLRVersion", CLRVersion.ToString()),
                                new XElement("Is64BitOS", Is64bitOS.ToString()),
                                new XElement("Is64BitProcess", Is64bitProcess.ToString()));
            return xmldoc;
        }

        public void LoadFromXElement(XElement xelement)
        {
            if (xelement.Name.ToString().Equals("SystemInformation"))
            {
                XElement xe = (XElement)xelement.FirstNode;
                if (PlatformID.Win32NT.ToString().Equals(xe.Value))
                    PlatformID = PlatformID.Win32NT;
                else if(PlatformID.Win32S.ToString().Equals(xe.Value))
                    PlatformID = PlatformID.Win32S;
                else if (PlatformID.Win32Windows.ToString().Equals(xe.Value))
                    PlatformID = PlatformID.Win32Windows;
                else if (PlatformID.WinCE.ToString().Equals(xe.Value))
                    PlatformID = PlatformID.WinCE;
                else if (PlatformID.Xbox.ToString().Equals(xe.Value))
                    PlatformID = PlatformID.Xbox;
                else
                    PlatformID = 0;
                
                xe = (XElement)xe.NextNode;
                ServicePack = xe.Value;
                xe = (XElement)xe.NextNode;
                OSVersion = new Version(xe.Value);
                xe = (XElement)xe.NextNode;
                CLRVersion = new Version(xe.Value);
                xe = (XElement)xe.NextNode;
                Is64bitOS = bool.Parse(xe.Value);
                xe = (XElement)xe.NextNode;
                Is64bitProcess = bool.Parse(xe.Value);
            }
            else
                throw new Exception("This XElement is not SystemInformation XElement.");
            
        }

        override public string ToString()
        {
            string temp = "System Information";
            temp += ":PlatformID:" + PlatformID.ToString();
            temp += ":ServicePack:" + ServicePack;
            temp += ":OSVersion:" + OSVersion.ToString();
            temp += ":CLRVersion:" + CLRVersion.ToString();
            temp += ":Is64bitOS:" + Is64bitOS.ToString();
            temp += ":Is64bitProcess:" + Is64bitProcess.ToString();
            return temp;
        }
        #endregion
    }
}
