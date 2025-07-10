using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{
    public  class IStudentService
    {
        public Task<IActionResult> AddStuent(StudentDto model);
    }
}
