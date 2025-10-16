using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
 
namespace MyTrackerApp.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAddressAsync(double lat, double lon)
        {
            if (lat == 0 && lon == 0) return "-";

            string url = $"https://nominatim.openstreetmap.org/reverse?lat={lat}&lon={lon}&format=json";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyTrackerApp/1.0");

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return "-";

            string json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("display_name", out var address))
                return address.GetString();

            return "-";
        }
    }
}
