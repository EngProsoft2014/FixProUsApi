//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FixProUsApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_EstimateItemsServices
    {
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> BrancheId { get; set; }
        public Nullable<int> EstimateId { get; set; }
        public Nullable<int> ItemsServicesId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> TaxId { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<bool> Taxable { get; set; }
        public Nullable<bool> Discountable { get; set; }
        public string Unit { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Tbl_Estimate Tbl_Estimate { get; set; }
        public virtual Tbl_Tax Tbl_Tax { get; set; }
        public virtual Tbl_ItemsServices Tbl_ItemsServices { get; set; }
        public virtual Tbl_Account Tbl_Account { get; set; }
        public virtual Tbl_Branches Tbl_Branches { get; set; }
        public virtual Tbl_Employee Tbl_Employee { get; set; }
    }
}
