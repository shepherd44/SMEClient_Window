using SME.SMEDumpAnalyze.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SMEDumpAnalyze.MinidumpStream
{
    public class MiniDumpModule
    {
        private MINIDUMP_MODULE m_module;
        private SMEMiniDumpReader m_owner;

        internal MiniDumpModule(MINIDUMP_MODULE module, SMEMiniDumpReader owner)
        {
            m_owner = owner;
            m_module = module;
        }

        public string FileName
        {
            get { return System.IO.Path.GetFileName(this.PathAndFileName); }
        }

        public string DirectoryName
        {
            get { return System.IO.Path.GetDirectoryName(this.PathAndFileName); }
        }

        public string PathAndFileName
        {
            get { return m_owner.ReadString(m_module.ModuleNameRva); }
        }

        public ulong BaseOfImage
        {
            get { return m_module.BaseOfImage; }
        }

        public string BaseOfImageFormatted
        {
            get { return String.Concat("0x", m_module.BaseOfImage.ToString("X8")); }
        }

        public int SizeOfImage
        {
            get { return (int)m_module.SizeOfImage; }
        }

        public string SizeOfImageFormatted
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = this.SizeOfImage;
                int order = 0;
                while (len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len / 1024;
                }

                return String.Format("{0:0.#} {1}", len, sizes[order]);
            }
        }

        public int CheckSum
        {
            get { return (int)m_module.CheckSum; }
        }

        public DateTime TimeDateStamp
        {
            get { return SMEMiniDumpReader.TimeTToDateTime(m_module.TimeDateStamp); }
        }

        public uint TimeDateStampRaw { get { return m_module.TimeDateStamp; } }

        private UInt32 HiWord(UInt32 number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            else
                return (number >> 16) & 0xffff;
        }

        private UInt32 LoWord(UInt32 number)
        {
            return number & 0xffff;
        }

        public string FileVersion
        {
            get
            {
                return String.Format("{0}.{1}.{2}.{3}",
                    HiWord(m_module.VersionInfo.dwFileVersionMS),
                    LoWord(m_module.VersionInfo.dwFileVersionMS),
                    HiWord(m_module.VersionInfo.dwFileVersionLS),
                    LoWord(m_module.VersionInfo.dwFileVersionLS));
            }
        }

        public string ProductVersion
        {
            get
            {
                return String.Format("{0}.{1}.{2}.{3}",
                    HiWord(m_module.VersionInfo.dwProductVersionMS),
                    LoWord(m_module.VersionInfo.dwProductVersionMS),
                    HiWord(m_module.VersionInfo.dwProductVersionLS),
                    LoWord(m_module.VersionInfo.dwProductVersionLS));
            }
        }

        public bool IsDebug
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_DEBUG) > 0;
            }
        }

        public bool IsInfoInferred
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_INFOINFERRED) > 0;
            }
        }

        public bool IsPatched
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_PATCHED) > 0;
            }
        }

        public bool IsPreRelease
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_PRERELEASE) > 0;
            }
        }

        public bool IsPrivateBuild
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_PRIVATEBUILD) > 0;
            }
        }

        public bool IsSpecialBuild
        {
            get
            {
                return (m_module.VersionInfo.dwFileFlags & windows.VS_FF_SPECIALBUILD) > 0;
            }
        }

        public uint FileOS { get { return m_module.VersionInfo.dwFileOS; } }
        
        public string FileOSFormatted
        {
            get
            {
                // Technically these values can be combined
                switch (m_module.VersionInfo.dwFileOS)
                {
                    case windows.VOS_DOS: return "MS-DOS";
                    case windows.VOS_NT: return "Windows NT";
                    case windows.VOS__WINDOWS16: return "16-bit Windows";
                    case windows.VOS__WINDOWS32: return "32-bit Windows";
                    case windows.VOS_OS216: return "16-bit OS/2";
                    case windows.VOS_OS232: return "32-bit OS/2";
                    case windows.VOS__PM16: return "16-bit Presentation Manager";
                    case windows.VOS__PM32: return "32-bit Presentation Manager";

                    case windows.VOS_DOS_WINDOWS16: return "16-bit Windows running on MS-DOS";
                    case windows.VOS_DOS_WINDOWS32: return "32-bit Windows running on MS-DOS";
                    case windows.VOS_NT_WINDOWS32: return "Windows NT";
                    case windows.VOS_OS216_PM16: return "16-bit Presentation Manager running on 16-bit OS/2";
                    case windows.VOS_OS232_PM32: return "32-bit Presentation Manager running on 32-bit OS/2";

                    case windows.VOS_UNKNOWN: return "Unknown";
                    default:
                        throw new NotSupportedException("Unknown dwFileOS: '" + m_module.VersionInfo.dwFileOS + "'.");
                }
            }
        }

       
        public uint FileType { get { return m_module.VersionInfo.dwFileType; } }

        public string FileTypeFormatted
        {
            get
            {
                switch (m_module.VersionInfo.dwFileType)
                {
                    case windows.VFT_APP: return "Application";
                    case windows.VFT_DLL: return "DLL";
                    case windows.VFT_DRV: return "Device driver";
                    case windows.VFT_FONT: return "Font";
                    case windows.VFT_STATIC_LIB: return "Static-link library";
                    case windows.VFT_UNKNOWN: return "Unknown";
                    case windows.VFT_VXD: return "Virtual device";
                    default:
                        throw new NotSupportedException("Unknown dwFileType: '" + m_module.VersionInfo.dwFileType + "'.");
                }
            }
        }

        public uint FileSubType { get { return m_module.VersionInfo.dwFileSubtype; } }

        public string FileSubTypeFormatted
        {
            get
            {
                if (m_module.VersionInfo.dwFileType == windows.VFT_DRV)
                {
                    switch (m_module.VersionInfo.dwFileSubtype)
                    {
                        case windows.VFT2_DRV_COMM: return "Communications driver";
                        case windows.VFT2_DRV_DISPLAY: return "Display driver";
                        case windows.VFT2_DRV_INSTALLABLE: return "Installable driver";
                        case windows.VFT2_DRV_KEYBOARD: return "Keyboard driver";
                        case windows.VFT2_DRV_LANGUAGE: return "Language driver";
                        case windows.VFT2_DRV_MOUSE: return "Mouse driver";
                        case windows.VFT2_DRV_NETWORK: return "Network driver";
                        case windows.VFT2_DRV_PRINTER: return "Printer driver";
                        case windows.VFT2_DRV_SOUND: return "Sound driver";
                        case windows.VFT2_DRV_SYSTEM: return "System driver";
                        case windows.VFT2_DRV_VERSIONED_PRINTER: return "Versioned printer driver";
                        case windows.VFT2_UNKNOWN: return "unknown ";
                        default:
                            throw new NotSupportedException("Unknown dwFileSubtype: '" + m_module.VersionInfo.dwFileSubtype + "'.");
                    }
                }
                else if (m_module.VersionInfo.dwFileType == windows.VFT_FONT)
                {
                    switch (m_module.VersionInfo.dwFileSubtype)
                    {
                        case windows.VFT2_FONT_RASTER: return "Raster font";
                        case windows.VFT2_FONT_TRUETYPE: return "TrueType font";
                        case windows.VFT2_FONT_VECTOR: return "Vector font";
                        default:
                            throw new NotSupportedException("Lookup failed. Unknown dwFileSubtype: '" + m_module.VersionInfo.dwFileSubtype + "'.");
                    }
                }
                else
                    return "";
            }
        }
    }
}
