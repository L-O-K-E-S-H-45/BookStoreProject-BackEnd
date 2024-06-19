using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection sqlConnection = new SqlConnection();
        private readonly string SqlConnectionString;
        private readonly IConfiguration configuration;
        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            SqlConnectionString = configuration.GetConnectionString("DBConnection");
            sqlConnection.ConnectionString = SqlConnectionString;
        }

        private string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in password encode using base64Encode: " + ex.Message);
            }
        }

        public User UserRegistration(UserModel userModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_InsertUser", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@FullName", userModel.FullName);
                    sqlCommand.Parameters.AddWithValue("@Email", userModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", EncodePassword(userModel.Password));
                    sqlCommand.Parameters.AddWithValue("@Mobile", userModel.Mobile);

                    sqlConnection.Open();
                    //int nora = sqlCommand.ExecuteNonQuery();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        User user = new User()
                        {
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Email = (string)dataReader["Email"],
                            Password = (string)dataReader["Password"],
                            Mobile = (long)dataReader["Mobile"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return user;
                    }
                    return null;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<User> users = new List<User>();

                    SqlCommand sqlCommand = new SqlCommand(" exec usp_GetAllUsers", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        User user = new User()
                        {
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Email = (string)dataReader["Email"],
                            Password = (string)dataReader["Password"],
                            Mobile = (long)dataReader["Mobile"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        users.Add(user);
                    }
                   return users;

                }
                else throw new Exception("SqlConnection is not established");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
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

        public string UserLogin(LoginModel loginModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_UserLogin", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Email", loginModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", EncodePassword(loginModel.Password));

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var token = GenerateToken((string)dataReader["Email"], (int)dataReader["UserId"]);
                        return token;
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


        public ForgotPasswordModel ForgotPassword(string email)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_GetUserByEmail", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Email", email);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var token = GenerateToken((string)dataReader["Email"], (int)dataReader["UserId"]);
                        ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel()
                        {
                            Email = email,
                            UserId = (int)dataReader["UserId"],
                            Token = token
                        };
                        return forgotPasswordModel;
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

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            try
            {
                if (resetPasswordModel.Password.Equals(resetPasswordModel.ConfirmPassword))
                {
                    if (sqlConnection != null)
                    {
                        SqlCommand sqlCommand = new SqlCommand("usp_ResetPassword", sqlConnection);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Email", email);
                        sqlCommand.Parameters.AddWithValue("@Password", EncodePassword(resetPasswordModel.Password));

                        sqlConnection.Open();
                        int nora = sqlCommand.ExecuteNonQuery();
                        return true;
                    }
                    else throw new Exception("Sql Connection not established");
                }
                else throw new Exception("Missmatch password");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public User GetUserById(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_GetUserByUserId", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        User user = new User()
                        {
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Email = (string)dataReader["Email"],
                            Password = (string)dataReader["Password"],
                            Mobile = (long)dataReader["Mobile"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return user;
                    }
                    return null;
                }
                else throw new Exception("Sql Connection not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public User UpdateUser(int userId, UserModel userModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_UpdateUser", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@FullName", userModel.FullName);
                    sqlCommand.Parameters.AddWithValue("@Email", userModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", EncodePassword(userModel.Password));
                    sqlCommand.Parameters.AddWithValue("@Mobile", userModel.Mobile);

                    sqlConnection.Open();
                    
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        User user = new User()
                        {
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Email = (string)dataReader["Email"],
                            Password = (string)dataReader["Password"],
                            Mobile = (long)dataReader["Mobile"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return user;
                    }
                    return null;
                }
                else throw new Exception("Sql Connection not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_DeleteUser", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    int nora = sqlCommand.ExecuteNonQuery();
                    return true;
                }
                else throw new Exception("Sql Connection not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

    }
}
