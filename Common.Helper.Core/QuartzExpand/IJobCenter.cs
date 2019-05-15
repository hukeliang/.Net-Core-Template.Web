using Common.Helper.Core.QuartzExpand.Implementation;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Common.Helper.Core.QuartzExpand
{
    public interface IJobCenter
    {

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Task<bool> AddScheduleJobAsync(JobCenterOptions options, Type jobType);

        /// <summary>
        /// 暂停定时任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<bool> StopScheduleJobAsync(string jobGroup, string jobName);

        /// <summary>
        /// 恢复定时任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<bool> RunScheduleJobAsync(string jobGroup, string jobName);
    }
}
