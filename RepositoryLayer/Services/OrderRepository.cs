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
    public class OrderRepository : IOrderRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        public OrderRepository(BookContext bookContext)
        {
            this.bookContext = bookContext;
            this.sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
        }

        public object PlaceOrder(int userId, int cartId, int addressId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_PlaceOrder", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@CartId", cartId);
                    sqlCommand.Parameters.AddWithValue("@AddressId", addressId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)dataReader["OrderId"],
                            UserId = (int)dataReader["UserId"],
                            AddressId = (int)dataReader["AddressId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            TotalOriginalBookPrice = (int)dataReader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)dataReader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)dataReader["OrderDateTime"],
                            IsDeleted = (bool)dataReader["IsDeleted"]
                        };
                        return order;
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

        public List<Orders> ViewAllOrders()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Orders> orders = new List<Orders>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_ViewAllOrders", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)dataReader["OrderId"],
                            UserId = (int)dataReader["UserId"],
                            AddressId = (int)dataReader["AddressId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            TotalOriginalBookPrice = (int)dataReader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)dataReader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)dataReader["OrderDateTime"],
                            IsDeleted = (bool)dataReader["IsDeleted"]
                        };
                        orders.Add(order);
                    }
                    return orders;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<Orders> ViewAllOrdersByUser(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Orders> orders = new List<Orders>();

                    SqlCommand sqlCommand = new SqlCommand("usp_ViewOrdersByUser", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)dataReader["OrderId"],
                            UserId = (int)dataReader["UserId"],
                            AddressId = (int)dataReader["AddressId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            TotalOriginalBookPrice = (int)dataReader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)dataReader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)dataReader["OrderDateTime"],
                            IsDeleted = (bool)dataReader["IsDeleted"]
                        };
                        orders.Add(order);
                    }
                    return orders;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public Orders GetOrderById(int orderId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_ViewOrderById", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@OrderId", orderId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)dataReader["OrderId"],
                            UserId = (int)dataReader["UserId"],
                            AddressId = (int)dataReader["AddressId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            TotalOriginalBookPrice = (int)dataReader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)dataReader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)dataReader["OrderDateTime"],
                            IsDeleted = (bool)dataReader["IsDeleted"]
                        };
                        return order;
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

        public Orders CancelOrder(int userId, int orderId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_CancelOrder", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@OrderId", orderId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Orders order = new Orders()
                        {
                            OrderId = (int)dataReader["OrderId"],
                            UserId = (int)dataReader["UserId"],
                            AddressId = (int)dataReader["AddressId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            TotalOriginalBookPrice = (int)dataReader["TotalOriginalBookPrice"],
                            TotalFinalBookPrice = (int)dataReader["TotalFinalBookPrice"],
                            OrderDateTime = (DateTime)dataReader["OrderDateTime"],
                            IsDeleted = (bool)dataReader["IsDeleted"]
                        };
                        return order;
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


    }
}
