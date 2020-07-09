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
using Microsoft.Extensions.Logging;
using Subscriber.Data;
using Subscriber.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using AutoMapper;
using Subscriber.Data.Profiles;
using Weight_Watchers.Middleware;
using NServiceBus;



namespace Weight_Watchers
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
                //var endpointConfiguration = new EndpointConfiguration("Subscriber");
                //var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                //transport.UseConventionalRoutingTopology();
                //transport.ConnectionString("host= localhost:5672;username=guest;password=guest");


                //var connection = Configuration.GetConnectionString("MeasurePersistanceDB");

                //var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

                //var subscription = persistence.SubscriptionSettings();
                //subscription.CacheFor(TimeSpan.FromMinutes(1));
                //persistence.SqlDialect<SqlDialect.MsSqlServer>();
                //persistence.ConnectionBuilder(
                //    connectionBuilder: () =>
                //    {
                //        return new SqlConnection(connection);
                //    });

                //endpointConfiguration.SendFailedMessagesTo("errorQueue");
                //endpointConfiguration.AuditProcessedMessagesTo("audit");
                //var conventions = endpointConfiguration.Conventions();
                //conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                //conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

                //var endpointInstance = await Endpoint.Start(endpointConfiguration)
                //    .ConfigureAwait(false);
            }
            //var endpointConfiguration = new EndpointConfiguration("Subscriber");

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

            //endpointConfiguration.SendFailedMessagesTo("error");
            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            ////var conventions = endpointConfiguration.Conventions();
            ////conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            ////conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");


            //var endpointInstance = await Endpoint.Start(endpointConfiguration)
            //    .ConfigureAwait(false);

            //services.AddScoped(typeof(IEndpointInstance), x => endpointInstance);
            
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SubscriberProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                       builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .WithExposedHeaders("X-Pagination");
                       });
            });

            services.AddControllers();
            services.AddMvc();
            services.AddAuthorization();

            services.AddDbContext<WeightWatchersContext>(
                  options => options.UseSqlServer(Configuration.GetConnectionString("WeightWatchersDBConnectionString")));

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "WeightWatchersOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "WeightWatchers API",
                        Version = "1"
                    });
            });
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

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/WeightWatchersOpenAPISpecification/swagger.json",
                    "WeightWatchers API"
                    );
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
