using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper.Core.QuartzExpand.Implementation
{
    public class JobCenterOptions
    {

        /// <summary>
        /// 任务组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Corm表达式
        /// </summary>
        public string CromExpress { get; set; }

        /// <summary>
        /// 开始运行时间
        /// </summary>
        public DateTime StarRunTime { get; set; }

        /// <summary>
        /// 结束运行时间
        /// </summary>
        public DateTime? EndRunTime { get; set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>
        public DateTime? NextRunTime { get; set; }

        
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunStatus { get; set; }
        

        /// <summary>
        /// 实现IJob接口 功能类的名字
        /// </summary>
        public string ClassName { get; set; }
    }
}
