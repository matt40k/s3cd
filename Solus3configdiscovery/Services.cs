using System;
using System.IO;
using Microsoft.Win32;

namespace Solus3configdiscovery
{
    public class Services
    {
        private RegistryKey baseRegistryKey = Registry.LocalMachine;
        private string defaultBinn;
        private string subKey = @"SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL10.SIMS2008\MSSQLServer".ToUpper();

        public string GetDmsDirectory
        {
            get
            {
                string value = GetPathForService("SIMS .net Document Server");
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
                string value = "";
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

        private string GetPathForService(string serviceName)
        {
            string value = "";
            try
            {
                // Reference: http://dotnetstep.blogspot.com/2009/06/get-windowservice-executable-path-in.html
                RegistryKey services = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services");
                if (services != null)
                {
                    object pathtoexecutable = services.OpenSubKey(serviceName).GetValue("ImagePath");
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
            RegistryKey key2 = this.baseRegistryKey.OpenSubKey(this.subKey);
            if (key2 == null)
            {
                return null;
            }
            try
            {
                return (string)key2.GetValue(KeyName.ToUpper());
            }
            catch (Exception)
            {
                return null;
            }
        }

        private RegistryKey BaseRegistryKey
        {
            get
            {
                return this.baseRegistryKey;
            }
            set
            {
                this.baseRegistryKey = value;
            }
        }


        private string DefaultBinn
        {
            get
            {
                return this.defaultBinn;
            }
            set
            {
                this.defaultBinn = value;
            }
        }

        private string SubKey
        {
            get
            {
                return this.subKey;
            }
            set
            {
                this.subKey = value;
            }
        }
    }
}
