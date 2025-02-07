using Newtonsoft.Json;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using RestSharp;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Net;
using System.Collections;
using System.Text;
using System.Net.Http.Formatting;
using System.Security.Policy;
using System.Web.UI.WebControls;
using System.Web.Helpers;
using System.Web.Razor.Tokenizer;
using System.Data.Entity;
using System.Web.Script.Serialization;
using System.Data.Entity.Core.Objects;
using System.Reactive.Concurrency;

namespace FixProUsApi.DAL
{
    public class RepositoryNotificationsDAL
    {
        public readonly Entities _db;
        private bool disposed = false;

        PropertyAccountDTO accountDTO = new PropertyAccountDTO();

        public RepositoryNotificationsDAL()
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

        //Get Account Keys Method
        public async Task<PropertyAccountDTO> GetAccountKeys(int AccountId)
        {
            accountDTO = _db.Tbl_Account.Where(x => x.Id == AccountId).Select(s => new PropertyAccountDTO
            {
                Id = s.Id,
                OneSignalAuthApi = s.OneSignalAuthApi,
                OneSignalRestApikey = s.OneSignalRestApikey,
                OneSignalAppId = s.OneSignalAppId,
            }).FirstOrDefault();

            return accountDTO;
        }

        //Get View Device Method
        public async Task<string> ViewDevice(int AccountId, string PlayerId)
        {
            accountDTO = await GetAccountKeys(AccountId); //Get Account Keys

            if (!string.IsNullOrEmpty(accountDTO.OneSignalAppId) && !string.IsNullOrEmpty(accountDTO.OneSignalRestApikey))
            {
                try
                {
                    var httpClient = new HttpClient();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"https://onesignal.com/api/v1/players/{PlayerId}?app_id={accountDTO.OneSignalAppId}")
                    };

                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", accountDTO.OneSignalRestApikey);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    else
                    {
                        return $"Request failed with status code {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
                return "";
        }

        //Post for Add a Device Method
        public async Task<string> AddDevice(int AccountId, int EmpId, PropertyDeviceDetailsDTO DeviceBody)
        {
            try
            {
                var options = new RestClientOptions("https://onesignal.com/api/v1/players");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(DeviceBody), false);
                var response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var entityId = _db.Tbl_Employee.FirstOrDefault(x => x.AccountId == AccountId && x.Id == EmpId);
                    if (entityId != null)
                    {
                        List<string> Contents = response.Content.Split('"').ToList();
                        entityId.OneSignalPlayerId = Contents[5];
                        _db.Entry(entityId).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                    return entityId.OneSignalPlayerId;
                }
                else
                {
                    return "api not responding";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string UpdateEmpPlayerId(int? AccountId, string PlayerId, int? EmpId)
        {
            var entity = _db.Tbl_Employee.FirstOrDefault(x => x.AccountId == AccountId && x.Id == EmpId);
            if (entity != null)
            {
                entity.OneSignalPlayerId = PlayerId;
            }
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();

            return entity.OneSignalPlayerId;
        }

        //Put for Update Device Method
        public async Task<string> EditDevice(string PlayerId, PropertyDeviceDetailsDTO DeviceBody)
        {
            if (!string.IsNullOrEmpty(PlayerId) && PlayerId != "0")
            {
                try
                {
                    var options = new RestClientOptions("https://onesignal.com/api/v1/players/" + PlayerId);
                    var client = new RestClient(options);
                    var request = new RestRequest("");
                    request.AddHeader("accept", "application/json");
                    request.AddBody(DeviceBody, null);
                    var response = await client.PutAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content;
                    }
                    else
                    {
                        return "api not responding";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
                return "";
        }
         
        //Post for Send Notification Method
        public string CreateNotification(object obj)
        {

            if (obj != null)
            {
                //var options = new RestClientOptions("https://onesignal.com/api/v1/notifications");
                //var client = new RestClient(options);
                //var request = new RestRequest("");
                //request.AddHeader("accept", "application/json");
                ////request.AddHeader("Authorization", $"Basic {accountDTO.OneSignalAuthApi}");
                ////Obj == = new string[] {"6392d91a-b206-4b7b-a620-cd68e32c3a76","76ece62b-bcfe-468c-8a78-839aeaa8c5fa","8e0f21fa-9a5a-4ae7-a9a6-ca1f24294b86"} 
                //request.AddJsonBody(JsonConvert.SerializeObject(obj), false);
                //try
                //{
                //    var response = await client.PostAsync(request);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        // Read the response content
                //        return response.Content;
                //    }
                //    else
                //    {
                //        // Handle the error response
                //        return "API Error: " + response.StatusCode;
                //    }
                //}
                //catch (HttpRequestException ex)
                //{
                //    // Handle any exceptions that occurred during the API call
                //    return "API Exception: " + ex.Message;
                //}
                //==============================================================================================


                string apiUrl = "https://onesignal.com/api/v1/notifications";

                // Create an instance of HttpClient
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        // Make the API call  
                        HttpResponseMessage HttpResponse = httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")).Result;

                        // Check if the request was successful
                        if (HttpResponse.IsSuccessStatusCode)
                        {
                            // Read the response content
                            string responseBody = HttpResponse.Content.ReadAsStringAsync().Result;
                            //ViewBag.kkkk = "API Response: " + responseBody;
                            return "API Response: " + responseBody;
                        }
                        else
                        {
                            // Handle the error response
                            return "API Error: " + HttpResponse.StatusCode;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Handle any exceptions that occurred during the API call
                        return "API Exception: " + ex.Message;
                    }
                }
            }
            else
                return "";


            //==========================================================================================================
            //string apiKey = "YWY4NWY0ZmUtY2I3MC00ZmQzLTkzMDQtYTFmOGMwZTQ3Yjlh";

            //////// URL for the API endpoint
            //string apiUrl = "https://onesignal.com/api/v1/notifications";

            //// Create an instance of HttpClient
            //using (var httpClient = new HttpClient())
            //{
            //    // Set the base address of the API
            //    httpClient.BaseAddress = new Uri(apiUrl);

            //    // Set the Basic Authentication header
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);

            //    try
            //    {
            //        // Make the API call  
            //        HttpResponseMessage HttpResponse = httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")).Result;

            //        // Check if the request was successful
            //        if (HttpResponse.IsSuccessStatusCode)
            //        {
            //            // Read the response content
            //            string responseBody = HttpResponse.Content.ReadAsStringAsync().Result;
            //            //ViewBag.kkkk = "API Response: " + responseBody;
            //            return "API Response: " + responseBody; 
            //        }
            //        else
            //        {
            //            // Handle the error response
            //            return "API Error: " + HttpResponse.StatusCode;
            //        }
            //    }
            //    catch (HttpRequestException ex)
            //    {
            //        // Handle any exceptions that occurred during the API call
            //        return "API Exception: " + ex.Message;
            //    }
            //}
            ///
        }

        public void InsertOneSignalNotification(string Employees,int AccountId, int ScheduleId, int ScheduleDateId, int NotificationType, string NotificationHeader, string NotificationContent, bool Active, int CreateUser, DateTime CreateDate)
        {
            _db.Sp_InsertOneSignalNotification(Employees, AccountId, ScheduleId, ScheduleDateId, NotificationType, NotificationHeader, NotificationContent, Active, CreateUser, CreateDate);
        }

        public void UpdateOneSignalNotification(int? NotifyId, string Employees, int ScheduleId, int ScheduleDateId, string NotificationHeader, string NotificationContent, int TypeUpdate, int UpdateUser, DateTime UpdateDate)
        {
            _db.Sp_UpdateOneSignalNotification(NotifyId, Employees, ScheduleId, ScheduleDateId, NotificationHeader, NotificationContent, TypeUpdate, UpdateUser, UpdateDate);
        }


        public List<PropertyOneSignalNotificationDTO> GetOneSignalNotificationByEmp(int? EmployeeId)
        {
            var OneSignalNotifications = _db.Sp_GetOneSignalNotificationByEmp(EmployeeId).Select(x => new PropertyOneSignalNotificationDTO
            {
                Id = x.Id,
                ScheduleId = x.ScheduleId,
                ScheduleDateId = x.ScheduleDateId,
                ScheduleDate = x.ScheduleDate,
                EmployeeId = x.EmployeeId,
                NotificationType = x.NotificationType,
                NotificationHeader = x.NotificationHeader,
                NotificationContent = x.NotificationContent,
            });
            return OneSignalNotifications.ToList();
        }

        public void GetEmp_Remove_Add_CreateSchedule(int? ScheduleId, string Employees, out string MsgEmpRemove, out string MsgEmpAdd)
        {
            ObjectParameter MsgEmpRemove_ = new ObjectParameter("MsgEmpRemove", typeof(string));
            ObjectParameter MsgEmpAdd_ = new ObjectParameter("MsgEmpAdd", typeof(string));

            _db.Sp_GetEmp_Remove_Add_CreateSchedule(ScheduleId, Employees, MsgEmpRemove_, MsgEmpAdd_);

            MsgEmpRemove = MsgEmpRemove_.Value.ToString();
            MsgEmpAdd = MsgEmpAdd_.Value.ToString();
        }

        public void RemoveNotify(int NotifyId)
        {
            var entity = _db.Tbl_OneSignalNotification.FirstOrDefault(x => x.Id == NotifyId);
            if (entity == null)
            {
                throw new ArgumentException(message: $"This Call Number " + NotifyId + " Is Not Deleted");
                return;
            }
            else
            {
                _db.Tbl_OneSignalNotification.Remove(entity);
                _db.SaveChanges();
            }
        }


        public bool DeactiveNotifyforEmp(PropertyOneSignalNotificationDTO model)
        {
            UpdateOneSignalNotification(model.Id.Value, "", 0, 0, "", "", 4, model.UpdateUser.Value, model.UpdateDate.Value);

            return _db.Tbl_OneSignalNotification.Where(x => x.Id == model.Id).Select(s => s.Active.Value).FirstOrDefault();// return active
        }

        public void SendSpecificNotifications(PropertyNotificationsSpecificDTO model)
        {
            if(model != null)
            {
                var obj = new
                {
                    app_id = model.app_id,
                    contents = new Dictionary<string, string>
                    {
                        ["en"] = model.Content
                    },
                    headings = new Dictionary<string, string>
                    {
                        ["en"] = model.Header
                    },
                    data = new Dictionary<string, string>
                    {
                        ["deeplink"] = "Meeting"
                    },

                    include_player_ids = model.include_player_ids
                };

                CreateNotification(obj);
                InsertOneSignalNotification(model.Employees,model.AccountId.Value, 0, 0, 4, model.Header, model.Content, true, model.CreateUser, DateTime.Now); // Create in notification table in SQL
            }
        }

    }
}