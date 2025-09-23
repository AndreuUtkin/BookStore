using Microsoft.AspNetCore.Mvc;

namespace СОСapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Users : ControllerBase
    {
        [Tags("Пользователь")]
        [HttpPost]
        public IActionResult Post(string name, string pwd)
        {
            Business bn = new Business();
            bn.AddUser(name, pwd);
            return Ok();
        }
        [Tags("Пользователь")]
        [HttpGet]
        public IActionResult GetHist(int id)
        {
            Business bn = new Business();
            List<Visit> vis = bn.History(id);
            return Ok(vis);
        }
        [Tags("Пользователь")]
        [HttpPut]
        public IActionResult GetV(string name,string password)
        {
            Business bn = new Business();
            User? us = bn.checkUsr(name, password);
            if (us == null)
            {
                return Unauthorized();
            }
            return Ok(us);
        }
    }
}
