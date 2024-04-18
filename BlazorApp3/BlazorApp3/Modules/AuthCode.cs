namespace BlazorApp3.Models
{
    public static class AuthCode
    {
        public static async Task<string> GenerateCode(int seed)
        {
            string result = "";
            Random rand = new Random(seed);
            for(int i = 0;i<6;i++)
            {
                if (rand.Next(0, 1) == 0)
                    result += rand.Next('A', 'Z');
                else result += rand.Next('0', '9');
            }
            return result;
        }
    }
}
