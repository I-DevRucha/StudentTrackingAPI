using System;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.DataAccess.Context;

namespace StudentTrackingAPI.Core.Repository
{
    public class AuthRepository
    {
        private readonly DatabaseContext _dbContext;

        public AuthRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> VerifyUser(LoginDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetLogin(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    var Model = queryResult.ReadSingleOrDefault<dynamic>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    //if (outcomeId == 5)
                    //{
                    //    return new ObjectResult(Model)
                    //    {
                    //        StatusCode = 200
                    //    };

                    //}

                    string UserId = string.Empty;
                    if (((IDictionary<string, object>)Model).TryGetValue("UserId", out var userIdValue))
                    {
                        UserId = userIdValue?.ToString() ?? string.Empty;
                    }

                    //var ipaddress = model.IpAddress;
                    //var sessionid = model.SessionId;
                    //var Model = queryResult.Read<Login>().ToList();

                    //if (outcomeId == 1)
                    //{
                    //	var result = new Result
                    //	{

                    //		Outcome = outcome,
                    //		Data = Model,
                    //                       UserId = model.UserId
                    //                   };
                    //	// Login successful
                    //	return new ObjectResult(result)
                    //	{
                    //		StatusCode = 200
                    //	};
                    //}
                    //else
                    //{
                    //	var result = new Result
                    //	{

                    //		Outcome = outcome,

                    //	};
                    //	// Login failed
                    //	return new ObjectResult(result)
                    //	{
                    //		StatusCode = 400
                    //	};
                    //}
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model,
                        UserId = Model.UserId.ToString()

                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else if (outcomeId == 4)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 403
                        };
                    }
                    else if (outcomeId == 2)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 409
                        };
                    }
                    else if (outcomeId == 3)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 402
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //return true; 
        }



        public async Task<IActionResult> ChangePassword(LoginDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetLogin(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    var Model = queryResult.ReadSingleOrDefault<dynamic>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;

                    string UserId = string.Empty;
                    if (((IDictionary<string, object>)Model).TryGetValue("UserId", out var userIdValue))
                    {
                        UserId = userIdValue?.ToString() ?? string.Empty;
                    }
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model,
                        UserId = Model.UserId.ToString()

                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else if (outcomeId == 2)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 409
                        };
                    }
                    else if (outcomeId == 3)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 402
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //return true; 
        }
        public async Task<IActionResult> ForgotPassword(LoginDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetLogin(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    var Model = queryResult.ReadSingleOrDefault<dynamic>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;

                    string UserId = string.Empty;
                    if (((IDictionary<string, object>)Model).TryGetValue("UserId", out var userIdValue))
                    {
                        UserId = userIdValue?.ToString() ?? string.Empty;
                    }
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model,
                        UserId = Model.UserId.ToString()

                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else if (outcomeId == 2)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 409
                        };
                    }
                    else if (outcomeId == 3)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 402
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //return true; 
        }
        public async Task<IActionResult> UserMaster(LoginDto model)
        {

            using (var connection = _dbContext.CreateConnection())

            {
                var parameter = SetLogin(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("Proc_LoginDetails", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    var Model = queryResult.Read<Object>().ToList();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model,
                        UserId = model.UserId
                    };

                    if (outcomeId == 1)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else if (outcomeId == 2)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 409
                        };
                    }
                    else if (outcomeId == 3)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 423
                        };
                    }
                    else if (outcomeId == 4)
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 424
                        };
                    }
                    else
                    {
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public DynamicParameters SetLogin(LoginDto user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@Username", user.Username, DbType.String);
            parameters.Add("@Id", user.UserId, DbType.String);
            parameters.Add("@RoleId", user.RoleId, DbType.String);
            parameters.Add("@MailId", user.MailId, DbType.String);
            parameters.Add("@Password", user.Password, DbType.String);
            parameters.Add("@OldPassword", user.OldPassword, DbType.String);
            parameters.Add("@NewPassword", user.NewPassword, DbType.String);
            parameters.Add("@IpAddress", user.IpAddress, DbType.String);
            parameters.Add("@SessionId", user.SessionId, DbType.String);
            parameters.Add("@SessionIds", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            parameters.Add("@IpAddresss", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }

    }
}
