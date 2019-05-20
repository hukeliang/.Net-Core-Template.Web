using Microsoft.EntityFrameworkCore;
using Template.Model.TemplateModels;

namespace Template.Entity
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public  class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
            //数据读写分离合理做法
            //创建一个只读上下文 重写SaveChanges
        }

        

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //注入Sql链接字符串  NuGet: MySQL.Data.EntityFrameworkCore
        //    //optionsBuilder.UseMySql(@"Server=.;Database=Test1;Trusted_Connection=True;");
        //}


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable("Course");
        //}

        //public DbSet<User> User { get; set; }

        //public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

    }

    
}
