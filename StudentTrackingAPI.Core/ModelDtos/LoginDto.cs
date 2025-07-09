using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class LoginDto
    {
        public BaseModel? BaseModel { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? MailId { get; set; }
        public string? Staffid { get; set; }
        public string? Staffname { get; set; }
        public string? RoleId { get; set; }
        public string? DutyId { get; set; }
        public string? UserId { get; set; }
        public string? RoleName { get; set; }
        public string? SessionId { get; set; }
        public string? IpAddress { get; set; }
        public string? DutyName { get; set; }
        public string? RecruitId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
