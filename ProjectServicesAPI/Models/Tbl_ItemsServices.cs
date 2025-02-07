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
    
    public partial class Tbl_ItemsServices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_ItemsServices()
        {
            this.Tbl_Calls = new HashSet<Tbl_Calls>();
            this.Tbl_EmployeeAssignItems = new HashSet<Tbl_EmployeeAssignItems>();
            this.Tbl_EmployeeAssignItemsLogs = new HashSet<Tbl_EmployeeAssignItemsLogs>();
            this.Tbl_EstimateItemsServices = new HashSet<Tbl_EstimateItemsServices>();
            this.Tbl_InvoiceItemsServices = new HashSet<Tbl_InvoiceItemsServices>();
            this.Tbl_ScheduleItemsServices = new HashSet<Tbl_ScheduleItemsServices>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> BrancheId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public Nullable<bool> InventoryItem { get; set; }
        public Nullable<int> QTYTime { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> CostperUnit { get; set; }
        public Nullable<bool> MemeberType { get; set; }
        public Nullable<int> MemeberId { get; set; }
        public Nullable<int> TaxId { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<bool> Taxable { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }
        public string SKU { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Calls> Tbl_Calls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItems> Tbl_EmployeeAssignItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItemsLogs> Tbl_EmployeeAssignItemsLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EstimateItemsServices> Tbl_EstimateItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_InvoiceItemsServices> Tbl_InvoiceItemsServices { get; set; }
        public virtual Tbl_Tax Tbl_Tax { get; set; }
        public virtual Tbl_Tax Tbl_Tax1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleItemsServices> Tbl_ScheduleItemsServices { get; set; }
        public virtual Tbl_Account Tbl_Account { get; set; }
        public virtual Tbl_Branches Tbl_Branches { get; set; }
        public virtual Tbl_ItemsServicesCategory Tbl_ItemsServicesCategory { get; set; }
        public virtual Tbl_ItemsServicesSubCategory Tbl_ItemsServicesSubCategory { get; set; }
        public virtual Tbl_Employee Tbl_Employee { get; set; }
    }
}
