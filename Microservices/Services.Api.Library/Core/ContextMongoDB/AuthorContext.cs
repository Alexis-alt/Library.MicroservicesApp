using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.Core.ContextMongoDB
{
    /// <summary>
    ///  Este contexto unicamente funciona para el Repositorio Authors debido
    ///  a que primero trabajamos en el repositorio particular y despues se creo el 
    ///  Repositorio generico
    ///  
    /// Usamos repositorio particular cuando las transacciones son muy complejas 
    /// para esa colección
    /// </summary>

    public class AuthorContext : IAuthorContext
    {

        private readonly IMongoDatabase _db;

        //Esta es la forma en la que creamos el contexto para la Base de Datos
        public AuthorContext(IOptions<MongoSettings> options)
        {
            //Instanciamos un Cliente de BD indicandole la ruta del motor MongoDB el cual se esta ejecunado dentro del container en los puertos indicados
            var client = new MongoClient(options.Value.ConnectionString);

            //Al cliente le indicamos el nombre de la BD con la cual trabajaremos
            //Obtenemos el Contexto de la DB
            _db = client.GetDatabase(options.Value.Database);
        }


        //Lo asignamos a esta propiedad
        public IMongoCollection<Author> Authors =>_db.GetCollection<Author>("Author");
    }
}
