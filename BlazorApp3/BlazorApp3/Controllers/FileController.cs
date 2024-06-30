using BlazorApp3.Models;
using BlazorApp3.Modules;
using BlazorApp3.Modules.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
            byte[] login;
            string path;
            try
            {
                login = Encoding.UTF8.GetBytes(Request.Headers["Login"]);
                path = Request.Headers["Path"];
            }
            catch { return BadRequest(); }
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
            var info = new FileInfo(Vars.Path(login) + file.Route);
            Response.StatusCode = 200;
            Response.Headers.Append("FileName", info.Name);
            Response.Headers.Append("FileCreationTime", info.CreationTime.ToString());
            Response.Headers.Append("FileAttributes", info.Attributes.ToString());
            await Response.SendFileAsync(Vars.host + file.Route);
            return Ok();


        }

        //Загрузка файла на сервер
        [HttpPost("/upload")]
        [Authorize]
        public async Task<IActionResult> Upload()
        {
            byte[] login;
            string path;
            try
            {
                login = Encoding.UTF8.GetBytes(Request.Headers["Login"]);
                path = Request.Headers["Path"];
            }
            catch { return BadRequest("Bad account info"); }
            string filePath = Vars.Path(login) + path;
            Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('\\') - 1));
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await Request.Body.CopyToAsync(file);
                file.Close();
                var info = new FileInfo(filePath);
                try
                {
                    info.CreationTime = DateTime.Parse(Request.Headers["FileCreationTime"]);
                    info.Attributes = (FileAttributes)Convert.ToInt32(Request.Headers["FileAttributes"]);
                }
                catch
                {
                    return BadRequest("Bad file info");
                }
            }
            if (await SQLquery.CreateData(SQLquery.SearchData(login).Result[0].Id, path) != null)
                return Problem("Can't add file to database");
            return Ok();
        }

        //Удаление файла
        [HttpDelete("/delete")]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            byte[] login;
            string path;
            try
            {
                login = Encoding.UTF8.GetBytes(Request.Headers["Login"]);
                path = Request.Headers["Path"];
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
            int ObjIndex = filesystem.FindIndex(x => x.Route == path);
            if(ObjIndex == -1)
                return NotFound("File or folder not found");
            try
            {
                if (filesystem[ObjIndex].Route.Last() == '\\')
                    Directory.Delete(Vars.Path(login) + filesystem[ObjIndex].Route, true);
                else
                    System.IO.File.Delete(Vars.Path(login) + filesystem[ObjIndex].Route);
            }
            catch
            {
                return Problem("Error was accured while file or directory try to delete");
            }
            return Ok("Deleted was succesful");
        }
    }
}
