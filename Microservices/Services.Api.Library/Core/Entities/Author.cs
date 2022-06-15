using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.Core.Entities
{
    public class Author
    {
        [BsonId]//Es un Id
        [BsonRepresentation(BsonType.ObjectId)]//Id de un objeto
        public string Id { get; set; }


        [BsonElement("name")]//Indicando a que propiedad se va a mapear
        public string Name { get; set; }


        [BsonElement("lastname")]
        public string Lastname { get; set; }


        [BsonElement("academicGrade")]
        public string AcademicGrade { get; set; }

    }
}
