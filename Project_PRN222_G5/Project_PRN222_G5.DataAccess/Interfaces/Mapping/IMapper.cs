using Project_PRN222_G5.DataAccess.Entities.Common;

namespace Project_PRN222_G5.DataAccess.Interfaces.Mapping;

public interface IMapper<TE, in TC, in TU, out TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    TE MapToEntity(TC request);

    TR MapToResponse(TE entity);

    void UpdateEntity(TE entity, TU request);
}