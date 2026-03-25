using Microsoft.Extensions.Options;
using SOS100GroupProjectMVC.Models;
using System.Net.Http.Json;

namespace SOS100GroupProjectMVC.Services
{
    public class SchemaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceRegistrySettings _settings;

        public SchemaApiService(HttpClient httpClient, IOptions<ServiceRegistrySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> GetAllActivitiesAsync()
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetActivityByIdAsync(int id)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities/{id}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetActivitiesForCourseAsync(int courseId)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities/course/{courseId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetActivitiesForUserAsync(int userId)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities/user/{userId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<HttpResponseMessage> CreateActivityAsync(object activity)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities";
            return await _httpClient.PostAsJsonAsync(url, activity);
        }

        public async Task<HttpResponseMessage> UpdateActivityAsync(int id, object activity)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities/{id}";
            return await _httpClient.PutAsJsonAsync(url, activity);
        }

        public async Task<HttpResponseMessage> DeleteActivityAsync(int id)
        {
            var url = $"{_settings.Schematjansten}/api/ScheduleActivities/{id}";
            return await _httpClient.DeleteAsync(url);
        }
    }
}