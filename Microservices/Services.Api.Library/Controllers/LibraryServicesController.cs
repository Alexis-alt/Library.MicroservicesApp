using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Library.Core.ContextMongoDB;
using Services.Api.Library.Core.Entities;
using Services.Api.Library.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryServicesController : ControllerBase
    {


        private readonly IAuthorRepository _authorRepository;

        private readonly IMongoRepository<AuthorEntity> _mongoRepositoryAuthor;
        


        public LibraryServicesController(IAuthorRepository authorRepository, IMongoRepository<AuthorEntity> mongoRepositoryAuthor)
        {

            _mongoRepositoryAuthor = mongoRepositoryAuthor;
            _authorRepository = authorRepository;
        }


        [HttpGet("authors")]
        public async Task<ActionResult<IEnumerable<Author>>> Authors()
        {

            var authors = await _authorRepository.GetAuthors();

            return Ok(authors);

        }

        [HttpGet("authorsGeneric")]
        public async Task<ActionResult<IEnumerable<AuthorEntity>>> AuthorsGeneric()
        {

            var authors = await _mongoRepositoryAuthor.GetAll();

            return Ok(authors);

        }


    }
}
