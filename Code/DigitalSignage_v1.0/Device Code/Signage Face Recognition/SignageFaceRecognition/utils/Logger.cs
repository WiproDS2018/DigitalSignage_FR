using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition
{
    class Logger
    {
        private static readonly ILog playerLogger;
        private static readonly ILog faceLogger;

        private static readonly ILog connectorLogger;
        private static ILog GetLogger(string logName)
        {
            ILog log = LogManager.GetLogger(logName);
            return log;
        }

        static Logger()
        {
            //logger names are mentioned in <log4net> section of config file
            playerLogger = GetLogger("Player");
            faceLogger = GetLogger("FaceRecognition");
            connectorLogger = GetLogger("Connector");
        }

        /// <summary>
        /// This method will write log in Log_USERNAME_date{yyyyMMdd}.log file
        /// </summary>
        /// <param name="message"></param>
        public static void LogToPlayer(string message)
        {
            playerLogger.DebugFormat(message);
        }
        public static void LogToFaceRecog(string message)
        {
            faceLogger.DebugFormat(message);
        }
        public static void LogToConnector(string message)
        {
            connectorLogger.DebugFormat(message);
        }

    }

}
