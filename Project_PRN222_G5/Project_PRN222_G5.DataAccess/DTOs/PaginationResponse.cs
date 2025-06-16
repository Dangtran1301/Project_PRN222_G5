namespace Project_PRN222_G5.DataAccess.DTOs;

public class PaginationResponse<T>(IEnumerable<T> data, int currentPage, int totalCount, int pageSize)
{
    public IEnumerable<T> Data { get; } = data;
    public Paging Paging { get; } = new(currentPage, pageSize, totalCount);
}

public class Paging(int currentPage, int pageSize, int totalCount)
{
    public int CurrentPage { get; set; } = currentPage;
    public int PageSize { get; set; } = pageSize;
    public int TotalPage { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    public bool HasNextPage => CurrentPage < TotalPage;
    public bool HasPreviousPage => CurrentPage > 1;
}