namespace EmployeeManagement.Contracts.Tasks
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
    }
}
