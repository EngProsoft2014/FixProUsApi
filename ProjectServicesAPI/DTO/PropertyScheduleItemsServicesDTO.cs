using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyScheduleItemsServicesDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? ScheduleId { get; set; }
        public int? ScheduleDateId { get; set; }
        public int? ItemsServicesId { get; set; }
        public string ItemsServicesName { get; set; }
        public string ItemServiceDescription { get; set; }
        public decimal? CostRate { get; set; }
        public int? Quantity { get; set; }
        public int? TaxId { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public decimal? Price { get; set; }
        public int? TypeMaterial_Services { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}