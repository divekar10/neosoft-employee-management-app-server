namespace EmployeeManagement.Contracts.EmployeeFeature
{
    public class UpdateEmployeeRequest
    {
        public int Row_Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string PanNumber { get; set; } = string.Empty;
        public string? PassportNumber { get; set; }
        public byte Gender { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoinee { get; set; }
        public bool IsActive { get; set; }
    }
}
