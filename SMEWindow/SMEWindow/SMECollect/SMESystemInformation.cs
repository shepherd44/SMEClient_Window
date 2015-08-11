using System;
using System.Xml.Linq;

namespace SME.SMECollect
{
    //현재 system의 정보 수집
    public class SMESystemInformation
    {
        #region Members
        PlatformID m_platformID;
        string m_servicePack;
        Version m_OSVersion;
        Version m_CLRVersion;
        bool m_Is64bitOS;
        bool m_Is64bitProcess;
        #endregion

        #region Constructor
        public SMESystemInformation()
        {
            OperatingSystem os = Environment.OSVersion;

            m_platformID = os.Platform;
            m_OSVersion = os.Version;
            m_CLRVersion = Environment.Version;
            m_servicePack = os.ServicePack;
            m_Is64bitOS = Environment.Is64BitOperatingSystem;
            m_Is64bitProcess = Environment.Is64BitProcess;
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
                                new XElement("PlatformID", m_platformID.ToString()),
                                new XElement("ServicePack", m_servicePack),
                                new XElement("OSVersion", m_OSVersion.ToString()),
                                new XElement("CLRVersion", m_CLRVersion.ToString()),
                                new XElement("Is64BitOS", m_Is64bitOS.ToString()),
                                new XElement("Is64BitProcess", m_Is64bitProcess.ToString()));
            return xmldoc;
        }

        public void LoadFromXElement(XElement xelement)
        {
            if (xelement.Name.ToString().Equals("SystemInformation"))
            {
                XElement xe = (XElement)xelement.FirstNode;
                if (PlatformID.Win32NT.ToString().Equals(xe.Value))
                    m_platformID = PlatformID.Win32NT;
                else if(PlatformID.Win32S.ToString().Equals(xe.Value))
                    m_platformID = PlatformID.Win32S;
                else if (PlatformID.Win32Windows.ToString().Equals(xe.Value))
                    m_platformID = PlatformID.Win32Windows;
                else if (PlatformID.WinCE.ToString().Equals(xe.Value))
                    m_platformID = PlatformID.WinCE;
                else if (PlatformID.Xbox.ToString().Equals(xe.Value))
                    m_platformID = PlatformID.Xbox;
                else
                    m_platformID = 0;
                
                xe = (XElement)xe.NextNode;
                m_servicePack = xe.Value;
                xe = (XElement)xe.NextNode;
                m_OSVersion = new Version(xe.Value);
                xe = (XElement)xe.NextNode;
                m_CLRVersion = new Version(xe.Value);
                xe = (XElement)xe.NextNode;
                m_Is64bitOS = bool.Parse(xe.Value);
                xe = (XElement)xe.NextNode;
                m_Is64bitProcess = bool.Parse(xe.Value);
            }
            else
                throw new Exception("This XElement is not SystemInformation XElement.");
            
        }

        override public string ToString()
        {
            string temp = "System Information";
            temp += ":PlatformID:" + m_platformID.ToString();
            temp += ":ServicePack:" + m_servicePack;
            temp += ":OSVersion:" + m_OSVersion.ToString();
            temp += ":CLRVersion:" + m_CLRVersion.ToString();
            temp += ":Is64bitOS:" + m_Is64bitOS.ToString();
            temp += ":Is64bitProcess:" + m_Is64bitProcess.ToString();
            return temp;
        }
        #endregion
    }
}
