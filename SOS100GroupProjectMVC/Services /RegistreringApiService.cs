using Microsoft.Extensions.Options;
using SOS100GroupProjectMVC.Models;
using System.Net.Http.Json;

namespace SOS100GroupProjectMVC.Services
{
    public class RegistreringApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceRegistrySettings _settings;

        public RegistreringApiService(HttpClient httpClient, IOptions<ServiceRegistrySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> GetAllRegistrationsAsync()
        {
            var url = $"{_settings.Registreringstjansten}/api/registrations";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetByUserAsync(int userId)
        {
            var url = $"{_settings.Registreringstjansten}/api/registrations/user/{userId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetByCourseAsync(int courseId)
        {
            var url = $"{_settings.Registreringstjansten}/api/registrations/course/{courseId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<HttpResponseMessage> CreateRegistrationAsync(object registration)
        {
            var url = $"{_settings.Registreringstjansten}/api/registrations";
            return await _httpClient.PostAsJsonAsync(url, registration);
        }

        public async Task<HttpResponseMessage> DeleteRegistrationAsync(int id)
        {
            var url = $"{_settings.Registreringstjansten}/api/registrations/{id}";
            return await _httpClient.DeleteAsync(url);
        }
    }
}