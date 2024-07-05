using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using user_crud_api.Data;

namespace user_crud_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("Register")]
        public ActionResult AddNewUser([FromBody] User user)
        {
            DB_Context db = new DB_Context();
            db.addUser(user);
            return Ok();
        }
        [HttpGet("GetAllUsers")]
        [Authorize]
        public ActionResult GetAllUsers()
        {
            DB_Context db = new DB_Context();
            var users = new List<User>();
            users = db.getAllUser();
            return Ok(users);
        }
        [HttpPut("ApproveUser{id}")]
        public ActionResult approveUser(int id)
        {
            DB_Context db = new DB_Context();
            db.approveUser(id);
            return Ok();
        }
    }
}
