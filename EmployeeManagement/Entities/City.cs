using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Entities
{
    public class City
    {
        [Key]
        public int Row_Id { get; set; }
        public required string CityName { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
