﻿using BlazorApp3.Modules;
using BlazorApp3.Client.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using BlazorApp3.Models;
using BlazorApp3.Modules;

namespace BlazorApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmailController : ControllerBase 
    {
        private byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes("123"));
        string host = "localhost:";

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            JsonObject json;
            json = JsonNode.ParseAsync(Request.Body).Result.AsObject();
            byte[] login = Encoding.ASCII.GetBytes( json["login"].GetValue<string>());
            string mail = json["email"].GetValue<string>();
            string body;
            string code = await AuthCode.GenerateCode(DateTime.Now.Nanosecond, login);
            using (FileStream fs = new FileStream("GmailAPI/GmailAttachment/gmailReqest.html", FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, (int)fs.Length);
                body = Encoding.UTF8.GetString(buffer);
            }

            var byteArray = "verification link VNCloud";
            body = body.Replace("EmailInsert", mail);
            body = body.Replace("KeyInsert", code);


            GmailMessage message = new GmailMessage(byteArray,new List<string> { mail },body);
            GmailSendService GSS = new GmailSendService("VNCloud");
            await GSS.SendMailAsync(message);
            return StatusCode(200);
        }
    }
}
