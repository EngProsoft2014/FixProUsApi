﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyInvoiceItemServicesDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? InvoiceId { get; set; }
        public string ItemServiceDescription { get; set; }
        public int? ItemsServicesId { get; set; }
        public string ItemsServicesName { get; set; }
        public decimal? Price { get; set; }
        public int? TaxId { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }
        public bool? Taxable { get; set; }
        public bool? Discountable { get; set; }
        public string Unit { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? SkipOfTotal { get; set; }
        public bool? InventoryInvoiceItemService { get; set; }
    }
}