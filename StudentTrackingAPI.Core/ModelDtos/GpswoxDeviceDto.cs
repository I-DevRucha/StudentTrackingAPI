using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class GpswoxDevice
    {
        public BaseModel? BaseModel { get; set; }
        public int? Id { get; set; }
        public int? device_id  { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? HashKey { get; set; }
        public string? Time { get; set; }
        public long? Timestamp { get; set; }
        public int Speed { get; set; }

        [JsonConverter(typeof(FlexibleDoubleConverter))]
        public double? Altitude { get; set; }
        public string? Online { get; set; }
        public List<DeviceItem>? Items { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? imei { get; set; }
        public double? Battery { get; set; }
        public string? sim_number { get; set; }
        public string? status { get; set; }
        public string? lang { get; set; }
        public string? Power { get; set; }
        public string? Driver { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }
        public string? from_time { get; set; }
        public string? to_time { get; set; }

        public bool? snap_to_road { get; set; }
        public DeviceData? Device_Data { get; set; }
        public Parameters? Parameters { get; set; }
        public DataTable? DataTable { get; set; }
    }
    public class Parameters
    {
        public string Power { get; set; }
    }

}
