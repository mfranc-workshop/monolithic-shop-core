using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using monolithic_shop_core.Services;
using Quartz.Impl;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace monolithic_shop_core.DI
{
    public static class AppExtension
    {
        public static void InitializeContainer(this IApplicationBuilder app, Container container, IHostingEnvironment env)
        {
            container.RegisterMvcControllers(app);

            container.RegisterSingleton<IBusClient>(() => {
                var busConfig = new RawRabbitConfiguration
                {
                    Username = "guest",
                    Password = "guest",
                    Port = 5672,
                    VirtualHost = "/",
                    Hostnames = { "localhost" }
                };

                return BusClientFactory.CreateDefault(busConfig);
            });

            container.Register<IEmailService, EmailCommandService>();
            container.Register(() =>
            {
                var sched = new StdSchedulerFactory().GetScheduler().Result;
                sched.JobFactory = new SimpleInjectiorJobFactory(container);
                return sched;
            });

            container.Register<IPaymentProvider, PaymentProvider>();
            container.Register<IReportGenerator, ReportGenerator>();
            container.Register<ITransferCheckService, TransferCheckService>();

            container.Verify();
        }
    }
}