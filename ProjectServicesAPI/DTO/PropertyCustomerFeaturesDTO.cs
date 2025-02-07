using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyCustomerFeaturesDTO
    {
        public List<PropertyCustomersCategoryDTO> LstCustomerCategory { get; set; }
        public List<PropertyCustomersCustomFieldDTO> LstCustomersCustomField { get; set; }
        public List<PropertyMemberDTO> LstMemberships { get; set; }
        public List<PropertyTaxDTO> LstTaxes { get; set; }
    }
}