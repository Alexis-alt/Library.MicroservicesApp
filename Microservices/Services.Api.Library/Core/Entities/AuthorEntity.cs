using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.Core.Entities
{
    [BsonCollection("Author")]
    public class AuthorEntity: Document
    {

        [BsonElement("name")]
        public string Name { get; set; }


        [BsonElement("lastname")]
        public string Lastname { get; set; }


        [BsonElement("academicGrade")]
        public string AcademicGrade { get; set; }

    }
}
