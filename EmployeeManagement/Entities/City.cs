using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Entities.BaseEntity;

namespace EmployeeManagement.Entities
{
    public class City : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
