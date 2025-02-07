using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace FixProUsApi.DAL
{
    public class RepositoryPaymentsDAL
    {
        public readonly Entities _db;
        private const string PROPERTY_Payment_CACHE_KEY = "Property Payment";
        private bool disposed = false;

        public RepositoryPaymentsDAL()
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

        public void InsertPayment(PropertyPaymentsDTO model)
        {
            var entityInvoice = _db.Tbl_Invoice.FirstOrDefault(x => x.Id == model.InvoiceId);
            if (entityInvoice == null)
            {
                throw new ArgumentException(message: $"The Invoice Id {model.InvoiceId} is deleted.");
            }

            if (entityInvoice.Net < model.Amount)
            {
                throw new ArgumentException(message: $"The Amount {model.Amount} is Over.");
            }

            string FileNameSignatureImage = "";
            if (string.IsNullOrEmpty(model.SignatureDraw) != true)
            {
                string pathPhoto = PropertyBaseDTO.PathUrlImageSignatureInvoice + "\\";

                Guid obj = Guid.NewGuid();
                pathPhoto += model.AccountId + "_" + obj + ".jpg";
                FileNameSignatureImage = model.AccountId + "_" + obj + ".jpg";

                byte[] imageData = Convert.FromBase64String(model.SignatureDraw);
                MemoryStream ms = new MemoryStream(imageData);
                Image postedFile = Image.FromStream(ms);
                postedFile.Save(pathPhoto);

            }

            var entityPayment = new Tbl_Payment
            {
                AccountId = model.AccountId,
                BrancheId = model.BrancheId,
                CustomerId = model.CustomerId,
                ContractId = model.ContractId,
                InvoiceId = model.InvoiceId,
                ExpensesId = model.ExpensesId,
                PaymentDate = model.PaymentDate,
                Amount = model.Amount,
                OverAmount = model.OverAmount,
                Type = model.Type,
                Method = model.Method,
                IncreaseDecrease = model.IncreaseDecrease,
                TransactionID = model.TransactionID,
                CheckNumber = model.CheckNumber,
                BankName = model.BankName,
                AccountNumber = model.AccountNumber,
                SignatureDraw = FileNameSignatureImage,
                SignaturePrintName = model.SignaturePrintName,
                Notes = model.Notes,
                Active = model.Active,
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate,
            };

            _db.Tbl_Payment.Add(entityPayment);
            _db.SaveChanges();

            entityInvoice.Net = entityInvoice.Net - model.Amount;
            entityInvoice.Paid = entityInvoice.Paid + model.Amount;
            entityInvoice.Status = entityInvoice.Net > 0 ? 1 : 2;

            _db.Entry(entityInvoice).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public PropertyStripeAccountDTO GetStripeAccountInBranch(int? AccountId, int? BranchId)
        {
            var Stripe = _db.Tbl_StripeAccount.Where(x => x.AccountId == AccountId && x.BrancheId == BranchId).Select(x => new PropertyStripeAccountDTO
            {
                Id = x.Id,
                Username = x.Username,
                SecretKey = x.SecretKey,
            });
            return Stripe.FirstOrDefault();
        }
    }
}