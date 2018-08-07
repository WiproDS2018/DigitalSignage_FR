using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digital_Signage
{
    public class LogHelper
    {
        private static readonly ILog _debugLogger;
        private static ILog GetLogger(string logName)
        {
            ILog log = LogManager.GetLogger(logName);
            return log;
        }

        static LogHelper()
        {
            //logger names are mentioned in <log4net> section of config file
            _debugLogger = GetLogger("MyApplicationDebugLog");
        }

        /// <summary>
        /// This method will write log in Log_USERNAME_date{yyyyMMdd}.log file
        /// </summary>
        /// <param name="message"></param>
        public static void WriteDebugLog(string message)
        {
            _debugLogger.DebugFormat(message);
        }
    }
}