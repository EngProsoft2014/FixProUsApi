using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyScheduleMaterialReceiptDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? ScheduleId { get; set; }
        public int? ScheduleDateId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? TechnicianId { get; set; }
        public decimal? Cost { get; set; }
        public string Notes { get; set; }
        public string ReceiptPhoto { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public PropertyCustomersDTO OneSupplier { get; set; }
    }
}