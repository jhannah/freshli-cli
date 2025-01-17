﻿using System;
using System.Collections.Generic;
using System.IO;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Formatters;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.OutputStrategies;

public class FileOutputStrategy : IOutputStrategy
{
    public OutputStrategyType Type => OutputStrategyType.File;

    public void Send(IList<ScanResult> results, IOutputFormatter formatter, ScanCommandOptions options)
    {
        var path = Path.Combine(options.Path.FullName,
            $"freshli-scan-{DateTime.Now:yyyyMMddTHHmmss}.{options.Format}");
        var file = File.CreateText(path);
        file.WriteLine(formatter.Format(results));
    }
}
