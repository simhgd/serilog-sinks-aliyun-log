﻿using Aliyun.Api.LogService;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.AliyunLog;
using System;
using System.Collections.Generic;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.AliyunLog() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class AliyunLoggerConfigurationExtensions
    {
        //const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
        const string DefaultOutputTemplate = "{Message}";

        /// <summary>
        /// Writes log events to <see cref="Aliyun.Api.LogService.ILogServiceClient" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="logServiceClient"></param>
        /// <param name="logstoreName"></param>
        /// <param name="project"></param>
        /// <param name="logTags"></param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">Message template describing the output format.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration AliyunLog(
            this LoggerSinkConfiguration sinkConfiguration,
            ILogServiceClient logServiceClient,
            string logstoreName = null,
            string project = null,
            IDictionary<string, string> logTags = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (logstoreName == null) throw new ArgumentNullException(nameof(logstoreName));
            if (logServiceClient == null) throw new ArgumentNullException(nameof(logServiceClient));

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.AliyunLog(logServiceClient, formatter, logstoreName, project, logTags, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="logServiceClient"></param>
        /// <param name="formatter">Text formatter used by sink.</param>
        /// <param name="logstoreName"></param>
        /// <param name="project"></param>
        /// <param name="logTags"></param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration AliyunLog(
            this LoggerSinkConfiguration sinkConfiguration,
            ILogServiceClient logServiceClient,
            ITextFormatter formatter,
            string logstoreName = null,
            string project = null,
            IDictionary<string, string> logTags = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (logstoreName == null) throw new ArgumentNullException(nameof(logstoreName));
            if (logServiceClient == null) throw new ArgumentNullException(nameof(logServiceClient));
            var sink = new AliyunLogSink(logServiceClient, formatter, logstoreName, project, logTags);
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
