using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Text;


namespace BlazorApp3.Client.Modules
{
    public class EncoderStream{

        //private MemoryStream _stream = new MemoryStream();
        private Encoder _encoder;
        private Stream _stream;

        public EncoderStream(Stream stream, Encoder encoder)
        {
            _stream = stream;
            _encoder = encoder;
         
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await _encoder.Encode(buffer);
            await _stream.WriteAsync(buffer, offset, count);

        }

        public async Task ReadAsync(byte[] buffer, int offset, int count)
        {
            await _stream.ReadAsync(buffer, offset, count);
            await _encoder.Decode(buffer);
        }

        public Stream GetStream()
        {
            return _stream;
        }

        public async Task CopyToAsync(Stream destination)
        {
            await _stream.CopyToAsync(destination);
        }
        

    }
    public class Encoder
    {
        public Encoder(IJSRuntime runtime, byte[] encoderKey)
        {
            _runtime = runtime;
            _encoderKey = encoderKey;

        }

        private IJSRuntime _runtime;

        private byte[] _encoderKey;

        public async Task<byte[]> Encode(byte[] buffer)
        {
            buffer = Encoding.UTF8.GetBytes(await _runtime.InvokeAsync<string>("encryptAES", [buffer, _encoderKey, _encoderKey]));
            return buffer;
        }

        public async Task<byte[]> Decode(byte[] buffer)
        {
            buffer = Encoding.UTF8.GetBytes(await _runtime.InvokeAsync<string>("dencryptAES", [buffer, _encoderKey, _encoderKey]));
            return buffer;
        }
    }
}
