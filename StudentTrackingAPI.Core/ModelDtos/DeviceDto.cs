using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class DeviceDto
    {
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public string?  device_id { get; set; }
        public string? lang { get; set; }
        public string? imei { get; set; }
        public string? sim_number { get; set; }
        public string? status { get; set; }
    }
}
