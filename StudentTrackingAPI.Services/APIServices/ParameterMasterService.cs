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
	public class ParameterMasterService:IParameterMasterService
	{
		ParameterMasterRepository _parameterMasterRepository;
		public ParameterMasterService(ParameterMasterRepository parameterMasterRepository)
		{
			_parameterMasterRepository = parameterMasterRepository;
		}
		public async Task<IActionResult> Parameter(StateMasterDto model)
		{
			return await _parameterMasterRepository.Parameter(model);

		}

		public async Task<IActionResult> Get(StateMasterDto model)
		{
			return await _parameterMasterRepository.Get(model);

		}
	}
}
