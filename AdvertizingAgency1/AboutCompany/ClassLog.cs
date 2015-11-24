
using System;
using System.Web;
using System.Text;
using System.IO;






namespace AdvertizingAgency1.AboutCompany
{

    class ClassLog
    {
        private static object sync = new object();
        public static void Write(String message)
        {
            Write(message, null);
        }
        public static void Write(Exception ex)
        {
            Write(null, ex);
        }
        public static void Write(String message, Exception ex)
        {
            try
            {
                if (message == null && ex == null)
                {
                    return;
                }
                // Путь .\\

                string pathToLog = HttpContext.Current.Server.MapPath("~//Log//");
                //string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                    if (!Directory.Exists(pathToLog))
                        Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
                "AdvertizingAgencyWebApplication ", DateTime.Now));
                string fullText = null;
                    if (ex != null && message != null)
                    {
                        fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{4]}] - [{1}.{2}()] {3}\r\n",
                          DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message, message);
                    }
                    else if (ex != null)
                    {
                        fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n",
                      DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                    }
                    else if (message != null)
                    {
                        fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] {1}\r\n",
                      DateTime.Now, message);
                    }
                lock (sync)
                {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }
    }
}
    

