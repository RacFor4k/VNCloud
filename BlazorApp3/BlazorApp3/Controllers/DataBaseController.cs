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
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BlazorApp3.Models;
using Microsoft.EntityFrameworkCore.Storage;
namespace BlazorApp3.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DataBaseController : ControllerBase
	{

		private byte[] CreateJWT(string login)
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
			return Encoding.UTF8.GetBytes(jsth.WriteToken(token));
		}

		[HttpPost("CreateAccount")]
		public async Task<IActionResult> CreateAccount()
		{
			JsonObject json;
			json = JsonNode.ParseAsync(Request.Body).Result.AsObject();
			byte[] login = Encoding.ASCII.GetBytes(json["login"].GetValue<string>());
			string email = json["email"].GetValue<string>();
			string code = json["code"].GetValue<string>();
			if(!await AuthCode.IsExsist(code, login))
			{
				return StatusCode(StatusCodes.Status406NotAcceptable);
			}
			if(SQLquery.CreateData(login, email)!=null) return StatusCode(StatusCodes.Status409Conflict);
			return StatusCode(200);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login()
		{
			JsonObject json;
			json = JsonNode.ParseAsync(Request.Body).Result.AsObject();
			byte[] login = Encoding.ASCII.GetBytes(json["login"].GetValue<string>());
			List<Models.AccountModel> i = new List<Models.AccountModel>();
			if ((i = await SQLquery.SearchData(login)) != null) {
				return Ok(CreateJWT(Convert.ToBase64String(login)));
			}
			return StatusCode(404);
		}

		[HttpGet("GetData")]
		[Authorize]
		public async Task<IActionResult> GetData()
		{
			JsonObject json;
			json = JsonNode.ParseAsync(Response.Body).Result.AsObject();
			string path = json["path"].GetValue<string>();
			byte[] login = json["login"].GetValue<byte[]>();
			List<RoutesModel> filesystem;
			try
			{
				filesystem = await SQLquery.SearchData(SQLquery.SearchData(login).Result[0].Id);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status406NotAcceptable);
			}
			var files = filesystem.FindAll(x => StringHelper.FirstContains(path, x.Route));
			string parse = "";
			foreach(var file in files)
			{
				parse += file.Route + "?" + Convert.ToString(file.IsFolder)+'\n';
			}
			return Ok(parse);
		}


		//я не знаю зачем это
		[HttpPut("CreateData")]
		[Authorize]
		public async Task<IActionResult> CreateData()
		{
			JsonObject json;
			json = JsonNode.ParseAsync(Response.Body).Result.AsObject();
			int parentID = json["ParentID"].GetValue<int>();
			string url = json["URL"].GetValue<string>();
			if (SQLquery.CreateData(parentID, url) != null) return StatusCode(StatusCodes.Status409Conflict);
			return StatusCode(200);
		}

		[HttpDelete("DeleteData")]
		[Authorize]
		public async Task<IActionResult> DeleteData()
		{
			JsonObject json;
			json = JsonNode.ParseAsync(Response.Body).Result.AsObject();
			int id = json["id"].GetValue<int>();// Convert.ToInt32(Base64UrlTextEncoder.Decode(ID));
			string nameTable = json["NameTable"].GetValue<string>();//Base64Url.Decode(NameTable);
			if (SQLquery.DeleteData(id, nameTable) != null) return StatusCode(StatusCodes.Status409Conflict);
			return StatusCode(200);
		}
	}
}
