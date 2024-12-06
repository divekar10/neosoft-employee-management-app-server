namespace EmployeeManagement.Contracts.Employee
{
    public class AddEmployeeRequest
    {
        public required string EmployeeCode { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public required string PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public byte Gender { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfJoinee { get; set; }
        public bool IsActive { get; set; }
    }
}
