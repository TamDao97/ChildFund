using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NTS.Common.Utils
{
    public class LogUtils
    {

        public static void ExceptionLog(string method, string message, Object inputObject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================== START LOGGING ====================================");
            sb.AppendLine(string.Format("{0}: {1}: {2}", DateTime.Now.ToString("hh:mm:ss:fff"), method, message));
            sb.AppendLine(JsonConvert.SerializeObject(inputObject).ToString());

            if (ConfigurationManager.AppSettings["LoggerEnable"].ToLower().Equals("true"))
            {
                WriteLog(sb.ToString());
            }
        }

        public static void LogTracer(string method, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0}: {1}: {2}", DateTime.Now.ToString("hh:mm:ss:fff"), method, message));

            if (ConfigurationManager.AppSettings["LoggerEnable"].Equals("true"))
            {
                WriteLog(sb.ToString());
            }
        }

        private static void WriteLog(string content)
        {
            FileStream fs = null;
            StreamWriter st = null;
            try
            {
                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "/Logs/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(logFilePath))
                {
                    fs = File.Create(logFilePath);
                    fs.Close();
                }

                st = new StreamWriter(logFilePath, true);
                st.Write(content);
                st.Flush();
                st.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (st != null)
                    st.Close();
            }
        }
    }
}
