namespace Project_PRN222_G5.BusinessLogic.DTOs;

public class PaginationResponse<T>
{
    public IEnumerable<T>? Data { get; } = [];
    public Paging Paging { get; }

    public PaginationResponse(IEnumerable<T> data, int currentPage, int totalCount, int pageSize)
    {
        Data = data;
        Paging = new Paging(currentPage, pageSize, totalCount);
    }
}

public class Paging
{
    public int CurrentPage { get; }
    public int PageSize { get; }
    public int TotalPage { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }

    public Paging(int currentPage, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize);

        HasNextPage = currentPage < TotalPage;
        HasPreviousPage = currentPage > 1;
    }
}