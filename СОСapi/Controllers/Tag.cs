using Microsoft.AspNetCore.Mvc;

namespace СОСapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        [Tags("Категория")]
        [HttpPost]
        public IActionResult Post(int id, string name)
        {
            Business bn = new Business();
            bn.BookTag(id, name);
            return Ok();
        }
        [Tags("Категория")]
        [HttpGet]
        [ProducesResponseType(typeof(List<global::Tag>), 200)]
        public IActionResult GetTags()
        {
            Business bn = new Business();
            List<Tag> authors = bn.Tags();
            return Ok(authors);
        }
        [Tags("Категория")]
        [HttpGet("{id}")]
        public IActionResult AllAboutAu(int id)
        {
            Business bn = new Business();
            List<int> booksId = bn.Tagged(id);
            List<global::Book> books = new List<global::Book>();
            foreach (var book in booksId)
            {
                Book bk = bn.BookInf(book);
                books.Add(bk);
            }
            var v = books.ToArray();
            return Ok(v);
        }
    }
}
