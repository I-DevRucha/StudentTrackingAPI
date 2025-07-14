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
	public class CityValueMasterService : ICityValueMasterService
    {
		CityValueMasterRepository _cityMastervValueRepository;
		public CityValueMasterService(CityValueMasterRepository cityValueRepository)
		{
			_cityMastervValueRepository = cityValueRepository;
		}
		public async Task<IActionResult> ParameterValue(CityValueMasterDto model)
		{
			return await _cityMastervValueRepository.ParameterValue(model);

		}

		public async Task<IActionResult> Get(CityValueMasterDto model)
		{
			return await _cityMastervValueRepository.Get(model);

		}
	}
}
