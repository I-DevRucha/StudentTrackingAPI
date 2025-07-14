using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{
	public interface IRoleMasterService
	{
		public Task<IActionResult> Role(RoleMasterDto model);
		public Task<IActionResult> Get(RoleMasterDto model);
	}
}
