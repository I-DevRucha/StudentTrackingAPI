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
    public class AuthService : IAuthService
    {
        AuthRepository _authRepository;
        public AuthService(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<IActionResult> VerifyUser(LoginDto model)
        {
            return await _authRepository.VerifyUser(model);

        }
        public async Task<IActionResult> ChangePassword(LoginDto model)
        {
            return await _authRepository.ChangePassword(model);

        }
        public async Task<IActionResult> ForgotPassword(LoginDto model)
        {
            return await _authRepository.ForgotPassword(model);

        }
        public async Task<IActionResult> UserMaster(LoginDto model)
        {
            return await _authRepository.UserMaster(model);

        }
    }
}
