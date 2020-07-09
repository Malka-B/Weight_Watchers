using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SubscriberHandlerInMeasure
{
    class Program
    {
        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            //Console.Title = GetQueue("queueName");

            //var endpointConfiguration = new EndpointConfiguration(GetQueue("queueName"));
            Console.Title = "Subscriber";

            var endpointConfiguration = new EndpointConfiguration("Subscriber");
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableInstallers();

            var connection = configuration.GetConnectionString("MeasurePersistanceDB");

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var subscription = persistence.SubscriptionSettings();
            subscription.CacheFor(TimeSpan.FromMinutes(1));

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");

            var recoverability = endpointConfiguration.Recoverability();
            //recoverability.AddUnrecoverableException<MyBusinessException>();
            //recoverability.CustomPolicy(MyCustomRetryPolicy);            
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(3);
                });
            recoverability.Delayed(
                delayed =>
                {
                    var retries = delayed.NumberOfRetries(3);
                    retries.TimeIncrease(TimeSpan.FromSeconds(2));
                });

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            //endpointConfiguration.AuditSagaStateChanges(
            //        serviceControlQueue: GetQueue("serviceControlQueue"));

            //var conventions = endpointConfiguration.Conventions();
            //conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            //conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);

            string GetQueue(string queueName)
            {
                return configuration.GetSection("Queues").GetSection(queueName).Value;
            }
        }
    }
}
