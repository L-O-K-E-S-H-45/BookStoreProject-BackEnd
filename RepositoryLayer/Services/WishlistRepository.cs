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
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        public WishlistRepository(BookContext bookContext)
        {
            this.bookContext = bookContext;
            this.sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
        }

        public Wishlist AddBookToWishlist(Cart_WishListModel cart_WishListModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AddBookToWishlist", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", cart_WishListModel.UserId);
                    sqlCommand.Parameters.AddWithValue("@BookId", cart_WishListModel.BookId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Wishlist wishlist = new Wishlist()
                        {
                            WishlistId = (int)dataReader["WishlistId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        return wishlist;
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

        public List<Wishlist> ViewAllWishlist()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Wishlist> wishlists = new List<Wishlist>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_ViewAllWhishlists", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Wishlist wishlist = new Wishlist()
                        {
                            WishlistId = (int)dataReader["WishlistId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        wishlists.Add(wishlist);
                    }
                    return wishlists;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<Wishlist> ViewWhishlistByUser(int userId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Wishlist> wishlists = new List<Wishlist>();

                    SqlCommand sqlCommand = new SqlCommand("usp_ViewWhishlistByUser", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Wishlist wishlist = new Wishlist()
                        {
                            WishlistId = (int)dataReader["WishlistId"],
                            UserId = (int)dataReader["UserId"],
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Image = (string)dataReader["Image"],
                            OriginalBookPrice = (int)dataReader["OriginalBookPrice"],
                            FinalBookPrice = (int)dataReader["FinalBookPrice"],
                        };
                        wishlists.Add(wishlist);
                    }
                    return wishlists;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }
        public bool RemoveBookFromWishlist(int wishlistId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_RemoveBookFromWhishlist", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@WhishlistId", wishlistId);

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

        public int NoOfBooksInUserWishlist(int userId)
        {
            try
            {
                List<Wishlist> wishlists = ViewWhishlistByUser(userId);
                if (wishlists.Count > 0)
                    return wishlists.Count;
                else throw new Exception("Wishlist is empty for user id: " + userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }
    }
}
