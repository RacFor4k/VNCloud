using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BlazorApp3.Modules;
using Mysqlx.Crud;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {

        private string CreateJWT(string login)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("2b7d568bbf5dc44935f6af1edb111eb7867a9ecbb14de5609ee4c1f133986d5c")); // NOTE: SAME KEY AS USED IN Program.cs FILE; DO NOT REUSE THIS GUID
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
			{
                new Claim(ClaimTypes.Name, login), // NOTE: this will be the "User.Identity.Name" value
				new Claim(JwtRegisteredClaimNames.Sub, login),
                new Claim(JwtRegisteredClaimNames.Email, login),
                new Claim(JwtRegisteredClaimNames.Jti, login) // NOTE: this could a unique ID assigned to the user by a database
			};

            var token = new JwtSecurityToken(issuer: "domain.com", audience: "domain.com", claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials); // NOTE: ENTER DOMAIN HERE
            var jsth = new JwtSecurityTokenHandler();
            return jsth.WriteToken(token);
        }


        [HttpPut("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            JsonNode json = JsonSerializer.Deserialize<JsonNode>(Request.Body);
            byte[] login = Base64UrlTextEncoder.Decode(json["login"].ToString());
            string email = Base64Url.Decode(json["Email"].ToString());
            if(SQLquery.CreateData(login, email)!=null) return StatusCode(StatusCodes.Status409Conflict);
            return StatusCode(200);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            JsonNode json = JsonSerializer.Deserialize<JsonNode>(Request.Body);
            byte[] login = Base64UrlTextEncoder.Decode(json["login"].ToString());
            List<Models.AccountModel> i = new List<Models.AccountModel>();
            if ((i = await SQLquery.SearchData(login)) != null) {
                return Ok(CreateJWT(Convert.ToBase64String(login)));
            }
            return StatusCode(404);
        }

        [HttpPut("CreateData/{ParentID}/{Url}")]
        public async Task<IActionResult> CreateData(string ParentID, string URL)
        {
            int parentID = Convert.ToInt32(Base64UrlTextEncoder.Decode(ParentID));
            string url = Base64Url.Decode(URL);
            if (SQLquery.CreateData(parentID, url) != null) return StatusCode(StatusCodes.Status409Conflict);
            return StatusCode(200);
        }

        [HttpDelete("DeleteData/{ID}/{NameTable})")]
        public async Task<IActionResult> DeleteData(string ID, string NameTable)
        {
            int id = Convert.ToInt32(Base64UrlTextEncoder.Decode(ID));
            string nameTable = Base64Url.Decode(NameTable);
            if (SQLquery.DeleteData(id, nameTable) != null) return StatusCode(StatusCodes.Status409Conflict);
            return StatusCode(200);
        }
    }
}
