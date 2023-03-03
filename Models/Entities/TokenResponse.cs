using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class TokenResponse
    {
        public string id_token { get; set; }
    }

    public class AccountRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
