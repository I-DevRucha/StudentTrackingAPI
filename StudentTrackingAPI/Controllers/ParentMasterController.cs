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
                var dataList = createduser?.Value?.Data as IEnumerable<dynamic>;
                var insertedId = dataList?.FirstOrDefault()?.Id as Guid?;
                // ✅ Send email only if insert is successful
                if (outcomeidvalue == 1 && user.Id == null && insertedId != null)
                {
                    // Set operation type for fetching email data
                    user.Id = insertedId;
                    user.BaseModel.OperationType = "Emailsenddata";
                    var emailResult = await _parentmaster.Email(user);

                    if (emailResult is ObjectResult emailObject && emailObject.StatusCode == 200)
                    {
                        var resultData = emailObject.Value as Result;
                        var dataList1 = resultData?.Data as List<dynamic>;
                        var firstRecord = dataList1?.FirstOrDefault();

                        if (firstRecord != null)
                        {
                            string ParentEmail = firstRecord.um_EmailId;
                            string UserNameId = firstRecord.um_user_name;
                            string Password = firstRecord.Password;

                            if (!string.IsNullOrWhiteSpace(ParentEmail))
                            {
                                SendApprovalMail(ParentEmail, UserNameId, Password);
                            }
                        }
                    }
                }


                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteEvent([FromBody] ParentMasterDto model)
        {
            try
            {

                if (model.BaseModel == null)
                {
                    model.BaseModel = new BaseModel();
                }
                model.BaseModel.OperationType = "Delete";

                var result = await _parentmaster.ParentMaster(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("Email")]
        public async Task<IActionResult> Email([FromQuery] ParentMasterDto user)
        {
            try
            {
                if (user.BaseModel == null)
                    user.BaseModel = new BaseModel();

                user.BaseModel.OperationType = "Emailsenddata";

                var result = await _parentmaster.Email(user);

                if (result is ObjectResult objectResult && objectResult.StatusCode == 200)
                {
                    //  Send email after approval
                    //SendApprovalMail(user.Email);
                    var resultData = objectResult.Value as Result;
                    if (result is ObjectResult objectResult1 && objectResult.StatusCode == 200)
                    {
                        var dataList = ((Result)objectResult.Value).Data as List<dynamic>;
                        var firstRecord = dataList?.FirstOrDefault();

                        if (firstRecord != null)
                        {
                            string ParentEmail = firstRecord.um_EmailId;
                            string UserNameId = firstRecord.um_user_name;
                            string Password = firstRecord.Password;
                            //   string hrName = user.Name;
                            //string ComapnyName = firstRecord.Name;
                            // Check if the candidate email is valid before sending
                            if (!string.IsNullOrWhiteSpace(ParentEmail))
                            {
                                SendApprovalMail(ParentEmail, UserNameId, Password);
                            }
                        }
                    }

                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        private void SendApprovalMail(string ParentEmail, string UserNameId, string Password)
        {
            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient(_configuration["MailSettings:Host"])
                {
                    Port = int.Parse(_configuration["MailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["MailSettings:Username"],
                        _configuration["MailSettings:Password"]
                    ),
                    EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"])
                };

                var fromAddress = new MailAddress(_configuration["MailSettings:Username"]);
                var mail = new MailMessage
                {
                    Body = $@"
                           <p>Dear Parent,</p>
                           <p>We are pleased to inform you that your account has been successfully created on the <strong>Student Tracking Portal</strong>.</p>
                           <p>Below are your login credentials to access the portal and monitor your child's academic and attendance progress:</p>
                           <p><strong>Username:</strong> {UserNameId}</p>
                           <p><strong>Password:</strong> {Password}</p>
                           <p>You can log in via the portal link provided by your school.</p>
                           <br />
                           <p>If you have any questions or face any issues, please contact the school administration.</p>
                           <br />
                           <p>Regards,<br />{ParentEmail} School Administration Team</p>",
                    IsBodyHtml = true
                };

                mail.To.Add(ParentEmail);
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }


}
