using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FixProUsApi.DAL
{
    public class RepositoryMainDAL
    {
        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryMainDAL()
        {
            _db = new Entities(); 
        }

        public PropertyCom_MainDTO GetCom_Main()
        {
            var main = _db.Com_Main.Select(x => new PropertyCom_MainDTO
            {
                Id = x.Id,
                TwilioAccountSid = x.TwilioAccountSid,
                TwilioauthToken = x.TwilioauthToken,
                TwilioFromPhoneNumber = x.TwilioFromPhoneNumber,
                RealtyRapidApi = x.RealtyRapidApi,
                AddressAutoCompleteKey = x.AddressAutoCompleteKey,

            }).FirstOrDefault();

            return main;
        }


        public PropertyAccountDTO GetExpiredDayForAccount(int AccountId)
        {

            PropertyAccountDTO AccountObj = _db.Tbl_Account.Where(x => x.Id == AccountId).Select(s=> new PropertyAccountDTO
            {
                Id=s.Id,
                CompanyName = s.CompanyName,
                ExpireDate = s.ExpireDate,
            }).FirstOrDefault();

            return AccountObj;
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