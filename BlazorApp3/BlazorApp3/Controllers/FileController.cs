using BlazorApp3.Models;
using BlazorApp3.Modules;
using BlazorApp3.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        //Скачивание файла с сервера
        [HttpGet("/download")]
        [Authorize]
        public async Task<IActionResult> Download()
        {
            //Получение метаданных из раголовка запроса
            byte[] login = Encoding.UTF8.GetBytes(Request.Headers["Login"]);
            string path = Request.Headers["Path"];

            //Обработка запроса
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
            var file = filesystem.Find(x => x.IsFolder == false && x.Route == path);
            if (file == null)
            {
                Response.StatusCode = 404;
                return Problem("File not found");
            }
            var info = new FileInfo(Vars.host + file.Route);     
            Response.StatusCode = 200;
            Response.Headers.Append("FileName", info.Name);
            Response.Headers.Append("FileCreationTime", info.CreationTime.ToString());
            Response.Headers.Append("FileAttributes", info.Attributes.ToString());
            await Response.SendFileAsync(Vars.host + file.Route);
            return Ok();
            

        }
    }
}
