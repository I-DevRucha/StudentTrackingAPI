using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class StateMasterDto
    {
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public Guid? s_id { get; set; }
        public string? s_Statename { get; set; }
        public string? s_code { get; set; }
        public string? s_isactive { get; set; }
        public DateTime? s_createddate { get; set; }
        public DateTime? s_updateddate { get; set; }
    }
}
