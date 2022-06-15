using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Api.Library.Core;
using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.IRepository
{
    //Este repositorio es generico y se Adapta a todo los modelos que implementen IDocument
    public class MongoRepository<T> : IMongoRepository<T> where T : IDocument
    {
        //Representa la Tabla
        //Viene del paquete Mongo Driver que instalamos
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<MongoSettings> options)
        {
            //Abrimos la conexión al Server de MongoDB
            var client = new MongoClient(options.Value.ConnectionString);

            //Indicamos la Base de Datos con la cual se trabajará
            var db = client.GetDatabase(options.Value.Database);

            //Establecemos la Tabla o Colección 
                                          //Tipo de la colección
                                               //String con nombre de la colección
            _collection = db.GetCollection<T>(GetCollectionName(typeof(T)));

        }


        private protected string GetCollectionName(Type documentType)
        {

            //Se castea a una clase tipo BsonCollectionAttribute (Creada por nostros)
            //Retorna el atributo de tipo string que contiene el nombre de la colección

            var t = typeof(BsonCollectionAttribute);

            var type = documentType.GetCustomAttributes(t,true);



            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }


        public async Task<IEnumerable<T>> GetAll()
        {

            return await _collection.Find(p => true).ToListAsync();
        }

        public async Task<T> GetById(string Id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, Id);

           var collection = await _collection.Find(filter).SingleOrDefaultAsync();

            return collection;
        }

        public async Task Add(T document)
        {
           await _collection.InsertOneAsync(document);

        }

        public async Task Update(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);

            await _collection.FindOneAndReplaceAsync(filter,document);

        }

        public async Task DeleteById(string Id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, Id);

           await _collection.DeleteOneAsync(filter);


        }
    }
}
