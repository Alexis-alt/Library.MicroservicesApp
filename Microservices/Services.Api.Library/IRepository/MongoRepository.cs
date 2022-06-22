using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Services.Api.Library.Core;
using Services.Api.Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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




        //Método para implementar la paginación en las consultas GetAll de cada Colección

        public async Task<PaginationEntity<T>> PaginationBy(Expression<Func<T, bool>> filter, PaginationEntity<T> pagination)
        {


            var sort = Builders<T>.Sort.Ascending(pagination.Sort);


            if (pagination.Sort == "desc")
            {

                sort = Builders<T>.Sort.Descending(pagination.Sort);


            }
            

              //Filtar por:
            if (string.IsNullOrEmpty(pagination.Filter))
            {
                pagination.Data = await _collection.Find(d=>true)
                    .Sort(sort)
                    .Skip((pagination.Page-1)*pagination.PageSize)//Extrae datos a partir de cierta colección especificada y elimina los anteriores
                                                                  //Si solicitamos la pagina 2: 2-1=1  1*20(numero de elemnetos por pagina)= 20 NÚMERO DE ELEMTOS QUE SE SALTARAN  
                    .Limit(pagination.PageSize)//Limite de elementos a extraer
                    .ToListAsync();


            }
            else
            {
                pagination.Data = await _collection.Find(filter)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)//Extrae datos a partir de cierta colección especificada y elimina los anteriores
                                                                      //Si solicitamos la pagina 2: 2-1=1  1*20(numero de elemnetos por pagina)= 20 NÚMERO DE ELEMTOS QUE SE SALTARAN  
                    .Limit(pagination.PageSize)//Limite de elementos a extraer
                    .ToListAsync();



            }


            //Obtener el total de documentos en una colección
            long totalDocumentsinDB = await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);

           //Obtenemos un string con número de paginas que se generaron en base al número de registros en BD
            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocumentsinDB / pagination.PageSize)));

            
            pagination.PageQuantity = totalPages;


            return pagination;
        }

        public async Task<PaginationEntity<T>> PaginationByFilter(PaginationEntity<T> pagination)
        {

            var sort = Builders<T>.Sort.Ascending(pagination.Sort);


            if (pagination.Sort == "desc")
            {

                sort = Builders<T>.Sort.Descending(pagination.Sort);


            }


            var totalDocumentsInDB = 0;

            //Filtar por:
            if (pagination.FilterValue == null)
            {
                pagination.Data = await _collection.Find(d => true)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)//Extrae datos a partir de cierta colección especificada y elimina los anteriores
                                                                      //Si solicitamos la pagina 2: 2-1=1  1*20(numero de elemnetos por pagina)= 20 NÚMERO DE ELEMTOS QUE SE SALTARAN  
                    .Limit(pagination.PageSize)//Limite de elementos a extraer
                    .ToListAsync();


                totalDocumentsInDB = (await _collection.Find(p => true).ToListAsync()).Count();

            }
            else
            {

                //Expresion regular que busca todos los valores que coincidan con una parte del texto del parametro
                var filterValue = ".*" + pagination.FilterValue.Value + ".*";

                var filter = Builders<T>.Filter.Regex(pagination.FilterValue.Property, new BsonRegularExpression(filterValue,"i"));


                pagination.Data = await _collection.Find(filter)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)//Extrae datos a partir de cierta colección especificada y elimina los anteriores
                                                                      //Si solicitamos la pagina 2: 2-1=1  1*20(numero de elemnetos por pagina)= 20 NÚMERO DE ELEMTOS QUE SE SALTARAN  
                    .Limit(pagination.PageSize)//Limite de elementos a extraer
                    .ToListAsync();

                totalDocumentsInDB = (await _collection.Find(filter).ToListAsync()).Count();


            }


            //Obtener el total de documentos en una colección
            //long totalDocumentsInDB = await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);

            var rounded = Math.Ceiling(totalDocumentsInDB/ Convert.ToDecimal(pagination.PageSize));

            var totalPages = Convert.ToInt32(rounded);

            pagination.PageQuantity = totalPages;

            pagination.TotalRows = Convert.ToInt32(totalDocumentsInDB);

            return pagination;
        }
    }
}
