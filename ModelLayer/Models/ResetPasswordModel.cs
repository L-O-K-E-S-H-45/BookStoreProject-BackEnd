using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, " +
            "one lowercase letter, one digit, and one special character")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
