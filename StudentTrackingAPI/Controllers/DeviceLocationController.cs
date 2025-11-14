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
        public async Task<IActionResult> GetDevicesWithAddress()
        {
            try
            {
                string baseUrl = _configuration["Gpswox:BaseUrl"];
                string hash = Uri.EscapeDataString(_configuration["Gpswox:Hash"]);

                string url = $"{baseUrl}get_devices?user_api_hash={hash}&lang=en";

                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, $"Error fetching devices from GPSWOX: {response.StatusCode}");

                string json = await response.Content.ReadAsStringAsync();

                List<GpswoxDevice>? devices;
                try
                {
                    devices = JsonSerializer.Deserialize<List<GpswoxDevice>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (Exception jsonEx)
                {
                    return BadRequest(new
                    {
                        Message = "Failed to parse GPSWOX response",
                        JsonError = jsonEx.Message,
                        RawResponse = json
                    });
                }

                if (devices == null || devices.Count == 0)
                    return NotFound("No devices found from GPSWOX.");

                // ✅ Enrich with address and power
                foreach (var d in devices)
                {
                    if (d.Parameters?.Power != null)
                        d.Power = d.Parameters.Power;

                    if (d.Items != null)
                    {
                        foreach (var item in d.Items)
                        {
                            try
                            {
                                item.Address = await _geocoding.GetAddressAsync(item.Lat, item.Lng);
                                if (item.Parameters?.Power != null)
                                    item.Power = item.Parameters.Power;
                            }
                            catch
                            {
                                item.Address = "Geocoding failed";
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            d.Address = await _geocoding.GetAddressAsync(d.Lat, d.Lng);
                        }
                        catch
                        {
                            d.Address = "Geocoding failed";
                        }
                    }
                }

                // ✅ Build DataTable
                DataTable dataTable = new DataTable("GpsDeviceData");
                dataTable.Columns.Add("Name", typeof(string));
                dataTable.Columns.Add("Online", typeof(string));
                dataTable.Columns.Add("Time", typeof(string));
                dataTable.Columns.Add("Timestamp", typeof(string));
                dataTable.Columns.Add("Lat", typeof(string));
                dataTable.Columns.Add("Lng", typeof(string));
                dataTable.Columns.Add("Speed", typeof(string));
                dataTable.Columns.Add("Altitude", typeof(string));
                dataTable.Columns.Add("Address", typeof(string));
                dataTable.Columns.Add("Power", typeof(string));
                dataTable.Columns.Add("Battery", typeof(string));
                //dataTable.Columns.Add("Protocol", typeof(string));
                dataTable.Columns.Add("Driver", typeof(string));



                foreach (var device in devices)
                {
                    
                        foreach (var item in device.Items)
                        {
                            dataTable.Rows.Add(
                                device.Name ?? item.Name ?? "",
                                item.Online ?? device.Online ?? "",
                                item.Time ?? device.Time ?? "",
                                item.Timestamp.ToString() ?? device.Timestamp?.ToString() ?? "",
                                item.Lat.ToString(),
                                item.Lng.ToString(),
                                item.Speed.ToString(),
                                item.Altitude?.ToString() ?? "0",
                                item.Address ?? device.Address ?? "",
                                item.Power ?? device.Power ?? "",
                                item.Battery?.ToString() ?? device.Battery?.ToString() ?? "",
                                item.Driver ?? device.Driver ?? ""
                            );
                        }
                    
                }


                // ✅ Prepare DTO for DB insert
                var user = new GpswoxDevice
                {
                   BaseModel = new BaseModel
                    {
                        OperationType = "InsertGpsdata"
                    }
                };
                user.DataTable = dataTable;
                // ✅ Insert into DB
                var createduser = await _devicemaster.DevicedataValue(user);

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching devices with address");
                return StatusCode(500, new
                {
                    Message = "Internal Server Error",
                    Exception = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
        [HttpGet("GetDeviceHistory")]
        public async Task<IActionResult> GetDeviceHistory( int device_id,string from_date,string from_time,string to_date,
                                                            string to_time, bool snap_to_road = true, string lang = "en")
        {
            try
            {
                string baseUrl = _configuration["Gpswox:BaseUrl"]; // "https://api.gpswox.com/api/"
                string hash = Uri.EscapeDataString(_configuration["Gpswox:Hash"]);

                string url = $"{baseUrl}get_history?" +
                             $"user_api_hash={hash}" +
                             $"&lang=en" +
                             $"&device_id={device_id}" +
                             $"&from_date={from_date}" +
                             $"&from_time={from_time}" +
                             $"&to_date={to_date}" +
                             $"&to_time={to_time}" +
                             $"&snap_to_road={(snap_to_road ? "true" : "false")}";


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
                var data = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                
                return Content(jsonResponse, "application/json");
                //return Ok(new
                //{
                //    Message = "Data fetched successfully",
                //    Data = data

                //});
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

         
        //[HttpGet("GetDeviceHistory")]
        //public async Task<IActionResult> GetDeviceHistory([FromQuery] GpswoxDevice user)
        //{
        //    try
        //    {
        //        string baseUrl =  _configuration["Gpswox:BaseUrl"];
        //        string hash = Uri.EscapeDataString(_configuration["Gpswox:Hash"]);
        //        using (var httpClient = new HttpClient())
        //        {
        //            user.device_id = 12345;        // your actual device ID
        //            user.from_date = "2025-10-01"; // YYYY-MM-DD
        //            user.from_time = "00:00:00";
        //            user.to_date = "2025-10-09";
        //            user.to_time = "23:59:59"; 
        //            user.snap_to_road = true;
        //            string url = $"{baseUrl}get_history?user_api_hash={hash}&lang=en&device_id={user.device_id}&from_date={user.from_date}&from_time={user.from_time}&to_date={user.to_date}&snap_to_road={user.snap_to_road}";

        //            using var client = new HttpClient();
        //            var response = await client.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                return StatusCode((int)response.StatusCode, new
        //                {
        //                    Message = $"Failed to call external API",
        //                    Status = response.StatusCode
        //                });
        //            }

        //            // Read the response body
        //            string jsonResponse = await response.Content.ReadAsStringAsync();

        //            // If you know what type of data the API returns, define a class and deserialize
        //            // Example: List<HistoryData> history = JsonSerializer.Deserialize<List<HistoryData>>(jsonResponse);
        //            // Otherwise, just return raw JSON
        //            return Ok(new
        //            {
        //                Message = "Data fetched successfully",
        //                Data = JsonSerializer.Deserialize<object>(jsonResponse)
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            Message = "Internal Server Error",
        //            Exception = ex.Message
        //        });
        //    }
        //}

    }



}
