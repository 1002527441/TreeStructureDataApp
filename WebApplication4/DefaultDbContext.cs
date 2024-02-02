using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
                .WithMany(x=>x.Children)
                .HasForeignKey(x=>x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }


        public async Task<List<Employee>> GetChilds(string Id)
        {

            var employee =await Employees.FromSqlRaw(
                @"WITH Organization (id, name, parentId, below) AS 
                    (
                        SELECT id, name, ParentId, 0
                        FROM dbo.Employees    
                        WHERE Employees.Id = {0}         
                        UNION ALL
                        SELECT e.Id, e.name,e.ParentId, o.below + 1
                        FROM dbo.Employees e    
                        INNER JOIN organization o 
                        ON o.Id = e.ParentId
                    ) SELECT * FROM organization", Id)                 
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return employee;
        }
    }
}
