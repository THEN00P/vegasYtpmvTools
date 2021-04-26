using System;
using System.IO;

//Really happy that i payed 300€ for the stean verison that doesn't support debugging :)

namespace logger
{
    public static class LogWriter
    {
        public static string m_exePath = string.Empty;

        public static void LogWrite(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch {}
        }

        public static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.Write(":");
                txtWriter.WriteLine(" {0}", logMessage);
            }
            catch {}
        }
    }
}
