using BlazorApp3.Models;
using BlazorApp3.Modules; 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : Controller
    {
        [HttpGet("getdata")]
        public async Task<IActionResult> GetData()
        {
            byte[] login;
            try
            {
                login = Encoding.UTF8.GetBytes(Request.Headers["Login"]);
            }
            catch { return BadRequest("Bad request"); }
            List<RoutesModel> filesystem;
            try
            {
                filesystem = await SQLquery.SearchData(SQLquery.SearchData(login).Result[0].Id);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 406;
                return Problem("DataBase error");
            }
            string s =  JsonConvert.SerializeObject(filesystem);
            return Ok(s);

        }
    }
}
