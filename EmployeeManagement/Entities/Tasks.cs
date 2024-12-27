﻿namespace EmployeeManagement.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string TaskStatus { get; set; } = string.Empty;  
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}