using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        public CartRepository(BookContext bookContext)
        {
            this.bookContext = bookContext;
            this.sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
        }
        public Cart AddBookToCart(Cart_WishListModel cart_WishListModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AddBookToCart", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", cart_WishListModel.UserId);
                    sqlCommand.Parameters.AddWithValue("@BookId", cart_WishListModel.BookId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            CartId = (int)dataReader["CartId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        return cart;
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
        public List<Cart> ViewAllCarts()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Cart> carts = new List<Cart>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_ViewAllCarts", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            CartId = (int)dataReader["CartId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        carts.Add(cart);
                    }
                    return carts;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<Cart> ViewCartByUser(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Cart> carts = new List<Cart>();

                    SqlCommand sqlCommand = new SqlCommand("usp_ViewCartByUser", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            CartId = (int)dataReader["CartId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        carts.Add(cart);
                    }
                    return carts;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }
        public Cart UpdateCart(int cartId, int quantity)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_UpdateCart", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CartId", cartId);
                    sqlCommand.Parameters.AddWithValue("@Quantity", quantity);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            CartId = (int)dataReader["CartId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            Quantity = (int)dataReader["Quantity"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        return cart;
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

        public bool RemoveBookFromCart(int cartId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_RemoveBookFromcart", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CartId", cartId);

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


        public int NoOfBooksInUserCart(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_CountBooksInUserCart", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    //sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    var returnParameter = sqlCommand.Parameters.Add("@NoOfBooks", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    sqlConnection.Open();
                    int nora = sqlCommand.ExecuteNonQuery();

                    int result = (int)returnParameter.Value;

                    return result;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        //Review task
        // -- 3) Display wishlist or cart details alongwith the user who has added it
        public object GetCartDetailsWithUser()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Dictionary<string,object>> carts = new List<Dictionary<string,object>>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_CartDetilsWithUser", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {

                        Dictionary<string, object> data = new Dictionary<string, object>()
                        {
                            
                                {"CartId", dataReader["CartId"]},
                                {"BookId" , dataReader["BookId"] },
                                {"Title" , dataReader["Title"] },
                                {"Author" , dataReader["Author"] },
                                {"Image" , (string)dataReader["Image"] },
                                {"Quantity" , dataReader["Quantity"] },
                                {"OriginalBookPrice" , dataReader["OriginalBookPrice"] },
                                {"FinalBookPrice" , dataReader["FinalBookPrice"] },
                                {"UserId" , dataReader["UserId"] },
                                {"FullName" , dataReader["FullName"] },
                                {"Email" , dataReader["Email"] },
                                {"Mobile" , dataReader["Mobile"] }
                            
                        };
                        carts.Add(data);
                    }
                    return carts;
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
