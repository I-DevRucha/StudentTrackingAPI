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
    public class PrincipleMasterService : IPrincipleMasterService
    {
        PrincipleMasterRepository _principlemasterrepository;
        public PrincipleMasterService(PrincipleMasterRepository principlmasterrepository)
        {
            _principlemasterrepository = principlmasterrepository;
        }
        public async Task<IActionResult> PrincipleMaster(PrincipleMasterDto model)
        {
            return await _principlemasterrepository.PrincipleMaster(model);

        }
        public async Task<IActionResult> Get(PrincipleMasterDto model)
        {
            return await _principlemasterrepository.Get(model);

        }
        //public async Task<IActionResult> Email(PrincipleMasterDto model)
        //{
        //    return await _principlemasterrepository.Email(model);

        //}
    }
}
