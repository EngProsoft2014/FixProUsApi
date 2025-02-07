using Antlr.Runtime.Misc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Ajax.Utilities;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Web;
using System.Web.Security;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace FixProUsApi.DAL
{
    public class RepositoryEstimatesDAL  
    {
        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryEstimatesDAL()
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

        public void InsertEstimate(PropertyEstimateDTO model)
        {
            int Check = 0;

            //Check Customer
            var entityEstimate = _db.Tbl_Estimate.FirstOrDefault(x => x.CustomerId == model.CustomerId && x.EstimateDate == model.EstimateDate);
            if (entityEstimate != null)
            {
                Check = 1;
                throw new ArgumentException(message: $"The Estimate number {model.Id} already exists.");
            }

            if (model.LstScdDate != null)
            {
                int[] ScdDTId = model.LstScdDate.Select(x => x.Id).ToArray();
                var DuplicatedEstimateScheduleDate = _db.Tbl_EstimateScheduleDate.Where(c => ScdDTId.Contains((int)c.ScheduleDateId)).FirstOrDefault();
                if (DuplicatedEstimateScheduleDate != null && DuplicatedEstimateScheduleDate.EstimateId != 0)
                {
                    Check = 1;
                    throw new ArgumentException(message: $"There Is Estimate #" + DuplicatedEstimateScheduleDate.EstimateId + " Already Exist For This Schedule Date#" + DuplicatedEstimateScheduleDate.ScheduleDateId);
                }
            }
            if (Check == 0)
            {
                string FileNameSignatureImage = "";
                if (string.IsNullOrEmpty(model.SignatureDraw) != true)
                {
                    string pathPhoto = PropertyBaseDTO.PathUrlImageSignatureEstimate + "\\";

                    Guid obj = Guid.NewGuid();
                    pathPhoto += model.AccountId + "_" + obj + ".jpg";
                    FileNameSignatureImage = model.AccountId + "_" + obj + ".jpg";

                    byte[] imageData = Convert.FromBase64String(model.SignatureDraw);
                    MemoryStream ms = new MemoryStream(imageData);
                    System.Drawing.Image postedFile = System.Drawing.Image.FromStream(ms);
                    postedFile.Save(pathPhoto);
                }

                var Estimate = new Tbl_Estimate
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    ScheduleId = model.ScheduleId,
                    EstimateDate = model.EstimateDate,
                    CustomerId = model.CustomerId,
                    Total = model.Total,
                    TaxId = model.TaxId,
                    Tax = model.Tax,
                    Taxval = model.Taxval,
                    MemberId = model.MemberId,
                    Discount = model.Discount,
                    DiscountAmountOrPercent = model.DiscountAmountOrPercent,
                    Net = model.Net,
                    Status = model.Status,
                    SignaturePrintName = model.SignaturePrintName,
                    SignatureDraw = FileNameSignatureImage,
                    Terms = model.Terms,
                    NotesForCustomer = model.NotesForCustomer,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                    InvoiceId = model.InvoiceId,
                };

                _db.Tbl_Estimate.Add(Estimate);
                _db.SaveChanges();

                int EstimateId = Estimate.Id;

                foreach (var item in model.LstEstimateItemServices)
                {
                    var entityItemsServices = new Tbl_EstimateItemsServices
                    {
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        EstimateId = EstimateId,
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
                        CreateDate = model.CreateDate,
                    };

                    _db.Tbl_EstimateItemsServices.Add(entityItemsServices);
                    _db.SaveChanges();
                }



                if (model.LstScdDate != null)
                {
                    foreach (PropertyScheduleDateDTO item in model.LstScdDate)
                    {
                        if (item.Id != 0)
                        {
                            Tbl_EstimateScheduleDate entityEstimateScheduleDate = new Tbl_EstimateScheduleDate()
                            {
                                AccountId = model.AccountId,
                                EstimateId = EstimateId,
                                ScheduleDateId = item.Id,
                                Active = true,
                                CreateUser = model.CreateUser,
                                CreateDate = model.CreateDate,
                            };

                            _db.Tbl_EstimateScheduleDate.Add(entityEstimateScheduleDate);
                            _db.SaveChanges();

                            var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.Id).FirstOrDefault();
                            modelScheduleDate.EstimateId = EstimateId;
                            _db.Entry(modelScheduleDate).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }
            }



        }

        //public void PostEstimateItemService(PropertyEstimateItemServicesDTO item)
        //{
        //    var entityItemsServices = new Tbl_EstimateItemsServices
        //    {
        //        AccountId = item.AccountId,
        //        BrancheId = item.BrancheId,
        //        EstimateId = item.EstimateId,
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
        //    };

        //    _db.Tbl_EstimateItemsServices.Add(entityItemsServices);
        //    _db.SaveChanges();
        //}

        public void UpdateEstimate(PropertyEstimateDTO model)
        {

            if (model.LstScdDate != null)
            {
                int[] ScdDTId = model.LstScdDate.Select(x => x.Id).ToArray();
                var DuplicatedEstimateScheduleDate = _db.Tbl_EstimateScheduleDate.Where(c => c.EstimateId != model.Id && ScdDTId.Contains((int)c.ScheduleDateId)).FirstOrDefault();
                if (DuplicatedEstimateScheduleDate != null && DuplicatedEstimateScheduleDate.EstimateId != 0)
                {
                    throw new ArgumentException(message: $"There Is Estimate #" + DuplicatedEstimateScheduleDate.EstimateId + " Already Exist For This Schedule Date#" + DuplicatedEstimateScheduleDate.ScheduleDateId);
                }
            }

            var entityId = _db.Tbl_Estimate.FirstOrDefault(x => model.ScheduleId != null ? x.ScheduleId == model.ScheduleId : x.Id == model.Id);
            if (entityId != null)
            {
                //============== SignatureImage  =============
                string FileNameSignatureImage = "";
                if (model.SignatureDraw != entityId.SignatureDraw)
                {
                    string pathPhoto = PropertyBaseDTO.PathUrlImageSignatureEstimate + "\\";

                    Guid obj = Guid.NewGuid();
                    pathPhoto += model.AccountId + "_" + obj + ".jpg";
                    FileNameSignatureImage = model.AccountId + "_" + obj + ".jpg";

                    byte[] imageData = Convert.FromBase64String(model.SignatureDraw);
                    MemoryStream ms = new MemoryStream(imageData);
                    System.Drawing.Image postedFile = System.Drawing.Image.FromStream(ms);
                    postedFile.Save(pathPhoto);
                }

                entityId.AccountId = model.AccountId;
                entityId.BrancheId = model.BrancheId;
                entityId.ScheduleId = model.ScheduleId;
                entityId.EstimateDate = model.EstimateDate;
                entityId.CustomerId = model.CustomerId;
                entityId.Total = model.Total;
                entityId.TaxId = model.TaxId;
                entityId.Tax = model.Tax;
                entityId.Taxval = model.Taxval;
                entityId.MemberId = model.MemberId;
                entityId.Discount = model.Discount;
                entityId.DiscountAmountOrPercent = model.DiscountAmountOrPercent;
                entityId.Net = model.Net;
                entityId.Status = model.Status;
                entityId.SignaturePrintName = model.SignaturePrintName;
                entityId.SignatureDraw = FileNameSignatureImage == "" ? model.SignatureDraw : FileNameSignatureImage;
                entityId.Terms = model.Terms;
                entityId.NotesForCustomer = model.NotesForCustomer;
                entityId.Notes = model.Notes;
                entityId.Active = model.Active;
                entityId.CreateUser = model.CreateUser;
                entityId.CreateDate = DateTime.Now;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                var LstItems = _db.Tbl_EstimateItemsServices.Where(x => x.EstimateId == model.Id).ToList();

                if (LstItems.Count > 0)
                {
                    _db.Tbl_EstimateItemsServices.RemoveRange(LstItems);
                    _db.SaveChanges();
                }

                foreach (var item in model.LstEstimateItemServices)
                {
                    var entityItemsServices = new Tbl_EstimateItemsServices
                    {
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        EstimateId = model.Id,
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
                    };

                    _db.Tbl_EstimateItemsServices.Add(entityItemsServices);
                    _db.SaveChanges();
                }

                var listScheduleDateEstimate = _db.Tbl_EstimateScheduleDate.Where(l => l.EstimateId == model.Id).ToList();
                foreach (var item in listScheduleDateEstimate)
                {
                    var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.ScheduleDateId).FirstOrDefault();
                    modelScheduleDate.EstimateId = null;
                    _db.Entry(modelScheduleDate).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                _db.Tbl_EstimateScheduleDate.RemoveRange(listScheduleDateEstimate);
                _db.SaveChanges();

                if (model.LstScdDate != null)
                {
                    foreach (PropertyScheduleDateDTO item in model.LstScdDate)
                    {
                        if (item.Id != 0)
                        {
                            Tbl_EstimateScheduleDate entityEstimateScheduleDate = new Tbl_EstimateScheduleDate()
                            {
                                AccountId = model.AccountId,
                                EstimateId = model.Id,
                                ScheduleDateId = item.Id,
                                Active = true,
                                CreateUser = model.CreateUser,
                                CreateDate = model.CreateDate,
                            };

                            _db.Tbl_EstimateScheduleDate.Add(entityEstimateScheduleDate);
                            _db.SaveChanges();

                            var modelScheduleDate = _db.Tbl_ScheduleDate.Where(it => it.Id == item.Id).FirstOrDefault();
                            modelScheduleDate.EstimateId = model.Id;
                            _db.Entry(modelScheduleDate).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }

            }
            else
            {
                throw new ArgumentException(message: $"This Estimate Number " + model.Id + " Is Deleted");
            }
        }


        //public void DeleteEstimateItemService(int ItemId)
        //{
        //    var entityItem = _db.Tbl_EstimateItemsServices.FirstOrDefault(x => x.Id == ItemId);

        //    if (entityItem != null)
        //    {
        //        _db.Tbl_EstimateItemsServices.Remove(entityItem);
        //        _db.SaveChanges();
        //    }
        //}

        public void DeleteEstimate(int EstId)
        { 
            var entity = _db.Tbl_Estimate.FirstOrDefault(x => x.Id == EstId);
            if (entity != null)
            {
                var Invoice = _db.Tbl_Invoice.ToList().Where(c => c.Id == entity.InvoiceId);
                if (Invoice != null && Invoice.ToList().Count() != 0)
                { 
                    throw new ArgumentException(message: $"This Estimate Can`t Deleted Because Related With Invoice");
                }
                 
                var entityEstimateItemsServices = _db.Tbl_EstimateItemsServices.Where(x => x.EstimateId == EstId);

                if (entityEstimateItemsServices != null)
                {
                    _db.Tbl_EstimateItemsServices.RemoveRange(entityEstimateItemsServices);
                    _db.SaveChanges();
                }

                var listScheduleDateEstimate = _db.Tbl_EstimateScheduleDate.Where(k => k.EstimateId == EstId);
                foreach (var item in listScheduleDateEstimate.ToList())
                {
                    var entityId = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.Id == item.ScheduleDateId);
                    entityId.EstimateId = null;
                    _db.Entry(entityId).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                if (listScheduleDateEstimate != null)
                {
                    _db.Tbl_EstimateScheduleDate.RemoveRange(listScheduleDateEstimate);
                    _db.SaveChanges();
                } 

                _db.Tbl_Estimate.Remove(entity);
                _db.SaveChanges();
                 
                if (!string.IsNullOrEmpty(entity.SignatureDraw) == true)
                {
                    string pdfFilePath = PropertyBaseDTO.PathUrlImageSignatureEstimate + "\\" + entity.SignatureDraw;
                    if (System.IO.File.Exists(pdfFilePath))
                    {
                        System.IO.File.Delete(pdfFilePath);
                    } 
                }
            }
            else
            {
                throw new ArgumentException(message: $"This Estimate Is Deleted");
            }
        }

        public string SendEstimateMail(string EstimateId, string MailTo)
        {
            int Inv = int.Parse(EstimateId);
            var modelEstimate = _db.Tbl_Estimate.Where(h => h.Id == Inv).FirstOrDefault();
            EncryptUrl EncUrl = new EncryptUrl();
            var test = PropertyBaseDTO.PathFolderUpload + "StaticeImages\\";
            var urlpath = Path.Combine(test + Basics.FilesName.EstimateTemplate + ".txt");
            var template = System.IO.File.ReadAllText(urlpath);
            var Domain = PropertyBaseDTO.DomainUrl;
            string UrlVew = Domain + "/Client/WL/PortalEstimate/" + EncUrl.Encrypt(EstimateId);
            //Domain = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); 
            string Url = Domain + "/BrancheLogo/" + modelEstimate.Tbl_Branches.Logo;
            var replacements = new Dictionary<string, string>
                {
                { "{CompanyLogo}", Url },
                { "{CompanyName}", modelEstimate.Tbl_Account.CompanyName  },
                { "{CompanyPhone}", modelEstimate.Tbl_Branches.Phone },
                { "{CompanyAddress}", modelEstimate.Tbl_Branches.Address },
                { "{Email}", modelEstimate.Tbl_Branches.Email },
                { "{EstimateId}", EstimateId.ToString() },
                { "{EstimateTitle}","Emergency Call" },
                { "{EstimateDetails}", modelEstimate.NotesForCustomer},
                { "{EstimateViewUrl}", UrlVew},
                };

            string pdfFilePath = "";
            if (modelEstimate.ScheduleId != null)
            {
                if (modelEstimate.ScheduleId != 0)
                {
                    //var ImagesScdDateInvoice = _SchedulePicturesBLL.ToListByInvoice(ScheduleId, int.Parse(InvoiceId)).Where(k => k.ShowToCust == true).ToList();
                    var ImagesScdDateInvoice = _db.Tbl_SchedulePictures.Where(g => g.ScheduleId == modelEstimate.ScheduleId && g.ShowToCust == true).ToList();
                    ImagesScdDateInvoice = ImagesScdDateInvoice.Where(k => k.Tbl_ScheduleDate.EstimateId == modelEstimate.Id).ToList();
                    if (ImagesScdDateInvoice.Count > 0)
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        pdfFilePath = PropertyBaseDTO.PathUrlPDFStaffPhotos + "\\" + $"JobPictures_{timestamp}.pdf";
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
        .Add("\n Estimate ID: " + EstimateId);

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
                                            iText.Layout.Element.Image img = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(Img));
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

            PropertyBaseDTO.SendMail(MailTo, template, "New Estimate #" + EstimateId + " form " + modelEstimate.Tbl_Branches.Name, new List<string> { pdfFilePath });
            if (!string.IsNullOrEmpty(pdfFilePath) == true)
            {
                System.IO.File.Delete(pdfFilePath);
            }
            return template;
        }
    }
}