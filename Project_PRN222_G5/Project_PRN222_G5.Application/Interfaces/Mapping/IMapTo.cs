namespace Project_PRN222_G5.Application.Interfaces.Mapping;

public interface IMapTo<out TEntity>
{
    TEntity ToEntity();
}