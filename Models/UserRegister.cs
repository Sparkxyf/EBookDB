using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookDB.Models
{
    public class UserRegister
    {
        public int Identity { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EnsurePassword { get; set; }
        public int Authority { get; set; }
    }
}