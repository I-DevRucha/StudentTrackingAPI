using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
	public class GetWebMenuDto
	{
		public BaseModel? BaseModel { get; set; }
		public Guid? RoleId { get; set; }
		public string? UserId { get; set; }
		public string? RecruitId { get; set; }
        public string? IpAddress { get; set; }
        public string? SessionId { get; set; }
    }
}
