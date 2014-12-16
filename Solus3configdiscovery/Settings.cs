using System;
using System.IO;
using System.Management;

namespace Solus3configdiscovery
{
    public class Settings
    {
        private string fmsDir;
        private string simsDir;
        private readonly Proxy proxy;
        private readonly Services services;
        private readonly SimsApi simsApi;

        public Settings()
        {
            services = new Services();
            proxy = new Proxy();
            simsApi = new SimsApi(simsDirectory);
            simsApi.Connect();
        }

        public string GetDateNow
        {
            get { return DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString(); }
        }

        public string GetComputerName
        {
            get { return Environment.MachineName; }
        }

        public string GetUpdateRepository
        {
            get { return @"\\" + GetComputerName + @"\UpdateRepository$"; }
        }

        private string simsDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(simsDir))
                {
                    return simsDir;
                }
                var iniSims = new IniFile(simsIni);
                return simsDir = iniSims.Read("Setup", "SIMSDotNetDirectory");
            }
        }

        private string fmsDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(fmsDir))
                {
                    return fmsDir;
                }
                var iniFms = new IniFile(simsIni);
                return fmsDir = iniFms.Read("Setup", "FinanceDirectory");
            }
        }

        private string simsIni
        {
            get
            {
                var value = Path.Combine(winDir, "sims.ini");
                if (File.Exists(value))
                {
                    return value;
                }
                return "";
            }
        }

        private string winDir
        {
            get { return Environment.GetEnvironmentVariable("windir"); }
        }

        public string GetInstanceName
        {
            get
            {
                var connect = Path.Combine(simsDirectory, "Connect.ini");
                var iniFile = new IniFile(connect);
                var value = iniFile.Read("SIMSConnection", "ServerName");
                try
                {
                    return value.Split('\\')[1];
                }
                catch (Exception)
                {
                }
                return "";
            }
        }

        public string GetDatabaseName
        {
            get
            {
                var connect = Path.Combine(simsDirectory, "Connect.ini");
                var iniFile = new IniFile(connect);
                return iniFile.Read("SIMSConnection", "DatabaseName");
            }
        }

        public string GetFmsInstanceName
        {
            get
            {
                var value = "";
                var connect = Path.Combine(fmsDirectory, "FMSConnect.ini");
                if (File.Exists(connect))
                {
                    var iniFile = new IniFile(connect);
                    value = iniFile.Read("FMSConnection", "Server");
                    try
                    {
                        return value.Split('\\')[1];
                    }
                    catch (Exception)
                    {
                    }
                }
                return value;
            }
        }

        public string GetFmsDatabaseName
        {
            get
            {
                var value = "";
                var connect = Path.Combine(fmsDirectory, "FMSConnect.ini");
                if (File.Exists(connect))
                {
                    var iniFile = new IniFile(connect);
                    value = iniFile.Read("FMSConnection", "Database");
                }
                return value;
            }
        }

        public string GetSimsShare
        {
            get
            {
                var iniFile = new IniFile(simsIni);
                return iniFile.Read("Setup", "SIMSDirectory");
            }
        }

        private string GetProgramFiles
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles); }
        }

        public string GetDiscoverServiceDirectory
        {
            get
            {
                /* Because Express only allows you to compile as 32bit, 
                 * this will return the x86 program files.              */
                var programFiles = GetProgramFiles;
                if (programFiles.EndsWith("\\"))
                {
                    programFiles = programFiles.Substring(0, programFiles.Length - 1);
                }
                if (programFiles.ToLower().EndsWith(" (x86)"))
                {
                    programFiles = programFiles.Substring(0, programFiles.Length - 6);
                }
                return Path.Combine(programFiles, "SIMS\\SIMS Discover Services");
            }
        }

        public string GetDmsDirectory
        {
            get { return services.GetDmsDirectory; }
        }

        public string GetProxyAddress
        {
            get { return proxy.GetProxyAddress; }
        }

        public string GetProxyPort
        {
            get { return proxy.GetProxyPort; }
        }

        public string GetSchoolName
        {
            get { return simsApi.GetSchName; }
        }

        public string GetSchoolCode
        {
            get { return simsApi.GetDfeCode; }
        }

        public string GetSchoolPostCode
        {
            get { return simsApi.GetPostCode; }
        }

        private string AddToNotes
        {
            set
            {
                if (string.IsNullOrEmpty(GetNotes))
                {
                    GetNotes = value;
                }
                else
                {
                    GetNotes = GetNotes + "\n" + value;
                }
            }
        }

        public string GetNotes { get; private set; }

        public string GetSimsSqlBinn
        {
            get
            {
                string sqlBinn = null;
                var iniFile = new IniFile(simsIni);
                sqlBinn = iniFile.Read("Setup", "SIMSSQLAppsDirectory");

                if (string.IsNullOrEmpty(sqlBinn))
                {
                    return null;
                }
                if (!isLocalDrive(GetDriveName(sqlBinn)))
                {
                    AddToNotes = "SIMS SQL Binn appears to incorrect - not a local drive - " + sqlBinn;
                }
                return sqlBinn;
            }
        }

        public string GetFmsSqlBinn
        {
            get
            {
                string sqlBinn = null;
                var iniFile = new IniFile(simsIni);
                sqlBinn = iniFile.Read("FMSSQL", "FMSSQLAppsDirectory");

                if (string.IsNullOrEmpty(sqlBinn))
                {
                    return null;
                }
                if (!isLocalDrive(GetDriveName(sqlBinn)))
                {
                    AddToNotes = "FMS SQL Binn appears to incorrect - not a local drive - " + sqlBinn;
                }
                return sqlBinn;
            }
        }

        public bool isLocalDrive(string descriptor)
        {
            var queryString = "SELECT * From Win32_LogicalDisk where name = '" + descriptor + "' and DriveType = 3";
            var now = DateTime.Now;
            var objects = new ManagementObjectSearcher(queryString).Get();
            DateTime.Now.Subtract(now);
            foreach (ManagementObject obj2 in objects)
            {
                if (obj2["name"].ToString().ToUpper() == descriptor.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        private string GetDriveName(string fileName)
        {
            var index = fileName.IndexOf(Path.VolumeSeparatorChar);
            if (index > 0)
            {
                return fileName.Substring(0, index + 1);
            }
            return fileName;
        }
    }
}