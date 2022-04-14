using Microsoft.EntityFrameworkCore;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MOCI.Core.DTOs;

namespace MOCI.DAL.DbContexts
{
    public class MTRSDBContext : DbContext
    {
        public MTRSDBContext(DbContextOptions<MTRSDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
      
       public DbSet<ImportedData> ImportedData { get; set; }

        public DbSet<TerminalDto> Terminals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
           
        }
    }


   
}
