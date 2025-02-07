using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Management.Smo;
using System.Web.Http;

namespace FixProUsApi.DTO
{
    public class PropertyBaseDTO
    {

        //public static string PathFolderUpload = "C:\\Inetpub\\vhosts\\fixpro.engprosoft.net\\httpdocs\\";
        public static string PathFolderUpload = "C:\\Inetpub\\vhosts\\app.fixprous.com\\httpdocs\\";

        public static string AccountName = "";

        public static string CustomerPhone = "";

        //public static string PathUrlProfileImage = PathFolderUpload + "EmployeePic_"+ AccountName; 

        //public static string PathUrlImage = PathFolderUpload + "ScheduleAttachments_" + AccountName;

        //public static string PathUrlImageMaterialReceipt = PathFolderUpload + "ScheduleMaterialReceipt_" + AccountName; 

        //public static string PathUrlXml = PathFolderUpload + "XMLData_" + AccountName;

        //public static string PathUrlImageSignatureInvoice = PathFolderUpload + "InvoiceSignture_" + AccountName;

        //public static string PathUrlImageSignatureEstimate = PathFolderUpload + "EstimateSignture_" + AccountName;

        //public static string PathUrlPDFStaffPhotos = PathFolderUpload + "PDFStaffPhotos_" + AccountName;

        public static string PathUrlProfileImage = "";

        public static string PathUrlImage = "";

        public static string PathUrlImageMaterialReceipt = "";

        public static string PathUrlXml = "";

        public static string PathUrlImageSignatureInvoice = "";

        public static string PathUrlImageSignatureEstimate = "";

        public static string PathUrlPDFStaffPhotos = "";

        //public static string DomainUrl = "https://fixpro.engprosoft.net/";
        public static string DomainUrl = "https://app.fixprous.com/";


        public static string DBDataSourceFreetrial = "5.9.215.14";
        public static string DBDataSourceCommertional = "144.91.69.135"; 
        public static string DBUserId = "ProjectServicesUser";
        public static string DBPassword = "$ProjectServices";
        public static string DBConnectMasterFreetrial = "Data Source=" + DBDataSourceFreetrial + ";Initial Catalog=master;Integrated Security=false;User ID =" + DBUserId + "; Password =" + DBPassword + ";";
        public static string DBConnectMasterCommertional = "Data Source=" + DBDataSourceCommertional + ";Initial Catalog=master;Integrated Security=false;User ID =" + DBUserId + "; Password =" + DBPassword + ";";



