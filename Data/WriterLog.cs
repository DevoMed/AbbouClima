using AbbouClima.Services;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Text;

namespace AbbouClima.Data
{
    public class WriterLog
    {

        private static readonly object padlock = new object();
        private static StreamWriter writer;
        private static string logFileName = "";
        private static WriterLog instance = null;
        private string LogFolder;

        private WriterLog(string logFolderPath)
        {
            LogFolder = string.IsNullOrEmpty(logFolderPath)
                ? AppDomain.CurrentDomain.BaseDirectory + "Logs\\"
                : logFolderPath;
        }

        public void Log(string logText)
        {
            try
            {
                writer.WriteLine(DateTime.Now.ToString() + " " + logText);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el log: {ex.Message}");
            }

        }

        public static WriterLog Instance(string logFolderPath)
        {         
            
                if (instance == null || CheckDateUp())
                {
                    lock (padlock)
                    {
                        if (instance == null || CheckDateUp())
                        {
                            instance = new WriterLog(logFolderPath);
                            instance.Initialization();
                        }
                    }
                }
                return instance;
            
        }


        private void Initialization()
        {
            string msg = "Log";
            string folderAddr = LogFolder;
            if (folderAddr == null)
            {
                folderAddr = AppDomain.CurrentDomain.BaseDirectory;
            }
            logFileName = DateTime.Now.ToString("yyyyMMdd");
            System.IO.FileInfo file = new System.IO.FileInfo(folderAddr + logFileName + logFileName + msg + ".txt");
            file.Directory.Create();
            writer = new StreamWriter(folderAddr + logFileName + msg + ".txt", true, Encoding.GetEncoding("ISO-8859-1"));
        }
        private static bool CheckDateUp()
        {
            return !logFileName.Equals(DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
