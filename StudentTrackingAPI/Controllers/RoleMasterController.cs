using common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.Services.Interfaces;
using System.Data;

namespace StudentTrackingAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ExampleFilterAttribute]
	public class RoleMasterController : ControllerBase
	{
		public IConfiguration _configuration;
		private readonly ILogger<RoleMasterController> _logger;
		public readonly IRoleMasterService _rolemaster;

		public RoleMasterController(ILogger<RoleMasterController> logger, IConfiguration configuration, IRoleMasterService rolemaster)
		{
			_logger = logger;
			_configuration = configuration;
			_rolemaster = rolemaster;
		}
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetRole([FromQuery]RoleMasterDto user)
		{
			try
			{
				
				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.OperationType = "GetAll";

				var createduser = await _rolemaster.Role(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("GetAllDuty")]
		public async Task<IActionResult> GetAllDuty([FromQuery] RoleMasterDto user)
		{
			try
			{
				
				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.OperationType = "GetAllDuty";

				var createduser = await _rolemaster.Role(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("GetMenu")]
		public async Task<IActionResult> Getmenu([FromQuery] RoleMasterDto user)
		{
			try
			{
				

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.OperationType = "GetMenu";

				var createduser = await _rolemaster.Role(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}

        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById([FromQuery] RoleMasterDto user)
        {
            try
            {
                
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
               
                var createduser = await _rolemaster.Role(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost("Insert")]
		public async Task<IActionResult> InsertRole([FromBody] RoleMasterDto user)
		{
			try
			{
				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
			
				if (user.r_id == null)
				{
					user.BaseModel.OperationType = "Insert";
				}
				else
				{
					user.BaseModel.OperationType = "Update";
				}
				user.r_createddate = DateTime.Now;
				user.r_updateddate = DateTime.Now;
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("a_menuid", typeof(string));
				dataTable.Columns.Add("a_addaccess", typeof(string));
				dataTable.Columns.Add("a_editaccess", typeof(string));
				dataTable.Columns.Add("a_deleteaccess", typeof(string));
				dataTable.Columns.Add("a_viewaccess", typeof(string));
				dataTable.Columns.Add("a_workflow", typeof(string));
				if (user.r_id != null && user.Privilage == null)
				{
					user.BaseModel.OperationType = "UpdateStatus";
				}
				else
				{
					foreach (var privilage in user.Privilage)
					{
						dataTable.Rows.Add(
							privilage.a_menuid,
							privilage.a_addaccess,
							privilage.a_editaccess,
							privilage.a_deleteaccess,
							privilage.a_viewaccess,
							privilage.a_workflow
						);
					}
					// user.Privilage = null;
					user.DataTable = dataTable;
				}
				var createduser = await _rolemaster.Get(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpPost("DeleteRole")]
		public async Task<IActionResult> DeleteRole([FromBody] RoleMasterDto user)
		{
			if (user.BaseModel == null)
			{
				user.BaseModel = new BaseModel();
			}

			user.BaseModel.OperationType = "Delete";
			var productDetails = await _rolemaster.Get(user);
			return productDetails;
		}
	}
}
