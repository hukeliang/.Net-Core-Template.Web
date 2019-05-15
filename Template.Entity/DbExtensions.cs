using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace Template.Entity
{

    public static class DbExtensions
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        public static IApplicationBuilder UseCreateDb<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
        {

            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                TDbContext dbContext = serviceScope.ServiceProvider.GetService<TDbContext>();

                //如果数据库不存在就会去创建

                if (!dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }

                return app;
            }
        }


        /// <summary>
        /// 数据库读写分离
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseCreateSlave(this DbContextOptionsBuilder builder, params string[] slaveConnectionString)
        {
            if (slaveConnectionString == null || slaveConnectionString.Length == 0)
            {
                throw new ArgumentNullException(nameof(slaveConnectionString));
            }

            CommandListener commandListener = new CommandListener(slaveConnectionString);

            DiagnosticListener.AllListeners.Subscribe(commandListener);

            return builder;
        }

        /// <summary>
        ///  重新设置链接字符串 需要保证数据库上下文被释放后才能重新设置
        /// </summary>
        /// <param name="dbContext"></param>
        public static void ChangeConnectionString(this DbContext dbContext, string connectionString)
        {

            dbContext.Database.GetDbConnection().ConnectionString = connectionString;
        }

    }
}
