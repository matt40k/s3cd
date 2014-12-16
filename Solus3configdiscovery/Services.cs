using System;
using System.IO;
using Microsoft.Win32;

namespace Solus3configdiscovery
{
    public class Services
    {
        private RegistryKey baseRegistryKey = Registry.LocalMachine;
        private string subKey = @"SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL10.SIMS2008\MSSQLServer".ToUpper();

        public string GetDmsDirectory
        {
            get
            {
                var value = GetPathForService("SIMS .net Document Server");
                try
                {
                    value = Path.GetDirectoryName(value);
                }
                catch (Exception)
                {
                }
                return value;
            }
        }

        public string GetSqlDirectory
        {
            get
            {
                var value = "";
                /* TO-DO
                 * ===== 
                 * The SIMS.ini contains the BINN folder, but this is set
                 * by the user and it's not unheard of for this path to be
                 * wrong. Services must be on local drives. Sql doesn't like
                 * anything but local drives, so it's just another check :)
                 * 
                 * I've pulled the code from what I submitted for dbAttach
                 */
                return value;
            }
        }

        private RegistryKey BaseRegistryKey
        {
            get { return baseRegistryKey; }
            set { baseRegistryKey = value; }
        }

        private string DefaultBinn { get; set; }

        private string SubKey
        {
            get { return subKey; }
            set { subKey = value; }
        }

        private string GetPathForService(string serviceName)
        {
            var value = "";
            try
            {
                // Reference: http://dotnetstep.blogspot.com/2009/06/get-windowservice-executable-path-in.html
                var services = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services");
                if (services != null)
                {
                    var pathtoexecutable = services.OpenSubKey(serviceName).GetValue("ImagePath");
                    value = pathtoexecutable.ToString();
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

        private string Read(string KeyName)
        {
            var key2 = baseRegistryKey.OpenSubKey(subKey);
            if (key2 == null)
            {
                return null;
            }
            try
            {
                return (string) key2.GetValue(KeyName.ToUpper());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}