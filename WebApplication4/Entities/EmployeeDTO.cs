using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4.Entities
{
    public class EmployeeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmployeeDTO> Children { get; set; }
    }
}
