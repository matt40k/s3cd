using Microsoft.Win32;

namespace Solus3configdiscovery
{
    public class Proxy
    {
        private string proxyAddress;
        private string proxyPort;
        private string proxyServer;
        private readonly RegistryKey proxysettings;
        private readonly bool useProxy;

        public Proxy()
        {
            proxysettings =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            var tmp = proxysettings.GetValue("ProxyEnable").ToString();
            if (tmp == "1") useProxy = true;
            GetProxyServer();
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

        private void GetProxyServer()
        {
            proxyServer = proxysettings.GetValue("ProxyServer").ToString();
            var parts = proxyServer.Split(':');
            proxyAddress = parts[0];
            proxyPort = parts[1];
        }
    }
}