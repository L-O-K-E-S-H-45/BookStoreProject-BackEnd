using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class AddressRepository : IAddressRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        public AddressRepository(BookContext bookContext)
        {
            this.bookContext = bookContext;
            sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
        }

        public AddressEntity AddAddress(int userId, AddressModel addressModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AddAddress", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@FullName", addressModel.FullName);
                    sqlCommand.Parameters.AddWithValue("@Mobile", addressModel.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Address", addressModel.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressModel.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressModel.State);
                    sqlCommand.Parameters.AddWithValue("@Type", addressModel.Type);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AddressEntity address = new AddressEntity()
                        {
                            AddressId = (int)dataReader["AddressId"],
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Mobile = (long)dataReader["Mobile"],
                            Address = (string)dataReader["Address"],
                            City = (string)dataReader["City"],
                            State = (string)dataReader["State"],
                            Type = (string)dataReader["Type"],
                        };
                        return address;
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

        public List<AddressEntity> GetAllAddresses()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<AddressEntity> addresses = new List<AddressEntity>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_GetAllAddress", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AddressEntity address = new AddressEntity()
                        {
                            AddressId = (int)dataReader["AddressId"],
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Mobile = (long)dataReader["Mobile"],
                            Address = (string)dataReader["Address"],
                            City = (string)dataReader["City"],
                            State = (string)dataReader["State"],
                            Type = (string)dataReader["Type"],
                        };
                        addresses.Add(address);
                    }
                    return addresses;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<AddressEntity> GetAddressesByUser(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<AddressEntity> addresses = new List<AddressEntity>();

                    SqlCommand sqlCommand = new SqlCommand("usp_GetAddressByUser", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AddressEntity address = new AddressEntity()
                        {
                            AddressId = (int)dataReader["AddressId"],
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Mobile = (long)dataReader["Mobile"],
                            Address = (string)dataReader["Address"],
                            City = (string)dataReader["City"],
                            State = (string)dataReader["State"],
                            Type = (string)dataReader["Type"],
                        };
                        addresses.Add(address);
                    }
                    return addresses;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public AddressEntity GetAddressById(int userId, int addressId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_GetAddressById", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure; 
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@AddressId", addressId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AddressEntity address = new AddressEntity()
                        {
                            AddressId = (int)dataReader["AddressId"],
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Mobile = (long)dataReader["Mobile"],
                            Address = (string)dataReader["Address"],
                            City = (string)dataReader["City"],
                            State = (string)dataReader["State"],
                            Type = (string)dataReader["Type"],
                        };
                        return address;
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

        public AddressEntity UpdateAddress(int userId, int addressId, AddressModel addressModel)
        {
            try
            {
                if (sqlConnection != null)
                {

                    SqlCommand sqlCommand = new SqlCommand("usp_UpdateAddress", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@AddressId", addressId);
                    sqlCommand.Parameters.AddWithValue("@FullName", addressModel.FullName);
                    sqlCommand.Parameters.AddWithValue("@Mobile", addressModel.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Address", addressModel.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressModel.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressModel.State);
                    sqlCommand.Parameters.AddWithValue("@Type", addressModel.Type);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AddressEntity address = new AddressEntity()
                        {
                            AddressId = (int)dataReader["AddressId"],
                            UserId = (int)dataReader["UserId"],
                            FullName = (string)dataReader["FullName"],
                            Mobile = (long)dataReader["Mobile"],
                            Address = (string)dataReader["Address"],
                            City = (string)dataReader["City"],
                            State = (string)dataReader["State"],
                            Type = (string)dataReader["Type"],
                        };
                        return address;
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

        public bool DeleteAddress(int userId, int addressId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_DeleteAddress", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@AddressId", addressId);

                    sqlConnection.Open();
                    int nora = sqlCommand.ExecuteNonQuery();
                    return true;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

    }
}
