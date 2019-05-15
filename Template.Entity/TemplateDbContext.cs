using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using Template.Model;
using Template.Model.TemplateModels;

namespace Template.Entity
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public sealed class TemplateDbContext : DbContext

    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {

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
