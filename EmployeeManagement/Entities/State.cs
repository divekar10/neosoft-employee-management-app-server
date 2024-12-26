using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Entities.BaseEntity;

namespace EmployeeManagement.Entities
{
    public class State : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string StateName { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
