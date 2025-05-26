using System.Collections;

namespace Project_PRN222_G5.Infrastructure.DTOs;

public class PagedResponse
{
    public IEnumerable? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }
}