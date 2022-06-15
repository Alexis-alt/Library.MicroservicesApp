using MongoDB.Driver;
using Services.Api.Library.Core.ContextMongoDB;
using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.IRepository
{

    //Este repository es particular de la coleccion  Authors por tanto no es generico
    //Implementamos este tipo de Respositorios cuando nuestra logica de inserción es muy compleja
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IAuthorContext _authorContext;

        public AuthorRepository(IAuthorContext authorContext)
        {
            _authorContext = authorContext;
        }

        /// <summary>
        ///  Usar métodos async nos permite reducir el trafico de procesos 
        ///  que se esten ejecutando simultaneamente a la función
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            //Aqui es donde regresamos la collecion como una lista al Controller
            return await _authorContext.Authors.Find(a=>true).ToListAsync();
        }
    }
}
