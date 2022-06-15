using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.IRepository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors();

    }
}
