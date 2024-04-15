using BlazorApp3.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController : ControllerBase 
    {
        private byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes("123"));
        string host = "localhost:";

        [HttpGet("Link/{login}")]
        public async Task<IActionResult> Link(string login) {
            login = EncryptionHelper.DecryptString(login,key);

                }

        [HttpGet("{mail}/{login}")]
        public async Task<IActionResult> SendMail(string mail, string login)
        {
            System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create(); 
            aes.Mode = CipherMode.CBC;
            aes.Key = key;
            aes.GenerateIV();
            string link = host + "/api/Link/" + EncryptionHelper.EncryptString(login, key);
            string body;
            using (FileStream fs = new FileStream("GmailAPI/GmailAttachment/gmailReqest.html", FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, (int)fs.Length);
                body = Encoding.UTF8.GetString(buffer);
            }

            var byteArray = "verification link VNCloud";
            body = body.Replace("EmailInsert", mail);
            body = body.Replace("KeyInsert", link);


            GmailMessage message = new GmailMessage(byteArray,new List<string> { mail },body);
            GmailSendService GSS = new GmailSendService("VNCloud");
            await GSS.SendMailAsync(message);
            return StatusCode(200);
        }
    }
}
