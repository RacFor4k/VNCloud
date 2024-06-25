namespace BlazorApp3.Client.Modules
{
    public static class User
    {
        public static HttpClient HttpClient { get; } = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7225")
        };
        public static int Id { get; set; }
        public static string Jwt { get; set; }
        public static byte[] Login { get; set; }
    }
}
