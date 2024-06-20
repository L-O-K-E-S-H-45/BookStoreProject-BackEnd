using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class BookContext
    {
        private readonly IConfiguration configuration;
        private readonly string sqlConnectionString;

        public BookContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.sqlConnectionString = configuration.GetConnectionString("DBConnection");
        }

        public IDbConnection GetDbConnection() => new SqlConnection(sqlConnectionString);
    }
}
