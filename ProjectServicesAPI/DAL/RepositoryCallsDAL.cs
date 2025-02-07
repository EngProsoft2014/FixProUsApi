using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FixProUsApi.DAL
{
    public class RepositoryCallsDAL 
    {
        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryCallsDAL()
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

        public IEnumerable<PropertyCallsDTO> GetAllCalls(int? AccountId)
        {
            var lastDt = DateTime.Now.AddMonths(-1);
            var Day = DateTime.Now;

            var Calls = _db.Tbl_Calls.Where(x => x.AccountId == AccountId && (x.CreateDate >= lastDt && x.CreateDate <= Day)).Select(x => new PropertyCallsDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                CustomerId = x.CustomerId,
                //CustomerName = _db.Tbl_Customers.Where(c => c.Id == x.CustomerId).Select(s => s.FirstName + "" + s.LastName).FirstOrDefault(),
                CustomerName = x.Tbl_Customers.FirstName + " " + x.Tbl_Customers.LastName,
                ScheduleId = x.ScheduleId,
                ScheduleDateId = x.Tbl_Schedule.Tbl_ScheduleDate.Where(m => m.CreateOrginal_Custom == 1 && m.ScheduleId == x.ScheduleId).FirstOrDefault().Id,
                ScheduleTitle = x.Tbl_Schedule.Title,
                PhoneNum =  x.PhoneNum,
                ReasonId = x.ReasonId,
                //ReasonName = _db.Tbl_CallReason.Where(c => c.Id == x.ReasonId).Select(s => s.Lable).FirstOrDefault(),
                ReasonName = x.Tbl_CallReason.Lable,
                CampaignId = x.CampaignId,
                //CampaignName = _db.Tbl_Campaigns.Where(c => c.Id == x.CampaignId).Select(s => s.Lable).FirstOrDefault(),
                CampaignName = x.Tbl_Campaigns.Lable,
                EmployeeName = x.Tbl_Employee.FirstName + " " + x.Tbl_Employee.LastName,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            }).OrderByDescending(x=> x.CreateDate);

            return Calls.ToList();
        }

        public IEnumerable<PropertyCallsDTO> GetFilterCalls(string StartDate, string EndDate, string PhoneNum, string ReasonId, string CampaignId, string EmployeeId, string SchTitle)
        {
            var Calls = _db.Tbl_Calls.ToList().Select(x => new PropertyCallsDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                CustomerId = x.CustomerId,
                CustomerName = x.Tbl_Customers?.FirstName + " " + x.Tbl_Customers?.LastName,
                ScheduleId = x.ScheduleId,
                ScheduleTitle = x.Tbl_Schedule?.Title,
                PhoneNum = x.PhoneNum,
                ReasonId = x.ReasonId,
                ReasonName = x.Tbl_CallReason?.Lable,
                CampaignId = x.CampaignId,
                CampaignName = x.Tbl_Campaigns?.Lable,
                EmployeeName = x.Tbl_Employee?.FirstName + " " + x.Tbl_Employee?.LastName,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            });

            if (string.IsNullOrEmpty(PhoneNum) != true)
            {
                Calls = Calls.Where(m => m.PhoneNum == PhoneNum);
            }

            if (string.IsNullOrEmpty(ReasonId) != true)
            {
                Calls = Calls.Where(m => m.ReasonId == int.Parse(ReasonId));
            }

            if (string.IsNullOrEmpty(CampaignId) != true)
            {
                Calls = Calls.Where(m => m.CampaignId == int.Parse(CampaignId));
            }

            if (string.IsNullOrEmpty(SchTitle) != true)
            {
                Calls = Calls.Where(m => m.ScheduleId != null && m.ScheduleTitle.Contains(SchTitle) == true);
            }

            if (string.IsNullOrEmpty(EmployeeId) != true)
            {
                Calls = Calls.Where(m => m.CreateUser == int.Parse(EmployeeId));
            }

            if (string.IsNullOrEmpty(StartDate) != true && string.IsNullOrEmpty(EndDate) != true)
            {
                DateTime StartDt = DateTime.Parse(DateTime.Parse(StartDate).ToString("yyyy-MM-dd"));
                DateTime EndDt = DateTime.Parse(DateTime.Parse(EndDate).ToString("yyyy-MM-dd"));

                Calls = Calls.Where(m => m.CreateDate >= StartDt && m.CreateDate <= EndDt);
            }

            return Calls.ToList().OrderByDescending(x => x.CreateDate);
        }

        public IEnumerable<PropertyReasonDTO> GetReasons(int? AccountId)
        {
            var Reasons = _db.Tbl_CallReason.Where(x => x.AccountId == AccountId).Select(x => new PropertyReasonDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                Lable = x.Lable,
                ReasonCode = x.ReasonCode,
                Description = x.Description,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            });

            return Reasons.ToList();
        }


        public IEnumerable<PropertyCampaignDTO> GetCampaigns(int? AccountId)
        {
            var Campaigns = _db.Tbl_Campaigns.Where(x => x.AccountId == AccountId).Select(x => new PropertyCampaignDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                Lable = x.Lable,
                Description = x.Description,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            });

            return Campaigns.ToList();
        }


        public PropertyCallsDTO InsertCall(PropertyCallsDTO model)
        {
            var entityCall = new Tbl_Calls
            {
                AccountId = model.AccountId,
                BrancheId = model.BrancheId,
                CustomerId = model.CustomerId == 0 ? null : model.CustomerId,
                ScheduleId = model.ScheduleId == 0 ? null : model.ScheduleId,
                PhoneNum = model.PhoneNum,
                ReasonId = model.ReasonId == 0 ? null : model.ReasonId,
                CampaignId = model.CampaignId == 0 ? null : model.CampaignId,
                Notes = model.Notes,
                Active = model.Active,
                CreateUser = model.CreateUser,
                CreateDate = DateTime.Now,
            };

            _db.Tbl_Calls.Add(entityCall);
            _db.SaveChanges();

            var Call = _db.Tbl_Calls.Where(x => x.Id == entityCall.Id).Select(x => new PropertyCallsDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                CustomerId = x.CustomerId,
                CustomerName = x.Tbl_Customers.FirstName + " " + x.Tbl_Customers.LastName,
                ScheduleId = x.ScheduleId,
                ScheduleTitle = x.Tbl_Schedule.Title,
                PhoneNum = x.PhoneNum,
                ReasonId = x.ReasonId,
                ReasonName = x.Tbl_CallReason.Lable,
                CampaignId = x.CampaignId,
                CampaignName = x.Tbl_Campaigns.Lable,
                EmployeeName = x.Tbl_Employee.FirstName + " " + x.Tbl_Employee.LastName,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            }).FirstOrDefault();

            return Call;
        }


        public PropertyCallsDTO UpdateCall(PropertyCallsDTO model)
        {
            var entityId = _db.Tbl_Calls.FirstOrDefault(x => x.Id == model.Id);
            if (entityId != null)
            {
                entityId.AccountId = model.AccountId;
                entityId.BrancheId = model.BrancheId;
                entityId.ScheduleId = model.ScheduleId;
                entityId.CustomerId = model.CustomerId;
                entityId.PhoneNum = model.PhoneNum;
                entityId.ReasonId = model.ReasonId;
                entityId.CampaignId = model.CampaignId;
                entityId.Notes = model.Notes;
                entityId.Active = model.Active;
                entityId.CreateUser = model.CreateUser;
                entityId.CreateDate = model.CreateDate;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                var Call = _db.Tbl_Calls.Where(x => x.Id == model.Id).Select(x => new PropertyCallsDTO
                {
                    Id = x.Id,
                    AccountId = x.AccountId,
                    BrancheId = x.BrancheId,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Tbl_Customers.FirstName + " " + x.Tbl_Customers.LastName,
                    ScheduleId = x.ScheduleId,
                    ScheduleTitle = x.Tbl_Schedule.Title,
                    PhoneNum = x.PhoneNum,
                    ReasonId = x.ReasonId,
                    ReasonName = x.Tbl_CallReason.Lable,
                    CampaignId = x.CampaignId,
                    CampaignName = x.Tbl_Campaigns.Lable,
                    EmployeeName = x.Tbl_Employee.FirstName + " " + x.Tbl_Employee.LastName,
                    Notes = x.Notes,
                    Active = x.Active,
                    CreateUser = x.CreateUser,
                    CreateDate = x.CreateDate,
                }).FirstOrDefault();

                return Call;
            }
            else
            {
                throw new ArgumentException(message: $"This Call Number " + model.Id + " Is Deleted");
            }
        }

        public void RemoveCall(int CallId)
        {
            var entity = _db.Tbl_Calls.FirstOrDefault(x => x.Id == CallId);
            if (entity == null)
            {
                throw new ArgumentException(message: $"This Call Number " + CallId + " Is Not Deleted");
                return;
            }
            else
            {
                _db.Tbl_Calls.Remove(entity);
                _db.SaveChanges();
            }
        }


    }

}