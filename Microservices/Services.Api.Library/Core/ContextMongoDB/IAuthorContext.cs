using MongoDB.Driver;
using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.Core.ContextMongoDB
{
    public interface IAuthorContext
    {

        IMongoCollection<Author> Authors { get; }

    }
}
