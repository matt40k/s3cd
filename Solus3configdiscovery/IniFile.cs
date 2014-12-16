using System.Runtime.InteropServices;
using System.Text;

namespace Solus3configdiscovery
{
    public class IniFile
    {
        public string path;

        /// <summary>
        ///     INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        ///     Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string Read(string Section, string Key)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(Section, Key, "", temp,
                255, path);
            return temp.ToString();
        }
    }
}