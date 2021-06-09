
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NTS.Api.Models;
using NTS.Api;
using SwipeSafe.Business;
using SwipeSafe.Model.Entity;

namespace SwipeSafe.Api.Repositories
{
    /// <summary>
    /// We are depending on the “UserManager” that provides the domain logic for working with user information.
    /// The “UserManager” knows when to hash a password, how and when to validate a user, and how to manage claims
    /// </summary>
    public class AuthRepository
    {
        private AuthenBusiness _authen;
        //public async Task<LoginEntity> Login(string userName, string password)
        //{
        //    try
        //    {
        //        _authen = new AuthenBusiness();
        //        LoginEntity loginBO = _authen.Login(userName, password);
        //        return loginBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public bool IsTokenAlive(string userId)
        //{
        //    var modelLogin = new AuthenBusiness().GetLoginModelCache(userId);
        //    if (modelLogin != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public ClientEntity FindClient(string clientId)
        {
            _authen = new AuthenBusiness();

            var client = _authen.FindClients(clientId);

            return client;
        }
    }

}