using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class GpswoxDevice
    {
        public BaseModel? BaseModel { get; set; }
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? HashKey { get; set; }
        public List<DeviceItem>? Items { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? Address { get; set; }
        public double? Battery { get; set; }
        public DeviceData? Device_Data { get; set; }
    }
   
   
}
