using BlazorApp3.Components.Pages;
using BlazorApp3.Models;
using BlazorApp3.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudController : ControllerBase
    {
        private const string host = "C:/Prifiles";
        [HttpPost("UploadFile/{Path}/{Login}/{Token}")]
        public async Task<IActionResult> UploadFile(string Path, string Login, string Token)
        {
            string path;
            byte[] login;
            byte[] token;
            try
            {
                path = Base64Url.Decode(Path);
                login = Base64UrlTextEncoder.Decode(Login);
                //token = Base64UrlTextEncoder.Decode(Token);
                token = new byte[32];
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            List<RoutesModel> filesystem;
            try
            {
                filesystem = await SQLquery.SearchData(SQLquery.SearchData(login).Result[0].Id);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            int temp_i = 0;
            string temp_path = path;
            while (filesystem.FindIndex(x => x.Route == temp_path) != -1)
            {
                temp_i = 0;
                int temp = path.LastIndexOf('.') == -1 ? path.Length - 1 : path.LastIndexOf('.');
                string extension = path.Substring(temp, path.Length - temp);
                temp_path = temp_path.Substring(0, temp);
                temp_path += '(' + Convert.ToString(temp_i) + ").";
                temp_path += extension;
            }
            temp_path = host+Login+temp_path;
            Directory.CreateDirectory(Path);
            Directory.CreateDirectory(temp_path.Substring(0, temp_path.LastIndexOf('/')));
            int offset = 0;
            using(FileStream fs = new FileStream(temp_path, FileMode.CreateNew))
            {
                byte[] buffer = new byte[4096];
                int length = Request.Body.Read(buffer,offset,4096);
                fs.Write(buffer, offset, length);
                offset += length;
            }
            return StatusCode(200);
        }

        [HttpPost("CreateFolder/{Path}/{Login}/{Token}")]
        public async Task<IActionResult> CreateFolder(string Path, string Login, string Token)
        {
            string path;
            byte[] login;
            byte[] token;
            try
            {
                path = Base64Url.Decode(Path);
                login = Base64UrlTextEncoder.Decode(Login);
                //token = Base64UrlTextEncoder.Decode(Token);
                token = new byte[32];
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            List<RoutesModel> filesystem;
            try
            {
                filesystem = await SQLquery.SearchData(SQLquery.SearchData(login).Result[0].Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            if (filesystem.FindIndex(x => x.Route == path && x.IsFolder == true) != -1)
                return Conflict("409.1;The folder already exist");
            if (await SQLquery.CreateData(SQLquery.SearchData(login).Result[0].Id, host + Login + path, true) == null)
            {
                return Conflict("409.2;Connot create a new folder");
            }
            return StatusCode(200);
        }
    }
}
