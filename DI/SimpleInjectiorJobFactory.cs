using System;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using SimpleInjector;

namespace monolithic_shop_core.DI
{
    class SimpleInjectiorJobFactory : SimpleJobFactory
    {
        private readonly Container _container;

        public SimpleInjectiorJobFactory(Container container)
        {
            _container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)_container.GetInstance(bundle.JobDetail.JobType);
            }
            catch (Exception ex)
            {
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from the NinjectJobFactory.", ex);
            }
        }
    }
}