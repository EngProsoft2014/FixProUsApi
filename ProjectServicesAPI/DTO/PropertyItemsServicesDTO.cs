using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyItemsServicesDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }    //<option value="1">Service</option><option selected = "selected" value="2">Item</option><option value = "3" > Labor </ option >< option value="4">Other</option>
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public bool? InventoryItem { get; set; }
        public int? QTYTime { get; set; }
        public string Unit { get; set; }
        public decimal? CostperUnit { get; set; }
        public bool? MemeberType { get; set; }
        public int? MemeberId { get; set; }
        public int? TaxId { get; set; }
        public decimal? Tax { get; set; }
        public bool? Taxable { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }
        public string SKU { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}