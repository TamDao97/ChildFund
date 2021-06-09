using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SwipeSafe.Api.Repositories;
using SwipeSafe.Model.Entity;
using NTS.Common.Utils;

namespace SwipeSafe.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            ClientEntity client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }
            AuthRepository _repo = new AuthRepository();
            client = _repo.FindClient(context.ClientId);
            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }
            switch (client.ApplicationType)
            {
                case 1:
                    //hàm xử lý từ các nguồn đăng nhập # nhau
                    // context.SetError("invalid_clientId", "Client secret should be sent.");
                    break;
                case 2:
                    //  context.SetError("invalid_clientId", "Client secret should be sent.");
                    break;
                case 3:
                    // context.SetError("invalid_clientId", "Client secret should be sent.");
                    break;
            }
            var clientSecretHash = SecurityHelper.GetHash(clientSecret);
            if (client.SecretKey != clientSecretHash)
            {
                context.SetError("invalid_clientId", "Client secret is invalid.");
                return Task.FromResult<object>(null);

            }
            if (client.Active != 1)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }
            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
            LoginEntity loginBO;
            AuthRepository _repo = new AuthRepository();
            //loginBO = await _repo.Login(context.UserName, context.Password);
            //if (loginBO.ResponseCode != 0)
            //{
            //    context.SetError("invalid_grant", loginBO.ResponseMessage);
            //    return;
            //}

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("user  ", context.UserName));
          //  identity.AddClaim(new Claim("userId", loginBO.UserId == null ? string.Empty : loginBO.UserInfor.UserId));

           // var listStrPermission = loginBO.UserInfor.ListPermission != null ? JsonConvert.SerializeObject(loginBO.UserInfor.ListPermission) : string.Empty;
            //identity.AddClaim(new Claim("AuthorizeString", listStrPermission));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId},
                    { "userName", context.UserName== null? string.Empty : context.UserName},
                    //{ "userfullname", loginBO.UserInfor.FullName == null? string.Empty : loginBO.UserInfor.FullName},
                    //{ "userid", loginBO.UserInfor.UserId== null? string.Empty : loginBO.UserInfor.UserId},
                    //{ "email", loginBO.UserInfor.Email== null? string.Empty : loginBO.UserInfor.Email},
                    //{ "homeUrl", loginBO.UserInfor.HomeUrl== null? string.Empty : loginBO.UserInfor.HomeUrl},
                    //{ "permission", listStrPermission== null? string.Empty : listStrPermission},
                    //{ "imageLink", loginBO.UserInfor.ImageLink == null? string.Empty: loginBO.UserInfor.ImageLink},
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            //lưu cache thong tin login
            //LoginCacheModel cacheModel = new LoginCacheModel();
            //cacheModel.ListPermission = loginBO.UserInfor.ListPermission;
            //new AuthenBussiness().AddLoginModelCache(loginBO.UserInfor.UserId, cacheModel);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}