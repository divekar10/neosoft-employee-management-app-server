namespace EmployeeManagement.Entities.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class |
                     AttributeTargets.Struct
      | AttributeTargets.Field | AttributeTargets.Property)]
    public class IncludeAttribute : Attribute
    {
        public IncludeAttribute()
        {
        }
    }
}
