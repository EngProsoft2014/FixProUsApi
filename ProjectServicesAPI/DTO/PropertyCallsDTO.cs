﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyCallsDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? ScheduleId { get; set; }
        public int? ScheduleDateId { get; set; }
        public string ScheduleTitle { get; set; }
        public string PhoneNum { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }
        public int? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int? ServiceId { get; set; }
        public string EmployeeName { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}