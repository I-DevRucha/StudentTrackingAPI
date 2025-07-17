using Context;
using DocumentFormat.OpenXml.Wordprocessing;
using StudentTrackingAPI.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.DataAccess;
using StudentTrackingAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Token;


namespace StudentTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public readonly IAuthService _authService;
        private readonly DatabaseContext _dbContext;
        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IAuthService authService, DatabaseContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _authService = authService;
            _dbContext = dbContext;

        }

        [HttpGet("GetRole")]  //new
        public async Task<IActionResult> Getrole([FromQuery] LoginDto user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }

            user.BaseModel.OperationType = "GetRole";
            try
            {
                var parameter = await _authService.UserMaster(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // new login method
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            _logger.LogInformation("Received login request: {@model}", model);

            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                _logger.LogWarning("Invalid request payload: {model}", model);
                var result = new Result
                {
                    Data = null,
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "Invalid request payload",
                        Tokens = null,
                        Expiration = null
                    }
                };
                return BadRequest(result);
            }

            try
            {
                if (model.BaseModel == null)
                {
                    model.BaseModel = new BaseModel();
                }
                model.BaseModel.OperationType = "ValidateLogin";
                var SessionId = model.SessionId.ToString();
                var IpAddress = model.IpAddress.ToString();
                jwtTokenCreate tk = new jwtTokenCreate(_configuration);
                dynamic userDetail = await _authService.VerifyUser(model);

                if (userDetail.Value == null)
                {
                    _logger.LogWarning("Unexpected result from authentication service.");
                    var result = new Result
                    {
                        Data = null,
                        Outcome = new Outcome
                        {
                            OutcomeId = 0,
                            OutcomeDetail = "Unexpected result from authentication service",
                            Tokens = null,
                            Expiration = null
                        }
                    };
                    return BadRequest(result);
                }

                var outcome = userDetail.Value.Outcome;
                var Model = userDetail.Value.Data;
                var outcomeId = outcome.OutcomeId;
                var outcomeDetail = outcome.OutcomeDetail;


                if (Model != null)
                {
                    if (outcomeId == 1)
                    {
                        string staffid = Model.Staffid.ToString();
                        string staffname = Model.Staffname;
                        string roleid = Model.RoleId?.ToString();
                        string dutyid = Model.DutyId?.ToString();
                        string dutyname = Model.DutyName;
                        var UserId = Model.UserId.ToString();
                        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var token = GetToken(authClaims);
                        var Token = new JwtSecurityTokenHandler().WriteToken(token);
                        var expiration = token.ValidTo.ToString();
                        var OperationType = "InsertToken";
                        TokenRepo TR = new TokenRepo(_dbContext);
                        var InsterToken = await TR.InsertToken(Token, expiration, UserId, OperationType, SessionId, IpAddress);

                        string encrypttext = tk.Encrypt(Token, "abcdefghijklmnop");

                        var result = new Result
                        {
                            Data = Model,
                            Outcome = new Outcome
                            {
                                OutcomeId = outcomeId,
                                OutcomeDetail = outcomeDetail,
                                Tokens = encrypttext,
                                Expiration = expiration,
                                UserId= Model.UserId.ToString(),
                                SessionId = model.SessionId,
                                IpAddress = model.IpAddress
                            }
                        };

                        return Ok(new { result });
                    }

                    else if (outcomeId == 4)
                    {
                        var result = new Result
                        {
                            Data = null,
                            Outcome = new Outcome
                            {
                                OutcomeId = outcomeId,
                                OutcomeDetail = "Access denied: Payment required",
                                Tokens = null,
                                Expiration = null,
                                SessionId = model.SessionId,
                                IpAddress = model.IpAddress
                            }
                        };

                        return StatusCode(StatusCodes.Status403Forbidden, result);
                    }
                    else
                    {
                        var result = new Result
                        {
                            Data = null,
                            Outcome = new Outcome
                            {
                                OutcomeId = outcomeId,
                                OutcomeDetail = "Invalid username or password",
                                Tokens = null,
                                Expiration = null,
                                SessionId = model.SessionId,
                                IpAddress = model.IpAddress
                            }
                        };

                        return StatusCode(StatusCodes.Status400BadRequest, result);
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid username or password.");
                    var result = new Result
                    {
                        Data = null,
                        Outcome = new Outcome
                        {
                            OutcomeId = 0,
                            OutcomeDetail = "Invalid username or password",
                            Tokens = null,
                            Expiration = null,
                            SessionId = model.SessionId,
                            IpAddress = model.IpAddress
                        }
                    };
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                var result = new Result
                {
                    Data = null,
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "An error occurred during login",
                        Tokens = null,
                        Expiration = null,
                        SessionId = model.SessionId,
                        IpAddress = model.IpAddress
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] LoginDto model)
        {
            _logger.LogInformation("Received change password request for user: {Username}", model?.Username);

            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.UserId))
            {
                return BadRequest(new Result
                {
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "Username, current password,UserId or new password is missing"
                    }
                });
            }

            try
            {
                model.BaseModel ??= new BaseModel();
                model.BaseModel.OperationType = "ChangePassword";

                var result = await _authService.ChangePassword(model);

                return Ok(new Result
                {
                    Outcome = new Outcome
                    {
                        OutcomeId = 1,
                        OutcomeDetail = "Password changed successfully"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing password.");
                return StatusCode(StatusCodes.Status500InternalServerError, new Result
                {
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "An error occurred while changing password"
                    }
                });
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginDto model)
        {
            _logger.LogInformation("Received change password request for user: {Username}", model?.Username);

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.UserId)|| string.IsNullOrEmpty(model.MailId))
            {
                return BadRequest(new Result
                {
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "Username, current password,UserId or new password is missing"
                    },

                });
            }

            try
            {
                model.BaseModel ??= new BaseModel();
                model.BaseModel.OperationType = "ForgotPassword";

                var result = await _authService.ForgotPassword(model);

                if (result == null)
                {
                    return Ok(new Result
                    {
                        Outcome = new Outcome
                        {
                            OutcomeId = 0,
                            OutcomeDetail = "No response from service"
                        },
                        Data = null
                    });
                }


                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing password.");
                return StatusCode(StatusCodes.Status500InternalServerError, new Result
                {
                    Outcome = new Outcome
                    {
                        OutcomeId = 0,
                        OutcomeDetail = "An error occurred while changing password"
                    }
                });
            }
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
