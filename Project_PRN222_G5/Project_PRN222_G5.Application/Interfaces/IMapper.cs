namespace Project_PRN222_G5.Application.Interfaces;

public interface IMapper<TEntity, in TRequest, TResponse>
    where TEntity : class
    where TRequest : class
    where TResponse : class
{
    TEntity MapToEntity(TRequest request);
    TResponse MapToResponse(TEntity entity);
    void UpdateEntity(TEntity entity, TRequest request);
}