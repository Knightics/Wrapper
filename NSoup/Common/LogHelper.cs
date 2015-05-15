using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace NSoup.Common
{
    public static class LogHelper
    {
        public static void WriteLog(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }

        public static void WriteLog(Type t, Exception ex, string customizedErrMsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(customizedErrMsg, ex);
        }

        public static void WriteLog(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }
    }
}
