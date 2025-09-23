using Microsoft.AspNetCore.Mvc;

namespace СОСapi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class File : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile(int id,IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", file.FileName);

            // Создаем папку, если она не существует
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Business bn= new Business();
            bn.AddLok(id, filePath);

            return Ok(new { filePath });
        }

        [HttpGet("{id}")]
        public IActionResult GetFile(int id)
        {
            Business bn= new Business();
            Book bk=bn.BookInf(id);
            var path = bk.Location;
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", bk.name+".txt");
        }
    }
}

    

