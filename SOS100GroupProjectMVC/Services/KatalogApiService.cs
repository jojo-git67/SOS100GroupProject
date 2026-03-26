using Microsoft.Extensions.Options;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Services
{
    public class KatalogApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceRegistrySettings _settings;

        public KatalogApiService(HttpClient httpClient, IOptions<ServiceRegistrySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> GetAllCoursesAsync()
        {
            var url = $"{_settings.Katalogtjansten}/api/courses";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetCourseByIdAsync(int id)
        {
            var url = $"{_settings.Katalogtjansten}/api/courses/{id}";
            return await _httpClient.GetStringAsync(url);
        }
    }
}