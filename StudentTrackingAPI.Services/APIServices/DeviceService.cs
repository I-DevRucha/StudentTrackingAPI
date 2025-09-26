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
    public class DeviceService : IDeviceService
    {
        DeviceRepository _deviceRepository;
        public DeviceService(DeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }
        public async Task<IActionResult> DeviceValue(DeviceDto model)
        {
            return await _deviceRepository.ParameterValue(model);

        }

        public async Task<IActionResult> Get(DeviceDto model)
        {
            return await _deviceRepository.Get(model);

        }
    }
}
