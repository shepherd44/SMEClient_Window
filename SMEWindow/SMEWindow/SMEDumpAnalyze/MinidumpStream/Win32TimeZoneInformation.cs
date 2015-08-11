using SME.SMEDumpAnalyze.Native;
using System;

namespace SME.SMEDumpAnalyze.MinidumpStream
{
    public class Win32TimeZoneInformation
    {
        private TIME_ZONE_INFORMATION _timeZoneInformation;
        private DateTime _standardDate;
        private DateTime _daylightDate;

        internal Win32TimeZoneInformation(TIME_ZONE_INFORMATION timeZoneInformation)
        {
            _timeZoneInformation = timeZoneInformation;

            if (timeZoneInformation.StandardDate.wMonth == 0)
            {
                _standardDate = DateTime.MinValue;
            }
            else
            {
                _standardDate = new DateTime(timeZoneInformation.StandardDate.wYear, timeZoneInformation.StandardDate.wMonth, timeZoneInformation.StandardDate.wDay,
                    timeZoneInformation.StandardDate.wHour, timeZoneInformation.StandardDate.wMinute, timeZoneInformation.StandardDate.wSecond, timeZoneInformation.StandardDate.wMilliseconds);
            }

            if (timeZoneInformation.DaylightDate.wMonth == 0)
            {
                _daylightDate = DateTime.MinValue;
            }
            else
            {
                _daylightDate = new DateTime(timeZoneInformation.DaylightDate.wYear, timeZoneInformation.DaylightDate.wMonth, timeZoneInformation.DaylightDate.wDay,
                    timeZoneInformation.DaylightDate.wHour, timeZoneInformation.DaylightDate.wMinute, timeZoneInformation.DaylightDate.wSecond, timeZoneInformation.DaylightDate.wMilliseconds);
            }
        }

        public int Bias { get { return _timeZoneInformation.Bias; } }
        public string StandardName { get { return _timeZoneInformation.StandardName; } }
        public DateTime StandardDate { get { return _standardDate; } }
        public int StandardBias { get { return _timeZoneInformation.StandardBias; } }
        public string DaylightName { get { return _timeZoneInformation.DaylightName; } }
        public DateTime DaylightDate { get { return _daylightDate; } }
        public int DaylightBias { get { return _timeZoneInformation.DaylightBias; } }
    }
}
