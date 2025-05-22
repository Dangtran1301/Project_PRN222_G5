namespace Project_PRN222_G5.Domain.Common;

public interface IBaseAuditable
{
    DateTimeOffset CreatedAt { get; set; }
    Guid CreatedBy { get; set; }
    Guid? UpdatedBy { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}

public interface IBaseAuditable<T>
{
    DateTimeOffset CreatedAt { get; set; }
    T CreatedBy { get; set; }
    T? UpdatedBy { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}