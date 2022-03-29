using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Insurance.Api.Configuration
{
    public static class SerilogConfiguration
    {
        public static ILogger CreateLogger(this IConfiguration configuration)
        {
            var config = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("App Name", "Insurance")
                .Enrich.FromLogContext();

            return config.CreateLogger();
        }
    }
}
