using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FixProUsApi.DAL
{
    public class RepositoryPlansDAL
    {
        private readonly Entities _db;
        private bool disposed = false;
        private const string PROPERTY_Plans_CACHE_KEY = "Property Plans";
        private PropertyEmployeeDTO EmployeeCookie;
        public RepositoryPlansDAL()
        {
            _db = new Entities();
        }
        public int InsertPlans(PropertyPlansDTO model)
        {
            var entity = _db.Tbl_Plans.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower());

            if (entity != null)
            {
                throw new ArgumentException(message: $"This User Name {model.Name} already exists.");
            }
            else
            {
                entity = new Tbl_Plans
                {
                    Name = model.Name,
                    AnnualPrice = model.AnnualPrice,
                    MonthlyPrice = model.MonthlyPrice,
                    Branches = model.Branches,
                    UsersPermission = model.UsersPermission,
                    Support = model.Support,
                    CustomFields = model.CustomFields,
                    ReminderRules = model.ReminderRules,
                    Customers = model.Customers,
                    CustomersSection = model.CustomersSection,
                    CountUsers = model.CountUsers,
                    UsersNotificationSettings = model.UsersNotificationSettings,
                    Scheduling = model.Scheduling,
                    Equipmets = model.Equipmets,
                    Expenses = model.Expenses,
                    InvoiceQuotes = model.InvoiceQuotes,
                    Contracts = model.Contracts,
                    Payment = model.Payment,
                    Tracking = model.Tracking,
                    TimeSheet = model.TimeSheet,
                    Map = model.Map,
                    MessagesChat = model.MessagesChat,
                    Notes = model.Notes,
                    Reporting = model.Reporting,
                    Active = model.Active,
                    CreateDate = model.CreateDate,
                    CreateUser = model.CreateUser,
                };

                _db.Tbl_Plans.Add(entity);
                _db.SaveChanges();
            }

            return entity.Id;
        }

        public void UpdatePlans(PropertyPlansDTO model)
        {
            var entity = _db.Tbl_Plans.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
            if (EmployeeCookie != null && EmployeeCookie.UserType != null)
            {
                if (EmployeeCookie.UserType == 0)
                {
                    entity = _db.Tbl_Plans.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
                }
            }

            if (entity == null)
            {
                var entityId = _db.Tbl_Plans.FirstOrDefault(x => x.Id == model.Id);

                if (entityId != null)
                {
                    entityId.Name = model.Name;
                    entityId.AnnualPrice = model.AnnualPrice;
                    entityId.MonthlyPrice = model.MonthlyPrice;
                    entityId.Branches = model.Branches;
                    entityId.UsersPermission = model.UsersPermission;
                    entityId.Support = model.Support;
                    entityId.CustomFields = model.CustomFields;
                    entityId.ReminderRules = model.ReminderRules;
                    entityId.Customers = model.Customers;
                    entityId.CustomersSection = model.CustomersSection;
                    entityId.CountUsers = model.CountUsers;
                    entityId.UsersNotificationSettings = model.UsersNotificationSettings;
                    entityId.Scheduling = model.Scheduling;
                    entityId.Equipmets = model.Equipmets;
                    entityId.Expenses = model.Expenses;
                    entityId.InvoiceQuotes = model.InvoiceQuotes;
                    entityId.Contracts = model.Contracts;
                    entityId.Payment = model.Payment;
                    entityId.Tracking = model.Tracking;
                    entityId.TimeSheet = model.TimeSheet;
                    entityId.Map = model.Map;
                    entityId.MessagesChat = model.MessagesChat;
                    entityId.Notes = model.Notes;
                    entityId.Reporting = model.Reporting;

                    _db.Entry(entityId).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException(message: $"This Plans " + model.Name + " Is Deleted");
                }
            }
            else
            {
                throw new ArgumentException(message: $"The Plans Name {model.Name} already exists.");
            }
        }

        public void DeletePlans(PropertyPlansDTO model)
        {
            var entity = _db.Tbl_Plans.FirstOrDefault(x => x.Id == model.Id);
            if (EmployeeCookie != null && EmployeeCookie.UserType == 0)
            {
                entity = _db.Tbl_Plans.FirstOrDefault(x => x.Id == model.Id);
            }

            if (entity != null)
            {
                _db.Tbl_Plans.Remove(entity);
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This Plans " + model.Name + " Is Deleted");
            }
        }

        public List<PropertyPlansDTO> ToList()
        {
            var Planss = _db.Tbl_Plans.Where(x => x.Id != 0).Select(x => new PropertyPlansDTO
            {
                Id = x.Id,
                Name = x.Name,
                AnnualPrice = x.AnnualPrice,
                MonthlyPrice = x.MonthlyPrice,
                Branches = x.Branches,
                Support = x.Support,
                CustomFields = x.CustomFields,
                ReminderRules = x.ReminderRules,
                Customers = x.Customers,
                CustomersSection = x.CustomersSection,
                CountUsers = x.CountUsers,
                UsersPermission = x.UsersPermission,
                UsersNotificationSettings = x.UsersNotificationSettings,
                Scheduling = x.Scheduling,
                Equipmets = x.Equipmets,
                Expenses = x.Expenses,
                InvoiceQuotes = x.InvoiceQuotes,
                Contracts = x.Contracts,
                Payment = x.Payment,
                Tracking = x.Tracking,
                TimeSheet = x.TimeSheet,
                Map = x.Map,
                MessagesChat = x.MessagesChat,
                Notes = x.Notes,
                Reporting = x.Reporting,
                Active = x.Active,
                CreateDate = x.CreateDate,
                CreateUser = x.CreateUser,
            });

            return Planss.ToList();
        }

        public PropertyPlansDTO FindPlansById(int? id)
        {
            var Plans = _db.Tbl_Plans.Where(x => x.Id == id).Select(x => new PropertyPlansDTO
            {
                Id = x.Id,
                Name = x.Name,
                AnnualPrice = x.AnnualPrice,
                MonthlyPrice = x.MonthlyPrice,
                Branches = x.Branches,
                Support = x.Support,
                CustomFields = x.CustomFields,
                ReminderRules = x.ReminderRules,
                Customers = x.Customers,
                CustomersSection = x.CustomersSection,
                CountUsers = x.CountUsers,
                UsersPermission = x.UsersPermission,
                UsersNotificationSettings = x.UsersNotificationSettings,
                Scheduling = x.Scheduling,
                Equipmets = x.Equipmets,
                Expenses = x.Expenses,
                InvoiceQuotes = x.InvoiceQuotes,
                Contracts = x.Contracts,
                Payment = x.Payment,
                Tracking = x.Tracking,
                TimeSheet = x.TimeSheet,
                Map = x.Map,
                MessagesChat = x.MessagesChat,
                Notes = x.Notes,
                Reporting = x.Reporting,
                Active = x.Active,
                CreateDate = x.CreateDate,
                CreateUser = x.CreateUser,
            });

            return Plans.FirstOrDefault();
        }

        public List<SelectListItem> GetPropertyPlans()
        {
            WebCache.Remove(PROPERTY_Plans_CACHE_KEY);
            var result = WebCache.Get(PROPERTY_Plans_CACHE_KEY) as List<SelectListItem>;
            result = result == null ? new List<SelectListItem>() : result;
            result = _db.Tbl_Plans.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            WebCache.Set(PROPERTY_Plans_CACHE_KEY, result);

            return result;
        }

        public int Plans_Max()
        {
            var max = _db.Tbl_Plans.Max(x => (int?)x.Id);
            int LastId = 0;
            if (max == null)
            {
                LastId = 1;
            }
            else
            {
                LastId = Convert.ToInt32(max) + 1;
            }
            return LastId;
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
    }
}