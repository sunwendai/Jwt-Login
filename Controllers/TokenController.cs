using Hermes.Models.Token;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Hermes.WebManage.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("GetToken")]
        public TokenInfo GetToken([FromBody] LoginRequest loginRequest)
        {
            TokenInfo tokenInfo = new TokenInfo();
            if (loginRequest != null)
            {
                string userName = loginRequest.UserName;
                string passWord = loginRequest.Password;
                bool isAdmin = (userName == "SWD") ? true : false;
                AuthInfo authInfo = new AuthInfo { UserName = userName, Roles = new List<string>(), IsAdmin = isAdmin, ExpiryDateTime = DateTime.Now.AddDays(1)};
                const string secretKey = "ShunKai";//
                try
                {
                    byte[] key = Encoding.UTF8.GetBytes(secretKey);
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                    var token = encoder.Encode(authInfo, key);
                    tokenInfo.Success = true;
                    tokenInfo.Token = token;
                    tokenInfo.Message = "OK";
                }
                catch (Exception ex)
                {
                    tokenInfo.Success = false;
                    tokenInfo.Message = ex.Message.ToString();
                }
            }
            else
            {
                tokenInfo.Success = false;
                tokenInfo.Message = "用户信息为空";
            }
            return tokenInfo;
        }


    }
    
}