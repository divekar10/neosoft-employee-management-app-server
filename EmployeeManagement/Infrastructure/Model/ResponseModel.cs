using System.Text.Json;

namespace EmployeeManagement.Infrastructure.Model
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public List<Errors> Errors { get; set; } = [];
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class Errors
    {
        public string PropertyName { get; set; } = string.Empty;

        public required string[] ErrorMessages { get; set; }
    }
}
