using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.Core.Repositry;
using StudentTrackingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.APIServices
{
    public class StudentService: IStudentService
    {
        StudentMasterRepository _studentRepository;
        public StudentService(StudentMasterRepository authRepository)
        {
            _studentRepository = authRepository;
        }
        public async Task<IActionResult> AddStuent(StudentDto model)
        {
            return await _studentRepository.AddStuent(model);

        }

        public async Task<IActionResult> Get(StudentDto model)
        {
            return await _studentRepository.Get(model);

        }
        public async Task<IActionResult> StudentMaster(StudentDto model)
        {
            return await _studentRepository.StudentMaster(model);

        }
    }
}
