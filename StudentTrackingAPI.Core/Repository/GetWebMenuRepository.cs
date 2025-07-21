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
	public class GetWebMenuRepository
	{
		private readonly DatabaseContext _dbContext;

		public GetWebMenuRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> GetWebMenu(GetWebMenuDto model)
		{

			using (var connection = _dbContext.CreateConnection())
			{
				var parameter = SetMenu(model);
				try
				{

					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();

					var queryResult = await connection.QueryMultipleAsync("proc_GetMenu", parameter, commandType: CommandType.StoredProcedure,commandTimeout: 2000);
                    var Model = queryResult.Read<Object>().ToList();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
					var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
					var result = new Result
					{

						Outcome = outcome,
						Data = Model,
						UserId = model.UserId,
                        SessionId = model.SessionId,
                        IpAddress = model.IpAddress
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

		public DynamicParameters SetMenu(GetWebMenuDto user)
		{

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Operation_type", user.BaseModel.OperationType, DbType.String);
			parameters.Add("@RoleId", user.RoleId, DbType.Guid);
			parameters.Add("@recruitid", user.RecruitId, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@IpAddress", user.IpAddress, DbType.String);
            parameters.Add("@SessionId", user.SessionId, DbType.String);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


			return parameters;

		}
	}
}
