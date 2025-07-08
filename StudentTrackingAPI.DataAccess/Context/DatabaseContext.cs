using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopPortalAPI.DataAccess.Context
{
    public partial class DatabaseContext: DbContext
    {
		private readonly IConfiguration _configuration;
		private readonly string? _connectionString;
		public DatabaseContext(IConfiguration configuration)//,string Connectionstring)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("SqlConnection");
		}
		public IDbConnection CreateConnection()
			=> new SqlConnection(_connectionString);

	}
}
