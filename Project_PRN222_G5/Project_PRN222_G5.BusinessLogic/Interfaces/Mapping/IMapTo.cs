namespace Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;

public interface IMapTo<out TEntity>
{
    TEntity ToEntity();
}