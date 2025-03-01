﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyOneSignalNotificationDTO
    {
        public int? Id { set; get; }
        public int? AccountId { get; set; }
        public int? ScheduleId { set; get; }
        public int? ScheduleDateId { set; get; }
        public string ScheduleDate { set; get; }
        public int? EmployeeId { set; get; }
        public int? NotificationType { set; get; }
        public string NotificationHeader { set; get; }
        public string NotificationContent { set; get; }
        public bool? Active { set; get; }
        public int? CreateUser { set; get; }
        public DateTime? CreateDate { set; get; }
        public int? UpdateUser { set; get; }
        public DateTime? UpdateDate { set; get; }
    }
}