using DumpReader.Native;
using System;

namespace DumpReader.MinidumpStream
{
    [Serializable]
    public class MiniDumpLocationDescriptor
    {
        private MINIDUMP_LOCATION_DESCRIPTOR m_locationDescriptor;

        internal MiniDumpLocationDescriptor(MINIDUMP_LOCATION_DESCRIPTOR locationDescriptor)
        {
            m_locationDescriptor = locationDescriptor;
        }
        public uint DataSize { get { return m_locationDescriptor.DataSize; } }
        public string DataSizeFormatted { get { return String.Concat("0x", this.DataSize.ToString("X8")); } }
        public string DataSizePretty
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = this.DataSize;
                int order = 0;
                while(len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len / 1024;
                }
                return String.Format("{0:0.#} {1}", len, sizes[order]);
            }
        }
        public uint Rva { get { return m_locationDescriptor.Rva; } }
    }
}
