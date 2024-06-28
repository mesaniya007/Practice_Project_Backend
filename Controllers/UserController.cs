using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using user_crud_api.Data;

namespace user_crud_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddNewUser([FromBody] User user)
        {
            DB_Context db = new DB_Context();
            db.addUser(user);
            return Ok();
        }

    }
}
