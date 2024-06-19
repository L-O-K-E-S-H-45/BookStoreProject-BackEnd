using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class ForgotPasswordModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
