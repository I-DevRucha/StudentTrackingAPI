﻿using Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTrackingAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using StudentTrackingAPI.Core.ModelDtos;
using System.Net;
using StudentTrackingAPI.DataAccess.Context;

namespace Token
{

    public class TokenRepo
    {
        private readonly DatabaseContext _dbContext;

        public TokenRepo(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DynamicParameters SetToken(TokenInfo user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);

            parameters.Add("@Id", user.Id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.String);
            parameters.Add("@LoginId", user.LoginId, DbType.String);
            parameters.Add("@Token", user.Token, DbType.String);
            parameters.Add("@RefreshToken", user.RefreshToken, DbType.String);
            parameters.Add("@ExpiryDate", user.ExpiryDate, DbType.String);
            parameters.Add("@CreatedDate", user.CreatedDate, DbType.DateTime);
            parameters.Add("@SessionId", user.SessionId, DbType.String);
            parameters.Add("@IpAddress", user.IpAddress, DbType.String);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            parameters.Add("@Tokens", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            parameters.Add("@Expiration", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }

        public async Task<IActionResult> DeleteToken(TokenInfo tokenInfo)
        {

            try
            {
                string connectionString = tokenInfo.BaseModel.Server_Value;
                dynamic? connectionstr = null;
                if (connectionString == null)
                {
                    connectionstr = _dbContext.CreateConnection();
                }


                using (IDbConnection connection = connectionstr)
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_TokenAndException", SetToken(tokenInfo), commandType: CommandType.StoredProcedure);
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    if (outcomeId == 1)
                    {
                        // Login successful
                        return new ObjectResult(outcomeDetail)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        // Login failed
                        return new ObjectResult(outcomeDetail)
                        {
                            StatusCode = 400
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<IActionResult> InsertToken(string token, string expiration, string? Id, string OperationType, string SessionId, string IpAddress)
        {
            TokenInfo model = new TokenInfo();
            if (model.BaseModel == null)
            {
                model.BaseModel = new BaseModel();
            }
            model.BaseModel.OperationType = OperationType;
            if (OperationType == "InsertToken")
            {
                model.Token = token;
            }
            else
            {
                model.RefreshToken = token;
            }
            dynamic? connectionstr = null;


            model.SessionId = SessionId;
            model.IpAddress = IpAddress;
            model.ExpiryDate = expiration;
            model.UserId = Id;

            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetToken(model);
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("Proc_TokenAndException", SetToken(model), commandType: CommandType.StoredProcedure);

                    // Retrieve the outcome parameters
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();

                    return new ObjectResult(outcome);

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
        public async Task<string> ValidateTokenAsync(string token, string? Id, string SessionId, string IpAddress)
        {
            TokenInfo model = new TokenInfo();
            if (model.BaseModel == null)
            {
                model.BaseModel = new BaseModel();
            }
            model.BaseModel.OperationType = "ValidateToken";

            model.Token = token;
            //model.ExpiryDate = expiration;
            model.UserId = Id;
            model.IpAddress = IpAddress;
            model.SessionId=SessionId;
            using (var connection = _dbContext.CreateConnection())
            {
                var parameter = SetToken(model);

                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("Proc_TokenAndException", parameter, commandType: CommandType.StoredProcedure);
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    string outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;

                    return outcomeDetail;

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }



        public async Task<string> Access(string comid)
        {
            TokenInfo model = new TokenInfo();
            if (model.BaseModel == null)
            {
                model.BaseModel = new BaseModel();
            }
            model.BaseModel.OperationType = "Access";

            string connectionString = "";
            dynamic? connectionstr = null;
            if (connectionString == null)
            {
                connectionstr = _dbContext.CreateConnection();
            }


            using (IDbConnection connection = connectionstr)
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_CheckAccess", commandType: CommandType.StoredProcedure);

                    // Retrieve the outcome parameters
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();

                    return null;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


    }
}
