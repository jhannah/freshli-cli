﻿using System.Collections.Generic;
using System.IO;
using Corgibytes.Freshli.Cli.Formatters;
using Corgibytes.Freshli.Cli.OutputStrategies;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Corgibytes.Freshli.Cli.CommandOptions;

public class ScanCommandOptions : CommandOptions
{
    public FormatType Format { get; set; }

    // ReSharper disable once CollectionNeverUpdated.Global
    public IList<OutputStrategyType> Output { get; set; } = new List<OutputStrategyType>();
    public DirectoryInfo Path { get; set; } = null!;
}
