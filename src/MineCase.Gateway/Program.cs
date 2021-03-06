﻿using Microsoft.Extensions.Configuration;
using Orleans;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Microsoft.Extensions.Logging;
using MineCase.Gateway.Network;

namespace MineCase.Gateway
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        private static IClusterClient _clusterClient;
        private static readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, e) => _exitEvent.Set();

            Configuration = LoadConfiguration();
            _clusterClient = new ClientBuilder()
                .LoadConfiguration("OrleansConfiguration.dev.xml")
                .ConfigureServices(ConfigureServices)
                .Build();
            Startup();
            _exitEvent.WaitOne();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ConfigureLogging());
            services.AddLogging();
            services.AddSingleton<ConnectionRouter>();
        }

        private static ILoggerFactory ConfigureLogging()
        {
            var factory = new LoggerFactory();
            factory.AddConsole();

            return factory;
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", true, false);
            return builder.Build();
        }

        private static async void Startup()
        {
            var serviceProvider = _clusterClient.ServiceProvider;
            var logger = _clusterClient.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
            try
            {
                logger.LogInformation("Connecting to cluster...");
                await _clusterClient.Connect();
                logger.LogInformation("Connected to cluster.");
            }
            catch (Exception e)
            {
                logger.LogError("Cluster connection failed.\n" + "Exception stack trace:" + e.StackTrace);
                _exitEvent.Set();
                return;
            }
            var connectionRouter = serviceProvider.GetRequiredService<ConnectionRouter>();
            await connectionRouter.Startup(default(CancellationToken));
        }
    }
}