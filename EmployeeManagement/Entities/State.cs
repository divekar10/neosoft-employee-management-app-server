using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Entities
{
    public class State
    {
        [Key]
        public int Row_Id { get; set; }
        public required string StateName { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
