using Antlr.Runtime.Misc;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.BuilderProperties;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Web;
using System.Web.Http.Results;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace FixProUsApi.DAL
{
    public class RepositoryInvoicesDAL
    {
        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryInvoicesDAL()
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

        //public IEnumerable<PropertyPaymentsDTO> GetAllPaymentsForInvoice(int? InvoiceId)
        //{
        //    List<PropertyPaymentsDTO> Payments = _db.Tbl_Payment.Where(x => x.InvoiceId == InvoiceId).Select(x => new PropertyPaymentsDTO
        //    {
        //        Id = x.Id,
        //        AccountId = x.AccountId,
        //        BrancheId = x.BrancheId,
        //        CustomerId = x.CustomerId,
        //        ContractId = x.ContractId,
        //        InvoiceId = x.InvoiceId,
        //        ExpensesId = x.ExpensesId,
        //        PaymentDate = x.PaymentDate,
        //        Amount = x.Amount,
        //        OverAmount = x.OverAmount,
        //        Type = x.Type,
        //        Method = x.Method,
        //        IncreaseDecrease = x.IncreaseDecrease,
        //        TransactionID = x.TransactionID,
        //        CheckNumber = x.CheckNumber,
        //        BankName = x.BankName,
        //        AccountNumber = x.AccountNumber,
        //        Notes = x.Notes,
        //        Active = x.Active,
        //        CreateUser = x.CreateUser,
        //        CreateDate = x.CreateDate,
        //    }).ToList();

        //    return Payments;
        //}

        public int InsertInvoice(PropertyInvoiceDTO model)
        {
            int Check = 0;

            //Check Invoice
            var entityInvoice = _db.Tbl_Invoice.FirstOrDefault(x => (x.CustomerId == model.CustomerId && x.ScheduleId == model.ScheduleId && x.ScheduleId != null && x.InvoiceDate == model.InvoiceDate) || (x.EstimateId == model.EstimateId && x.EstimateId != null));
            if (entityInvoice != null)
            {
                Check = 1;
                throw new ArgumentException(message: $"The_invoice_exists");
            }

            if (model.LstScdDate != null)
            {
                int[] ScdDTId = model.LstScdDate.Select(x => x.Id).ToArray();
                var DuplicatedInvoiceScheduleDate = _db.Tbl_InvoiceScheduleDate.Where(c => ScdDTId.Contains((int)c.ScheduleDateId)).FirstOrDefault();
                if (DuplicatedInvoiceScheduleDate != null && DuplicatedInvoiceScheduleDate.InvoiceId != 0)
                {
                    Check = 1;
                    throw new ArgumentException(message: $"There Is Invoice #" + DuplicatedInvoiceScheduleDate.InvoiceId + " Already Exist For This Schedule Date#" + DuplicatedInvoiceScheduleDate.ScheduleDateId);
                }
            }

            //var ProductBranmodel = _db.Tbl_ItemsServices.ToList().ForEach(f=> model.LstInvoiceItemServices.Where(c=>c.Id == f.Id)).ToList();

            List<Tbl_ItemsServices> lstItems = new List<Tbl_ItemsServices>();
            foreach (var OneItem in model.LstInvoiceItemServices)
            {
                var Pro = _db.Tbl_ItemsServices.Where(x => x.Id == OneItem.ItemsServicesId).FirstOrDefault();
                lstItems.Add(Pro);
            }

            string Msg = "";
            foreach (var item in model.LstInvoiceItemServices)
            {
                var Productmodel = lstItems.Where(k => k.Id == item.ItemsServicesId).FirstOrDefault();

                if (Productmodel != null && Productmodel.Id != 0 && Productmodel.InventoryItem == true)
                {
                    if (Productmodel.QTYTime < item.Quantity)
                    {
                        Check = 2;
                        //Msg += $"The Quantity Of Product(s) " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not_Enough - ";
                        if (string.IsNullOrEmpty(Msg) == false)
                        {
                            Msg += ", " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not_Enough ";
                        }
                        else
                        {
                            Msg += "The Quantity Of Product(s) " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not_Enough ";
                        }
                    }
                }
            }

            if (Check == 0)
            {
                var invoice = new Tbl_Invoice
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    ContractId = model.ContractId,
                    ContractInvoiceId = model.ContractInvoiceId,
                    EstimateId = model.EstimateId,
                    ScheduleId = model.ScheduleId,
                    InvoiceDate = model.InvoiceDate,
                    CustomerId = model.CustomerId,
                    Total = model.Total,
                    TaxId = model.TaxId,
                    Tax = model.Tax,
                    Taxval = model.Taxval,
                    MemberId = model.MemberId,
                    Discount = model.Discount,
                    DiscountAmountOrPercent = model.DiscountAmountOrPercent,
                    Paid = model.Paid,
                    Net = model.Net,
                    Status = model.Status,
                    Type = model.Type,
                    Terms = model.Terms,
                    NotesForCustomer = model.NotesForCustomer,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                };

                _db.Tbl_Invoice.Add(invoice);
                _db.SaveChanges();

                int InvoiceId = invoice.Id;
                foreach (var item in model.LstInvoiceItemServices)
                {
                    var entityItemsServices = new Tbl_InvoiceItemsServices
                    {
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        InvoiceId = InvoiceId,
                        ItemServiceDescription = item.ItemServiceDescription,
                        ItemsServicesId = item.ItemsServicesId,
                        Price = item.Price,
                        //TaxId = item.TaxId,
                        //Tax = item.Tax,
                        Total = item.Total,
                        Quantity = item.Quantity,
                        Taxable = true,
                        Discountable = true,
                        //Unit = item.Unit,
                        Active = item.Active,
                        CreateUser = model.CreateUser,
                        CreateDate = DateTime.Now,
                        SkipOfTotal = item.SkipOfTotal,
                    };

                    _db.Tbl_InvoiceItemsServices.Add(entityItemsServices);
                    _db.SaveChanges();

                    var Items = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();
                    Items.QTYTime -= item.Quantity;
                    _db.Entry(Items).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                if (model.EstimateId != null)
                {
                    var ent = _db.Tbl_Estimate.FirstOrDefault(x => x.Id == model.EstimateId);
                    ent.InvoiceId = InvoiceId;
                    _db.Entry(ent).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                if (model.LstScdDate != null)
                {
                    foreach (PropertyScheduleDateDTO item in model.LstScdDate)
                    {
                        if (item.Id != 0)
                        {
                            Tbl_InvoiceScheduleDate entityInvoiceScheduleDate = new Tbl_InvoiceScheduleDate()
                            {
                                AccountId = model.AccountId,
                                InvoiceId = InvoiceId,
                                ScheduleDateId = item.Id,
                                Active = true,
                                CreateUser = model.CreateUser,
                                CreateDate = model.CreateDate,
                            };

                            _db.Tbl_InvoiceScheduleDate.Add(entityInvoiceScheduleDate);
                            _db.SaveChanges();

                            var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.Id).FirstOrDefault();
                            modelScheduleDate.InvoiceId = InvoiceId; ;
                            _db.Entry(modelScheduleDate).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }

                return InvoiceId;
            }
            else if (Check == 2)
            {
                throw new ArgumentException(message: Msg);
            }

            return 0;
        }

        //public void PostInvoiceItemService(PropertyInvoiceItemServicesDTO item)
        //{

        //    var Productmodel = _db.Tbl_ItemsServices.Where(k => k.Id == item.ItemsServicesId).FirstOrDefault();

        //    if (Productmodel != null && Productmodel.Id != 0 && Productmodel.InventoryItem == true)
        //    {
        //        if (Productmodel.QTYTime < item.Quantity)
        //        {
        //            throw new ArgumentException(message: $"The Quantity Of Product(s) " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not Enough ");
        //            return;
        //        }
        //    }

        //    var entityItemsServices = new Tbl_InvoiceItemsServices
        //    {
        //        AccountId = item.AccountId,
        //        BrancheId = item.BrancheId,
        //        InvoiceId = item.InvoiceId,
        //        ItemServiceDescription = item.ItemServiceDescription,
        //        ItemsServicesId = item.ItemsServicesId,
        //        Price = item.Price,
        //        //TaxId = item.TaxId,
        //        //Tax = item.Tax,
        //        Total = item.Total,
        //        Quantity = item.Quantity,
        //        Taxable = item.Taxable,
        //        Discountable = item.Discountable,
        //        //Unit = item.Unit,
        //        Active = item.Active,
        //        CreateUser = item.CreateUser,
        //        CreateDate = DateTime.Now,
        //        SkipOfTotal = item.SkipOfTotal,
        //    };

        //    _db.Tbl_InvoiceItemsServices.Add(entityItemsServices);
        //    _db.SaveChanges();

        //    var Items = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();
        //    if (Items.InventoryItem == true)
        //    {
        //        Items.QTYTime -= item.Quantity;
        //        _db.Entry(Items).State = EntityState.Modified;
        //        _db.SaveChanges();
        //    }
        //}

        public void UpdateInvoice(PropertyInvoiceDTO model)
        {
            int Check = 0;
            string Msg = "";


            if (model.LstScdDate != null)
            {
                int[] ScdDTId = model.LstScdDate.Select(x => x.Id).ToArray();
                var DuplicatedInvoiceScheduleDate = _db.Tbl_InvoiceScheduleDate.Where(c => c.InvoiceId != model.Id && ScdDTId.Contains((int)c.ScheduleDateId)).FirstOrDefault();
                if (DuplicatedInvoiceScheduleDate != null && DuplicatedInvoiceScheduleDate.InvoiceId != 0)
                {
                    Check = 1;
                    throw new ArgumentException(message: $"There Is Invoice #" + DuplicatedInvoiceScheduleDate.InvoiceId + " Already Exist For This Schedule Date#" + DuplicatedInvoiceScheduleDate.ScheduleDateId);
                }
            }

            List<PropertyInvoiceItemServicesDTO> LstItemsModelAfterRturnItems = new List<PropertyInvoiceItemServicesDTO>();
            var InvoiceLstItems = _db.Tbl_InvoiceItemsServices.Where(x => x.InvoiceId == model.Id).ToList();

            if (InvoiceLstItems != null)
            {
                foreach (var item in InvoiceLstItems)
                {
                    var Product = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();
                    var ProductDTO = new PropertyInvoiceItemServicesDTO
                    {
                        Id = Product.Id,
                        ItemsServicesName = Product.Name,
                        Quantity = Product.QTYTime + item.Quantity,
                        InventoryInvoiceItemService = Product.InventoryItem,
                    };
                    //Product.QTYTime += item.Quantity;

                    LstItemsModelAfterRturnItems.Add(ProductDTO);
                }
            }


            //var LstItems = _db.Tbl_InvoiceItemsServices.Where(x => x.InvoiceId == model.Id).ToList();
            //if (LstItems != null)
            //{
            //    foreach (var item in LstItems)
            //    {
            //        var OItem = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();

            //        if(OItem.QTYTime > item.Quantity)
            //        {

            //        }
            //    }
            //}

            foreach (var item in model.LstInvoiceItemServices)
            {
                var Productmodel = LstItemsModelAfterRturnItems.Where(k => k.Id == item.ItemsServicesId).FirstOrDefault();
                //if (Productmodel == null)
                //{
                //    Productmodel = _db.Tbl_ItemsServices.Where(k => k.Id == item.ItemsServicesId).FirstOrDefault();
                //}

                if (Productmodel != null && Productmodel.Id != 0 && Productmodel.InventoryInvoiceItemService == true)
                {
                    if (Productmodel.Quantity < item.Quantity)
                    {
                        Check = 2;
                        //Msg += $"The Quantity Of Product(s) " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not_Enough - ";
                        if (string.IsNullOrEmpty(Msg) == false)
                        {
                            Msg += ", " + Productmodel.ItemsServicesName + " N.O# " + Productmodel.Id + " Not_Enough ";
                        }
                        else
                        {
                            Msg += "The Quantity Of Product(s) " + Productmodel.ItemsServicesName + " N.O# " + Productmodel.Id + " Not_Enough ";
                        }
                    }
                }
            }

            if (Check == 0)
            {
                var entityId = _db.Tbl_Invoice.FirstOrDefault(x => model.ScheduleId != null ? x.ScheduleId == model.ScheduleId : x.Id == model.Id);
                if (entityId != null)
                {
                    entityId.AccountId = model.AccountId;
                    entityId.BrancheId = model.BrancheId;
                    entityId.ContractId = model.ContractId;
                    entityId.ContractInvoiceId = model.ContractInvoiceId;
                    entityId.EstimateId = model.EstimateId;
                    entityId.ScheduleId = model.ScheduleId;
                    entityId.InvoiceDate = model.InvoiceDate;
                    entityId.CustomerId = model.CustomerId;
                    entityId.Total = model.Total;
                    entityId.TaxId = model.TaxId;
                    entityId.Tax = model.Tax;
                    entityId.Taxval = model.Taxval;
                    entityId.MemberId = model.MemberId;
                    entityId.Discount = model.Discount;
                    entityId.DiscountAmountOrPercent = model.DiscountAmountOrPercent;
                    entityId.Paid = model.Paid;
                    entityId.Net = model.Net;
                    entityId.Status = model.Status;
                    entityId.Type = model.Type;
                    entityId.Terms = model.Terms;
                    entityId.NotesForCustomer = model.NotesForCustomer;
                    entityId.Notes = model.Notes;
                    entityId.Active = model.Active;
                    entityId.CreateUser = model.CreateUser;
                    entityId.CreateDate = DateTime.Now;

                    _db.Entry(entityId).State = EntityState.Modified;
                    _db.SaveChanges();

                    var LstItem = _db.Tbl_InvoiceItemsServices.Where(x => x.InvoiceId == model.Id).ToList();

                    if (LstItem != null)
                    {
                        foreach (var item in LstItem)
                        {
                            var OItem = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();

                            if (OItem != null)
                            {
                                OItem.QTYTime += item.Quantity;
                                _db.Entry(OItem).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                        }

                        _db.Tbl_InvoiceItemsServices.RemoveRange(LstItem);
                        _db.SaveChanges();
                    }

                    foreach (var item in model.LstInvoiceItemServices)
                    {
                        var entityItemsServices = new Tbl_InvoiceItemsServices
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            InvoiceId = model.Id,
                            ItemServiceDescription = item.ItemServiceDescription,
                            ItemsServicesId = item.ItemsServicesId,
                            Price = item.Price,
                            //TaxId = item.TaxId,
                            //Tax = item.Tax,
                            Total = item.Total,
                            Quantity = item.Quantity,
                            Taxable = true,
                            Discountable = true,
                            //Unit = item.Unit,
                            Active = item.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = DateTime.Now,
                            SkipOfTotal = item.SkipOfTotal,
                        };

                        _db.Tbl_InvoiceItemsServices.Add(entityItemsServices);
                        _db.SaveChanges();

                        var Items = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();
                        Items.QTYTime -= item.Quantity;
                        _db.Entry(Items).State = EntityState.Modified;
                        _db.SaveChanges();
                    }


                    var listScheduleDateInvoice = _db.Tbl_InvoiceScheduleDate.Where(l => l.InvoiceId == model.Id).ToList();
                    foreach (var item in listScheduleDateInvoice)
                    {
                        var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.ScheduleDateId).FirstOrDefault();
                        modelScheduleDate.InvoiceId = null;
                        _db.Entry(modelScheduleDate).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                    _db.Tbl_InvoiceScheduleDate.RemoveRange(listScheduleDateInvoice);
                    _db.SaveChanges();

                    if (model.LstScdDate != null)
                    {
                        foreach (PropertyScheduleDateDTO item in model.LstScdDate)
                        {
                            if (item.Id != 0)
                            {
                                Tbl_InvoiceScheduleDate entityInvoiceScheduleDate = new Tbl_InvoiceScheduleDate()
                                {
                                    AccountId = model.AccountId,
                                    InvoiceId = model.Id,
                                    ScheduleDateId = item.Id,
                                    Active = true,
                                    CreateUser = model.CreateUser,
                                    CreateDate = model.CreateDate,
                                };

                                _db.Tbl_InvoiceScheduleDate.Add(entityInvoiceScheduleDate);
                                _db.SaveChanges();

                                var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.Id).FirstOrDefault();
                                modelScheduleDate.InvoiceId = model.Id;
                                _db.Entry(modelScheduleDate).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                        }
                    }

                }

                else
                {
                    throw new ArgumentException(message: $"This Invoice Number " + model.Id + " Is Deleted");
                }
            }
            else
            {
                throw new ArgumentException(message: Msg);
            }
        }

        //public void DeleteInvoiceItemService(int ItemId)
        //{
        //    var entityItem = _db.Tbl_InvoiceItemsServices.FirstOrDefault(x => x.Id == ItemId);

        //    var Items = _db.Tbl_ItemsServices.Where(x => x.Id == entityItem.ItemsServicesId).FirstOrDefault();

        //    if (entityItem != null)
        //    {
        //        _db.Tbl_InvoiceItemsServices.Remove(entityItem);
        //        _db.SaveChanges();

        //        if(Items != null)
        //        {
        //            Items.QTYTime += entityItem.Quantity;
        //            _db.Entry(Items).State = EntityState.Modified;
        //            _db.SaveChanges();
        //        }
        //    }
        //}

        public void RemoveInvoice(int InvoiceId)
        {
            var entity = _db.Tbl_Invoice.FirstOrDefault(x => x.Id == InvoiceId);
            if (entity != null)
            {
                if (entity.ContractId != null || entity.Status != 0)
                {
                    throw new ArgumentException(message: $"This Invoice Number " + InvoiceId + " Is Not Deleted");
                    return;
                }
            }

            if (entity.EstimateId != null)
            {
                var ent = _db.Tbl_Estimate.FirstOrDefault(x => x.Id == entity.EstimateId);
                ent.InvoiceId = null;
                _db.Entry(ent).State = EntityState.Modified;
                _db.SaveChanges();
            }

            var entityItems = _db.Tbl_InvoiceItemsServices.Where(x => x.InvoiceId == InvoiceId).ToList();

            if (entityItems != null)
            {
                foreach (var item in entityItems)
                {
                    var Items = _db.Tbl_ItemsServices.Where(x => x.Id == item.ItemsServicesId).FirstOrDefault();

                    _db.Tbl_InvoiceItemsServices.Remove(item);
                    _db.SaveChanges();

                    if (Items != null)
                    {
                        Items.QTYTime += item.Quantity;
                        _db.Entry(Items).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
            }

            var listScheduleDateInvoice = _db.Tbl_InvoiceScheduleDate.Where(l => l.InvoiceId == InvoiceId).ToList();
            foreach (var item in listScheduleDateInvoice)
            {
                var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.ScheduleDateId).FirstOrDefault();
                modelScheduleDate.InvoiceId = null;
                _db.Entry(modelScheduleDate).State = EntityState.Modified;
                _db.SaveChanges();
            }

            _db.Tbl_InvoiceScheduleDate.RemoveRange(listScheduleDateInvoice);
            _db.SaveChanges();

            if (entity != null)
            {
                _db.Tbl_Invoice.Remove(entity);
                _db.SaveChanges();
            }


        }



       public string SendInvoiceMail(string InvoiceId,string MailTo)
        {
            int Inv = int.Parse(InvoiceId);
            var modelinvoice = _db.Tbl_Invoice.Where(h => h.Id == Inv).FirstOrDefault();
            EncryptUrl EncUrl = new EncryptUrl(); 
            var test = PropertyBaseDTO.PathFolderUpload + "StaticeImages\\";
            var urlpath = Path.Combine(test + Basics.FilesName.InvoiceTemplate + ".txt");
            var template = System.IO.File.ReadAllText(urlpath);
            var Domain = PropertyBaseDTO.DomainUrl;
            string UrlVew = Domain + "/Client/WL/Portal/" + EncUrl.Encrypt(InvoiceId);
            //Domain = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); 
            string Url = Domain + "/BrancheLogo/" + modelinvoice.Tbl_Branches.Logo;
            var replacements = new Dictionary<string, string>
                {
                { "{CompanyLogo}", Url },
                { "{CompanyName}", modelinvoice.Tbl_Account.CompanyName  },
                { "{CompanyPhone}", modelinvoice.Tbl_Branches.Phone },
                { "{CompanyAddress}", modelinvoice.Tbl_Branches.Address },
                { "{Email}", modelinvoice.Tbl_Branches.Email },
                { "{InvoiceId}", InvoiceId.ToString() },
                { "{InvoiceTitle}","Emergency Call" },
                { "{InvoiceDetails}", modelinvoice.NotesForCustomer},
                { "{InvoiceViewUrl}", UrlVew},
            };
            string pdfFilePath = "";
            if (modelinvoice.ScheduleId != null)
            {
                if (modelinvoice.ScheduleId != 0)
                {
                    //var ImagesScdDateInvoice = _SchedulePicturesBLL.ToListByInvoice(ScheduleId, int.Parse(InvoiceId)).Where(k => k.ShowToCust == true).ToList();
                    var ImagesScdDateInvoice = _db.Tbl_SchedulePictures.Where(g => g.ScheduleId == modelinvoice.ScheduleId && g.ShowToCust==true).ToList();
                    ImagesScdDateInvoice = ImagesScdDateInvoice.Where(k => k.Tbl_ScheduleDate.InvoiceId == modelinvoice.Id).ToList();
                    if (ImagesScdDateInvoice.Count > 0)
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                         pdfFilePath = PropertyBaseDTO.PathUrlPDFStaffPhotos + "\\"+$"JobPictures_{timestamp}.pdf";
                        using (PdfWriter writer = new PdfWriter(pdfFilePath))
                        {
                            using (PdfDocument pdf = new PdfDocument(writer))
                            {
                                Document document = new Document(pdf);
                                // Add a title to the PDF
                                //Paragraph title = new Paragraph("Job Pictures").SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(20f).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                                Paragraph title = new Paragraph()
        .Add("Job Pictures")
        .SetFontColor(iText.Kernel.Colors.ColorConstants.RED)
        .SetFontSize(20f)
        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
        .Add("\n Invoice ID: " + InvoiceId);

                                document.Add(title);

                                // Add images to the PDF
                                // Assuming imagePaths is a list of paths to your images 
                                foreach (var imagePath in ImagesScdDateInvoice)
                                {
                                    string Img = Domain + "/ScheduleAttachments/" + imagePath.FileName;
                                    using (HttpClient client = new HttpClient())
                                    {
                                        HttpResponseMessage response = client.GetAsync(Img).Result;

                                        if (response.IsSuccessStatusCode)
                                        {
                                            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(Img));
                                            document.Add(img);
                                        }
                                    }
                                }
                                document.Close();
                                pdf.Close();
                                writer.Close();
                            }
                        }
                    }
                }
            }

            foreach (var item in replacements)
                template = template.Replace(item.Key, item.Value);

            PropertyBaseDTO.SendMail(MailTo, template, "New Invoice #" + InvoiceId + " form " + modelinvoice.Tbl_Branches.Name, new List<string> { pdfFilePath });
            if (!string.IsNullOrEmpty(pdfFilePath) == true)
            {
                System.IO.File.Delete(pdfFilePath);
            }
            return template;
        }

    }
}