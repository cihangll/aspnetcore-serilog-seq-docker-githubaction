using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace DemoApplication.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			if (string.IsNullOrEmpty(aspNetCoreEnvironment))
			{
				configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json", optional: true, reloadOnChange: false);
			}

			var configuration = configurationBuilder.Build();
			var loggerConfiguration = new LoggerConfiguration()
			  .ReadFrom.Configuration(configuration);

			const string serilogKey = "SerilogSeqServerUrl";
			var seqServerUrl = Environment.GetEnvironmentVariable(serilogKey);
			if (string.IsNullOrEmpty(seqServerUrl))
			{
				seqServerUrl = configuration.GetSection(serilogKey)?.Value;
			}

			if (!string.IsNullOrEmpty(seqServerUrl))
			{
				loggerConfiguration.WriteTo.Seq(seqServerUrl);
			}

			Log.Logger = loggerConfiguration.CreateLogger();

			try
			{
				Log.Information("Application Starting Up...");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception e)
			{
				Log.Fatal(e, "The application failed to start correctly.");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
			  .CreateDefaultBuilder(args)
			  .UseSerilog()
			  .ConfigureWebHostDefaults(webBuilder =>
			  {
				  webBuilder.UseStartup<Startup>();
			  });
		}
	}
}
