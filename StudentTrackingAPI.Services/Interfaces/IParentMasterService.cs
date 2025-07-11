using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTrackingAPI.Core.ModelDtos;

namespace StudentTrackingAPI.Services.Interfaces
{
    public interface IParentMasterService
    {
        public Task<IActionResult> Get(ParentMasterDto model);
        public Task<IActionResult> ParentMaster(ParentMasterDto model);
    }
}
