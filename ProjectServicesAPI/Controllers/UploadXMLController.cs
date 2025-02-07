using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace FixProUsApi.Controllers
{
    //[BasicAuthentication]
    [RoutePrefix("api/UploadXML")]
    public class UploadXMLController : ApiController
    {

        [HttpPost]
        [Route("PostXmlFile")]
        public HttpResponseMessage PostXmlFile(List<PropertyDataMapsDTO> LstData)
        {
            try
            {
                string FileName = PropertyBaseDTO.PathUrlXml + "\\" + LstData.FirstOrDefault().BranchId + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml";

                if (!File.Exists(FileName))
                {
                    foreach (PropertyDataMapsDTO row in LstData)
                    {
                        XmlTextWriter writer = new XmlTextWriter(FileName, System.Text.Encoding.UTF8);
                        writer.WriteStartDocument(true);
                        writer.Formatting = System.Xml.Formatting.Indented;
                        writer.Indentation = 2;
                        writer.WriteStartElement("Data");
                        createNode(row.Id.ToString(), row.BranchId.ToString(), row.EmployeeId.ToString(), row.Lat, row.Long, row.CreateDate, row.Time, writer);
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                    }
                }
                else
                {
                    XDocument xDocument = XDocument.Load(FileName);

                    XElement root = xDocument.Element("Data");
                    foreach (PropertyDataMapsDTO row in LstData)
                    {
                        //IEnumerable<XElement> rows = root.Descendants("Empoylee");
                        //IEnumerable<XElement> rows = root.;

                        //XElement firstRow = rows.First();
                        var newElement = new XElement("Employee");

                        newElement.Add(
                           new XElement("Tracking_id", row.Id.ToString()),
                           new XElement("BranchId", row.BranchId.ToString()),
                           new XElement("EmployeeId", row.EmployeeId.ToString()),
                           new XElement("lat", row.Lat),
                           new XElement("log", row.Long),
                           new XElement("date", row.CreateDate),
                           new XElement("time", row.Time));

                        //var newElementx = new XElement("Empoylee", newElement);

                        xDocument.Root.Element("Employee").AddBeforeSelf(newElement);

                        xDocument.Save(FileName);
                    }
                }

                string CurrentFileName = PropertyBaseDTO.PathUrlXml + "\\" + LstData.FirstOrDefault().EmployeeId + ".xml";
                if (!File.Exists(CurrentFileName))
                {
                    foreach (PropertyDataMapsDTO row in LstData)
                    {
                        XmlTextWriter writer = new XmlTextWriter(CurrentFileName, System.Text.Encoding.UTF8);
                        writer.WriteStartDocument(true);
                        writer.Formatting = System.Xml.Formatting.Indented;
                        writer.Indentation = 2;
                        writer.WriteStartElement("Data");
                        createNode(row.Id.ToString(), row.BranchId.ToString(), row.EmployeeId.ToString(), row.Lat, row.Long, row.CreateDate, row.Time, writer);
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                    }
                }
                else
                {
                    XDocument xDocument = XDocument.Load(CurrentFileName);
                    XElement school = xDocument.Element("Data");

                    //var newElement = new XElement("Employee");

                    school.ReplaceAll(
                        new XElement("Employee",
                       new XElement("Tracking_id", LstData.FirstOrDefault().Id),
                       new XElement("BranchId", LstData.FirstOrDefault().BranchId),
                       new XElement("EmployeeId", LstData.FirstOrDefault().EmployeeId),
                       new XElement("lat", LstData.FirstOrDefault().Lat),
                       new XElement("log", LstData.FirstOrDefault().Long),
                       new XElement("date", LstData.FirstOrDefault().CreateDate),
                       new XElement("time", LstData.FirstOrDefault().Time)));

                    xDocument.Save(CurrentFileName);

                    //var elementLat = xDocument.Elements("lat").Single();
                    //elementLat.Value = LstData.FirstOrDefault().Lat;
                    //var elementLong = xDocument.Elements("log").Single();
                    //elementLong.Value = LstData.FirstOrDefault().Long;
                    //var elementCreateDate = xDocument.Elements("date").Single();
                    //elementLong.Value = LstData.FirstOrDefault().CreateDate;
                    //var elementCreateTime = xDocument.Elements("time").Single();
                    //elementLong.Value = LstData.FirstOrDefault().Time;

                    //xDocument.Save(CurrentFileName);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }

        }


        private void createNode(string pID, string pBranchId, string pEmployeeId, string plat, string plog, string pdate, string ptime, XmlTextWriter writer)
        {
            writer.WriteStartElement("Employee");
            writer.WriteStartElement("Tracking_id");
            writer.WriteString(pID);
            writer.WriteEndElement();
            writer.WriteStartElement("BranchId");
            writer.WriteString(pBranchId);
            writer.WriteEndElement();
            writer.WriteStartElement("EmployeeId");
            writer.WriteString(pEmployeeId);
            writer.WriteEndElement();
            writer.WriteStartElement("lat");
            writer.WriteString(plat);
            writer.WriteEndElement();
            writer.WriteStartElement("log");
            writer.WriteString(plog);
            writer.WriteEndElement();
            writer.WriteStartElement("date");
            writer.WriteString(pdate);
            writer.WriteEndElement();
            writer.WriteStartElement("time");
            writer.WriteString(ptime);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }


        //void cc()
        //{
        //    if (!File.Exists(@"C:\Users\TGaber\Desktop\HCSO_MessageBoard\HCSO_MessageBoard\product.xml"))
        //    {
        //        XmlTextWriter writer = new XmlTextWriter(@"C:\Users\TGaber\Desktop\HCSO_MessageBoard\HCSO_MessageBoard\product.xml", System.Text.Encoding.UTF8);
        //        writer.WriteStartDocument(true);
        //        writer.Formatting = Formatting.Indented;
        //        writer.Indentation = 2;
        //        writer.WriteStartElement("Table");
        //        createNode("1", "Product 1", "1000", writer);
        //        createNode("2", "Product 2", "2000", writer);
        //        createNode("3", "Product 3", "3000", writer);
        //        createNode("4", "Product 4", "4000", writer);
        //        writer.WriteEndElement();
        //        writer.WriteEndDocument();
        //        writer.Close();
        //    }
        //    else
        //    {
        //        XDocument xDocument = XDocument.Load(@"C:\Users\TGaber\Desktop\HCSO_MessageBoard\HCSO_MessageBoard\product.xml");
        //        XElement root = xDocument.Element("Table");
        //        IEnumerable<XElement> rows = root.Descendants("Product");
        //        XElement lastRow = rows.Last();
        //        lastRow.AddBeforeSelf(
        //           new XElement("Product",
        //           new XElement("Product_id", "5"),
        //           new XElement("Product_name", "Product 5"),
        //           new XElement("Product_price", "5000")));
        //        xDocument.Save(@"C:\Users\TGaber\Desktop\HCSO_MessageBoard\HCSO_MessageBoard\product.xml");
        //    }
        //}

    }
}
