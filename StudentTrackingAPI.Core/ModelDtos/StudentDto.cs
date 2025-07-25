﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StudentTrackingAPI.Core.ModelDtos
{
    public class StudentDto
    {
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public Guid? Id { get; set; }
        public string? StudentName { get; set; }
        public string? Class { get; set; }
        public string? ParentName { get; set; }
        public string? WatchId { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
