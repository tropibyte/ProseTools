using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Logging;


namespace ProseTools
{
    internal static class LoggerProvider
    {
        public static ILoggerFactory LoggerFactory { get; } = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.File("logs/logfile.txt")
                .CreateLogger();

            builder.AddSerilog(serilogLogger);
        });

        public static Microsoft.Extensions.Logging.ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}