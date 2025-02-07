using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyDataMapsDTO
    {
        public int Id { get; set; } = 0;
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string CreateDate { get; set; }
        public string Time { get; set; }
    }
}