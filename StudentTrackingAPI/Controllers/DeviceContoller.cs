using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;
using common;
using System.Net.Mail;
using System.Net;
using System.Text.Json;

namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [ExampleFilterAttribute]
    public class DeviceContoller : ControllerBase
    {
        // public IConfiguration _configuration;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeviceContoller> _logger;
        public readonly IDeviceService _devicemaster;
        private static GpsTcpListener _gpsListener;

        public DeviceContoller(ILogger<DeviceContoller> logger, IConfiguration configuration, IDeviceService devicemaster)
        {
            _logger = logger;
            _configuration = configuration;
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


        [HttpGet("Get")]
        public async Task<IActionResult> Getprinciple([FromQuery] DeviceDto user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }

            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _devicemaster.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] DeviceDto user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.Id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _devicemaster.DeviceValue(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeletePrinciple([FromBody] DeviceDto model)
        {
            try
            {

                if (model.BaseModel == null)
                {
                    model.BaseModel = new BaseModel();
                }
                model.BaseModel.OperationType = "Delete";

                var result = await _devicemaster.DeviceValue(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
         [HttpGet("{imei}")]
    public IActionResult GetLocation(string imei)
    {
        // TODO: Fetch latest location from DB
        return Ok(new
        {
            IMEI = imei,
            Latitude = "22.5726",
            Longitude = "88.3639",
            LastUpdate = DateTime.UtcNow
        });
    }
        [HttpGet("start")]
        public IActionResult StartListener()
        {
            if (_gpsListener == null)
            {
                _gpsListener = new GpsTcpListener(9000);
                _ = _gpsListener.StartAsync(); // fire and forget
                return Ok("✅ GPS TCP Listener started on port 9000");
            }
            else
            {
                return Ok("⚠️ GPS TCP Listener is already running");
            }
        }

        //[HttpGet("GetGpswoxDevices")]
        //public async Task<IActionResult> GetGpswoxDevices()
        //{
        //    try
        //    {
        //        string baseUrl = _configuration["Gpswox:BaseUrl"];
        //        string hash = _configuration["Gpswox:Hash"];
        //        string encodedHash = Uri.EscapeDataString(hash);

        //        string url = $"{baseUrl}get_devices?user_api_hash={encodedHash}&lang=en";

        //        using (HttpClient client = new HttpClient())
        //        {
        //            var response = await client.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                return StatusCode((int)response.StatusCode, "Error fetching devices from GPSWOX");
        //            }

        //            string json = await response.Content.ReadAsStringAsync();

        //            // ✅ Deserialize into your DeviceDto model
        //            var devices = JsonSerializer.Deserialize<List<DeviceDto>>(json,
        //                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //            return Ok(devices);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching devices from GPSWOX");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //[HttpGet("GetGpswoxDevices")] DeviceDto device
        //public async Task<IActionResult> GetGpswoxDevices()
        //{
        //    try
        //    {
        //        string baseUrl = _configuration["Gpswox:BaseUrl"];
        //        string hash = _configuration["Gpswox:Hash"];
        //        string encodedHash = Uri.EscapeDataString(hash);

        //        string url = $"{baseUrl}get_devices?user_api_hash={encodedHash}&lang=en";

        //        using (HttpClient client = new HttpClient())
        //        {
        //            var response = await client.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                return StatusCode((int)response.StatusCode, "Error fetching devices from GPSWOX");
        //            }

        //            string json = await response.Content.ReadAsStringAsync();

        //            // Step 1: Deserialize into GPSWOX's structure (groups -> items)
        //            var groups = JsonSerializer.Deserialize<List<DeviceGroup>>(json,
        //                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //            if (groups == null || groups.Count == 0)
        //                return Ok(new List<DeviceDto>());

        //            // Step 2: Flatten group.items into your DeviceDto
        //            var devices = groups
        //                .SelectMany(g => g.Items)
        //                .Select(d => new DeviceDto
        //                {
        //                    id = d.Id,
        //                    name = d.Name,
        //                    imei = d.Device_Data?.Imei,
        //                    sim_number = d.Device_Data?.Sim_Number,
        //                    status = d.Online,
        //                    lat = d.Lat,
        //                    lng = d.Lng,
        //                    speed = d.Speed,
        //                    altitude = d.Altitude,
        //                    time = d.Time,
        //                    power = d.Power,
        //                    address = d.Address,
        //                    protocol = d.Protocol,
        //                    driver = d.Driver
        //                })
        //                .ToList();

        //            // Step 3: Return your DTO list
        //            return Ok(devices);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching devices from GPSWOX");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}


    }


}
