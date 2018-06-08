using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace ShikiNet.Utils
{
    public static class NLogConfigure
    {
        static NLogConfigure()
        {
            NLogConfigure.Configure(); //logging
        }

        public static void Configure()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget() { FileName = "ShikiNet.log", Name = "logfile" };
            var logconsole = new NLog.Targets.ConsoleTarget() { Name = "logconsole" };

            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logconsole));
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logfile));

            NLog.LogManager.Configuration = config;
        }
    }
}
