﻿using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;
using Project_PRN222_G5.DataAccess.Entities.Common;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service;

public interface IGenericService<TE, in TC, in TU, TR> : IMapper<TE, TC, TU, TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    #region CRUD

    Task<TR> GetByIdAsync(Guid id);

    Task<IEnumerable<TR>> GetAllAsync();

    Task<PaginationResponse<TR>> GetPagedAsync(
        PagedRequest request,
        Expression<Func<TE, bool>>? predicate = null,
        Func<IQueryable<TE>, IQueryable<TE>>? include = null);

    Task<TR> CreateAsync(TC request);

    Task<TR> UpdateAsync(Guid id, TU request);

    Task DeleteAsync(Guid id);

    #endregion CRUD
}