using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Entities
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        //public int CartId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int TotalOriginalBookPrice { get; set; }
        public int TotalFinalBookPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
