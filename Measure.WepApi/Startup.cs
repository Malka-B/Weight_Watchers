using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Measure.Services;
using Measure.Services.Interfaces;
using Measure.WepApi.Profiles;
using Measure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using AutoMapper;
using NServiceBus;
using Microsoft.Data.SqlClient;
using Messages.Commands;

namespace Measure.WepApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            {
            //    var endpointConfiguration = new EndpointConfiguration("Measure");
            //    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            //    var endpointInstance = await Endpoint.Start(endpointConfiguration)
            //        .ConfigureAwait(false);

            //    var connection = Configuration.GetConnectionString("MeasurePersistanceDB");

            //    var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            //    var subscription = persistence.SubscriptionSettings();
            //    subscription.CacheFor(TimeSpan.FromMinutes(1));
            //    persistence.SqlDialect<SqlDialect.MsSqlServer>();
            //    persistence.ConnectionBuilder(
            //        connectionBuilder: () =>
            //        {
            //            return new SqlConnection(connection);
            //        });

            //    var conventions = endpointConfiguration.Conventions();
            //    conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            //    conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
            }
            //NSB
            //var endpointConfiguration = new EndpointConfiguration("Measure");

            //endpointConfiguration.EnableOutbox();
            //endpointConfiguration.EnableInstallers();

            //var connection = Configuration.GetConnectionString("MeasurePersistanceDB");
            //var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            //persistence.SqlDialect<SqlDialect.MsSqlServer>();
            //persistence.ConnectionBuilder(
            //    connectionBuilder: () =>
            //    {
            //        return new SqlConnection(connection);
            //    });

            //var subscription = persistence.SubscriptionSettings();
            //subscription.CacheFor(TimeSpan.FromMinutes(1));

            //var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            //transport.UseConventionalRoutingTopology();
            //transport.ConnectionString("host= localhost:5672;username=guest;password=guest");

            //var recoverability = endpointConfiguration.Recoverability();            
            //recoverability.Immediate(
            //    immediate =>
            //    {
            //        immediate.NumberOfRetries(3);
            //    });
            //recoverability.Delayed(
            //    delayed =>
            //    {
            //        var retries = delayed.NumberOfRetries(3);
            //        retries.TimeIncrease(TimeSpan.FromSeconds(2));
            //    });

            
            //var routing = transport.Routing();
            //routing.RouteToEndpoint(typeof(UpdateMeasure), "Measure.WepApi");

            //endpointConfiguration.SendFailedMessagesTo("error");
            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            ////var conventions = endpointConfiguration.Conventions();
            ////conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            ////conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            //var endpointInstance = await Endpoint.Start(endpointConfiguration)
            //    .ConfigureAwait(false);

            //services.AddScoped(typeof(IEndpointInstance), x => endpointInstance);

            //NSB!
            services.AddScoped<IMeasureService, MeasureService>();
            services.AddScoped<IMeasureRepository, MeasureRepository>();

            services.AddDbContext<MeasureContext>(
                  options => options.UseSqlServer(Configuration.GetConnectionString("MeasureDB")));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MeasureProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllers();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
