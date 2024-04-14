using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace BlazorApp3
{
    public class GmailMessage
    {
        public string Subject { get; set; }
        public IEnumerable<string> TO { get; set; }
        public IEnumerable<string> CC { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public string HtmlBody { get; set; }

        public GmailMessage(string subject, IEnumerable<string> to, string htmlBody)
        {
            Subject = subject;
            TO = to;
            CC = null;
            Bcc = null;
            HtmlBody = htmlBody;
        }
        public GmailMessage(string subject, IEnumerable<string> to, IEnumerable<string> cc, string htmlBody)
        {
            Subject = subject;
            TO = to;
            CC = cc;
            Bcc = null;
            HtmlBody = htmlBody;
        }
        public GmailMessage(string subject, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string htmlBody)
        {
            Subject = subject;
            TO = to;
            CC = cc;
            Bcc = bcc;
            HtmlBody = htmlBody;
        }
    }
    public class GmailSendService
    {
        public string SenderName { get; set; }
        private GmailService GmailService { get; set; }
        private Message InternalMessage { get; set; }

        //User Inputs
        static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        static string ApplicationName = "Gmail API";
        static string SenderEmail = "ndtp.vncloud@gmail.com";

        public GmailSendService(string senderName)
        {
            SenderName = senderName;
            UserCredential credential;
            string gmailPath = "GmailAPI/ClientCredentials";
            using (var stream = new FileStream($"{gmailPath}/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = $"{gmailPath}/credentials.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            // Create Gmail API service.
            GmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        public async Task SendMailAsync(GmailMessage message)
        {
            try
            {
                GetSendMessage(message);
                await GmailService.Users.Messages.Send(InternalMessage, "me").ExecuteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SendMail(GmailMessage message)
        {
            try
            {
                GetSendMessage(message);
                GmailService.Users.Messages.Send(InternalMessage, "me").Execute();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void GetSendMessage(GmailMessage gmailMessage)
        {
            string plainText = $"From:{SenderName}<{SenderEmail}>\r\n" +
                               $"To:{GenerateReceipents(gmailMessage.TO)}\r\n" +
                               $"CC:{GenerateReceipents(gmailMessage.CC)}\r\n" +
                               $"Bcc:{GenerateReceipents(gmailMessage.Bcc)}\r\n" +
                               $"Subject:{gmailMessage.Subject}\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                               $"{gmailMessage.HtmlBody}";

            Message message = new Message();
            message.Raw = Encode(plainText.ToString());
            InternalMessage = message;
        }
        private string GenerateReceipents(IEnumerable<string> receipents)
        {
            return receipents == null ? string.Empty : string.Join(",", receipents);
        }
        private string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            //System.Convert.
            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}