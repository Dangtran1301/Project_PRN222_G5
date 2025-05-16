namespace Project_PRN222_G5.Domain.Common;

public interface IBaseAuditable
{
    DateTimeOffset CreatedAt { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}