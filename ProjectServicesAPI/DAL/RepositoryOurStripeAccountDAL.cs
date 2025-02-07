using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FixProUsApi.DAL
{
    public class RepositoryOurStripeAccountDAL
    {
        private readonly Entities _db;
        private bool disposed = false;
        private PropertyEmployeeDTO EmployeeCookie;

        public RepositoryOurStripeAccountDAL()
        {
            _db = new Entities(); 
        }

        public void InsertOurStripeAccount(PropertyOurStripeAccountDTO model)
        {
            var entity = new Tbl_OurStripeAccount
            {
                Username = model.Username,
                Password = model.Password,
                SecretKey = model.SecretKey,
                Active = model.Active,
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate,
            };

            _db.Tbl_OurStripeAccount.Add(entity);

            _db.SaveChanges();
        }

        public void UpdateOurStripeAccount(PropertyOurStripeAccountDTO model)
        {
            var entity = _db.Tbl_OurStripeAccount.FirstOrDefault(x => x.Id == model.Id);

            if (entity != null)
            {
                entity.Username = model.Username;
                entity.Password = model.Password;
                entity.SecretKey = model.SecretKey;
                entity.Active = model.Active;

                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"The Stripe Account to this Branche  already exists.");
            }
        }

        public void DeleteOurStripeAccount(PropertyOurStripeAccountDTO model)
        {
            var entity = _db.Tbl_OurStripeAccount.FirstOrDefault(x => x.Id == model.Id);

            if (entity != null)
            {
                _db.Tbl_OurStripeAccount.Remove(entity);
                _db.SaveChanges();
            }
        }

        public List<PropertyOurStripeAccountDTO> ToList()
        {
            var OurStripeAccount = _db.Tbl_OurStripeAccount.Where(x => x.Id != 0).Select(x => new PropertyOurStripeAccountDTO
            {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                SecretKey = x.SecretKey,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            }).ToList();

            return OurStripeAccount.OrderBy(c => c.Id).ToList();
        }

        public PropertyOurStripeAccountDTO FindOurStripeAccountById()
        {
            int? AccountId = 0;
            int? Id = 1;

            if (EmployeeCookie != null && EmployeeCookie.AccountId != null && EmployeeCookie.AccountId != 0)
            {
                AccountId = EmployeeCookie.AccountId;
            }

            var OurStripeAccount = _db.Tbl_OurStripeAccount.Where(x => x.Id == Id).Select(x => new PropertyOurStripeAccountDTO
            {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                SecretKey = x.SecretKey,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            });

            return OurStripeAccount.FirstOrDefault();
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