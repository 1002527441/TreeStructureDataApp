using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.Entities;

namespace WebApplication4
{
    public class DefaultDbContext:DbContext
    {

        public DbSet<Employee> Employees { get; set; }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(x=>x.Parent)
                .WithMany(x=>x.Childrens)
                .HasForeignKey(x=>x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
