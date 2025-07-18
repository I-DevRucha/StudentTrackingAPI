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
	public class RoleMasterRepository
	{
		private readonly DatabaseContext _dbContext;
		public RoleMasterRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Role(RoleMasterDto model)
		{

			using (var connection = _dbContext.CreateConnection())
			{
				var parameter = SetRole(model);
				try
				{
					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();
					var queryResult = await connection.QueryMultipleAsync("proc_RoleMaster", parameter, commandType: CommandType.StoredProcedure);
					var Model = queryResult.Read<Object>();
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
		public async Task<IActionResult> Get(RoleMasterDto model)
		{
			using (var connection = _dbContext.CreateConnection())

			{
				var parameter = SetRole(model);
				try
				{
					var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
					await sqlConnection.OpenAsync();
					var queryResult = await connection.QueryMultipleAsync("proc_RoleMaster", parameter, commandType: CommandType.StoredProcedure);
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

		public DynamicParameters SetRole(RoleMasterDto user)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OperationType", user.BaseModel?.OperationType, DbType.String);
			parameters.Add("@userId", user.UserId, DbType.String);
			parameters.Add("@r_id", user.r_id, DbType.Guid);
			
			parameters.Add("@r_rolename", user.r_rolename, DbType.String);
			parameters.Add("@r_description", user.r_description, DbType.String);
			parameters.Add("@r_module", user.r_module, DbType.String);
			parameters.Add("@r_isactive", user.r_isactive, DbType.String);
			parameters.Add("@r_is_Admin", user.r_is_Admin, DbType.String);
			parameters.Add("@r_recruitid", user.r_recruitid, DbType.String);
			//parameters.Add("@r_no_of_user", user.r_no_of_user, DbType.Int64);
			parameters.Add("@r_createddate", user.r_createddate, DbType.DateTime);
			parameters.Add("@r_updateddate", user.r_updateddate, DbType.DateTime);
			if (user.DataTable != null && user.DataTable.Rows.Count > 0)
			{
				parameters.Add("@RolePrivilege", user.DataTable.AsTableValuedParameter("[dbo].[Tbl_RolePrivilege]"));
			}
			parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
			return parameters;
		}
	}
}
