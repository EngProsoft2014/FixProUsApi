using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyEstimateScheduleDateDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? EstimateId { get; set; }
        public int? ScheduleDateId { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}