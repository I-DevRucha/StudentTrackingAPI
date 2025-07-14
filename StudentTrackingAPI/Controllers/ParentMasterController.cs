using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;
using common;

namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExampleFilterAttribute]
    public class ParentMasterController : ControllerBase
    {
        // public IConfiguration _configuration;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ParentMasterController> _logger;
        public readonly IParentMasterService _parentmaster;

        public ParentMasterController(ILogger<ParentMasterController> logger, IConfiguration configuration, IParentMasterService parentmaster)
        {
            _logger = logger;
            _configuration = configuration;
            _parentmaster = parentmaster;
        }


        [HttpGet("GetAllParent")]
        public async Task<IActionResult> GetAllParent([FromQuery] ParentMasterDto user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetAll";
                var createduser = await _parentmaster.ParentMaster(user);
                return createduser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         
        [HttpGet("GetParent")]
        public async Task<IActionResult> GetParent([FromQuery] ParentMasterDto user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }

            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _parentmaster.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost("InsertParent")]
        public async Task<IActionResult> InsertParent([FromBody] ParentMasterDto user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.UserId = "";
                //user.SessionId = user.SessionId.ToString();
                //user.IpAddress = user.IpAddress.ToString();

                if (user.Id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.Updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                dynamic createduser = await _parentmaster.ParentMaster(user);
                var outcomeidvalue = createduser.Value.Outcome.OutcomeId;
                //if (outcomeidvalue == 1)
                //{

                //	var datavalue = createduser.Value.Outcome.OutcomeDetail;

                //	await SendNo(datavalue);
                //}

                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }


}
