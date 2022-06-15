﻿using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.IoC;
using Corgibytes.Freshli.Lib;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Corgibytes.Freshli.Cli.Test;

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        new FreshliServiceBuilder(services).Register();
    }
}
