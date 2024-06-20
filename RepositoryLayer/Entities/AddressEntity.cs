using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class AddressEntity
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public long Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Type { get; set; }

    }
}
