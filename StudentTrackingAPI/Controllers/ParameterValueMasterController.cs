using common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoliceRecruitmentAPI.Core.ModelDtos;
using PoliceRecruitmentAPI.Core.Repository;
using PoliceRecruitmentAPI.Services.ApiServices;
using PoliceRecruitmentAPI.Services.Interfaces;
using System.Data;

namespace PoliceRecruitmentAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
	[ExampleFilterAttribute]

	public class ParameterValueMasterController : ControllerBase
    {
		public IConfiguration _configuration;
		private readonly ILogger<ParameterValueMasterController> _logger;
		public readonly IParameterValueMasterService _parameterValueMaster;

		public ParameterValueMasterController(ILogger<ParameterValueMasterController> logger, IConfiguration configuration, IParameterValueMasterService parameterValueMaster)
		{
			_logger = logger;
			_configuration = configuration;
			_parameterValueMaster = parameterValueMaster;
		}

		[HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]ParameterValueMasterDto user)
        {
            try
            {
                
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _parameterValueMaster.ParameterValue(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] ParameterValueMasterDto user)
        {
			
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
           
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _parameterValueMaster.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] ParameterValueMasterDto user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.pv_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _parameterValueMaster.ParameterValue(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] ParameterValueMasterDto user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _parameterValueMaster.ParameterValue(user);
            return productDetails;
        }

     
    }
}
