using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Api.Library.IRepository
{
    public interface IMongoRepository<T> where T : IDocument 
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(string Id);

        Task Add(T document);

        Task Update(T document);

        Task DeleteById(string Id);

        Task<PaginationEntity<T>> PaginationBy(Expression<Func<T,bool>> filter,PaginationEntity<T> pagination);

        Task<PaginationEntity<T>> PaginationByFilter(PaginationEntity<T> pagination);


    }
}
