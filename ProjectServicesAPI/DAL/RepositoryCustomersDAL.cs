using Antlr.Runtime.Misc;
using Autofac;
using FluentAssertions.Equivalency;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FixProUsApi.DAL
{
    public class RepositoryCustomersDAL
    {
        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryCustomersDAL()
        {
            _db = new Entities();
            _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        RepositorySchedulesDAL SchedulesDAL = new RepositorySchedulesDAL();
        public IEnumerable<PropertyCustomersDTO> GetAllCustInBranch(int? AccountId)
        {
            var Customers = _db.Tbl_Customers.Where(x => x.AccountId == AccountId).Select(x => new PropertyCustomersDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                YearBuit = x.YearBuit,
                Squirefootage = x.Squirefootage,
                EstimedValue = x.EstimedValue,
                YearEstimedValue = x.YearEstimedValue,
                Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ?  x.Phone1 : "xxxxxxxx",
                Phone1WithoutPermission = x.Phone1,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                MemeberType = x.MemeberType,
                MemeberExpireDate = x.MemeberExpireDate,
                CampaignDTO = _db.Tbl_Campaigns.Where(c => c.Id == x.Source).Select(s => new PropertyCampaignDTO
                {
                    Id = s.Id,
                    Lable = s.Lable,
                    Description = s.Description,
                }).FirstOrDefault(),
                MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                }).FirstOrDefault(),
                TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    Taxname = s.Taxname,
                    Rate = s.Rate,
                    Notes = s.Notes,
                    Active = s.Active,
                }).FirstOrDefault(),
            });

            return Customers.ToList();
        }

        public IEnumerable<PropertyCustomersDTO> GetAllCustInCall(int? AccountId)
        {
            var Customers = _db.Tbl_Customers.Where(x => x.AccountId == AccountId && x.CustomerType == 1).Select(x => new PropertyCustomersDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                YearBuit = x.YearBuit,
                Squirefootage = x.Squirefootage,
                EstimedValue = x.EstimedValue,
                YearEstimedValue = x.YearEstimedValue,
                Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Phone1 : "xxxxxxxx",
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                MemeberType = x.MemeberType,
                MemeberExpireDate = x.MemeberExpireDate,
                MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                }).FirstOrDefault(),
                TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    Taxname = s.Taxname,
                    Rate = s.Rate,
                    Notes = s.Notes,
                    Active = s.Active,
                }).FirstOrDefault(),
            });

            return Customers.ToList();
        }

        public IEnumerable<PropertyCustomersDTO> GetAllCustSuppliers(int? AccountId)
        {
            var Customers = _db.Tbl_Customers.Where(x => x.AccountId == AccountId && x.CustomerType == 3).Select(x => new PropertyCustomersDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                YearBuit = x.YearBuit,
                Squirefootage = x.Squirefootage,
                EstimedValue = x.EstimedValue,
                YearEstimedValue = x.YearEstimedValue,
                Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Phone1 : "xxxxxxxx",
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                MemeberType = x.MemeberType,
                MemeberExpireDate = x.MemeberExpireDate,
                MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                }).FirstOrDefault(),
                TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    Taxname = s.Taxname,
                    Rate = s.Rate,
                    Notes = s.Notes,
                    Active = s.Active,
                }).FirstOrDefault(),
            });

            return Customers.ToList();
        }


        public PropertyCustomersDTO GetOneCustDetails(int? CustId)
        {
            var Customer = _db.Tbl_Customers.Where(x => x.Id == CustId).Select(x => new PropertyCustomersDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                YearBuit = x.YearBuit,
                Squirefootage = x.Squirefootage,
                EstimedValue = x.EstimedValue,
                YearEstimedValue = x.YearEstimedValue,
                Phone1 = x.Phone1,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                MemeberType = x.MemeberType,
                MemeberExpireDate = x.MemeberExpireDate,
                MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                }).FirstOrDefault(),
                TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    Taxname = s.Taxname,
                    Rate = s.Rate,
                    Notes = s.Notes,
                    Active = s.Active,
                }).FirstOrDefault(),
            }).FirstOrDefault();

            if (Customer.MemberDTO != null)
            {
                if (Customer.MemeberExpireDate < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    Customer.MemberDTO.MemberValue = 0;
                    Customer.Discount = 0;
                }
            }

            return Customer;
        }

        public PropertyCustomersDTO GetListsOfCustomer(int? CustomerId)
        {
            var Customer = new PropertyCustomersDTO();

            Customer = _db.Tbl_Customers.Where(x => x.Id == CustomerId).Select(x => new PropertyCustomersDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Since = x.Since,
                CustomerType = x.CustomerType,
                CategoryId = x.CategoryId,
                TaxId = x.TaxId,
                Taxable = x.Taxable,
                Discount = x.Discount,
                Country = x.Country,
                Address = x.Address,
                City = x.City,
                State = x.State,
                YearBuit = x.YearBuit,
                Squirefootage = x.Squirefootage,
                EstimedValue = x.EstimedValue,
                YearEstimedValue = x.YearEstimedValue,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Phone1 : "xxxxxxxx",
                Phone1WithoutPermission = x.Phone1,
                Phone2 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Phone2 : "xxxxxxxx",
                Mobile = x.Mobile,
                Email = x.Email,
                Fax = x.Fax,
                Website = x.Website,
                AllowLogin = x.AllowLogin,
                UserName = x.UserName,
                Password = x.Password,
                Source = x.Source,
                MemeberType = x.MemeberType,
                MemeberId = x.MemeberId,
                MemeberExpireDate = x.MemeberExpireDate,
                MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                }).FirstOrDefault(),
                TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    Taxname = s.Taxname,
                    Rate = s.Rate,
                    Notes = s.Notes,
                    Active = s.Active,
                }).FirstOrDefault(),
                CampaignDTO = _db.Tbl_Campaigns.Where(c => c.Id == x.Source).Select(s => new PropertyCampaignDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    Lable = s.Lable,
                    Description = s.Description,
                    Active = s.Active,
                }).FirstOrDefault(),
                Credit = x.Credit,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                CustomerCategory = _db.Tbl_CustomersCategory.Where(c => c.Id == x.CategoryId).Select(s => new PropertyCustomersCategoryDTO
                {
                    Id = s.Id,
                    CategoryName = s.CategoryName,
                }).FirstOrDefault(),
                LstCustomersCustomField = _db.Tbl_CustomersCustomField.Where(c => c.AccountId == x.AccountId).Select(m => new PropertyCustomersCustomFieldDTO
                {
                    Id = m.Id,
                    AccountId = m.AccountId,
                    BrancheId = m.BrancheId,
                    CustomFieldName = m.CustomFieldName,
                    FieldType = m.FieldType,
                    DefaultValue = _db.Tbl_TransactionCustomersCustomField.Where(v => v.CustomerId == CustomerId && v.CustomFieldId == m.Id).FirstOrDefault().Value ?? m.DefaultValue ,
                    Required = m.Required,
                    Notes = m.Notes,
                    Active = m.Active
                }).ToList(),
            }).FirstOrDefault();


            var Schedules = _db.Tbl_Schedule.Where(s => s.CustomerId == CustomerId).ToList();

            List<PropertySchedulesDTO> lstSchedules = new List<PropertySchedulesDTO>();

            foreach (var sch in Schedules)
            {
                var SchedulesDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == sch.Id).ToList();

                foreach (var schedule in SchedulesDate)
                {
                    PropertySchedulesDTO Oobj = new PropertySchedulesDTO
                    {
                        Id = schedule.ScheduleId.Value,
                        ScheduleDateId = schedule.Id,
                        ContractId = sch.ContractId,
                        Title = schedule.Tbl_Schedule.Title,
                        ScheduleDate = schedule.Date,
                        CalendarColor = schedule.CalendarColor,
                    };

                    lstSchedules.Add(Oobj);
                }
            }

            Customer.LstSchedules = lstSchedules;

            //Customer.LstSchedules = _db.Tbl_Schedule.Where(v => v.CustomerId == CustomerId).Select(c => new PropertySchedulesDTO
            //{
            //    Id = c.Id,
            //    Title = c.Title,
            //    ScheduleDate = c.ScheduleDate,
            //}).ToList();

            Customer.LstInvoices = _db.Tbl_Invoice.Where(c => c.CustomerId == CustomerId).Select(s => new PropertyInvoiceDTO
            {
                Id = s.Id,
                ScheduleId = s.ScheduleId,
                EstimateId = s.EstimateId,
                ScheduleName = s.Tbl_Schedule.Title,
                CreateDate = s.CreateDate,
            }).ToList();

            Customer.LstEstimates = _db.Tbl_Estimate.Where(c => c.CustomerId == CustomerId).Select(s => new PropertyEstimateDTO
            {
                Id = s.Id,
                ScheduleId = s.ScheduleId,
                ScheduleName = s.Tbl_Schedule.Title,
                Status = s.Status,
                InvoiceId = s.InvoiceId != null ? s.InvoiceId : s.ScheduleId != null ? _db.Tbl_Invoice.Where(m => m.ScheduleId == s.ScheduleId).FirstOrDefault().Id : s.InvoiceId,
                CreateDate = s.CreateDate,
            }).ToList();


            return Customer;
        }

        public PropertyObjectCustomerDTO GetObjectOfCustomerDetails(int? InvoiceId, int? EstimateId)
        {
            var Customer = new PropertyObjectCustomerDTO();

            Customer.ObjEstimate = _db.Tbl_Estimate.Where(c => c.Id == EstimateId).Select(x => new PropertyEstimateDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                ScheduleId = x.ScheduleId,
                ScheduleName = x.Tbl_Schedule.Title,
                EstimateDate = x.EstimateDate,
                CustomerId = x.CustomerId,
                CustomerEmail = x.Tbl_Customers.Email,
                Total = x.Total,
                TaxId = x.TaxId,
                Tax = x.Tax,
                Taxval = x.Taxval,
                MemberId = x.MemberId,
                Discount = x.Discount,
                DiscountAmountOrPercent = x.DiscountAmountOrPercent,
                Net = x.Net,
                Status = x.Status,
                SignaturePrintName = x.SignaturePrintName,
                SignatureDraw = x.SignatureDraw,
                Terms = x.Terms,
                NotesForCustomer = x.NotesForCustomer,
                Notes = x.Notes,
                InvoiceId = x.InvoiceId != null ? x.InvoiceId : x.ScheduleId != null ? _db.Tbl_Invoice.Where(m => m.ScheduleId == x.ScheduleId).FirstOrDefault().Id : x.InvoiceId,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                LstEstimateItemServices = _db.Tbl_EstimateItemsServices.Where(p => p.EstimateId == EstimateId).Select(y => new PropertyEstimateItemServicesDTO
                {
                    Id = y.Id,
                    AccountId = y.AccountId,
                    BrancheId = y.BrancheId,
                    EstimateId = y.EstimateId,
                    ItemsServicesId = y.ItemsServicesId,
                    ItemsServicesName = _db.Tbl_ItemsServices.Where(v => v.Id == y.ItemsServicesId).Select(s => s.Name).FirstOrDefault(),
                    Price = y.Price,
                    TaxId = y.TaxId,
                    Tax = y.Tax,
                    Total = y.Total,
                    Quantity = y.Quantity,
                    Taxable = y.Taxable,
                    Discountable = y.Discountable,
                    Unit = y.Unit,
                    Active = y.Active,
                    CreateUser = y.CreateUser,
                    CreateDate = y.CreateDate,
                }).ToList(),

                LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.EstimateId == EstimateId).Select(D => new PropertyScheduleDateDTO
                {
                    Id = D.Id,
                    Date = D.Date,
                }).ToList(),
            }).FirstOrDefault();

            Customer.ObjInvoice = _db.Tbl_Invoice.Where(c => c.Id == InvoiceId).Select(s => new PropertyInvoiceDTO
            {
                Id = s.Id,
                AccountId = s.AccountId,
                BrancheId = s.BrancheId,
                ContractId = s.ContractId,
                ContractInvoiceId = s.ContractInvoiceId,
                EstimateId = s.EstimateId,
                ScheduleId = s.ScheduleId,
                ScheduleName = s.Tbl_Schedule.Title,
                InvoiceDate = s.InvoiceDate,
                CustomerId = s.CustomerId,
                CustomerEmail = s.Tbl_Customers.Email,
                Total = s.Total,
                TaxId = s.TaxId,
                Tax = s.Tax,
                Taxval = s.Taxval,
                MemberId = s.MemberId,
                Discount = s.Discount,
                DiscountAmountOrPercent = s.DiscountAmountOrPercent,
                Paid = s.Paid,
                Net = s.Net,
                Status = s.Status,
                Type = s.Type,
                Terms = s.Terms,
                NotesForCustomer = s.NotesForCustomer,
                Notes = s.Notes,
                Active = s.Active,
                CreateUser = s.CreateUser,
                CreateDate = s.CreateDate,
                LstInvoiceItemServices = _db.Tbl_InvoiceItemsServices.Where(p => p.InvoiceId == InvoiceId).Select(y => new PropertyInvoiceItemServicesDTO
                {
                    Id = y.Id,
                    AccountId = y.AccountId,
                    BrancheId = y.BrancheId,
                    InvoiceId = y.InvoiceId,
                    ItemServiceDescription = y.ItemServiceDescription,
                    ItemsServicesId = y.ItemsServicesId,
                    ItemsServicesName = _db.Tbl_ItemsServices.Where(v => v.Id == y.ItemsServicesId).Select(n => n.Name).FirstOrDefault(),
                    Price = y.Price,
                    TaxId = y.TaxId,
                    Tax = y.Tax,
                    Total = y.Total,
                    Quantity = y.Quantity,
                    Taxable = y.Taxable,
                    Discountable = y.Discountable,
                    Unit = y.Unit,
                    Active = y.Active,
                    CreateUser = y.CreateUser,
                    CreateDate = y.CreateDate,
                    SkipOfTotal = y.SkipOfTotal,
                }).ToList(),
                LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.InvoiceId == InvoiceId).Select(D => new PropertyScheduleDateDTO
                {
                    Id = D.Id,
                    Date = D.Date,
                }).ToList(),
            }).FirstOrDefault();

            return Customer;
        }

        public PropertyCustomerFeaturesDTO GetAllCustomerFeatures(int? AccountId)
        {
            PropertyCustomerFeaturesDTO ObjFeatures = new PropertyCustomerFeaturesDTO();
            ObjFeatures = new PropertyCustomerFeaturesDTO
            {
                LstCustomerCategory = _db.Tbl_CustomersCategory.Where(c => c.AccountId == AccountId).Select(s => new PropertyCustomersCategoryDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    CategoryName = s.CategoryName,
                    Description = s.Description,
                    Notes = s.Notes,
                    Active = s.Active,
                    CreateUser = s.CreateUser,
                    CreateDate = s.CreateDate,
                }).ToList(),

                LstCustomersCustomField = _db.Tbl_CustomersCustomField.Where(c => c.AccountId == AccountId).Select(s => new PropertyCustomersCustomFieldDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    CustomFieldName = s.CustomFieldName,
                    FieldType = s.FieldType,
                    DefaultValue = s.DefaultValue,
                    Required = s.Required,
                    Notes = s.Notes,
                    Active = s.Active,
                    CreateUser = s.CreateUser,
                    CreateDate = s.CreateDate,
                }).ToList(),
                LstMemberships = _db.Tbl_Member.Where(c => c.AccountId == AccountId).Select(s => new PropertyMemberDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    Name = s.MemberType == false ? (s.Name + " " + "%" + s.MemberValue) : (s.Name + " " + "$" + s.MemberValue),
                    MemberType = s.MemberType,
                    MemberValue = s.MemberValue,
                    Notes = s.Notes,
                    Active = s.Active,
                    CreateDate = s.CreateDate,
                    CreateUser = s.CreateUser,
                }).ToList(),
                LstTaxes = _db.Tbl_Tax.Where(c => c.AccountId == AccountId).Select(s => new PropertyTaxDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    Taxname = s.Taxname,
                    Description = s.Description,
                    Rate = s.Rate,
                    Globaldefaultrate = s.Globaldefaultrate,
                    Notes = s.Notes,
                    Active = s.Active,
                    CreateUser = s.CreateUser,
                    CreateDate = s.CreateDate,
                }).ToList(),

            };
            return ObjFeatures;
        }

        public PropertyCustomersDTO InsertCustomer(PropertyCustomersDTO model)
        {
            //Check Customer
            //var entityCustomer = _db.Tbl_Customers.FirstOrDefault(x => x.Email == model.Email || x.Phone1 == model.Phone1); 
            var entityCustomer = _db.Tbl_Customers.FirstOrDefault(x => x.Phone1 == model.Phone1);
            if (!string.IsNullOrEmpty(model.Email))
            {
                var entity = _db.Tbl_Customers.Where(x => string.IsNullOrEmpty(x.Email) == false).FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());

                if (entity != null && entity.Email.ToLower() == model.Email.ToLower())
                {
                    throw new ArgumentException(message: $"This Customer Email {model.Email}  Already Exist.");
                }
            }

            if (entityCustomer != null)
            {
                throw new ArgumentException(message: $"The Customer already exists.");
            }
            else
            {
                var entityCust = new Tbl_Customers
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Since = model.Since,
                    CustomerType = model.CustomerType,
                    CategoryId = model.CategoryId,
                    TaxId = model.TaxId,
                    Taxable = model.Taxable,
                    Discount = model.Discount,
                    Country = model.Country,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    PostalcodeZIP = model.PostalcodeZIP,
                    locationlatitude = model.locationlatitude,
                    locationlongitude = model.locationlongitude,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    Mobile = model.Mobile,
                    Email = model.Email,
                    Fax = model.Fax,
                    Website = model.Website,
                    AllowLogin = model.AllowLogin,
                    UserName = model.UserName,
                    Password = model.Password,
                    Source = model.Source,
                    MemeberType = model.MemeberType,
                    MemeberId = model.MemeberId,
                    MemeberExpireDate = model.MemeberExpireDate,
                    YearBuit = model.YearBuit,
                    Squirefootage = model.Squirefootage,
                    EstimedValue = model.EstimedValue,
                    Credit = model.Credit,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                };

                _db.Tbl_Customers.Add(entityCust);
                _db.SaveChanges();

                int CustomerId = entityCust.Id;

                foreach (var item in model.LstCustomersCustomField)
                {
                    var entityCustomField = new Tbl_TransactionCustomersCustomField
                    {
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        CustomFieldId = item.Id,
                        Value = item.DefaultValue,
                        CustomerId = CustomerId,
                        Active = model.Active,
                        CreateUser = model.CreateUser,
                        CreateDate = DateTime.Now,
                    };
                    _db.Tbl_TransactionCustomersCustomField.Add(entityCustomField);
                    _db.SaveChanges();
                }

                var Customer = _db.Tbl_Customers.Where(x => x.Id == CustomerId).Select(x => new PropertyCustomersDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    locationlatitude = x.locationlatitude,
                    locationlongitude = x.locationlongitude,
                    YearBuit = x.YearBuit,
                    Squirefootage = x.Squirefootage,
                    EstimedValue = x.EstimedValue,
                    Phone1 = x.Phone1,
                    MemeberType = x.MemeberType,
                    MemeberExpireDate = model.MemeberExpireDate,
                    MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.MemeberId).Select(s => new PropertyMemberDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MemberType = s.MemberType,
                        MemberValue = s.MemberValue,
                    }).FirstOrDefault(),
                    TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.TaxId).Select(s => new PropertyTaxDTO
                    {
                        Id = s.Id,
                        Taxname = s.Taxname,
                        Rate = s.Rate,
                        Notes = s.Notes,
                        Active = s.Active,
                    }).FirstOrDefault(),
                    CampaignDTO = _db.Tbl_Campaigns.Where(c => c.Id == x.Source).Select(s => new PropertyCampaignDTO
                    {
                        Id = s.Id,
                        AccountId = s.AccountId,
                        BrancheId = s.BrancheId,
                        Lable = s.Lable,
                        Description = s.Description,
                        Active = s.Active,
                    }).FirstOrDefault(),
                }).FirstOrDefault();
                return Customer;
            }
        }

        public string UpdateCustomers(PropertyCustomersDTO model)
        {
            var entityId = _db.Tbl_Customers.FirstOrDefault(x => x.Id == model.Id);
            if (entityId != null)
            {
                entityId.Squirefootage = model.Squirefootage;
                entityId.YearBuit = model.YearBuit;
                entityId.YearEstimedValue = model.YearEstimedValue;
                entityId.EstimedValue = model.EstimedValue;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                return entityId.YearEstimedValue;
            }
            else
            {
                throw new ArgumentException(message: $"This Employee " + model.UserName + " Is Deleted");
            }

        }


        public void EditCustomer(PropertyCustomersDTO model)
        {
            var entityId = _db.Tbl_Customers.Where(x => x.Id == model.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(model.Email))
            {
                var entity = _db.Tbl_Customers.Where(x => x.Id != model.Id && string.IsNullOrEmpty(x.Email) == false).FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());

                if (entity != null && entity.Email.ToLower() == model.Email.ToLower())
                {
                    model.Email = entityId.Email;
                }
            }

            if (entityId != null)
            {
                //entityId.AccountId = model.AccountId;
                //entityId.BrancheId = model.BrancheId;
                entityId.FirstName = model.FirstName;
                entityId.LastName = model.LastName;
                //entityId.Since = model.Since;
                //entityId.CustomerType = model.CustomerType;
                //entityId.CategoryId = model.CategoryId;
                //entityId.TaxId = model.TaxId;
                //entityId.Taxable = model.Taxable;
                //entityId.Discount = model.Discount;
                entityId.Country = model.Country;
                entityId.Address = model.Address;
                entityId.City = model.City;
                entityId.State = model.State;
                entityId.PostalcodeZIP = model.PostalcodeZIP;
                entityId.locationlatitude = model.locationlatitude;
                entityId.locationlongitude = model.locationlongitude;
                entityId.Phone1 = model.Phone1;
                entityId.EstimedValue = model.EstimedValue;
                entityId.Squirefootage = model.Squirefootage;
                entityId.YearBuit = model.YearBuit;
                //entityId.Phone2 = model.Phone2;
                //entityId.Mobile = model.Mobile;
                entityId.Email = model.Email;
                //entityId.Fax = model.Fax;
                //entityId.Website = model.Website;
                //entityId.AllowLogin = model.AllowLogin;
                //entityId.UserName = model.UserName;
                //entityId.Password = model.Password;
                //entityId.Source = model.Source;
                //entityId.MemeberType = model.MemeberType;
                //entityId.MemeberId = model.MemeberId;
                //entityId.MemeberExpireDate = model.MemeberExpireDate;
                //entityId.Credit = model.Credit;
                //entityId.Notes = model.Notes;
                //entityId.Active = model.Active;
                //entityId.CreateUser = model.CreateUser;
                //entityId.CreateDate = model.CreateDate;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                //var LstCustomFeilds = _db.Tbl_TransactionCustomersCustomField.Where(x => x.CustomerId == model.Id).ToList();

                //if (LstCustomFeilds.Count > 0)
                //{
                //    _db.Tbl_TransactionCustomersCustomField.RemoveRange(LstCustomFeilds);
                //    _db.SaveChanges();
                //}

                //foreach (var item in model.LstCustomersCustomField)
                //{
                //    var entityCustomField = new Tbl_TransactionCustomersCustomField
                //    {
                //        AccountId = item.AccountId,
                //        BrancheId = item.BrancheId,
                //        CustomFieldId = item.Id,
                //        Value = item.DefaultValue,
                //        CustomerId = model.Id,
                //        Active = item.Active,
                //        CreateUser = item.CreateUser,
                //        CreateDate = item.CreateDate,
                //    };
                //    _db.Tbl_TransactionCustomersCustomField.Add(entityCustomField);
                //    _db.SaveChanges();
                //}

                //var EntityMember = _db.Tbl_Member.Where(c => c.Id == model.MemberDTO.Id).FirstOrDefault();

                //if (EntityMember != null)
                //{
                //    EntityMember.Id = model.MemberDTO.Id;
                //    EntityMember.Name = model.MemberDTO.Name;
                //    EntityMember.MemberType = model.MemberDTO.MemberType;
                //    EntityMember.MemberValue = model.MemberDTO.MemberValue;

                //    _db.Entry(EntityMember).State = EntityState.Modified;
                //    _db.SaveChanges();
                //}

                //var EntityTax = _db.Tbl_Tax.Where(c => c.Id == model.TaxDTO.Id).FirstOrDefault();

                //if (EntityTax != null)
                //{
                //    EntityTax.Id = model.TaxDTO.Id;
                //    EntityTax.Taxname = model.TaxDTO.Taxname;
                //    EntityTax.Rate = model.TaxDTO.Rate;
                //    EntityTax.Notes = model.TaxDTO.Notes;
                //    EntityTax.Active = model.TaxDTO.Active;

                //    _db.Entry(EntityTax).State = EntityState.Modified;
                //    _db.SaveChanges();
                //}
            }
            else
            {
                throw new ArgumentException(message: $"This Customer " + model.UserName + " Is Deleted");
            }

        }

    }
}