
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using CommandLine.Text;
//using NHibernate.Criterion;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Models;

namespace ModelLayer.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be between 3 and 50 characters")]
        //[RegularExpression(@"^[A-Z][a-z0-9]{2,}\s?([A-Z]|[A-Z][a-z0-9]*)", ErrorMessage ="{0} must start with an uppercase letter followed by at least two lowercase letters or digits.An optional second word must start with an uppercase letter and can have lowercase letters or digits.")]
        [RegularExpression(@"[A-Z][a-z0-9]{2,}(\s([A-Z][a-z0-9]*))?", ErrorMessage = "{0} must start with an uppercase letter followed by at least two lowercase letters or digits. An optional second word must start with an uppercase letter and can have lowercase letters or digits.")]
        //[SwaggerSchema(Description = "The username of the user.", Example = "Lokesh")]
        //[SwaggerSchema(Description = "The username of the user.")]
        //[SwaggerSchemaExample("john_doe")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(50, MinimumLength = 13, ErrorMessage = "{0} should be between 13 and 50 characters")]
        [RegularExpression(@"[a-z][a-z0-9]{2,}(\@gmail.com)", ErrorMessage = "Email must be in lower case & must start with alphabet & end with @gmail.com")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, " +
            "one lowercase letter, one digit, and one special character")]
        public string Password {  get; set; }

        [Required]
        [RegularExpression(@"^[6-9][0-9]{9}", ErrorMessage = "Mobile number must start with 6 digit and must contain 10 digits only")]
        public long Mobile {  get; set; }

    }
}
