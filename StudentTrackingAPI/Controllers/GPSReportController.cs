using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTrackerApp.Services;
using System;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Data;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;


namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [ExampleFilterAttribute]

    public class GPSReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GeocodingService _geocoding;
        private readonly ILogger<GPSReportController> _logger;
        public readonly IDeviceService _devicemaster;
        public GPSReportController(IConfiguration configuration, GeocodingService geocoding, ILogger<GPSReportController> logger, IDeviceService devicemaster)
        {
            _configuration = configuration;
            _geocoding = geocoding;
            _logger = logger;
            _devicemaster = devicemaster;
        }

        [HttpGet("GetDeviceHistory")]
        public async Task<IActionResult> GetDeviceHistory(
    string title, string type, string format, string devices, string date_from,
    string date_to, string lang, string daily, string weekly, string monthly, string send_to_email)
        {
            try
            {
                string baseUrl = _configuration["Gpswox:BaseUrl"];
                string hash = Uri.EscapeDataString(_configuration["Gpswox:Hash"]);

                // Convert "1,2,3" → "devices[]=1&devices[]=2&devices[]=3"
                var deviceArray = devices.Split(',')
                                         .Select(d => $"devices[]={d}")
                                         .ToArray();
                string deviceQuery = string.Join("&", deviceArray);

                string url = $"{baseUrl}generate_report?" +
                             $"user_api_hash={hash}" +
                             $"&lang=en" +
                             $"&title={title}" +
                             $"&type={type}" +
                             $"&format={format}" +  // must be json/pdf/html/xls
                             $"&devices[]={devices}" +    // <-- array format
                             $"&date_from={date_from}" +
                             $"&date_to={date_to}" +
                             $"&daily={daily}" +
                             $"&weekly={weekly}" +
                             $"&monthly={monthly}" +
                             $"&send_to_email={send_to_email}";

                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        Message = $"Failed to call external API",
                        Status = response.StatusCode
                    });
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return Content(jsonResponse, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Internal Server Error",
                    Exception = ex.Message
                });
            }
        }

    }
}
