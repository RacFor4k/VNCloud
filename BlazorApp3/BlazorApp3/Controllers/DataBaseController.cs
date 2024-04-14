using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BlazorApp3.Modules;
using Mysqlx.Crud;
namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        [HttpPut("CreateAccount/{Login}/{Email}/{token}")]
        public async Task<IActionResult> CreateAccount(string Login, string Email, string token)
        {
            
            byte[] login = Base64UrlTextEncoder.Decode(Login);
            string email = Base64Url.Decode(Email);
            if(SQLquery.CreateData(login, email)!=null) return StatusCode(StatusCodes.Status409Conflict);
            return StatusCode(200);
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
