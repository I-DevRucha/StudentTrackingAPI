using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{
    public interface IDeviceService
    {
        public Task<IActionResult> DeviceValue(DeviceDto model);
        public Task<IActionResult> Get(DeviceDto model);
    }
}
