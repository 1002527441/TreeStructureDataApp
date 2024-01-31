using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.Entities;
using WebApplication4.Extensions;

namespace WebApplication4.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HomeController:Controller
    {
        private readonly IDbContextFactory<DefaultDbContext> _dbContextFactory;

        public HomeController(IDbContextFactory<DefaultDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        [HttpGet]
        public IActionResult Get()
        {

        

            return Ok("Hello world");
        }


        [HttpGet("GetChild")]
        public IActionResult GetChild(string Id)
        {

            var context = _dbContextFactory.CreateDbContext();
            var employees = context.Employees.FirstOrDefault(x => x.Id == Id);

            if (employees == null) { return NotFound(); }

            FindChild(employees);

            return Ok(employees);
        }




        [HttpGet("GetParent")]
        public IActionResult GetParent(string Id)
        {

            var context = _dbContextFactory.CreateDbContext();
            var employees = context.Employees.FirstOrDefault(x => x.Id == Id);

            if (employees == null) { return NotFound(); }

            FindParent(employees);

            return Ok(employees);
        }



        private  void FindChild(Employee employe)
        {
            var context = _dbContextFactory.CreateDbContext();
            var childs = context.Employees.Where(x => x.ParentId == employe.Id);

            if (childs == null)  return;

            foreach (var child in childs)
            {
                employe.Childrens.Add(child);
                FindChild(child);
            }

            return ;
        }

        private void FindParent(Employee employe)
        {
            if (string.IsNullOrEmpty(employe.ParentId)) return;

            var context = _dbContextFactory.CreateDbContext();
            var parent = context.Employees.FirstOrDefault(x => x.Id == employe.ParentId);

            employe.Parent = parent;
            if (parent != null) FindParent(parent);

            return;
        }



        [HttpPost]
        public IActionResult Post(string Id, string Name)
        {
            var context = _dbContextFactory.CreateDbContext();

            var employee = new Employee(Id, Name);

            context.Employees.Add(employee);

            context.SaveChanges();

            return Ok("add employee Success");

        }
    }
}
