using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.Repository
{
    public class PrincipleMasterRepository
    {
        private readonly DatabaseContext _dbContext;

        public PrincipleMasterRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> PrincipleMaster(PrincipleMasterDto model)
        {

            using (var connection = _dbContext.CreateConnection())

            {
                var parameter = SetUser(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_PrincipleMaster", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);
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

        public async Task<IActionResult> Get(PrincipleMasterDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetUser(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_PrincipleMaster", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                    var Model = queryResult.ReadSingleOrDefault<Object>();
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

        //public async Task<IActionResult> Email(PrincipleMasterDto model)
        //{
        //    using (var connection = _dbContext.CreateConnection())
        //    {
        //        var parameter = SetUser(model);

        //        try
        //        {
        //            var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
        //            await sqlConnection.OpenAsync();

        //            var queryResult = await connection.QueryMultipleAsync("proc_ParentMaster", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 300);

        //            var modelList = queryResult.Read<object>().ToList();
        //            var outcome = queryResult.ReadSingleOrDefault<Outcome>();

        //            var result = new Result
        //            {
        //                Outcome = outcome,
        //                Data = modelList,
        //                UserId = model.UserId
        //            };

        //            int outcomeId = outcome?.OutcomeId ?? 0;

        //            return new ObjectResult(result)
        //            {
        //                StatusCode = outcomeId switch
        //                {
        //                    1 => 200,
        //                    2 => 409,
        //                    3 => 423,
        //                    4 => 424,
        //                    _ => 400
        //                }
        //            };
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public DynamicParameters SetUser(PrincipleMasterDto user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@Id", user.Id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@FirstName", user.FisrtName, DbType.String);
            parameters.Add("@roleid", user.RoleId, DbType.String);
            parameters.Add("@LastName", user.LastName, DbType.String);
            parameters.Add("@MobileNo", user.MobileNo, DbType.String);
            parameters.Add("@SchoolAddress", user.SchoolAddress, DbType.String);
            parameters.Add("@SchoolName", user.SchoolName, DbType.String);
            parameters.Add("@AadharNo", user.AadharNo, DbType.String);
            parameters.Add("@EmailId", user.EmailId, DbType.String);
            parameters.Add("@ChildName", user.ChildName, DbType.String);
            parameters.Add("@City", user.City, DbType.String);
            parameters.Add("@State", user.State, DbType.String);
            parameters.Add("@Country", user.Country, DbType.String);
            parameters.Add("@PinCode", user.PinCode, DbType.String);
            parameters.Add("@Createddate", user.Createddate, DbType.DateTime);
            parameters.Add("@Updateddate", user.Updateddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }
    }

}
