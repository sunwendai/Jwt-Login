using Hermes.Models.Token;
using JWT;
using JWT.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Hermes.WebManage.AuthAttributes
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        //public  static string GetUserpower(string User)
        //{
        //    string url = "";
        //    try
        //    {
        //        string jsonfile = HttpContext.Current.Server.MapPath($"..\\Config\\TokenUserConfig.json");
        //        System.IO.StreamReader file = System.IO.File.OpenText(jsonfile);
        //        JsonTextReader reader = new JsonTextReader(file);
        //        JObject ojb = (JObject)JToken.ReadFrom(reader);
        //        var list = ojb["dtxx"].ToString();
        //        JObject jo = (JObject)JsonConvert.DeserializeObject(list);
        //        var dat = jo[User];
        //        if (dat == null)
        //            url = "";
        //        else
        //            url = dat.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        url = "";
        //        throw new Exception($"初始化JSON流程配置失败:{e.Message}");
        //    }
        //    return url;
        //}


        /// <summary>
        /// 指示指定的控件是否已获得授权
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            
            var authHeader = from t in actionContext.Request.Headers where t.Key == "auth" select t.Value.FirstOrDefault();
            if (authHeader != null)
            {
                const string secretKey = "ShunKai";
                string token = authHeader.FirstOrDefault();
                var arrlst = new Dictionary<string, string>();
                arrlst.Add("admin", "ALL");
                //arrlst.Add("dtxx", "GetProwlerLocus|GetAllPersonInfo");
                arrlst.Add("dtxx", "ALL");
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        byte[] key = Encoding.UTF8.GetBytes(secretKey);
                        IJsonSerializer serializer = new JsonNetSerializer();
                        IDateTimeProvider provider = new UtcDateTimeProvider();
                        IJwtValidator validator = new JwtValidator(serializer, provider);
                        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                        var json = decoder.DecodeToObject<AuthInfo>(token, key, verify: true);
                        if (json != null)
                        {
                            if (json.ExpiryDateTime < DateTime.Now)
                            {
                                return false;
                            }
                            foreach (var item in arrlst)
                            {
                                if (item.Key==json.UserName)
                                {
                                    if (!item.Value.Contains(actionContext.ActionDescriptor.ActionName.ToString())&& item.Value!="ALL")
                                    {
                                       return false;
                                    }
                                }
                            }
                            actionContext.RequestContext.RouteData.Values.Add("auth", json);
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 处理授权失败的请求
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var erModel = new
            {
                Success = "false",
                ErrorCode = "401"
            };
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, erModel, "application/json");
        }

        /// <summary>
        ///  为操作授权时调用
        /// </summary>
        /// <param name="actionContext"></param>
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{

        //}
    }
}