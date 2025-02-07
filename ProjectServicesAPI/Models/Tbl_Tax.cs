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
    
    public partial class Tbl_Tax
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Tax()
        {
            this.Tbl_Customers = new HashSet<Tbl_Customers>();
            this.Tbl_EmployeeAssignItems = new HashSet<Tbl_EmployeeAssignItems>();
            this.Tbl_EmployeeAssignItemsLogs = new HashSet<Tbl_EmployeeAssignItemsLogs>();
            this.Tbl_Estimate = new HashSet<Tbl_Estimate>();
            this.Tbl_EstimateItemsServices = new HashSet<Tbl_EstimateItemsServices>();
            this.Tbl_InvoiceItemsServices = new HashSet<Tbl_InvoiceItemsServices>();
            this.Tbl_ItemsServices = new HashSet<Tbl_ItemsServices>();
            this.Tbl_ItemsServices1 = new HashSet<Tbl_ItemsServices>();
            this.Tbl_ScheduleItemsServices = new HashSet<Tbl_ScheduleItemsServices>();
            this.Tbl_Invoice = new HashSet<Tbl_Invoice>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> BrancheId { get; set; }
        public string Taxname { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<bool> Globaldefaultrate { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customers> Tbl_Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItems> Tbl_EmployeeAssignItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItemsLogs> Tbl_EmployeeAssignItemsLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Estimate> Tbl_Estimate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EstimateItemsServices> Tbl_EstimateItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_InvoiceItemsServices> Tbl_InvoiceItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ItemsServices> Tbl_ItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ItemsServices> Tbl_ItemsServices1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleItemsServices> Tbl_ScheduleItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Invoice> Tbl_Invoice { get; set; }
        public virtual Tbl_Account Tbl_Account { get; set; }
        public virtual Tbl_Branches Tbl_Branches { get; set; }
        public virtual Tbl_Employee Tbl_Employee { get; set; }
    }
}
