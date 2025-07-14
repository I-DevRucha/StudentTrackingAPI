using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
	public class RoleMasterDto
	{
		public BaseModel? BaseModel { get; set; }
		public string? UserId { get; set; }
		public Guid? r_id { get; set; }
		public string? r_rolename { get; set; }
		public string? r_description { get; set; }
		public string? r_module { get; set; }
		public string? r_isactive { get; set; }
		public string? r_is_Admin { get; set; }
        public string? r_recruitid { get; set; }
        public int? r_no_of_user { get; set; }
		public DateTime? r_updateddate { get; set; }
		public DateTime? r_createddate { get; set; }
		public List<AccessPrivilege>? Privilage { get; set; }
		public DataTable? DataTable { get; set; }
		public string? a_id { get; set; }
		public DateTime? a_updateddate { get; set; }
		public DateTime? a_createddate { get; set; }
	}
}
