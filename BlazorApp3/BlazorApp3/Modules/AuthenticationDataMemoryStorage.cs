using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp3.Modules
{
    public class AuthenticationDataMemoryStorage
    {
        public string Token { get; set; } = "";
    }

    public class VNCloudUserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationDataMemoryStorage _authenticationDataMemoryStorage;

        public VNCloudUserService(HttpClient httpClient, AuthenticationDataMemoryStorage authenticationDataMemoryStorage)
        {
            _httpClient = httpClient;
            _authenticationDataMemoryStorage = authenticationDataMemoryStorage;
        }
    }

    public class VNCloudAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly VNCloudUserService _VNCloudUserService;

        public VNCloudAuthenticationStateProvider(VNCloudUserService VNCloudUserService)
        {
            _VNCloudUserService = VNCloudUserService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
