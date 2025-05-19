using System.Collections;

namespace Project_PRN222_G5.Application.DTOs;

public class PagedResponse
{
    public IEnumerable? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}