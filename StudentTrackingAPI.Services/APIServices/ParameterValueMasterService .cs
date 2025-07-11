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
	public class ParameterValueMasterService : IParameterValueMasterService
	{
		ParameterValueMasterRepository _parameterMastervValueRepository;
		public ParameterValueMasterService(ParameterValueMasterRepository parameterMasterValueRepository)
		{
			_parameterMastervValueRepository = parameterMasterValueRepository;
		}
		public async Task<IActionResult> ParameterValue(CityValueMasterDto model)
		{
			return await _parameterMastervValueRepository.ParameterValue(model);

		}

		public async Task<IActionResult> Get(CityValueMasterDto model)
		{
			return await _parameterMastervValueRepository.Get(model);

		}
	}
}
