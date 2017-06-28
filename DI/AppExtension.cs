using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using monolithic_shop_core.Services;
using Quartz.Impl;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;

namespace monolithic_shop_core.DI
{
    public static class AppExtension
    {
        public static void InitializeContainer(this IApplicationBuilder app, Container container, IHostingEnvironment env)
        {
            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

            container.RegisterMvcControllers(app);

            container.RegisterSingleton<IEmailService, FakeEmailService>();
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