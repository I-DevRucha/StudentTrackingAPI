using common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;
using System.Data;

namespace PoliceRecruitmentAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
	[ExampleFilterAttribute]

	public class CityValueMasterController : ControllerBase
    {
		public IConfiguration _configuration;
		private readonly ILogger<CityValueMasterController> _logger;
		public readonly ICityValueMasterService _cityValueMaster;

		public CityValueMasterController(ILogger<CityValueMasterController> logger, IConfiguration configuration, ICityValueMasterService cityValueMaster)
		{
			_logger = logger;
			_configuration = configuration;
            _cityValueMaster = cityValueMaster;
		}

		[HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]CityValueMasterDto user)
        {
            try
            {
                
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _cityValueMaster.ParameterValue(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] CityValueMasterDto user)
        {
			
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
           
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _cityValueMaster.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CityValueMasterDto user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.c_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _cityValueMaster.ParameterValue(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] CityValueMasterDto user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _cityValueMaster.ParameterValue(user);
            return productDetails;
        }

     
    }
}
