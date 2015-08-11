using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;
using System.IO.MemoryMappedFiles; // MemoryMap을 하기 위해 필요한 Namespace
using System.Runtime.InteropServices;//Marshaling을 위해 필요한 Namespace
using Microsoft.Win32.SafeHandles; //Windows의 SafeHandle을 사용하기 위해 필요한 Namespace


namespace SME.SMEDumpAnalyze
{
    using Native;
    using MinidumpStream;

    public class SMEMiniDumpReader
    {
        private MemoryMappedFile m_minidumpMappedFile;
        private SafeMemoryMappedViewHandle m_mappedFileView;

        //SMEMinidumpReader 생성자에서 파일 Mapping을 한다.
        public SMEMiniDumpReader(string filepath)
        {

            MemoryMappedFile minidumpMappedFile = MemoryMappedFile.CreateFromFile(filepath, System.IO.FileMode.Open);
            SafeMemoryMappedViewHandle mappedFileView = NativeMethods.MapViewOfFile(minidumpMappedFile.SafeMemoryMappedFileHandle, NativeMethods.FILE_MAP_READ, 0, 0, IntPtr.Zero);
            
            // SafeMemory에 ViewHandle이 Mapping 안되면 에러발생
            if (mappedFileView.IsInvalid)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            // SafeMemoryMappedViewHandle 초기화 루틴, 사용을 위해 반드시 해줘야 한다.
            MEMORY_BASIC_INFORMATION memoryInformation = default(MEMORY_BASIC_INFORMATION);
            if (NativeMethods.VirtualQuery(mappedFileView, ref memoryInformation, (IntPtr)Marshal.SizeOf(memoryInformation)) == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            mappedFileView.Initialize((ulong)memoryInformation.RegionSize);

            // 전역 field member에 저장
            this.m_minidumpMappedFile = minidumpMappedFile;
            this.m_mappedFileView = mappedFileView;

        }
        
        //1. ExceptionStream을 읽어온다.
        public MiniDumpExceptionStream ReadExceptionStream()
        {
            MINIDUMP_EXCEPTION_STREAM exceptionStream;
            IntPtr streamPointer;
            uint streamSize;
            if(!this.ReadStream<MINIDUMP_EXCEPTION_STREAM>(MINIDUMP_STREAM_TYPE.ExceptionStream, out exceptionStream, out streamPointer, out streamSize))
            {
                return null;
            }

            return new MiniDumpExceptionStream(exceptionStream);
        }

        //2. MiscInfo를 읽어온다.
        public MiniDumpMiscInfo ReadMiscInfo()
        {
            
            uint SIZEOF_INFO_1 = 24;
            uint SIZEOF_INFO_2 = 44;
            uint SIZEOF_INFO_3 = 232;
            uint SIZEOF_INFO_4 = 832;

            MINIDUMP_MISC_INFO miscInfo;
            MINIDUMP_MISC_INFO_2 miscInfo2;
            MINIDUMP_MISC_INFO_3 miscInfo3;
            MINIDUMP_MISC_INFO_4 miscInfo4;

            IntPtr streamPointer;
            uint streamSize;

            if (!this.ReadStream<MINIDUMP_MISC_INFO>(MINIDUMP_STREAM_TYPE.MiscInfoStream, out miscInfo, out streamPointer, out streamSize))
            {
                return null;
            }

            MiniDumpMiscInfo retVal;

            if (miscInfo.SizeOfInfo == SIZEOF_INFO_1)
            {
                retVal = new MiniDumpMiscInfo(miscInfo);
            }
            else if (miscInfo.SizeOfInfo == SIZEOF_INFO_2)
            {
                miscInfo2 = (MINIDUMP_MISC_INFO_2)Marshal.PtrToStructure(streamPointer, typeof(MINIDUMP_MISC_INFO_2));

                retVal = new MiniDumpMiscInfo2(miscInfo2);
            }
            else if (miscInfo.SizeOfInfo == SIZEOF_INFO_3)
            {
                miscInfo3 = (MINIDUMP_MISC_INFO_3)Marshal.PtrToStructure(streamPointer, typeof(MINIDUMP_MISC_INFO_3));

                retVal = new MiniDumpMiscInfo3(miscInfo3);
            }
            else if (miscInfo.SizeOfInfo == SIZEOF_INFO_4)
            {
                miscInfo4 = (MINIDUMP_MISC_INFO_4)Marshal.PtrToStructure(streamPointer, typeof(MINIDUMP_MISC_INFO_4));

                retVal = new MiniDumpMiscInfo4(miscInfo4);
            }
            else
            {
                throw new InvalidOperationException("Data returned from reading MiscInfoStream has an unrecognised SizeOfInfo field: " + miscInfo.SizeOfInfo + " bytes");
            }

            return retVal;
        }
        
        //3. MouduleList를 읽어온다.
        public MiniDumpModule[] ReadModuleList()
        {
            //module들의 갯수와 주소정보를 가지고있는 구조체
            MINIDUMP_MODULE_LIST moduleList;
            IntPtr streamPointer;
            uint streamSize;
            if(!this.ReadStream<MINIDUMP_MODULE_LIST>(MINIDUMP_STREAM_TYPE.ModuleListStream, out moduleList, out streamPointer, out streamSize))
            {
                return new MiniDumpModule[0];
            }

            //4 == NumberOfModules field를 건너뛴다.
            MINIDUMP_MODULE[] modules = ReadArray<MINIDUMP_MODULE>(streamPointer + 4, (int)moduleList.NumberOfModules);

            //MiniDumpModule 객체 배열을 반환한다.
            List<MiniDumpModule> returnList = new List<MiniDumpModule>(modules.Select(x => new MiniDumpModule(x, this)));

            return returnList.ToArray();
        }

        //4. MDRawBreakpadInfo를 읽어온다.
        public MiniDumpBreakpadInfo ReadBreakpadInfo()
        {
            MDRawBreakpadInfo breakpadInfoStream;
            IntPtr streamPointer;
            uint streamSize;
            if(!this.ReadStream<MDRawBreakpadInfo>(MINIDUMP_STREAM_TYPE.MD_BREAKPAD_INFO_STREAM,out breakpadInfoStream, out streamPointer, out streamSize))
            {
                return null;
            }
            return new MiniDumpBreakpadInfo(breakpadInfoStream);
        }

        //5. MDRawAssertionInfo를 읽어온다.
        public MiniDumpAssertionInfo ReadAssertionInfo()
        {
            MDRawAssertionInfo assertionInfoStream;
            IntPtr streamPointer;
            uint streamSize;
            if(!this.ReadStream<MDRawAssertionInfo>(MINIDUMP_STREAM_TYPE.MD_ASSERTION_INFO_STREAM,out assertionInfoStream, out streamPointer, out streamSize))
            {
                return null;
            }
            return new MiniDumpAssertionInfo(assertionInfoStream);
        }

        //명시된 offset(RVA)에서 MINIDUMP_STRING을 읽어온다.
        protected internal unsafe string ReadString(uint rva)
        {
            try
            {
                byte* baseOfView = null;
                m_mappedFileView.AcquirePointer(ref baseOfView);
                IntPtr positionToReadFrom = new IntPtr(baseOfView + rva);

                //처음 32bits는 char의 갯수가 아닌 바이트 갯수를 나타내는 Length field이다. 따라서 2로 나눠준다.
                int len = Marshal.ReadInt32(positionToReadFrom) / 2;

                //Length field를 뛰어넘기 위해 4바이트를 더해준다.
                positionToReadFrom += 4;

                //positionToreadFrom에서부터 len만큼 데이터를 읽어 string을 리턴한다. 
                return Marshal.PtrToStringUni(positionToReadFrom, len);
            }
            finally
            {
                m_mappedFileView.ReleasePointer();
            }
        }

        //시간을 DateTime으로 변환
        public static DateTime TimeTToDateTime(UInt32 time_t)
        {
            // 10 000 000 * january 1st 1970
            long win32FileTime = 10000000 * (long)time_t + 116444736000000000;

            return DateTime.FromFileTime(win32FileTime); // FromFileTimeUtc is the UCT time, FromFileTime adjusts for the local timezone
        }

        //템플릿을 이용하여 streamToRead 타입에 따라 덤프에서 streamPointer를 읽어온다.
        protected unsafe bool ReadStream<T>(MINIDUMP_STREAM_TYPE streamToRead, out T streamData, out IntPtr streamPointer, out uint streamSize)
        {
            MINIDUMP_DIRECTORY directory = new MINIDUMP_DIRECTORY();
            streamData = default(T);
            streamPointer = IntPtr.Zero;
            streamSize = 0;

            try
            {
                byte* baseOfView = null;
                
                //SafeBuffer 개체에서 메모리 블록에 대한 포인터를 가져온다.
                m_mappedFileView.AcquirePointer(ref baseOfView);

                if (baseOfView == null)
                    throw new Exception("Unable to acquire pointer to memory mapped view");
                if(!NativeMethods.MiniDumpReadDumpStream((IntPtr)baseOfView, streamToRead, ref directory, ref streamPointer, ref streamSize))
                {
                    int lastError = Marshal.GetLastWin32Error();

                    if (lastError == DbgHelpErrors.ERR_ELEMENT_NOT_FOUND)
                    {
                        return false;
                    }
                    else
                        throw new Win32Exception(lastError);
                }
                //관리되지 않는 메모리 블록의 데이터를 관리되는 개체로 마샬링한다.
                streamData = (T)Marshal.PtrToStructure(streamPointer, typeof(T));
            }

            finally
            {
                m_mappedFileView.ReleasePointer();
            }

            return true;
        }

        //템플릿을 이용하여 Array형식의 Stream을 읽어온다. 
        protected internal unsafe T[] ReadArray<T>(IntPtr absoluteStreamReadAddress, int count) where T: struct
        {
            T[] readItems = new T[count];
            try
            {
                byte* baseOfView = null;
                m_mappedFileView.AcquirePointer(ref baseOfView);
                ulong offset = (ulong)absoluteStreamReadAddress - (ulong)baseOfView;
                //메모리의 오프셋 시작위치에서 count 갯수만큼 T 형식을 읽어서 이를 readItems 배열의 인덱스 시작 위치에 쓴다.
                m_mappedFileView.ReadArray<T>(offset, readItems, 0, count);
            }
            finally
            {
                m_mappedFileView.ReleasePointer();
            }
            return readItems;
        }
    }
 
}
