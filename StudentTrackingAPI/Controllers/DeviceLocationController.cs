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
using System.Threading.Tasks;

namespace MyTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [ExampleFilterAttribute]
    public class DeviceLocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GeocodingService _geocoding;
        private readonly ILogger<DeviceLocationController> _logger;
        public readonly IDeviceService _devicemaster;
        public DeviceLocationController(IConfiguration configuration, GeocodingService geocoding, ILogger<DeviceLocationController> logger, IDeviceService devicemaster)
        {
            _configuration = configuration;
            _geocoding = geocoding;
            _logger = logger;
            _devicemaster = devicemaster;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] DeviceDto user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetAll";
                var createduser = await _devicemaster.DeviceValue(user);
                return createduser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet("GetDevicesWithAddress")]
        public async Task<IActionResult> GetDevicesWithAddress([FromQuery]  GpswoxDevice user)
        {
            try
            {
                string baseUrl = _configuration["Gpswox:BaseUrl"];
                string hash = user.HashKey; //Uri.EscapeDataString(_configuration["Gpswox:Hash"]);


                string url = $"{baseUrl}get_devices?user_api_hash={hash}&lang=en";

                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Error fetching devices from GPSWOX");

                string json = await response.Content.ReadAsStringAsync();

                var devices = JsonSerializer.Deserialize<List<GpswoxDevice>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // ✅ Enrich with address
                foreach (var d in devices)
                {
                    if (d.Items != null)
                    {
                        foreach (var item in d.Items)
                        {
                            item.Address = await _geocoding.GetAddressAsync(item.Lat, item.Lng);
                        }
                    }
                    else
                    {
                        d.Address = await _geocoding.GetAddressAsync(d.Lat, d.Lng);
                    }
                }

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching devices with address");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

    public class DeviceItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Online { get; set; }
        public string Time { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
    }
}
