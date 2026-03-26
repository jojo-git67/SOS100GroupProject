using Microsoft.Extensions.Options;
using SOS100GroupProjectMVC.Models;
using System.Net.Http.Json;

namespace SOS100GroupProjectMVC.Services
{
    public class RumsbokningApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceRegistrySettings _settings;

        public RumsbokningApiService(HttpClient httpClient, IOptions<ServiceRegistrySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> GetAllBookingsAsync()
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/roombookings";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBookingByIdAsync(int id)
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/roombookings/{id}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetAllRoomsAsync()
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/rooms";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<HttpResponseMessage> CreateBookingAsync(object booking)
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/roombookings";
            return await _httpClient.PostAsJsonAsync(url, booking);
        }

        public async Task<HttpResponseMessage> UpdateBookingAsync(int id, object booking)
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/roombookings/{id}";
            return await _httpClient.PutAsJsonAsync(url, booking);
        }

        public async Task<HttpResponseMessage> DeleteBookingAsync(int id)
        {
            var url = $"{_settings.Rumsbokningstjansten}/api/roombookings/{id}";
            return await _httpClient.DeleteAsync(url);
        }
    }
}