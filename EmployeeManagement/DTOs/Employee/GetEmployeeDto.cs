namespace EmployeeManagement.DTOs.EmployeeDto
{
    public class GetEmployeeDto
    {
        public int Row_Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string PanNumber { get; set; } = string.Empty;
        public string? PassportNumber { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public byte Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoinee { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public bool IsActive { get; set; }
    }
}
