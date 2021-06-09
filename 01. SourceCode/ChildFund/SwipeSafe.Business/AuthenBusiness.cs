using SwipeSafe.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Business
{
    public class AuthenBusiness
    {
        public ClientEntity FindClients(string clientId)
        {
            try
            {
                //var client = (from a in db.ApplicationClients.AsNoTracking()
                //              where a.Id.Equals(clientId)
                //              select new ClientEntity
                //              {
                //                  Id = a.Id,
                //                  SecretKey = a.SecretKey,
                //                  Name = a.Name,
                //                  ApplicationType = a.ApplicationType,
                //                  Active = a.Active,
                //                  RefreshTokenLifeTime = a.RefreshTokenLifeTime,
                //                  AllowedOrigin = a.AllowedOrigin
                //              }).FirstOrDefault();
                // return client;

                return new ClientEntity();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
