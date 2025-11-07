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
                 dataTable.Columns.Add("Lat", typeof(double));
                dataTable.Columns.Add("Lng", typeof(double));
                dataTable.Columns.Add("Speed", typeof(int));
                dataTable.Columns.Add("Altitude", typeof(double));
                dataTable.Columns.Add("Address", typeof(string));
                dataTable.Columns.Add("Power", typeof(string));
                dataTable.Columns.Add("Battery", typeof(string));
                dataTable.Columns.Add("Protocol", typeof(string));
                dataTable.Columns.Add("Driver", typeof(string));



                foreach (var d in devices)
                {
                    double safeAltitude = 0;
                    if (d.Altitude.HasValue)
                        safeAltitude = d.Altitude.Value;

                    dataTable.Rows.Add(
                        d.Name ?? "",
                        d.Online ?? "",
                        d.Time ?? "",
                        d.Timestamp,
                        d.Lat,
                        d.Lng,
                        d.Speed  ,
                        d.Address ?? "",
                        safeAltitude,
                        d.Power ?? "",
                        d.Battery  ,
                        d.Driver ?? ""


                    );
                }

                // ✅ Prepare DTO for DB insert
                var user = new GpswoxDevice
                {
                    DataTable = dataTable,
                    BaseModel = new BaseModel
                    {
                        OperationType = "InsertGpsdata"
                    }
                };

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

        //[HttpGet("GetDevicesWithAddress")]
        //public async Task<IActionResult> GetDevicesWithAddress([FromQuery] GpswoxDevice user)
        //{
        //    try
        //    {
        //        string baseUrl = _configuration["Gpswox:BaseUrl"];
        //        string hash = Uri.EscapeDataString(_configuration["Gpswox:Hash"]);
        //        string url = $"{baseUrl}get_devices?user_api_hash={hash}&lang=en";

        //        using var client = new HttpClient();
        //        var response = await client.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //            return StatusCode((int)response.StatusCode, "Error fetching devices from GPSWOX");

        //        string json = await response.Content.ReadAsStringAsync();

        //        // ✅ Parse JSON safely
        //        using var document = JsonDocument.Parse(json);
        //        var root = document.RootElement;
        //        var devices = new List<GpswoxDevice>();

        //        // ✅ Prepare DataTable
        //        DataTable dataTable = new DataTable();
        //        dataTable.Columns.Add("Name", typeof(string));
        //        dataTable.Columns.Add("Online", typeof(string));
        //        dataTable.Columns.Add("Time", typeof(string));
        //        dataTable.Columns.Add("Timestamp", typeof(string));
        //        dataTable.Columns.Add("Lat", typeof(double));
        //        dataTable.Columns.Add("Lng", typeof(double));
        //        dataTable.Columns.Add("Speed", typeof(string));
        //        dataTable.Columns.Add("Altitude", typeof(string));
        //        dataTable.Columns.Add("Power", typeof(string));
        //        dataTable.Columns.Add("Address", typeof(string));

        //        // ✅ Loop through devices
        //        foreach (var element in root.EnumerateArray())
        //        {
        //            var device = new GpswoxDevice
        //            {
        //                Name = element.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null,
        //                Online = element.TryGetProperty("online", out var onlineProp) ? onlineProp.GetString() : null,
        //                Time = element.TryGetProperty("time", out var timeProp) ? timeProp.GetString() : null,
        //                timestamp = element.TryGetProperty("timestamp", out var timestampProp) ? timestampProp.GetString() : null,
        //                Power = element.TryGetProperty("power", out var powerProp) ? powerProp.GetString() : null,
        //                Lat = element.TryGetProperty("lat", out var latProp) ? latProp.GetDouble() : 0,
        //                Lng = element.TryGetProperty("lng", out var lngProp) ? lngProp.GetDouble() : 0,
        //                speed = element.TryGetProperty("speed", out var speedProp) ? speedProp.GetInt32() : 0,
        //                altitude = element.TryGetProperty("altitude", out var altitudeProp) ? altitudeProp.GetInt32() : 0,
        //                Address = element.TryGetProperty("address", out var addrProp) ? addrProp.GetString() : null,
        //                Battery = element.TryGetProperty("battery", out var batteryProp) ? batteryProp.GetDouble() : null,
        //                Items = new List<DeviceItem>()
        //            };

        //            // ✅ Handle nested items safely
        //            if (element.TryGetProperty("items", out var itemsProp) && itemsProp.ValueKind == JsonValueKind.Array)
        //            {
        //                foreach (var item in itemsProp.EnumerateArray())
        //                {
        //                    var devItem = new DeviceItem
        //                    {
        //                        Name = item.TryGetProperty("name", out var itemNameProp) ? itemNameProp.GetString() : null,
        //                        Online = item.TryGetProperty("online", out var itemOnlineProp) ? itemOnlineProp.GetString() : null,
        //                        Time = item.TryGetProperty("time", out var itemTimeProp) ? itemTimeProp.GetString() : null,
        //                        Timestamp = item.TryGetProperty("timestamp", out var itemTimestampProp) ? itemTimestampProp.GetString() : null,
        //                        Lat = item.TryGetProperty("lat", out var itemLatProp) ? itemLatProp.GetDouble() : 0,
        //                        Lng = item.TryGetProperty("lng", out var itemLngProp) ? itemLngProp.GetDouble() : 0,
        //                        Speed = item.TryGetProperty("speed", out var itemSpeedProp) ? itemSpeedProp.GetInt32() : 0,
        //                        Altitude = item.TryGetProperty("altitude", out var itemAltitudeProp) ? itemAltitudeProp.GetInt32() : 0,
        //                        Power = item.TryGetProperty("power", out var itemPowerProp) ? itemPowerProp.GetString() : null,
        //                        Address = item.TryGetProperty("address", out var itemAddrProp) ? itemAddrProp.GetString() : null,
        //                        Battery = item.TryGetProperty("battery", out var itemBatteryProp) ? itemBatteryProp.GetDouble() : null,
        //                        Protocol = item.TryGetProperty("protocol", out var itemProtocolProp) ? itemProtocolProp.GetString() : null,
        //                        Driver = item.TryGetProperty("driver", out var itemDriverProp) ? itemDriverProp.GetString() : null
        //                    };

        //                    // ✅ Add row to DataTable
        //                    dataTable.Rows.Add(
        //                        devItem.Name,
        //                        devItem.Online,
        //                        devItem.Time,
        //                        devItem.Timestamp,
        //                        devItem.Lat,
        //                        devItem.Lng,
        //                        devItem.Speed,
        //                        devItem.Altitude,
        //                        devItem.Power,
        //                        devItem.Address,
        //                        devItem.Battery,
        //                        devItem.Protocol,
        //                        devItem.Driver
        //                    );

        //                    devItem.Address = await _geocoding.GetAddressAsync(devItem.Lat, devItem.Lng);
        //                    device.Items.Add(devItem);
        //                }
        //            }

        //            device.Address = await _geocoding.GetAddressAsync(device.Lat, device.Lng);
        //            devices.Add(device);
        //        }

        //        // ✅ Move all fetched data into user object
        //        user.Items = new List<DeviceItem>();
        //        foreach (var d in devices)
        //        {
        //            if (d.Items != null)
        //                user.Items.AddRange(d.Items);
        //        }

        //        if (user.BaseModel == null)
        //            user.BaseModel = new BaseModel();

        //        user.BaseModel.OperationType = "InsertGpsdata";
        //        var createduser = await _devicemaster.DevicedataValue(user);

        //        return Ok(devices);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching devices with address");
        //        return StatusCode(500, "Internal Server Error: " + ex.Message);
        //    }
        //}



        //    [HttpGet("GetDevicesWithAddress")]
        //    public async Task<IActionResult> GetDevicesWithAddress([FromQuery]  GpswoxDevice user)
        //    {
        //        try
        //        {
        //            string baseUrl = _configuration["Gpswox:BaseUrl"];

        //            string hash=Uri.EscapeDataString(_configuration["Gpswox:Hash"]);
        //            //if (!string.IsNullOrEmpty(user.HashKey))
        //            //{
        //            //    hash=Uri.EscapeDataString(_configuration["Gpswox:Hash"]);
        //            //}
        //            //else
        //            //{
        //            //    hash= user.HashKey;
        //            //}

        //            string url = $"{baseUrl}get_devices?user_api_hash={hash}&lang=en";

        //            using var client = new HttpClient();
        //            var response = await client.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //                return StatusCode((int)response.StatusCode, "Error fetching devices from GPSWOX");

        //            string json = await response.Content.ReadAsStringAsync();

        //            var devices = JsonSerializer.Deserialize<List<GpswoxDevice>>(json, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });
        //            if (devices == null || devices.Count == 0)
        //                return Ok(new { Message = "No devices found for this user." });

        //            // ✅ Enrich with address
        //            foreach (var d in devices)
        //            {
        //                if (d.Items != null)
        //                {
        //                    foreach (var item in d.Items)
        //                    {
        //                        item.Address = await _geocoding.GetAddressAsync(item.Lat, item.Lng);

        //                        // ✅ Normalize battery info
        //                        if (item.Battery == null && item.Device_Data?.Battery != null)
        //                            item.Battery = item.Device_Data.Battery;
        //                    }
        //                }
        //                else
        //                {
        //                    d.Address = await _geocoding.GetAddressAsync(d.Lat, d.Lng);

        //                    if (d.Battery == null && d.Items == null && d.Device_Data?.Battery != null)
        //                        d.Battery = d.Device_Data.Battery;
        //                }
        //            }

        //            return Ok(devices);

        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error fetching devices with address");
        //            return StatusCode(500, "Internal Server Error");
        //        }
        //    }
    }



}
