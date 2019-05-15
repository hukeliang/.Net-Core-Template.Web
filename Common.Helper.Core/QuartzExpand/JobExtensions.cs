using Common.Helper.Core.QuartzExpand.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Helper.Core.QuartzExpand
{
    public static class JobExtensions
    {
        /// <summary>
        /// 程序启动将任务调度表里所有状态为 执行中 任务启动起来
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddJobService(this IApplicationBuilder app, Func<List<JobCenterOptions>> options)
        {

            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IJobCenter jobCenter = serviceScope.ServiceProvider.GetService<IJobCenter>();

                List<JobCenterOptions> jobInfoList = options.Invoke();


                if (jobInfoList == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                //获取实现直接接口的所有子类
                //Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                //             .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(Quartz.IJob))))
                //             .ToArray();
                //IJob job = Activator.CreateInstance(personType) as IJob;
                

                //types.ForEach(/*async*/ (item) =>
                //{
                //    //await jobCenter.AddScheduleJobAsync(item);
                //});

                return app;
            }
        }


        /// <summary>
        /// 获取指定抽象类的所有子类
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAbstractSon(Type value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!value.IsClass)
            {
                throw new ArgumentException("传入不是Class");
            }

            if (!value.IsAbstract)
            {
                throw new ArgumentException("传入的不是一个抽象类");
            }

            Assembly[] assembliesArray = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assembliesArray)
            {
                Type[] typeArray = assembly.GetTypes();

                foreach (Type type in typeArray)
                {
                    //不是继承自传入的类型 不是class 不是抽象类 不是自己 
                    if (!value.IsAssignableFrom(type) || !type.IsClass || !type.IsAbstract || value == type)
                    {
                        continue;
                    }

                    yield return type;
                }
            }
        }
        private static Type GetJobType(Type[] types, string className)
        {
            foreach (Type item in types)
            {
                if (item.Equals(className))
                {
                    return item;
                }
            }

            return null;
        }
    }
}
