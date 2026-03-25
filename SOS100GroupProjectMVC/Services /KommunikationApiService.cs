using Microsoft.Extensions.Options;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Services
{
    public class KommunikationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceRegistrySettings _settings;

        public KommunikationApiService(HttpClient httpClient, IOptions<ServiceRegistrySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> GetMessagesForUserAsync(int userId)
        {
            var url = $"{_settings.Kommunikationstjansten}/api/Kommunication/user/{userId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetMessageByIdAsync(int messageId)
        {
            var url = $"{_settings.Kommunikationstjansten}/api/Kommunication/messages/{messageId}";
            return await _httpClient.GetStringAsync(url);
        }
    }
}