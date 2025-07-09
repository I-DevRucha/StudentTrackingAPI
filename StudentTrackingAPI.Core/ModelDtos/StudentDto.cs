using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public string? Class { get; set; }
        public string? Div { get; set; }
        public string? WatchId { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
