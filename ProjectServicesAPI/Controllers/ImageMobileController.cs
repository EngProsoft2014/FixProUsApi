using Microsoft.SqlServer.Server;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/ImageMobile")]
    public class ImageMobileController : ApiController
    {
        Entities _db = new Entities();

        [HttpPost]
        [Route("ReplacePostOneImageProfileImageOnlyMobile")]
        public HttpResponseMessage ReplacePostOneImageProfileImageOnlyMobile(PropertyEmployeeDTO model)
        {
            try
            {
                //Create the Directory. 
                string path = PropertyBaseDTO.PathUrlProfileImage + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string FileNameProfileImage = model.OldPicture;
                if (string.IsNullOrEmpty(model.Picture) != true && (model.Picture != model.OldPicture))
                {
                    string pathPhoto = "";
                    if (string.IsNullOrEmpty(model.OldPicture) == true)
                    {
                        Guid obj = Guid.NewGuid();
                        pathPhoto = path + model.AccountId + "_" + obj + ".jpg";
                        FileNameProfileImage = model.AccountId + "_" + obj + ".jpg";
                    }
                    else
                    {
                        pathPhoto = path + model.AccountId + "_" + model.OldPicture;
                        System.IO.File.Delete(pathPhoto);
                        FileNameProfileImage = model.OldPicture;
                    }

                    byte[] imageData = Convert.FromBase64String(model.Picture);
                    MemoryStream ms = new MemoryStream(imageData);
                    Image postedFile = Image.FromStream(ms);
                    postedFile.Save(pathPhoto);
                }

                _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
                var entityEmpPicture = _db.Tbl_Employee.FirstOrDefault(x => x.Id == model.Id);
                if(entityEmpPicture != null)
                {
                    entityEmpPicture.Picture = FileNameProfileImage;
                }
                _db.Entry(entityEmpPicture).State = EntityState.Modified;
                _db.SaveChanges();

                //var message = Ok(FileName);
                //return message; 
                return Request.CreateResponse(HttpStatusCode.OK, new { Picture = FileNameProfileImage });
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }


        [HttpPost]
        [Route("ReplacePostOneImageSignaturePaymentMobile")]
        public HttpResponseMessage ReplacePostOneImageSignaturePaymentMobile(List<PropertySchedulePicturesDTO> LstPictures)
        {
            try
            {
                foreach (var picture in LstPictures)
                {

                    string FileNameSchduleImage = "";
                    if (string.IsNullOrEmpty(picture.FileName) != true)
                    {
                        string pathPhoto = PropertyBaseDTO.PathUrlImage + "\\";

                        Guid obj = Guid.NewGuid();
                        pathPhoto += picture.AccountId + "_" + obj + ".jpg";
                        FileNameSchduleImage = picture.AccountId + "_" + obj + ".jpg";

                        byte[] imageData = Convert.FromBase64String(picture.FileName);
                        MemoryStream ms = new MemoryStream(imageData);
                        Image postedFile = Image.FromStream(ms);
                        postedFile.Save(pathPhoto);

                    }
                    _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
                    var entitySchedulePicture = new Tbl_SchedulePictures
                    {
                        AccountId = picture.AccountId,
                        BrancheId = picture.BrancheId,
                        ScheduleId = picture.ScheduleId,
                        FileName = FileNameSchduleImage,
                        Active = picture.Active,
                        CreateUser = picture.CreateUser,
                        CreateDate = picture.CreateDate,
                        ScheduleDateId = picture.ScheduleDateId,
                    };

                    _db.Tbl_SchedulePictures.Add(entitySchedulePicture);
                    _db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, LstPictures);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }


        [HttpPost]
        [Route("ReplacePostOneImagesScheduleMobile")]
        public HttpResponseMessage ReplacePostOneImagesScheduleMobile(List<PropertySchedulePicturesDTO> LstPictures)
        {
            try
            {
                foreach (var picture in LstPictures)
                {

                    string FileNameSchduleImage = "";
                    if (string.IsNullOrEmpty(picture.FileName) != true)
                    { 
                        string pathPhoto = PropertyBaseDTO.PathUrlImage + "\\";

                        Guid obj = Guid.NewGuid();
                        pathPhoto += picture.AccountId + "_" + obj + ".jpg";
                        FileNameSchduleImage = picture.AccountId + "_" + obj + ".jpg";

                        byte[] imageData = Convert.FromBase64String(picture.FileName);
                        MemoryStream ms = new MemoryStream(imageData);
                        Image postedFile = Image.FromStream(ms);
                        postedFile.Save(pathPhoto);

                    }
                    _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
                    var entitySchedulePicture = new Tbl_SchedulePictures
                    {
                        AccountId = picture.AccountId,
                        BrancheId = picture.BrancheId,
                        ScheduleId = picture.ScheduleId,
                        FileName = FileNameSchduleImage,
                        Active = picture.Active,
                        ShowToCust = picture.ShowToCust,
                        CreateUser = picture.CreateUser,
                        CreateDate = picture.CreateDate,
                        ScheduleDateId = picture.ScheduleDateId,
                    };

                    _db.Tbl_SchedulePictures.Add(entitySchedulePicture);
                    _db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, LstPictures);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }


        [HttpPost]
        [Route("PostDELOneImage/{IdSchedulePictures}/{ImageName}")]
        public IHttpActionResult PostDELOneImage(string IdSchedulePictures, string ImageName)
        {
            try
            {
                _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
                var entityPic = _db.Tbl_SchedulePictures.FirstOrDefault(x => x.Id == int.Parse(IdSchedulePictures));

                if (entityPic != null)
                {
                    _db.Tbl_SchedulePictures.Remove(entityPic);
                    _db.SaveChanges();
                }

                var pathDelete = PropertyBaseDTO.PathUrlImage + "\\" + ImageName;
                //FileInfo sFile = new FileInfo(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo.jpg");
                //bool fileExist = sFile.Exists;
                // bool fileExist = Directory.EnumerateFiles(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo", " *.jpg").Any();
                //bool xm = System.IO.File.Exists(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo.jpg");

                if (System.IO.File.Exists(pathDelete))
                {
                    System.IO.File.Delete(pathDelete);
                    return Ok("This Image Is Deleted");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [AllowAnonymous]
        [HttpDelete]
        [Route("DeleteOneImage/{PicId:int}")]
        public IHttpActionResult DeleteOneImage(int PicId)
        {
            try
            {
                _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
                var entityPic = _db.Tbl_SchedulePictures.FirstOrDefault(x => x.Id == PicId);

                string pathDelete = "";

                if (entityPic != null)
                {
                    pathDelete = PropertyBaseDTO.PathUrlImage + "\\" + entityPic.FileName;
                    _db.Tbl_SchedulePictures.Remove(entityPic);
                    _db.SaveChanges();
                }

                if (System.IO.File.Exists(pathDelete))
                {
                    System.IO.File.Delete(pathDelete);
                    return Ok("This Image Is Deleted");
                }
                else
                {
                    return NotFound();

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //public IHttpActionResult DeleteOneImage(PropertySchedulePicturesDTO model)
        //{
        //    try
        //    {
        //_db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
        //        var entityPic = _db.Tbl_SchedulePictures.FirstOrDefault(x => x.Id == model.Id);

        //        if (entityPic != null)
        //        {
        //            _db.Tbl_SchedulePictures.Remove(entityPic);
        //            _db.SaveChanges();
        //        }

        //        var pathDelete = PropertyBaseDTO.PathUrlImage + "\\" + model.FileName;
        //        //FileInfo sFile = new FileInfo(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo.jpg");
        //        //bool fileExist = sFile.Exists;
        //        // bool fileExist = Directory.EnumerateFiles(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo", " *.jpg").Any();
        //        //bool xm = System.IO.File.Exists(@"C:\Inetpub\vhosts\engprosoft.com\projectservices.engprosoft.com\ScheduleAttachments\1Logo.jpg");

        //        if (System.IO.File.Exists(pathDelete))
        //        {
        //            System.IO.File.Delete(pathDelete);
        //            return Ok("This Image Is Deleted");
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.ToString());
        //    }
        //}

    }
}
