//using Microsoft.AspNet.SignalR;
using Org.BouncyCastle.Asn1.X509;
using FixProUsApi.DAL;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.SignalR;

namespace FixProUsApi.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        IHubContext<ChatHub> _hubContext;
        public LoginController(IHubContext<ChatHub> hubContext) 
        {
            _hubContext = hubContext;

        }
        private StripeCustomer GetCustomer(string FirstName, string LastName, string Cvc, int? ExpirationMonth, int? ExpirationYear, string CardNumber, string SecretKey)
        {
            SourceCard SCard = new SourceCard
            {
                AddressCountry = "USA",
                AddressLine1 = "",
                AddressCity = "",
                AddressZip = "",
                Name = FirstName + " " + LastName,
                Cvc = Cvc,
                ExpirationMonth = (int)ExpirationMonth,
                ExpirationYear = (int)ExpirationYear,
                Number = CardNumber
            };

            var mycust = new StripeCustomerCreateOptions
            {
                Email = "",
                Description = "Donation.DonationDto.DonateToProject",
                SourceCard = SCard
            };

            var customerservice = new StripeCustomerService(SecretKey);
            return customerservice.Create(mycust);
        }

        [Route("GetLogin")]
        public HttpResponseMessage GetLogin(string UserName, string Password, string PlayerId)
        {
            PropertyBaseDTO.ConnectSt = null;
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                var entity = ClsEmployeeDAL.CheckPassword(UserName, Password);

                if (entity != null)
                {
                    var _ExpireDate = DateTime.Parse(entity.AccountExpireDate.ToString());
                    var _TodayDate = DateTime.Parse(DateTime.Now.ToString());
                    TimeSpan diff = _TodayDate - _ExpireDate;
                    var days = diff.Days;
                    var Hours = diff.Hours;

                    if (days > 0 || days == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, entity.EmailUserName + "-" + "Account Is Expired");
                    }
                    else
                    {
                        string ConnAcc = entity.DBConnection;
                        SqlConnectionStringBuilder sqlCnxStringBuilder;
                        if (string.IsNullOrEmpty(ConnAcc))
                        {
                            var entityCnxStringBuilder = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString);
                            sqlCnxStringBuilder = new SqlConnectionStringBuilder(entityCnxStringBuilder.ProviderConnectionString);
                        }
                        else
                        {

                            PropertyBaseDTO.ConnectSt = ConnAcc;
                            ClsEmployeeDAL = new RepositoryEmployeeDAL();
                            entity = ClsEmployeeDAL.CheckPassword(UserName, Password);
                            sqlCnxStringBuilder = new SqlConnectionStringBuilder(ConnAcc);
                        }
                        string GernTokenx = TokenService.GenerateToken(sqlCnxStringBuilder.ConnectionString, entity.PathFileUpload, entity.Id.ToString(), entity.UserName, entity.AccountSubdomainURL, entity.AccountName, entity.ActiveCustomerPhone.ToString());
                        entity.GernToken = GernTokenx;

                        //Send PlayerId by SignalR
                        if (PlayerId != entity.OneSignalPlayerId)
                        {
                            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
                            string PlayerIdAfterInsert = ClsNotificationsDAL.UpdateEmpPlayerId(entity.AccountId, PlayerId, entity.Id);

                            entity.OneSignalPlayerId = PlayerIdAfterInsert;

                            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                            //hubContext.Clients.All.ReceiveMessage(PlayerIdAfterInsert, UserName, "", "");

                            //ChatHub oChatHub = new ChatHub();
                            _hubContext.Clients.All.SendAsync("ReceiveMessage", PlayerIdAfterInsert, UserName, "", "").Wait();
                            
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Try Again");
                }
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }

        }

        [Route("GetLoginOwner")]
        public HttpResponseMessage GetLoginOwner(string UserName, string Password)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                var entity = ClsEmployeeDAL.CheckPasswordOwner(UserName, Password);

                if (entity != null)
                {
                    //string ConnAcc = entity.DBConnection;
                    //SqlConnectionStringBuilder sqlCnxStringBuilder;
                    //if (string.IsNullOrEmpty(ConnAcc))
                    //{
                    //    var entityCnxStringBuilder = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString);
                    //    sqlCnxStringBuilder = new SqlConnectionStringBuilder(entityCnxStringBuilder.ProviderConnectionString);
                    //}
                    //else
                    //{
                    //    sqlCnxStringBuilder = new SqlConnectionStringBuilder(ConnAcc);
                    //}
                    //string GernTokenx = TokenService.GenerateToken(sqlCnxStringBuilder.ConnectionString, entity.PathFileUpload, entity.Id.ToString(), entity.UserName, entity.AccountSubdomainURL);
                    //entity.GernToken = GernTokenx;
                    //return Request.CreateResponse(HttpStatusCode.OK, entity);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Try Again");
                }
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }

        }

        [Route("GetStripeAccount")]
        public PropertyStripeAccountDTO GetStripeAccount(int? AccountId,int? BranchId)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetStripeAccountInBranch(AccountId,BranchId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetChangeUserData")]
        public HttpResponseMessage GetChangeUserData(int AccountId, int EmpId, string UserName, string Password)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //hubContext.Clients.All.ChangeUserData(AccountId.ToString(), EmpId.ToString(), UserName, Password);

            _hubContext.Clients.All.SendAsync("ChangeUserData", AccountId.ToString(), EmpId.ToString(), UserName, Password).Wait();

            return Request.CreateResponse(HttpStatusCode.OK, UserName);
        }


        [Route("GetAccountToPayUpgrade")]
        public HttpResponseMessage GetAccountToPayUpgrade(string Email, string Plan, string AnnualMonthly, string Amount, string TransactionId, string OrderIdMySql, string UserIdMysql, string FirstName = "", string LastName = "", string Cvc = "", int? ExpirationMonth = 0, int? ExpirationYear = 0, string CardNumber = "")
        {
            PropertyBaseDTO.ConnectSt = null;
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();

            RepositoryOurStripeAccountDAL _OurStripeAccounDAL = new RepositoryOurStripeAccountDAL();
            var StripeInfo = new PropertyOurStripeAccountDTO();
            StripeInfo = _OurStripeAccounDAL.FindOurStripeAccountById();

            //if (StripeInfo != null && StripeInfo.Id != 0 && string.IsNullOrEmpty(StripeInfo.SecretKey) == false)
            //{
            //    StripeCustomer current = GetCustomer(FirstName, LastName, Cvc, ExpirationMonth, ExpirationYear, CardNumber, StripeInfo.SecretKey);
            //    double chargetotal = double.Parse(Amount.ToString()) * 100;
            //    var mycharge = new StripeChargeCreateOptions();
            //    mycharge.Amount = int.Parse(chargetotal.ToString());
            //    mycharge.Currency = "USD";
            //    mycharge.CustomerId = current.Id;
            //    var chargeservice = new StripeChargeService(StripeInfo.SecretKey);
            //    StripeCharge currentcharge = chargeservice.Create(mycharge);
            //    if (currentcharge.FailureCode == null)
            //    {
            //        TransactionId = currentcharge.Id;
            //    }
            //}


            try
            {
                var entity = ClsEmployeeDAL.GetAccountOfEmployee(Email);

                if (entity != null)
                {
                    using (Entities _dbAccount = new Entities())
                    {
                        var Account = _dbAccount.Tbl_Account.Where(m => m.Id == entity.AccountId).FirstOrDefault();
                        if (Account != null)
                        {
                            int PlanId = int.Parse(Plan);
                            PropertyBaseDTO.ConnectSt = Account.DBConnection;
                            int TypePayDb = (int)Account.Type;
                            //========================== Add Update Main =======================
                            Tbl_AccountPayment model = new Tbl_AccountPayment();
                            model.AccountId = Account.Id;
                            model.PlanId = Account.Id;
                            model.AnnualMonthly = AnnualMonthly;
                            model.Amount = decimal.Parse(Amount);
                            model.TransactionId = TransactionId;
                            model.PlanId = PlanId;
                            model.OrderIdMySql = OrderIdMySql;
                            model.CreateDate = DateTime.Now;
                            model.CreateUser = entity.Id;

                            _dbAccount.Tbl_AccountPayment.Add(model);
                            _dbAccount.SaveChanges();
                            //==========================
                            //Account.ExpireDate = DateTime.Now;

                            if (Account.ExpireDate == null)
                            {
                                Account.ExpireDate = DateTime.Now;
                            }
                            else
                            {
                                var _ExpireDate = DateTime.Parse(Account.ExpireDate.ToString());
                                var _TodayDate = DateTime.Parse(DateTime.Now.ToString());
                                TimeSpan diff = _ExpireDate - _TodayDate;
                                var days = diff.Days;
                                if (days < 0 || days == 0)
                                {
                                    Account.ExpireDate = DateTime.Now;
                                }
                            }

                            if (AnnualMonthly == "Annual")
                            {
                                Account.DayExpire = 365;
                                Account.ExpireDate = DateTime.Parse(Account.ExpireDate.ToString()).AddYears(1);
                            }
                            else if (AnnualMonthly == "Monthly")
                            {
                                Account.DayExpire = 30;
                                Account.ExpireDate = DateTime.Parse(Account.ExpireDate.ToString()).AddMonths(1);
                            }
                            Account.PlanId = PlanId;
                            Account.Type = 1;
                            if (TypePayDb == 0)
                            {
                                Account.DBDataSource = PropertyBaseDTO.DBDataSourceCommertional;
                                Account.DBConnection = "Data Source=" + Account.DBDataSource + ";Initial Catalog=" + Account.DBName + ";Integrated Security=false;User ID =" + Account.DBUserId + "; Password =" + Account.DBPassword + ";MultipleActiveResultSets=True;App=EntityFramework";
                            }
                            _dbAccount.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                            _dbAccount.SaveChanges();
                            //==========================
                            var memp = _dbAccount.Tbl_Employee.Where(k => k.Id == entity.Id).FirstOrDefault();
                            memp.UserIdMysql = UserIdMysql;
                            _dbAccount.Entry(memp).State = System.Data.Entity.EntityState.Modified;
                            _dbAccount.SaveChanges();
                            //========================== Add Update Owner =======================

                            _dbAccount.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;

                            //==========================
                            ClsEmployeeDAL = new RepositoryEmployeeDAL();
                            entity = ClsEmployeeDAL.GetAccountOfEmployee(Email);
                            model.CreateUser = entity.Id;

                            _dbAccount.Tbl_AccountPayment.Add(model);
                            _dbAccount.SaveChanges();
                            //========================== 
                            _dbAccount.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                            _dbAccount.SaveChanges();
                            //==========================

                            var memp2 = _dbAccount.Tbl_Employee.Where(k => k.Id == entity.Id).FirstOrDefault();
                            memp2.UserIdMysql = UserIdMysql;
                            _dbAccount.Entry(memp2).State = System.Data.Entity.EntityState.Modified;
                            _dbAccount.SaveChanges();

                            if (TypePayDb == 0)
                            {
                                PropertyAccountDTO Acc = new PropertyAccountDTO()
                                {
                                    Id = Account.Id,
                                    UserName = Account.UserName,
                                    EamilAddress = Account.EamilAddress,
                                    CompanyName = Account.CompanyName,
                                    Active = Account.Active,
                                    Type = Account.Type,
                                    Password = Account.Password,
                                    PlanId = Account.PlanId,
                                    DayExpire = Account.DayExpire,
                                    ExpireDate = Account.ExpireDate,
                                    HostName = Account.HostName,
                                    AccountSubdomainURL = Account.AccountSubdomainURL,
                                    AccountSubdomainApiURL = Account.AccountSubdomainApiURL,
                                    DBConnection = Account.DBConnection,
                                    CreateDate = Account.CreateDate,
                                    OneSignalAppId = Account.OneSignalAppId,
                                    OneSignalAuthApi = Account.OneSignalAuthApi,
                                    OneSignalRestApikey = Account.OneSignalRestApikey,
                                    TypeTrackingSch_Invo = Account.TypeTrackingSch_Invo,
                                    TimeOutLogout = Account.TimeOutLogout,
                                    DBDataSource = Account.DBDataSource,
                                    DBName = Account.DBName,
                                    DBPassword = Account.DBPassword,
                                    DBUserId = Account.DBUserId,
                                    PathFileUpload = Account.PathFileUpload,
                                };

                                PropertyAccountDTO Accountmodel = PropertyBaseDTO.Create_CopyDB(this, Acc, PropertyBaseDTO.DBConnectMasterCommertional, PropertyBaseDTO.DBConnectMasterFreetrial, PropertyBaseDTO.DBDataSourceFreetrial, Acc.DBName, Acc.DBUserId, Acc.DBPassword, PropertyBaseDTO.DBDataSourceCommertional, Acc.DBName, Acc.DBUserId, Acc.DBPassword);
                            }

                            //string connectionString = "server=5.9.215.6;database=turbositehost_Db;uid=turbositehost_tgaber;password=Eng@2014;";

                            //using (MSqlConnection connection = new MySqlConnection(connectionString))
                            //{
                            //    try
                            //    {
                            //        connection.Open();

                            //        Console.WriteLine("Connected to MySQL database.");

                            //        // Get WordPress database prefix
                            //        string prefix = "wp_"; // Change this if your WordPress database prefix is different

                            //        // Construct the SQL query
                            //        string query = $"SELECT * FROM {prefix}wc_orders left outer join {prefix}woocommerce_order_items on {prefix}woocommerce_order_items.order_id = {prefix}wc_orders.ID WHERE ID = 14853 and order_item_type = 'line_item';";

                            //        using (MySqlCommand command = new MySqlCommand(query, connection))
                            //        {
                            //            using (MySqlDataReader reader = command.ExecuteReader())
                            //            {
                            //                while (reader.Read())
                            //                {
                            //                    // Example: Retrieve order details
                            //                    int id = reader.GetInt32("ID");
                            //                    int customerId = reader.GetInt32("customer_id");
                            //                    string email = reader.GetString("billing_email");
                            //                    string transaction_id = reader.GetString("transaction_id");
                            //                    string item_name = reader.GetString("order_item_name");
                            //                }
                            //            }
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine("Error: " + eAccount.Message);
                            //    }
                            //}  
                        }

                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Try Again");
                }
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }

        }

        [Route("GetCom_Main")]
        public HttpResponseMessage GetCom_Main()
        {
            RepositoryMainDAL ClsMainDAL = new RepositoryMainDAL();
            try
            {
                var entity = ClsMainDAL.GetCom_Main();

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Try Again");
                }
            }
            finally
            {
                ClsMainDAL.Dispose();
            }
        }


        [Route("GetExpiredDate")]
        public HttpResponseMessage GetExpiredDate(int AccountId)
        {
            RepositoryMainDAL ClsMainDAL = new RepositoryMainDAL();
            try
            {
                var entity = ClsMainDAL.GetExpiredDayForAccount(AccountId);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Expired date not found");
                }
            }
            finally
            {
                ClsMainDAL.Dispose();
            }
        }
    }
}
