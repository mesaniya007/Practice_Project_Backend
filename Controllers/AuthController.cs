using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using user_crud_api.Data;

namespace user_crud_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTOption _options;
        private readonly object _authContext;

        public AuthController(IOptions<JWTOption> options)
        {
            _options = options.Value;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] Login model)
        {
            DB_Context db = new DB_Context();
            Login login = new Login();
            login = db.getUserByEmail(model.Email);
            if (login.Email is null)
            {
                return BadRequest(new { error = "Email does not exist..!" });
            }
            if (!BCrypt.Net.BCrypt.Verify(model.Password, login.Password))
            {
                return BadRequest(new { error = "Password Incorrect..!" });
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var sToken = new JwtSecurityToken(_options.Issuer, _options.Audience, null, expires: DateTime.Now.AddHours(5), signingCredentials: credential);
            var token = new JwtSecurityTokenHandler().WriteToken(sToken);
            return Ok(new { Token = token });
        }




        [HttpPost("getUserByEmail")]
        //[Authorize]
        public ActionResult GetUserByEmail([FromBody] Email model)
        {
            DB_Context db = new DB_Context();
            Login login = db.getUserByEmail(model.email);
            if (login.Email is null)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { error = "Email already exist..!" });
            }
        }
    }


}





