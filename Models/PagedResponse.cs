using System.ComponentModel.DataAnnotations;

namespace Craftmatrix.org.Model
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;
        public string? NextPageUrl { get; set; }
        public string? PreviousPageUrl { get; set; }
    }

    public class PaginationParams
    {
        private int _page = 1;
        private int _pageSize = 10;

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page 
        { 
            get => _page; 
            set => _page = value > 0 ? value : 1; 
        }

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value > 0 && value <= 100 ? value : 10; 
        }

        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";
    }
}