using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Text;


namespace BlazorApp3.Client.Modules
{
    public class EncoderStream : MemoryStream{

        //private MemoryStream _stream = new MemoryStream();
        private Encoder _encoder;

        public EncoderStream(Encoder encoder)
        {
            _encoder = encoder;
         
        }

        public EncoderStream(IJSRuntime runtime, byte[] encoderKey)
        {
            _encoder = new Encoder(runtime, encoderKey);
        }

        public MemoryStream GetDecodeStream()
        {
            return new MemoryStream(base.ToArray());
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            var i = base.Read(buffer, offset, length);
            _encoder.Decode(buffer);
            return i;
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            _encoder.Encode(buffer);
            base.Write(buffer, offset, length);
        }

        public async Task ReadAsync(byte[] buffer, int offset, int length)
        {
            await base.ReadAsync(buffer, offset, length);
            await _encoder.Decode(buffer);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int length)
        {
            await _encoder.Encode(buffer);
            await base.WriteAsync(buffer, offset, length);
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

        public async Task Encode(byte[] buffer)
        {
            string str = Encoding.ASCII.GetString(buffer);
            string encoderKey = Encoding.ASCII.GetString(_encoderKey);
            await _runtime.InvokeAsync<byte[]>("encryptAES", [str, encoderKey]);
        }

        public async Task Decode(byte[] buffer)
        {
            string str = Encoding.ASCII.GetString(buffer);
            string encoderKey = Encoding.ASCII.GetString(_encoderKey);
            await _runtime.InvokeAsync<byte[]>("dencryptAES", [str, encoderKey]);
        }
    }
}
