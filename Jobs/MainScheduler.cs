using System;
using Quartz;
using SimpleInjector;

namespace monolithic_shop_core.Jobs
{
    public static class MainScheduler
    {
        public static void Start(Container container)
        {
            try
            {
                var scheduler = container.GetInstance<IScheduler>();

                scheduler.Start();
                var job = JobBuilder.Create<WarehouseJob>().Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(60)
                        .RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);


                var triggerTransfer = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(60)
                        .RepeatForever())
                    .Build();

                var transferJob = JobBuilder.Create<CheckTransferJob>().Build();
                scheduler.ScheduleJob(transferJob, triggerTransfer);
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}