using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class PrincipleMasterDto
    {
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string? FisrtName { get; set; }
        public string? LastName { get; set; }
        public string? RoleName { get; set; }
        public Guid? Id { get; set; }
        public string? MobileNo { get; set; }
        public string? SchoolAddress { get; set; }
        public string? AadharNo { get; set; }
        public string? EmailId { get; set; }
        public string? ChildName { get; set; }
        public string? SchoolName { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PinCode { get; set; }
        public DataTable? DataTable { get; set; }
    }
}
