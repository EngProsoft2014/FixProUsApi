using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FixProUsApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _someFilterParameter;

        public CustomAuthorizeAttribute() { }
        //public CustomAuthorizeAttribute(string someFilterParameter)
        //{
        //    _someFilterParameter = someFilterParameter;
        //}

        public void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string token = actionContext.Request.Headers.Authorization.Parameter;

                var Check = TokenService.GetPrincipal(token);
                if (Check == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}