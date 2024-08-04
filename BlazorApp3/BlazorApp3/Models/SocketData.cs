using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace BlazorApp3.Models
{
    public class SocketData
    {
        public Dictionary<string, string> Headers { get; set; }

        public Content Content { get; set; }

        public SocketData(Dictionary<string, string> headers, Content content)
        {
            Headers = headers;
            Content = content;
        }
        public SocketData(string stringObj)
        {
            Parse(stringObj);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public void Parse(string stringObj)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var parsed = JsonSerializer.Deserialize<SocketData>(stringObj, options);
            Headers = parsed.Headers;
            Content = parsed.Content;
        }


    }

    public class Content
    {
        public ContentType Type { get; set; }
        public long Lenght { get; set; }
        public long ChunkSize { get; set; }

    }

    public enum ContentType
    {
        Text = 1,
        Stream = 2,
        Byte = 3,
        Number = 4
    }
}
