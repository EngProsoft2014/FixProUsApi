using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyScheduleDateDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? ScheduleId { get; set; }
        public string Date { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SpentTimeHour { get; set; }
        public string SpentTimeMin { get; set; }
        public int? Status { get; set; }
        public string CalendarColor { get; set; }
        public string Reasonnotserve { get; set; }
        public int? InvoiceId { get; set; }
        public int? EstimateId { get; set; }
        public int? CreateOrginal_Custom { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string GoogleReviewLink { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public PropertyEmployeeDTO OneEmployee { get; set; } = new PropertyEmployeeDTO();

    }
}