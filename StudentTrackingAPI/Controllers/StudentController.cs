using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using StudentTrackingAPI.Core.ModelDtos;

namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ILogger<StudentController> _logger;
        //public readonly ICandidateService _candidateService;

        public StudentController(ILogger<StudentController> logger, IConfiguration configuration /*ICandidateService candidateService*/)
        {
            _logger = logger;
            _configuration = configuration;
          //  _candidateService = candidateService;
        }

        [HttpPost("AddStuent")]
        public async Task<IActionResult> AddStuent([FromBody] StudentDto user)
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
                    user.um_updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                dynamic createduser = await _employeemaster.AddStuent(user);
                var outcomeidvalue = createduser.Value.Outcome.OutcomeId;

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
