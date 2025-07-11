using Dapper;
using Microsoft.AspNetCore.Mvc;
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
    public class ParameterMasterRepository 
    {
		private readonly DatabaseContext _dbContext;

		public ParameterMasterRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Parameter(StateMasterDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
				var parameter = SetParameter(model);
				try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_ParameterMaster", parameter, commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(StateMasterDto model)
        {
            using (var connection = _dbContext.CreateConnection())
            {
				var parameter = SetParameter(model);
				try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_ParameterMaster", parameter, commandType: CommandType.StoredProcedure);
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
        public DynamicParameters SetParameter(StateMasterDto user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@p_id", user.s_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@p_parametername", user.s_Statename, DbType.String);
            parameters.Add("@p_code", user.s_code, DbType.String);
            parameters.Add("@p_isactive", user.s_isactive, DbType.String);
            parameters.Add("@p_createddate", user.s_createddate, DbType.DateTime);
            parameters.Add("@p_updateddate", user.s_updateddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
