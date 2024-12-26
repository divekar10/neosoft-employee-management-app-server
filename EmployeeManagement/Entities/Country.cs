using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Entities.BaseEntity;

namespace EmployeeManagement.Entities
{
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
