//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using NServiceBus;

//using System;
//using System.IO;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using NServiceBus;
//using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Diagnostics;
using NServiceBus;

using Microsoft.Data.SqlClient;
using System.Text;
using NServiceBus.Faults;
using NServiceBus.Logging;
using Messages.Commands;

namespace Measure.WepApi
{
    public class Program
    {
        //    public static void Main(string[] args)
        //    {
        //        CreateHostBuilder(args).Build().Run();
        //    }

        //    public static IHostBuilder CreateHostBuilder(string[] args) =>
        //        Host.CreateDefaultBuilder(args)
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {
        //                webBuilder.UseStartup<Startup>();
        //            });
        //}

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
         .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder()
           .UseNServiceBus(context =>
           {
               var endpointConfiguration = new EndpointConfiguration("Measure");
               endpointConfiguration.EnableInstallers();
               var outboxSettings = endpointConfiguration.EnableOutbox();
               outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
               outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));
               var recoverability = endpointConfiguration.Recoverability();
               recoverability.Delayed(
                   customizations: delayed =>
                   {
                       delayed.NumberOfRetries(2);
                       delayed.TimeIncrease(TimeSpan.FromMinutes(4));
                   });
               recoverability.Immediate(
                   customizations: immediate =>
                   {
                       immediate.NumberOfRetries(3);

                   });

               var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
               transport.UseConventionalRoutingTopology()
               .ConnectionString("host = localhost:5672; username = guest; password = guest");
               //.ConnectionString("host= localhost:5672; username = Rena; password = Rena@rabbitmq");

               var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
               //var connection = "Server=.\\sqlexpress; Database= WeightWatchers; Trusted_Connection = True;";
               var connection = Configuration.GetConnectionString("MeasurePersistanceDB");
               persistence.SqlDialect<SqlDialect.MsSqlServer>();
               persistence.ConnectionBuilder(
                   connectionBuilder: () =>
                   {
                       return new SqlConnection(connection);
                   });

               var routing = transport.Routing();
               routing.RouteToEndpoint(
                    messageType: typeof(UpdateMeasure),
                    destination: "Subscriber");
               //routing.RouteToEndpoint(typeof(UpdateMeasure), "SubscriberHandlerInMeasure");
               var subscriptions = persistence.SubscriptionSettings();
               subscriptions.CacheFor(TimeSpan.FromMinutes(10));
               endpointConfiguration.SendFailedMessagesTo("error");
               endpointConfiguration.AuditProcessedMessagesTo("audit");
               //endpointConfiguration.AuditSagaStateChanges(
               //        serviceControlQueue: "Particular.weightwatchers");
               var conventions = endpointConfiguration.Conventions();
               conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
               conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
               // SubscribeToNotifications.Subscribe(endpointConfiguration);
               return endpointConfiguration;
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>()
                         .UseConfiguration(Configuration);
           });
    }

    public static class SubscribeToNotifications
    {
        static ILog log = LogManager.GetLogger(typeof(SubscribeToNotifications));

        public static void Subscribe(EndpointConfiguration endpointConfiguration)
        {
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(settings => settings.OnMessageBeingRetried(Log));
            recoverability.Delayed(settings => settings.OnMessageBeingRetried(Log));
            recoverability.Failed(settings => settings.OnMessageSentToErrorQueue(Log));
        }

        static string GetMessageString(byte[] body)
        {
            return Encoding.UTF8.GetString(body);
        }

        static Task Log(FailedMessage failed)
        {
            log.Fatal($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
            return Task.CompletedTask;
        }

        static Task Log(DelayedRetryMessage retry)
        {
            log.Fatal($@"Message sent to Delayed Retries.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
            return Task.CompletedTask;
        }

        static Task Log(ImmediateRetryMessage retry)
        {
            log.Fatal($@"Message sent to Immediate Retry.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
            return Task.CompletedTask;
        }
    }
}
