using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class LibraryAuthorController : ControllerBase
    {

        private readonly IMongoRepository<AuthorEntity> _mongoRepositoryAuthor;

        public LibraryAuthorController(IMongoRepository<AuthorEntity> mongoRepositoryAuthor)
        {
            _mongoRepositoryAuthor = mongoRepositoryAuthor;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorEntity>>> GetAll()
        {

            return Ok(await _mongoRepositoryAuthor.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorEntity>> GetById(string id)
        {

            return Ok(await _mongoRepositoryAuthor.GetById(id));
        }


        [HttpPost]
        public async Task<IActionResult> Add(AuthorEntity author)
        {
            await _mongoRepositoryAuthor.Add(author);

            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AuthorEntity author)
        {
            author.Id = id;

            await _mongoRepositoryAuthor.Update(author);

            return Ok();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoRepositoryAuthor.DeleteById(id);

            return Ok();
        }


        [HttpPost]
        [Route("pagination")]
        public async Task<ActionResult<PaginationEntity<AuthorEntity>>> SetPagination(PaginationEntity<AuthorEntity> pagination)
        {

            //Estamos filtrando por el nombre
            var resultados = await _mongoRepositoryAuthor.PaginationByFilter(pagination);

            return Ok(resultados);

        } 

    }
}
