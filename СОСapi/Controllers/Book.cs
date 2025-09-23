using Microsoft.AspNetCore.Mvc;

namespace СОСapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        

        //private readonly ILogger<BookController> _logger;

        //public BookController(ILogger<Book> logger)
        //{
        //    _logger = logger;
        //}

        /// <summary>
        /// Получить всю информацию о книге.
        /// </summary>
        /// <param></param>
        
        /// <returns> Book </returns>
        [Tags("Книги")]
        [HttpGet]
        [ProducesResponseType(typeof(List<global::Book>), 200)]
        public IActionResult GetBooks()
        {
            Business bn = new Business();
            List<int> booksId = bn.AllBooks();
            List<global::Book> books = new List<global::Book>();
            foreach (var book in booksId)
            {
                Book bk = bn.BookInf(book);
                books.Add(bk);
            }
            var v = books.ToArray();
            return Ok(v);
        }
        //[Tags("Книги")]
        //[HttpGet]
        //[ProducesResponseType(typeof(Book), 200)]
        //public IActionResult GetBook(int id)
        //{
        //    Business bn = new Business();
        //    return Ok(bn.BookInf(id));
        //}
        [Tags("Книги")]
        [HttpPost]
        public IActionResult Post(string name, string description)
        {
            Business bn = new Business();
            bn.AddBook(name, description);
            return Ok();
        }
        [Tags("Книги")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Business bn = new Business();
            bn.DeliteBook(id);
            return Ok();
        }

        /// <summary>
        /// Получить всю информацию о книге.
        /// </summary>
        /// <param name="name"></param>

        /// <returns> Book </returns>
        [Tags("Книги")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Book> AllAbout(int id, int userId)
        {
            Business bn = new Business();
            bn.BookUser(userId,id);
            Trans.mkTrans(userId, id);
            var myModel = bn.BookInf(id);
            return Ok(myModel); 
        }

    }
}
