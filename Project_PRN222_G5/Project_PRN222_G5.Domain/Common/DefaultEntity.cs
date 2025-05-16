namespace Project_PRN222_G5.Domain.Common;

public abstract class DefaultEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public abstract class DefaultEntity<T>
{
    public T Id { get; set; } = default!;
}