using System;
using Microsoft.Win32;

namespace Solus3configdiscovery
{
    public class Proxy
    {
        private bool useProxy = false;
        private string proxyServer;

        private string proxyAddress;
        private string proxyPort;

        private RegistryKey proxysettings;

        public Proxy()
        {
            proxysettings = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            string tmp = proxysettings.GetValue("ProxyEnable").ToString();
            if (tmp == "1") useProxy = true;
            GetProxyServer();
        }

        private void GetProxyServer()
        {
            proxyServer = proxysettings.GetValue("ProxyServer").ToString();
            string[] parts = proxyServer.Split(':');
            proxyAddress = parts[0];
            proxyPort = parts[1];
        }

        public string GetProxyAddress
        {
            get
            {
                if (useProxy)
                {
                    return proxyAddress;
                }
                return "";
            }
        }

        public string GetProxyPort
        {
            get
            {
                if (useProxy)
                {
                    return proxyPort;
                }
                return "";
            }
        }
    }
}
