namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    public class Response
    {
        public int Row_Id { get; set; }
        public required string EmployeeCode { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public required string PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? ProfileImage { get; set; }
        public byte Gender { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
    }
}
