using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4.Entities
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public string? ParentId { get; set; }
        public Employee? Parent { get; set; }
        public ICollection<Employee> Childrens { get; set; } 


        public Employee()
        {
            Childrens = new List<Employee>();
        }

        public Employee(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public void AddEmployee(Employee employee)
        {
            Childrens.Add(employee);
        }

        public void RemoveEmployee(string Id)
        {
            var employee  = Childrens.First(x => x.Id == Id);
            if (employee != null)
            {
                Childrens.Remove(employee);
            }
         
        }



    }
}
