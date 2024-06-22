using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace RepositoryLayer.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        private readonly IConfiguration configuration;
        public AdminRepository(BookContext bookContext, IConfiguration configuration)
        {
            this.bookContext = bookContext;
            sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
            this.configuration = configuration;
        }

        private string GenerateToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AdminLoginResponseModel AdminLogin(LoginModel loginModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AdminLogin", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Email", loginModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", loginModel.Password);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var token = GenerateToken((string)dataReader["Email"], (int)dataReader["AdminId"]);
                        AdminLoginResponseModel responseModel = new AdminLoginResponseModel()
                        {
                            AdminId = dataReader.GetInt32("AdminId"),
                            FullName = dataReader.GetString("FullName"),
                            Email = dataReader.GetString("email"),
                            Mobile = dataReader.GetInt64("Mobile"),
                            Token = token
                        };
                        return responseModel;
                    }

                }
                else throw new Exception("Sql Connection not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
            return null;
        }
    }
}
