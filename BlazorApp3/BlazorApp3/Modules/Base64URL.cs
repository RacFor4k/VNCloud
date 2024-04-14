namespace BlazorApp3.Modules
{
    using System;

    public class Base64Url
    {
        public static string Encode(string input)
        {
            var bytesToEncode = System.Text.Encoding.UTF8.GetBytes(input);
            string encodedText = Convert.ToBase64String(bytesToEncode)
                .TrimEnd('=') // Удаляем символы '=' в конце строки
                .Replace('+', '-') // Заменяем символ '+' на '-'
                .Replace('/', '_'); // Заменяем символ '/' на '_'
            return encodedText;
        }

        public static string Decode(string input)
        {
            string encodedText = input
                .Replace('-', '+') // Восстанавливаем символ '+'
                .Replace('_', '/'); // Восстанавливаем символ '/'

            // Добавляем символы '=', если необходимо
            int mod4 = encodedText.Length % 4;
            if (mod4 > 0)
            {
                encodedText += new string('=', 4 - mod4);
            }

            var encodedBytes = Convert.FromBase64String(encodedText);
            string originalText = System.Text.Encoding.UTF8.GetString(encodedBytes);
            return originalText;
        }
    }

}
