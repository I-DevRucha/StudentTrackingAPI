using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;
using common;
using System.Net.Mail;
using System.Net;

namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [ExampleFilterAttribute]
    public class DeviceContollercs : ControllerBase
    {
        // public IConfiguration _configuration;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeviceContollercs> _logger;
        public readonly IDeviceService _devicemaster;
        private static GpsTcpListener _gpsListener;

        public DeviceContollercs(ILogger<DeviceContollercs> logger, IConfiguration configuration, IDeviceService devicemaster)
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

    }


}
