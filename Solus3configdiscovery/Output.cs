using System.IO;

namespace Solus3configdiscovery
{
    public class Output
    {
        private readonly string fileName;

        public Output()
        {
            var path = Directory.GetCurrentDirectory();
            var filename = "solus3.config.settings.txt";
            fileName = Path.Combine(path, filename);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public void WriteTitle(string message)
        {
            var aLog = new FileStream(fileName, FileMode.Append);
            var sw = new StreamWriter(aLog);
            sw.WriteLine(message);
            sw.Close();
        }

        public void Write(string message)
        {
            var aLog = new FileStream(fileName, FileMode.Append);
            var sw = new StreamWriter(aLog);
            sw.WriteLine("      " + message);
            sw.Close();
        }

        public void WriteBlankLine(int no)
        {
            var aLog = new FileStream(fileName, FileMode.Append);
            var sw = new StreamWriter(aLog);
            for (var i = 1; i <= no; i++)
            {
                sw.WriteLine("");
            }
            sw.Close();
        }
    }
}