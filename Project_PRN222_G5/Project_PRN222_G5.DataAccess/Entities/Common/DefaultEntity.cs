namespace Project_PRN222_G5.DataAccess.Entities.Common;

public abstract class DefaultEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public abstract class DefaultEntity<T>
{
    public T Id { get; init; } = default!;
}