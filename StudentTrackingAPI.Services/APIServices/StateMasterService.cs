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
	public class StateMasterService:IStateMasterService
	{
        StateMasterRepository _stateMasterRepository;
		public StateMasterService(StateMasterRepository stateMasterRepository)
		{
            _stateMasterRepository = stateMasterRepository;
		}
		public async Task<IActionResult> Parameter(StateMasterDto model)
		{
			return await _stateMasterRepository.Parameter(model);

		}

		public async Task<IActionResult> Get(StateMasterDto model)
		{
			return await _stateMasterRepository.Get(model);

		}
	}
}
