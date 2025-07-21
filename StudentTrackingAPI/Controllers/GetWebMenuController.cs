using common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Services.Interfaces;

namespace StudentTrackingAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ExampleFilterAttribute]
	public class GetWebMenuController : ControllerBase
	{
		public IConfiguration _configuration;
		private readonly ILogger<GetWebMenuController> _logger;
		public readonly IGetWebMenuService _getWebMenuService;

		public GetWebMenuController(ILogger<GetWebMenuController> logger, IConfiguration configuration,IGetWebMenuService getWebMenuService)
		{
			_logger = logger;
			_configuration = configuration;
			_getWebMenuService = getWebMenuService;
		}



		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll([FromQuery]GetWebMenuDto user)
		{
			try
			{
				

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}

				user.BaseModel.OperationType = "GetWebMenu";
				
				var createduser = await _getWebMenuService.GetWebMenu(user);
				var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
				return Ok(data);
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("GetMenu")]
		public async Task<IActionResult> GetMenu([FromQuery] GetWebMenuDto user)
		{
			try
			{


				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}

				user.BaseModel.OperationType = "GetMenu";

				var createduser = await _getWebMenuService.GetWebMenu(user);
				var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
				return Ok(data);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
