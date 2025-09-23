using Microsoft.AspNetCore.Mvc;

namespace СОСapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Author : ControllerBase
    {
        [Tags("Автор")]
        [HttpPost]
        public IActionResult Post(int id, string name)
        {
            Business bn = new Business();
            bn.BookAuthor(id, name);
            return Ok();
        }
        [Tags("Автор")]
        [HttpDelete]
        public IActionResult Delete(int id, string name)
        {
            Business bn = new Business();
            bn.DeliteBookAuthor(id, name);
            return Ok();
        }
        [Tags("Автор")]
        [HttpGet]
        [ProducesResponseType(typeof(List<global::Author_short>), 200)]
        public IActionResult GetAuthors()
        {
            Business bn = new Business();
            List<Author_short> authors = bn.AllAuthors();
            return Ok(authors);
        }
        [Tags("Автор")]
        [HttpGet("{id}")]
        public IActionResult AllAboutAu(int id)
        {
            Business bn = new Business();
            var myModel = bn.AuthorInf(id);
            return Ok(myModel); ;
        }

    }
}
