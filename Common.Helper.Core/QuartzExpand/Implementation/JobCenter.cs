using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Helper.Core.QuartzExpand.Implementation
{
    public sealed class JobCenter : IJobCenter
    {
        private readonly ISchedulerFactory _SchedulerFactory = null;
        public JobCenter(ISchedulerFactory schedulerFactory)
        {
            this._SchedulerFactory = schedulerFactory;

        }

        async Task<bool> IJobCenter.AddScheduleJobAsync(JobCenterOptions options, Type jobType )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(options.StarRunTime, 1);

            DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(options.EndRunTime, 1);

            IScheduler scheduler = await _SchedulerFactory.GetScheduler();

            IJobDetail job = JobBuilder.Create(jobType)
                .WithIdentity(options.JobName, options.JobGroup)
                .Build();

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                         .StartAt(starRunTime)
                                         .EndAt(endRunTime)
                                         .WithIdentity(options.JobName, options.JobGroup)
                                         .WithCronSchedule(options.CromExpress)
                                         .Build();

            await scheduler.ScheduleJob(job, trigger);

            await scheduler.Start();

            return true;
        }


     
        async Task<bool> IJobCenter.RunScheduleJobAsync(string jobGroup, string jobName)
        {
            IScheduler scheduler = await _SchedulerFactory.GetScheduler();
            //resumejob 恢复
            await scheduler.ResumeJob(new JobKey(jobName, jobGroup));

            return true;
        }

        /// <summary>
        /// 暂停指定任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        async Task<bool> IJobCenter.StopScheduleJobAsync(string jobGroup, string jobName)
        {
            IScheduler scheduler = await _SchedulerFactory.GetScheduler();
            //使任务暂停
            await scheduler.PauseJob(new JobKey(jobName, jobGroup));

            return true;
        }
    }
}
