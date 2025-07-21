using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{
	public interface IGetWebMenuService
	{
		public Task<IActionResult> GetWebMenu(GetWebMenuDto model);
	}
}
