namespace Project_PRN222_G5.DataAccess.Interfaces.Mapping;

public interface IMapTo<out TEntity>
{
    TEntity ToEntity();
}