using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.BackEnd.Models
{
    public class AccountModel
    {
    }

    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class UserLoginViewModel
    {
        [Required, StringLength(50)]
        public string Username { get; set; }
        [Required, StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }

}
