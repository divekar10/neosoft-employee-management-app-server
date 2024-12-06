using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Database
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; private set; }
        public int TotalCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
