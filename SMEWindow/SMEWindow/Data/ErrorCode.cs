using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Data
{
    // HResult
    // S (1 bit): Severity. If set, indicates a failure result. If clear, indicates a success result.
    // R (1 bit): Reserved. If the N bit is clear, this bit MUST be set to 0. If the N bit is set, this bit is defined by the NTSTATUS numbering space (as specified in section 2.3).
    // C (1 bit): Customer. This bit specifies if the value is customer-defined or Microsoft-defined. The bit is set for customer-defined values and clear for Microsoft-defined values.<1>
    // N (1 bit): If set, indicates that the error code is an NTSTATUS value (as specified in section 2.3), except that this bit is set.
    // X (1 bit):  Reserved.  SHOULD be set to 0. <2>
    // Facility (11 bits): An indicator of the source of the error. New facilities are occasionally added by Microsoft.
    // code (16 bits)
    class HResultCode
    {
        internal const int FACILITY_NULL = 0;     
        internal const int FACILITY_RPC = 1;      
        internal const int FACILITY_DISPATCH = 2; 
        internal const int FACILITY_STORAGE = 3;  
        internal const int FACILITY_ITF = 4;      
        internal const int FACILITY_WIN32 = 7;    
        internal const int FACILITY_WINDOWS = 8;  
        internal const int FACILITY_SECURITY = 9; 
        internal const int FACILITY_SSPI = 9;     
        internal const int FACILITY_CONTROL = 10; 
        internal const int FACILITY_CERT = 11;    
        internal const int FACILITY_INTERNET = 12;
        internal const int FACILITY_MEDIASERVER = 13;
        internal const int FACILITY_MSMQ = 14;    
        internal const int FACILITY_SETUPAPI = 15;  
        internal const int FACILITY_SCARD = 16;
        internal const int FACILITY_COMPLUS = 17;
        internal const int FACILITY_AAF = 18;
        internal const int FACILITY_URT = 19;
        internal const int FACILITY_ACS = 20;
        internal const int FACILITY_DPLAY = 21;
        internal const int FACILITY_UMI = 22;
        internal const int FACILITY_SXS = 23;
        internal const int FACILITY_WINDOWS_CE = 24;
        internal const int FACILITY_HTTP = 25;
        internal const int FACILITY_USERMODE_COMMONLOG = 26;
        internal const int FACILITY_USERMODE_FILTER_MANAGER = 31;
        internal const int FACILITY_BACKGROUNDCOPY = 32;
        internal const int FACILITY_CONFIGURATION = 33;
        internal const int FACILITY_STATE_MANAGEMENT = 34;
        internal const int FACILITY_METADIRECTORY = 35;
        internal const int FACILITY_WINDOWSUPDATE = 36;
        internal const int FACILITY_DIRECTORYSERVICE = 37;
        internal const int FACILITY_GRAPHICS = 38;
        internal const int FACILITY_SHELL = 39;
        internal const int FACILITY_TPM_SERVICES = 40;
        internal const int FACILITY_TPM_SOFTWARE = 41;
        internal const int FACILITY_PLA = 48;
        internal const int FACILITY_FVE = 49;
        internal const int FACILITY_FWP = 50;
        internal const int FACILITY_WINRM = 51;
        internal const int FACILITY_NDIS = 52;
        internal const int FACILITY_USERMODE_HYPERVISOR = 53;
        internal const int FACILITY_CMI = 54;
        internal const int FACILITY_USERMODE_VIRTUALIZATION = 55;
        internal const int FACILITY_USERMODE_VOLMGR = 56;
        internal const int FACILITY_BCD = 57;
        internal const int FACILITY_USERMODE_VHD = 58;
        internal const int FACILITY_SDIAG = 60;
        internal const int FACILITY_WEBSERVICES = 61;
        internal const int FACILITY_WINDOWS_DEFENDER = 80;
        internal const int FACILITY_OPC = 81;

    }

    class HResultValue
    {
        public const int CO_E_ACCESSCHECKFAILED = -2147417814;
        public const int CO_E_ACESINWRONGORDER = -2147417798;
        public const int CO_E_ACNOTINITIALIZED = -2147417793;
        public const int CO_E_CANCEL_DISABLED = -2147417792;
        public const int CO_E_CONVERSIONFAILED = -2147417810;
        public const int CO_E_DECODEFAILED = -2147417795;
        public const int CO_E_EXCEEDSYSACLLIMIT = -2147417799;
        public const int CO_E_FAILEDTOCLOSEHANDLE = -2147417800;
        public const int CO_E_FAILEDTOCREATEFILE = -2147417801;
        public const int CO_E_FAILEDTOGENUUID = -2147417802;
        public const int CO_E_FAILEDTOGETSECCTX = -2147417820;
        public const int CO_E_FAILEDTOGETTOKENINFO = -2147417818;
        public const int CO_E_FAILEDTOGETWINDIR = -2147417804;
        public const int CO_E_FAILEDTOIMPERSONATE = -2147417821;
        public const int CO_E_FAILEDTOOPENPROCESSTOKEN = -2147417796;
        public const int CO_E_FAILEDTOOPENTHREADTOKEN = -2147417819;
        public const int CO_E_FAILEDTOQUERYCLIENTBLANKET = -2147417816;
        public const int CO_E_FAILEDTOSETDACL = -2147417815;
        public const int CO_E_INCOMPATIBLESTREAMVERSION = -2147417797;
        public const int CO_E_INVALIDSID = -2147417811;
        public const int CO_E_LOOKUPACCNAMEFAILED = -2147417806;
        public const int CO_E_LOOKUPACCSIDFAILED = -2147417808;
        public const int CO_E_NETACCESSAPIFAILED = -2147417813;
        public const int CO_E_NOMATCHINGNAMEFOUND = -2147417807;
        public const int CO_E_NOMATCHINGSIDFOUND = -2147417809;
        public const int CO_E_PATHTOOLONG = -2147417803;
        public const int CO_E_SETSERLHNDLFAILED = -2147417805;
        public const int CO_E_TRUSTEEDOESNTMATCHCLIENT = -2147417817;
        public const int CO_E_WRONGTRUSTEENAMESYNTAX = -2147417812;
        public const int DISP_E_ARRAYISLOCKED = -2147352563;
        public const int DISP_E_BADCALLEE = -2147352560;
        public const int DISP_E_BADINDEX = -2147352565;
        public const int DISP_E_BADPARAMCOUNT = -2147352562;
        public const int DISP_E_BADVARTYPE = -2147352568;
        public const int DISP_E_BUFFERTOOSMALL = -2147352557;
        public const int DISP_E_DIVBYZERO = -2147352558;
        public const int DISP_E_EXCEPTION = -2147352567;
        public const int DISP_E_MEMBERNOTFOUND = -2147352573;
        public const int DISP_E_NONAMEDARGS = -2147352569;
        public const int DISP_E_NOTACOLLECTION = -2147352559;
        public const int DISP_E_OVERFLOW = -2147352566;
        public const int DISP_E_PARAMNOTFOUND = -2147352572;
        public const int DISP_E_PARAMNOTOPTIONAL = -2147352561;
        public const int DISP_E_TYPEMISMATCH = -2147352571;
        public const int DISP_E_UNKNOWNINTERFACE = -2147352575;
        public const int DISP_E_UNKNOWNLCID = -2147352564;
        public const int DISP_E_UNKNOWNNAME = -2147352570;
        public const int E_ABORT = -2147467260;
        public const int E_ACCESSDENIED = -2147024891;
        public const int E_FAIL = -2147467259;
        public const int E_HANDLE = -2147024890;
        public const int E_INVALIDARG = -2147024809;
        public const int E_NOINTERFACE = -2147467262;
        public const int E_NOTIMPL = -2147467263;
        public const int E_OUTOFMEMORY = -2147024882;
        public const int E_PENDING = -2147483638;
        public const int E_POINTER = -2147467261;
        public const int E_UNEXPECTED = -2147418113;
        public const int OLE_E_ADVF = -2147221503;
        public const int OLE_E_ADVISENOTSUPPORTED = -2147221501;
        public const int OLE_E_BLANK = -2147221497;
        public const int OLE_E_CANT_BINDTOSOURCE = -2147221494;
        public const int OLE_E_CANT_GETMONIKER = -2147221495;
        public const int OLE_E_CANTCONVERT = -2147221487;
        public const int OLE_E_CLASSDIFF = -2147221496;
        public const int OLE_E_ENUM_NOMORE = -2147221502;
        public const int OLE_E_INVALIDHWND = -2147221489;
        public const int OLE_E_INVALIDRECT = -2147221491;
        public const int OLE_E_NOCACHE = -2147221498;
        public const int OLE_E_NOCONNECTION = -2147221500;
        public const int OLE_E_NOSTORAGE = -2147221486;
        public const int OLE_E_NOT_INPLACEACTIVE = -2147221488;
        public const int OLE_E_NOTRUNNING = -2147221499;
        public const int OLE_E_OLEVERB = -2147221504;
        public const int OLE_E_PROMPTSAVECANCELLED = -2147221492;
        public const int OLE_E_STATIC = -2147221493;
        public const int OLE_E_WRONGCOMPOBJ = -2147221490;
        public const int RPC_E_ACCESS_DENIED = -2147417829;
        public const int RPC_E_ATTEMPTED_MULTITHREAD = -2147417854;
        public const int RPC_E_CALL_CANCELED = -2147418110;
        public const int RPC_E_CALL_COMPLETE = -2147417833;
        public const int RPC_E_CALL_REJECTED = -2147418111;
        public const int RPC_E_CANTCALLOUT_AGAIN = -2147418095;
        public const int RPC_E_CANTCALLOUT_INASYNCCALL = -2147418108;
        public const int RPC_E_CANTCALLOUT_INEXTERNALCALL = -2147418107;
        public const int RPC_E_CANTCALLOUT_ININPUTSYNCCALL = -2147417843;
        public const int RPC_E_CANTPOST_INSENDCALL = -2147418109;
        public const int RPC_E_CANTTRANSMIT_CALL = -2147418102;
        public const int RPC_E_CHANGED_MODE = -2147417850;
        public const int RPC_E_CLIENT_CANTMARSHAL_DATA = -2147418101;
        public const int RPC_E_CLIENT_CANTUNMARSHAL_DATA = -2147418100;
        public const int RPC_E_CLIENT_DIED = -2147418104;
        public const int RPC_E_CONNECTION_TERMINATED = -2147418106;
        public const int RPC_E_DISCONNECTED = -2147417848;
        public const int RPC_E_FAULT = -2147417852;
        public const int RPC_E_FULLSIC_REQUIRED = -2147417823;
        public const int RPC_E_INVALID_CALLDATA = -2147417844;
        public const int RPC_E_INVALID_DATA = -2147418097;
        public const int RPC_E_INVALID_DATAPACKET = -2147418103;
        public const int RPC_E_INVALID_EXTENSION = -2147417838;
        public const int RPC_E_INVALID_HEADER = -2147417839;
        public const int RPC_E_INVALID_IPID = -2147417837;
        public const int RPC_E_INVALID_OBJECT = -2147417836;
        public const int RPC_E_INVALID_OBJREF = -2147417827;
        public const int RPC_E_INVALID_PARAMETER = -2147418096;
        public const int RPC_E_INVALID_STD_NAME = -2147417822;
        public const int RPC_E_INVALIDMETHOD = -2147417849;
        public const int RPC_E_NO_CONTEXT = -2147417826;
        public const int RPC_E_NO_GOOD_SECURITY_PACKAGES = -2147417830;
        public const int RPC_E_NO_SYNC = -2147417824;
        public const int RPC_E_NOT_REGISTERED = -2147417853;
        public const int RPC_E_OUT_OF_RESOURCES = -2147417855;
        public const int RPC_E_REMOTE_DISABLED = -2147417828;
        public const int RPC_E_RETRY = -2147417847;
        public const int RPC_E_SERVER_CANTMARSHAL_DATA = -2147418099;
        public const int RPC_E_SERVER_CANTUNMARSHAL_DATA = -2147418098;
        public const int RPC_E_SERVER_DIED = -2147418105;
        public const int RPC_E_SERVER_DIED_DNE = -2147418094;
        public const int RPC_E_SERVERCALL_REJECTED = -2147417845;
        public const int RPC_E_SERVERCALL_RETRYLATER = -2147417846;
        public const int RPC_E_SERVERFAULT = -2147417851;
        public const int RPC_E_SYS_CALL_FAILED = -2147417856;
        public const int RPC_E_THREAD_NOT_INIT = -2147417841;
        public const int RPC_E_TIMEOUT = -2147417825;
        public const int RPC_E_TOO_LATE = -2147417831;
        public const int RPC_E_UNEXPECTED = -2147352577;
        public const int RPC_E_UNSECURE_CALL = -2147417832;
        public const int RPC_E_VERSION_MISMATCH = -2147417840;
        public const int RPC_E_WRONG_THREAD = -2147417842;
        public const int RPC_S_CALLPENDING = -2147417835;
        public const int RPC_S_WAITONTIMER = -2147417834;
        public const int UNDO_E_CLIENTABORT = -2147205119;
        public const int VS_E_BUSY = -2147220992;
        public const int VS_E_CIRCULARTASKDEPENDENCY = -2147213305;
        public const int VS_E_INCOMPATIBLECLASSICPROJECT = -2147213308;
        public const int VS_E_INCOMPATIBLEDOCDATA = -2147213334;
        public const int VS_E_INCOMPATIBLEPROJECT = -2147213309;
        public const int VS_E_INCOMPATIBLEPROJECT_UNSUPPORTED_OS = -2147213307;
        public const int VS_E_PACKAGENOTLOADED = -2147213343;
        public const int VS_E_PROJECTALREADYEXISTS = -2147213344;
        public const int VS_E_PROJECTMIGRATIONFAILED = -2147213339;
        public const int VS_E_PROJECTNOTLOADED = -2147213342;
        public const int VS_E_PROMPTREQUIRED = -2147213306;
        public const int VS_E_SOLUTIONALREADYOPEN = -2147213340;
        public const int VS_E_SOLUTIONNOTOPEN = -2147213341;
        public const int VS_E_SPECIFYING_OUTPUT_UNSUPPORTED = -2147220991;
        public const int VS_E_UNSUPPORTEDFORMAT = -2147213333;
        public const int VS_E_WIZARDBACKBUTTONPRESS = -2147213313;
    }
    class Win32ErrorCode
    {
        internal const int FACILITY_WIN32 = 0x0007;
        
    }
}
