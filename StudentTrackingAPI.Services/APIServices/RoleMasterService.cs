using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.ApiServices
{
	public class RoleMasterService:IRoleMasterService
	{
		RoleMasterRepository _roleMasterRepository;
		public RoleMasterService(RoleMasterRepository roleMasterRepository)
		{
			_roleMasterRepository = roleMasterRepository;
		}
		public async Task<IActionResult> Role(RoleMasterDto model)
		{
			return await _roleMasterRepository.Role(model);

		}

		public async Task<IActionResult> Get(RoleMasterDto model)
		{
			return await _roleMasterRepository.Get(model);

		}

	}
}
