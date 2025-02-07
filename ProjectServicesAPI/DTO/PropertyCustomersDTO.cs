using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyCustomersDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Since { get; set; }
        public int? CustomerType { get; set; }
        public int? CategoryId { get; set; }
        public int? TaxId { get; set; }
        public bool? Taxable { get; set; }
        public decimal? Discount { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalcodeZIP { get; set; }
        public string YearBuit { get; set; }
        public string Squirefootage { get; set; }
        public string YearEstimedValue { get; set; }
        public string EstimedValue { get; set; }
        public string locationlatitude { get; set; }
        public string locationlongitude { get; set; }
        public string Phone1 { get; set; }
        public string Phone1WithoutPermission { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public bool? AllowLogin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Source { get; set; }
        public bool? MemeberType { get; set; }
        public int? MemeberId { get; set; }
        public string MemberName { get; set; }
        public decimal? Credit { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? MemeberExpireDate { get; set; }
        public PropertyCustomersCategoryDTO CustomerCategory { get; set; }
        public List<PropertyCustomersCustomFieldDTO> LstCustomersCustomField { get; set; }
        public PropertyMemberDTO MemberDTO { get; set; }
        public PropertyCampaignDTO CampaignDTO { get; set; }
        public PropertyTaxDTO TaxDTO { get; set; }
        public List<PropertySchedulesDTO> LstSchedules { get; set; }
        public List<PropertyEstimateDTO> LstEstimates { get; set; }
        public List<PropertyInvoiceDTO> LstInvoices { get; set; }
    }
}