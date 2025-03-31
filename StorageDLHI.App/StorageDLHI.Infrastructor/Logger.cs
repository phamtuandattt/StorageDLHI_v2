using log4net;
using log4net.Config;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StorageDLHI.Infrastructor
{
    public static class LoggerConfig
    {
        private static readonly ILog _logger;

        static LoggerConfig()
        {
            // Load the log4net configuration from the config file
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // Get the logger instance
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        }

        public static ILog Logger
        {
            get { return _logger; }
        }
    }
}
