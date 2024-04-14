using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController : ControllerBase 
    {
        [HttpGet("{mail}/{key}")]
        public async Task<IActionResult> SendMail(string mail, string key)
        {
            string body;
            using (FileStream fs = new FileStream("GmailAPI/GmailAttachment/gmailReqest.html", FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, (int)fs.Length);
                body = Encoding.UTF8.GetString(buffer);
            }
            var byteArray = "verification key VNCloud";
            body = body.Replace("EmailInsert", mail);
            body = body.Replace("KeyInsert", key);


            GmailMessage message = new GmailMessage(byteArray,new List<string> { mail },body);
            GmailSendService GSS = new GmailSendService("VNCloud");
            await GSS.SendMailAsync(message);
            return StatusCode(200);
        }
    }
}
