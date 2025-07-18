using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.APIServices
{
    public class ParentMasterService : IParentMasterService
    {
        ParentMasterRepository _parentmasterrepository;
        public ParentMasterService(ParentMasterRepository parentmasterrepository)
        {
            _parentmasterrepository = parentmasterrepository;
        }
        public async Task<IActionResult> ParentMaster(ParentMasterDto model)
        {
            return await _parentmasterrepository.ParentMaster(model);

        }
        public async Task<IActionResult> Get(ParentMasterDto model)
        {
            return await _parentmasterrepository.Get(model);

        }
        public async Task<IActionResult> Email(ParentMasterDto model)
        {
            return await _parentmasterrepository.Email(model);

        }
    }
}