        public int? CreateUser { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? CreateDate { get; set; }

        public static string ConnectSt { get; set; }

        public static void RefreshVariable()
        {
            PathUrlProfileImage = PathFolderUpload + "EmployeePic_" + AccountName;
            PathUrlImage = PathFolderUpload + "ScheduleAttachments_" + AccountName; 
            PathUrlImageMaterialReceipt = PathFolderUpload + "ScheduleMaterialReceipt_" + AccountName; 
            PathUrlXml = PathFolderUpload + "XMLData_" + AccountName; 
            PathUrlImageSignatureInvoice = PathFolderUpload + "InvoiceSignture_" + AccountName; 
            PathUrlImageSignatureEstimate = PathFolderUpload + "EstimateSignture_" + AccountName; 
            PathUrlPDFStaffPhotos = PathFolderUpload + "PDFStaffPhotos_" + AccountName;
        }

        public static void SendMail(string MailTo, string Body, string Subject, List<string> uniqfileattach)
        {
            //try
            //{ 
            var smtpServer = "smtp.office365.com";
            var port = 587;
            var username = "notifications@fixprous.com"; // Your Microsoft 365 email address
            var password = "Eng@2023##"; // Your Microsoft 365 email password

            // Sender and recipient email addresses
            var senderEmail = "notifications@fixprous.com";
            //var recipientEmail = "tarekfared348@hotmail.com";
            //var recipientEmail = "ahmedsakr7001@gmail.com";
            var recipientEmail = MailTo;

            // Create a MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            // Configure the SMTP client
            SmtpClient client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true; // Enable SSL
            client.Credentials = new NetworkCredential(username, password);
            try
            {
                if (uniqfileattach != null && uniqfileattach.Count() > 0)
                {
                    foreach (var file in uniqfileattach)
                    {
                        if (File.Exists(file))
                        {
                            mail.Attachments.Add(new Attachment(file));
                        }
                    }
                }
                // Send the email
                client.Send(mail);
                mail.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                mail.Dispose();
                client.Dispose();
                throw new ArgumentException($"There Email Not Send " + ex);
            }
            //}
            //catch (Exception ex)
            //{
            //}
        }

        //============================= Copy Tables ============================================
        static List<string> LoadList_Tables(CopyViewModel model)
        {
            List<string> FromDB_ListTables = new List<string>();
            var FromDB_Connection = "Data Source=" + model.From_DataSource + ";Initial Catalog=" + model.From_DBName + ";Integrated Security=false;User ID =" + model.From_UserId + "; Password =" + model.From_Password + ";";

            SqlConnection FromDB_sqlconn = new SqlConnection(FromDB_Connection);
            FromDB_sqlconn.Open();
            FromDB_ListTables = new List<string>();
            using (SqlCommand FromDB_sqlcomnd = new SqlCommand("SELECT name FROM sys.tables where left(name,4)='Tbl_'  ORDER BY name ", FromDB_sqlconn))
            {
                using (SqlDataReader reader = FromDB_sqlcomnd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FromDB_ListTables.Add(reader.GetString(0));
                    }
                }
            }
            FromDB_sqlconn.Close();

            return FromDB_ListTables;
        }

        static void CopySchema_RelationsYesNo(bool? WithRelations, string From_DBName, string From_Connection, string Destination_Connection, string From_DataSource, string From_UserId, string From_Password, List<string> FromDB_ListTables)
        {
            StringBuilder sb = new StringBuilder();
            //////////////Server srv = new Server(new Microsoft.SqlServer.Man-----Gement.Common.ServerConnection(From_DataSource));
            Server srv = new Server(new Microsoft.SqlServer.Management.Common.ServerConnection(From_DataSource, From_UserId, From_Password));
            //////////////Microsoft.SqlServer.Man-----Gement.Smo.Database dbs = srv.Databases["TestProjectServicesDB"];
            //////////Microsoft.SqlServer.Management.Smo.Database dbs = srv.Databases["ProjectServicesDB"];
            Microsoft.SqlServer.Management.Smo.Database dbs = srv.Databases[From_DBName];
            ScriptingOptions options = new ScriptingOptions();
            //options.ScriptData = true;////true if want  copy data
            //options.ScriptData = true;////false if did not want  copy data
            options.ScriptDrops = false;
            //options.FileName = FileName;
            options.EnforceScriptingOptions = true;
            options.ScriptSchema = true;////true if want  copy data 
            options.IncludeHeaders = true;
            options.AppendToFile = true;
            options.Indexes = true;
            options.SchemaQualifyForeignKeysReferences = true;
            if (WithRelations == true)
            {
                options.DriAll = true;
            }
            else
            {
                options.DriAll = false;
            }
            options.WithDependencies = false;////try use this only when create schema script and make it false when be in create script data 
            //options.ContinueScriptingOnError = true;
            options.IncludeIfNotExists = true;

            foreach (var tbl in FromDB_ListTables)
            {
                if (tbl.ToLower() != "Tbl_Subscribe".ToLower() && tbl.ToLower() != "Tbl_SubscribeMail".ToLower() && tbl.ToLower() != "Calls".ToLower() && tbl.ToLower() != "Customers$".ToLower() && tbl.ToLower() != "Jobs$".ToLower() && tbl.ToLower() != "Materials".ToLower() && tbl.ToLower() != "Tasks".ToLower())
                {
                    var er = dbs.Tables[tbl, "dbo"].EnumScript(options);
                    string sqlScript = string.Join(" ", er);
                    SqlConnection sqlconn = new SqlConnection(Destination_Connection);
                    SqlCommand sqlcomnd = new SqlCommand(sqlScript, sqlconn);
                    sqlconn.Open();
                    sqlcomnd.ExecuteNonQuery();
                    sqlconn.Close();
                }
                ///فكر فى طريقى نفلتر بيها الداتا بحيث ناخد الداتا بتاعة الاكونت ده بس نعملها انسرت والباقى نسيبه             
            }
        }
        static void CopySp_Func(string From_Connection, string Destination_Connection)
        {
            SqlConnection fromconn = new SqlConnection(From_Connection);
            SqlCommand cmd = new SqlCommand("SELECT definition FROM sys.sql_modules", fromconn);
            fromconn.Open();
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(dr);
            foreach (DataRow item in dt.Rows)
            {
                string script = string.Join(",", item.ItemArray.Select(x => x?.ToString() ?? string.Empty));

                using (SqlConnection connection = new SqlConnection(Destination_Connection))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            dr.Close();
            fromconn.Close();
        }

        static DataTable LoadAccount_TableData(int? AccountId, string Table_Name, string OptionalConditional, string From_Connection)
        {
            DataTable DataCurrenttable = new DataTable();
            SqlConnection fromconn = new SqlConnection(From_Connection);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + Table_Name + " " + OptionalConditional, fromconn);
            fromconn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(dr);

            return dt;
        }

