using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;

namespace Shared
{
    public class RegisterUser
    {
        [Required, EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required, StringLength(100, MinimumLength = 5)]

        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}