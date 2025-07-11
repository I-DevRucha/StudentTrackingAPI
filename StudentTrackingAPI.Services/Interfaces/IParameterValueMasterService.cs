using Microsoft.AspNetCore.Mvc;
using StudentTrackingAPI.Core.ModelDtos;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Services.Interfaces
{ 
    public interface IParameterValueMasterService
    {
        public Task<IActionResult> ParameterValue(CityValueMasterDto model);
        public Task<IActionResult> Get(CityValueMasterDto model);
    }
}