        static void CopyData(int? AccountId, string Table_Name, string OptionalConditional, string From_Connection, string Destination_Connection)
        {
            DataTable dt = LoadAccount_TableData(AccountId, Table_Name, OptionalConditional, From_Connection);

            using (var bulkCopy = new SqlBulkCopy(Destination_Connection, SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                //foreach (DataColumn column in dt.Columns)
                //{
                //    bulkCopy.ColumnMappings.Add(column.Ordinal, column.Ordinal);
                //}

                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = Table_Name;
                bulkCopy.WriteToServer(dt);
            }
        }

        //======================== Fun Copy From DB to DB =====================

        public static PropertyAccountDTO Create_CopyDB(ApiController c, PropertyAccountDTO Account, string CreateDBConnectMaster, string DropDBConnectMaster, string From_DataSource, string From_DBName, string From_UserId, string From_Password, string Destination_DataSource, string Destination_DBName, string Destination_UserId, string Destination_Password)
        {
            //=====================Create DataBae Account==========================   
            Account.DBDataSource = Destination_DataSource /* PropertyBaseDTO.DBDataSourceFreetrial*/;
            Account.DBName = Destination_DBName /*PropertyBaseDTO.MainDB + "_" + Account.CompanyName.Trim()*/;
            Account.DBUserId = Destination_UserId/*PropertyBaseDTO.DBUserId*/;
            Account.DBPassword = Destination_Password/*PropertyBaseDTO.DBPassword*/;

            Account.DBConnection = "data source=" + Destination_DataSource + ";initial catalog=" + Destination_DBName + ";user id=" + Destination_UserId + ";password=" + Destination_Password + ";MultipleActiveResultSets=True;App=EntityFramework";

            if (!string.IsNullOrEmpty(CreateDBConnectMaster) == true)
            {
                string sql = "CREATE DATABASE " + Destination_DBName;
                using (SqlConnection conn = new SqlConnection(CreateDBConnectMaster))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            //========================= Copt Table ================================
            CopyViewModel modelCopy = new CopyViewModel();
            modelCopy.From_DataSource = From_DataSource;
            modelCopy.From_DBName = From_DBName /*PropertyBaseDTO.MainDB*/;
            modelCopy.From_UserId = From_UserId /*model.DBUserId*/;
            modelCopy.From_Password = From_Password /*model.DBPassword*/;

            modelCopy.Destination_DataSource = Destination_DataSource /*model.DBDataSource*/;
            modelCopy.Destination_DBName = Destination_DBName /*model.DBName*/;
            modelCopy.Destination_UserId = Destination_UserId /*model.DBUserId*/;
            modelCopy.Destination_Password = Destination_Password /*model.DBPassword*/;

            List<string> FromDB_ListTables = new List<string>();
            var FromDB_Connection = "Data Source=" + modelCopy.From_DataSource + ";Initial Catalog=" + modelCopy.From_DBName + ";Integrated Security=false;User ID =" + modelCopy.From_UserId + "; Password =" + modelCopy.From_Password + ";";
            var DestinationDB_Connection = "Data Source=" + modelCopy.Destination_DataSource + ";Initial Catalog=" + modelCopy.Destination_DBName + ";Integrated Security=false;User ID =" + modelCopy.Destination_UserId + "; Password =" + modelCopy.Destination_Password + ";";

            FromDB_ListTables = LoadList_Tables(modelCopy);
            CopySchema_RelationsYesNo(false, modelCopy.From_DBName, FromDB_Connection, DestinationDB_Connection, modelCopy.From_DataSource, modelCopy.From_UserId, modelCopy.From_Password, FromDB_ListTables);
            CopySp_Func(FromDB_Connection, DestinationDB_Connection);
            //===================================================================== 
            Account.TypeTrackingSch_Invo = 2;
            Account.AccountSubdomainURL = "";
            Account.AccountSubdomainApiURL = "";
            Account.Active = true; 
            //=====================================================================
            foreach (var item in FromDB_ListTables)
            {
                var OptionalCondition = "";

                try
                {
                    if (item.ToLower() != "Tbl_Subscribe".ToLower() && item.ToLower() != "Tbl_SubscribeMail".ToLower())
                    {
                        if (item.ToLower() != "Tbl_Plans".ToLower() && item.ToLower() != "Tbl_OurStripeAccount".ToLower())
                        {
                            if (item.ToLower() == "Tbl_Account".ToLower())
                            {
                                OptionalCondition = "where Id =" + Account.Id; 
                            }
                            else
                            {
                                OptionalCondition = "where AccountId =" + Account.Id;
                            }
                        }

                        CopyData(Account.Id, item, OptionalCondition, FromDB_Connection, DestinationDB_Connection);
                    }
                }
                catch (Exception)
                {

                }
            }
            CopySchema_RelationsYesNo(true, modelCopy.From_DBName, FromDB_Connection, DestinationDB_Connection, modelCopy.From_DataSource, modelCopy.From_UserId, modelCopy.From_Password, FromDB_ListTables);
            if (!string.IsNullOrEmpty(DropDBConnectMaster) == true)
            {
                string sqldropd = "ALTER DATABASE " + From_DBName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlConnection conn = new SqlConnection(DropDBConnectMaster))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqldropd, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                string sqldrop = "drop DATABASE " + From_DBName;
                using (SqlConnection conn = new SqlConnection(DropDBConnectMaster))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqldrop, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return Account;
        }
      
    }

    public class CopyViewModel
    {
        public int? AccountId { get; set; }
        public string From_DataSource { get; set; }
        public string From_UserId { get; set; }
        public string From_Password { get; set; }
        public string From_DBName { get; set; }
        public string Destination_DataSource { get; set; }
        public string Destination_UserId { get; set; }
        public string Destination_Password { get; set; }
        public string Destination_DBName { get; set; }
        public string Destination_DBConnection { get; set; }
        public bool? CopyData { get; set; }
        public bool? CopySchema { get; set; }
        public List<SelectListItem> ListAccounts { get; set; }
        public List<string> FromDB_ListTables { get; set; }
        public string Hidden_ListAccounts { get; set; }
        public string Table_Name { get; set; }
        public string Account_SubDomain { get; set; }
        public string SourceFiles_SubDomain { get; set; }
    }
}