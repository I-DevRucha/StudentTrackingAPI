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
    [ExampleFilterAttribute]
    public class PrincipleMasterController : ControllerBase
    {
        // public IConfiguration _configuration;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PrincipleMasterController> _logger;
        public readonly IPrincipleMasterService _principlemaster;

        public PrincipleMasterController(ILogger<PrincipleMasterController> logger, IConfiguration configuration, IPrincipleMasterService principlemaster)
        {
            _logger = logger;
            _configuration = configuration;
            _principlemaster = principlemaster;
        }


        [HttpGet("GetAllprinciple")]
        public async Task<IActionResult> GetAllprinciple([FromQuery] PrincipleMasterDto user)
        {
            try
            {

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetAll";
                var createduser = await _principlemaster.PrincipleMaster(user);
                return createduser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("Getprinciple")]
        public async Task<IActionResult> Getprinciple([FromQuery] PrincipleMasterDto user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }

            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _principlemaster.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost("Insertprinciple")]
        public async Task<IActionResult> Insertprinciple([FromBody] PrincipleMasterDto user)
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
                dynamic createduser = await _principlemaster.PrincipleMaster(user);
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

        [HttpPost("Delete")]
        public async Task<IActionResult> DeletePrinciple([FromBody] PrincipleMasterDto model)
        {
            try
            {

                if (model.BaseModel == null)
                {
                    model.BaseModel = new BaseModel();
                }
                model.BaseModel.OperationType = "Delete";

                var result = await _principlemaster.PrincipleMaster(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


         
    }


}
