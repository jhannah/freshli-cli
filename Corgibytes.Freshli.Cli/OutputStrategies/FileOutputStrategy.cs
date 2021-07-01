﻿using Autofac.Extras.DynamicProxy;
using Corgibytes.Freshli.Cli.Formatters;
using Corgibytes.Freshli.Cli.IoC.Interceptors;
using Corgibytes.Freshli.Cli.Options;
using Freshli;
using System;
using System.Collections.Generic;
using System.IO;

namespace Corgibytes.Freshli.Cli.OutputStrategies
{
    [Intercept(typeof(LoggerInterceptor))]
    public class FileOutputStrategy : IOutputStrategy
    {
        public OutputStrategyType Type => OutputStrategyType.file;

        public virtual void Send( IList<MetricsResult> results, IOutputFormatter formatter, ScanOptions options )
        {            
            var path = Path.Combine(options.Path, $"freshli-scan-{DateTime.Now:yyyyMMddTHHmmss}.{options.Format}");
            var file = File.CreateText(path);
            file.WriteLine(formatter.Format(results));
        }
    }
}
