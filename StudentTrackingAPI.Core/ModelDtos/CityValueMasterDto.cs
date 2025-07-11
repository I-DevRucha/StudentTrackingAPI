using StudentTrackingAPI.Core.ModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class CityValueMasterDto
    { 
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public Guid? c_id { get; set; }
        public string? c_stateid { get; set; }
        public string? c_cityvalue { get; set; }
        public string? c_code { get; set; }
        public string? c_statename { get; set; }
        public string? c_isactive { get; set; }
        public DateTime? c_createddate { get; set; }
        public DateTime? c_updateddate { get; set; }


    }
}
