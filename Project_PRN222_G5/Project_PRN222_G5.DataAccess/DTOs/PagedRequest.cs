namespace Project_PRN222_G5.DataAccess.DTOs;

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? SortDir { get; set; }
}