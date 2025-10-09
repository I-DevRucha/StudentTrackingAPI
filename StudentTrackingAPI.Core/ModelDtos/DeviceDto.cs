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
        public List<DeviceItem> Items { get; set; }
        public string? UserId { get; set; }
        public string?  device_id { get; set; }
        public string? lang { get; set; }
        public string? imei { get; set; }
        public string? sim_number { get; set; }
        public string? status { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? online { get; set; }
        public string? alarm { get; set; }
        public string? time { get; set; }
        public long timestamp { get; set; }
        public int speed { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string? course { get; set; }
        public string? power { get; set; }
        public int altitude { get; set; }
        public string? address { get; set; }
        public string? protocol { get; set; }
        public string? driver { get; set; }
    }
    public class DeviceItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Online { get; set; }
        public string? Time { get; set; }
        public long Timestamp { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Speed { get; set; }
        public int Altitude { get; set; }
        public string? Power { get; set; }
        public string? Address { get; set; }
        public string? Protocol { get; set; }
        public string? Driver { get; set; }
        public DeviceData Device_Data { get; set; }
    }
    public class DeviceData
    {
        public string? Imei { get; set; }
        public string? Sim_Number { get; set; }
    }
    public class DeviceGroup
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<DeviceItem> Items { get; set; }
    }
}
