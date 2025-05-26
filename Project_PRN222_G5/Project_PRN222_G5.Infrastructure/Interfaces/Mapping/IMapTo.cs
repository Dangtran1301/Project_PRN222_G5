namespace Project_PRN222_G5.Infrastructure.Interfaces.Mapping;

public interface IMapTo<out TEntity>
{
    TEntity ToEntity();
}