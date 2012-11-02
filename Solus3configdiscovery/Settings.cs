using System;
using System.IO;
using System.Management;

namespace Solus3configdiscovery
{
    public class Settings
    {
        Services services;
        Proxy proxy;
        SimsApi simsApi;
        string notes;

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

        private string simsDir = null;
        private string fmsDir = null;

        private string simsDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(simsDir)) { return simsDir; }
                IniFile iniSims = new IniFile(simsIni);
                return simsDir = iniSims.Read("Setup", "SIMSDotNetDirectory");
            }
        }

        private string fmsDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(fmsDir)) { return fmsDir; }
                IniFile iniFms = new IniFile(simsIni);
                return fmsDir = iniFms.Read("Setup", "FinanceDirectory");
            }
        }

        private string simsIni
        {
            get
            {
                string value = Path.Combine(winDir, "sims.ini");
                if (File.Exists(value)) { return value; }
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
                string connect = System.IO.Path.Combine(simsDirectory, "Connect.ini");
                IniFile iniFile = new IniFile(connect);
                string value = iniFile.Read("SIMSConnection", "ServerName");
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
                string connect = System.IO.Path.Combine(simsDirectory, "Connect.ini");
                IniFile iniFile = new IniFile(connect);
                return iniFile.Read("SIMSConnection", "DatabaseName");
            }
        }

        public string GetFmsInstanceName
        {
            get
            {
                string value = "";
                string connect = System.IO.Path.Combine(fmsDirectory, "FMSConnect.ini");
                if (File.Exists(connect))
                {
                    IniFile iniFile = new IniFile(connect);
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
                string value = "";
                string connect = System.IO.Path.Combine(fmsDirectory, "FMSConnect.ini");
                if (File.Exists(connect))
                {
                    IniFile iniFile = new IniFile(connect);
                    value = iniFile.Read("FMSConnection", "Database");
                }
                return value;
            }
        }

        public string GetSimsShare
        {
            get
            {
                IniFile iniFile = new IniFile(simsIni);
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
                string programFiles = GetProgramFiles;
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

        public bool isLocalDrive(string descriptor)
        {
            string queryString = "SELECT * From Win32_LogicalDisk where name = '" + descriptor + "' and DriveType = 3";
            DateTime now = DateTime.Now;
            ManagementObjectCollection objects = new ManagementObjectSearcher(queryString).Get();
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
                if (string.IsNullOrEmpty(notes)) { notes = value; }
                else { notes = notes + "\n" + value; }
            }
        }

        public string GetNotes
        {
            get { return notes; }
        }

        private string GetDriveName(string fileName)
        {
            int index = fileName.IndexOf(Path.VolumeSeparatorChar);
            if (index > 0)
            {
                return fileName.Substring(0, index + 1);
            }
            return fileName;
        }

        public string GetSimsSqlBinn
        {
            get
            {
                string sqlBinn = null;
                IniFile iniFile = new IniFile(simsIni);
                sqlBinn = iniFile.Read("Setup", "SIMSSQLAppsDirectory");

                if (string.IsNullOrEmpty(sqlBinn)) { return null; }
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
                IniFile iniFile = new IniFile(simsIni);
                sqlBinn = iniFile.Read("FMSSQL", "FMSSQLAppsDirectory");

                if (string.IsNullOrEmpty(sqlBinn)) { return null; }
                if (!isLocalDrive(GetDriveName(sqlBinn)))
                {
                    AddToNotes = "FMS SQL Binn appears to incorrect - not a local drive - " + sqlBinn;
                }
                return sqlBinn;
            }
        }
    }
}