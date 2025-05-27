namespace Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;

public interface IMapper<TEntity, in TRequest, out TResponse>
    where TEntity : class
    where TRequest : class
    where TResponse : class
{
    TEntity MapToEntity(TRequest request);

    TResponse MapToResponse(TEntity entity);

    void UpdateEntity(TEntity entity, TRequest request);
}