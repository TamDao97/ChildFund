using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.Entity
{
    public class ClientEntity
    {
        public string Id { get; set; }
        public string SecretKey { get; set; }
        public string Name { get; set; }
        public int ApplicationType { get; set; }
        public int Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
