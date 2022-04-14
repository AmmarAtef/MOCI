using Microsoft.EntityFrameworkCore;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.DbContexts
{
    public class LoggerDBContext : DbContext
    {
        public LoggerDBContext(DbContextOptions<LoggerDBContext> options) : base(options)
        {
        }

        
        public DbSet<NLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
