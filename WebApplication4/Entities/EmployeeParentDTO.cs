namespace WebApplication4.Entities
{
    public class EmployeeParentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } 
        public EmployeeParentDTO? Parent { get; set; }
       
    }
}
