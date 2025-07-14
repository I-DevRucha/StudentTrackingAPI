using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
	public class AccessPrivilege
	{
		public string? a_roleid { get; set; }
		public string? a_menuid { get; set; }
		public string? a_isactive { get; set; }
		public string? a_addaccess { get; set; }
		public string? a_editaccess { get; set; }
		public string? a_deleteaccess { get; set; }
		public string? a_viewaccess { get; set; }
		public string? a_workflow { get; set; }
	}
}
