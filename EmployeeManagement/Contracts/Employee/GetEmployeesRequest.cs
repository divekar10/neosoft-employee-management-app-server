namespace EmployeeManagement.Contracts.EmployeeFeature
{
    public class GetEmployeesRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
}
