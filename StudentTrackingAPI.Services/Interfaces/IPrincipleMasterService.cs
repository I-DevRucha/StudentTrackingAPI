using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{
    public interface IPrincipleMasterService
    {
        public Task<IActionResult> Get(PrincipleMasterDto model);
        public Task<IActionResult> PrincipleMaster(PrincipleMasterDto model);
        //public Task<IActionResult> Email(PrincipleMasterDto model);
    }
}
