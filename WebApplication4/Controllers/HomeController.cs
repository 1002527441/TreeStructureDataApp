
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        private readonly IMapper _mapper;

        public HomeController(IDbContextFactory<DefaultDbContext> dbContextFactory,IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;

            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world");
        }


        [HttpGet("GetChild2")]
        public async Task<IActionResult> GetChild2(string Id)
        {


            var context = await _dbContextFactory.CreateDbContextAsync();

            var employee = context.Employees.Flatten1(x=>x.Children).FirstOrDefault(x=>x.Id == Id);    


            var employessDTO = _mapper.Map<EmployeeDTO>(employee);


            return Ok(employessDTO);
        }

        [HttpGet("GetChild3")]
        public async Task<IActionResult> GetChild3(string Id)
        {


            var context = await _dbContextFactory.CreateDbContextAsync();

            var employee = context.Employees.Flatten3(x => x.Children).FirstOrDefault(x => x.Id == Id);


            var employessDTO = _mapper.Map<EmployeeDTO>(employee);


            return Ok(employessDTO);
        }

        [HttpGet("GetChild4")]
        public async Task<IActionResult> GetChild4(string Id)
        {


            var context = await _dbContextFactory.CreateDbContextAsync();

            var employee =await context.GetChilds(Id);

            var employessDTO = _mapper.Map<EmployeeDTO>(employee.FirstOrDefault());


            return Ok(employessDTO);
        }


        [HttpGet("GetChild0")]
        public async Task<IActionResult> GetChild0(string Id)
        {
            // solution 1
            var context = await _dbContextFactory.CreateDbContextAsync();

            var employee = context.Employees.FirstOrDefault(employees => employees.Id == Id);

            await  FindChildAsync(employee);

            var employessDTO = _mapper.Map<EmployeeDTO>(employee);


            return Ok(employessDTO);
        }



        [HttpGet("GetChild1")]
        public async Task<IActionResult> GetChild1(string Id)
        {
            // solution 1
            var context     = await _dbContextFactory.CreateDbContextAsync();

            var employees   = context.Employees.ToList();

            var employee    = employees.Flatten(x => x.Children).FirstOrDefault(employees => employees.Id == Id);

            var employessDTO = _mapper.Map<EmployeeDTO>(employee);


            return Ok(employessDTO);
        }





        [HttpGet("GetParent")]
        public async Task<IActionResult> GetParent(string Id)
        {

            var context     = await _dbContextFactory.CreateDbContextAsync();
            var employees   = context.Employees.FirstOrDefault(x => x.Id == Id);

            if (employees == null) { return NotFound(); }

            await FindParentAsync(employees);

            var employessDTO = _mapper.Map<EmployeeParentDTO>(employees);

            return Ok(employessDTO);
        }



        private async Task FindChildAsync(Employee employe)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            var childs  = await context.Employees.Where(x => x.ParentId == employe.Id).ToListAsync();

            if (childs == null)  return;

            foreach (var child in childs)
            {
                employe.Children.Add(child);
                await FindChildAsync(child);
            }

            return ;
        }

        private async Task FindParentAsync(Employee employe)
        {
            if (string.IsNullOrEmpty(employe.ParentId)) return;

            var context = await _dbContextFactory.CreateDbContextAsync();
            var parent  = await context.Employees.FirstOrDefaultAsync(x => x.Id == employe.ParentId);

            employe.Parent = parent;
            if (parent != null) await FindParentAsync(parent);

            return;
        }



        [HttpPost]
        public async Task<IActionResult> Post(string Id, string Name)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();

            var employee = new Employee(Id, Name);

            await context.Employees.AddAsync(employee);

            await context.SaveChangesAsync();

            return Ok("add employee Success");

        }
    }
}
