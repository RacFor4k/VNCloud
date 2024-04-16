using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BlazorApp3.Modules;
using Mysqlx.Crud;
using System.Text.Json.Nodes;
using System.Text.Json;
namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        [HttpPut("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            JsonNode json = JsonSerializer.Deserialize<JsonNode>(Request.Body);
            byte[] login = Base64UrlTextEncoder.Decode(json["login"].ToString());
            string email = Base64Url.Decode(json["Email"].ToString());
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
