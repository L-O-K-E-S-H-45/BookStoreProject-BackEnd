using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace RepositoryLayer.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly SqlConnection sqlConnection = new SqlConnection();
        private readonly string SqlConnectionString;
        private readonly IConfiguration configuration;

        public BookRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.SqlConnectionString = configuration.GetConnectionString("DBConnection");
            sqlConnection.ConnectionString = SqlConnectionString;
        }
        public Book AddBook(BookModel bookModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AddBook", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Title", bookModel.Title);
                    sqlCommand.Parameters.AddWithValue("@Author", bookModel.Author);
                    sqlCommand.Parameters.AddWithValue("@Description", bookModel.Description);
                    sqlCommand.Parameters.AddWithValue("@OriginalPrice", bookModel.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@DiscountPercentage", bookModel.DiscountPercentage);
                    sqlCommand.Parameters.AddWithValue("@Quantity", bookModel.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Image", bookModel.Image);

                    sqlConnection.Open();
                    //int nora = sqlCommand.ExecuteNonQuery();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return book;
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

        public List<Book> GetAllBooks()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Book> books = new List<Book>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_GetAllBooks", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        books.Add(book);
                    }
                    return books;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public Book GetBookById(int bookId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_GetBookById", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BookId",bookId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return book;
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

        public List<Book> GetBookByName(string title, string author)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Book> books = new List<Book>();

                    SqlCommand sqlCommand = new SqlCommand("usp_GetBookByNames", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Title", title);
                    sqlCommand.Parameters.AddWithValue("@Author", author);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        books.Add(book);
                    }
                    return books;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public Book UpdateBook(int bookId, BookModel bookModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_UpdateBook", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    sqlCommand.Parameters.AddWithValue("@Title", bookModel.Title);
                    sqlCommand.Parameters.AddWithValue("@Author", bookModel.Author);
                    sqlCommand.Parameters.AddWithValue("@Description", bookModel.Description);
                    sqlCommand.Parameters.AddWithValue("@OriginalPrice", bookModel.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@DiscountPercentage", bookModel.DiscountPercentage);
                    sqlCommand.Parameters.AddWithValue("@Quantity", bookModel.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Image", bookModel.Image);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return book;
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

        public bool DeleteBook(int bookId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_DeleteBook", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);

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

        // Review Tasks

        public List<Book> GetBook_ByTitleAndPrice(string title, int price)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Book> books = new List<Book>();

                    SqlCommand sqlCommand = new SqlCommand("usp_Book_ByTitleAndPrice", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Title", title);
                    sqlCommand.Parameters.AddWithValue("@Price", price);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    //if (!dataReader.HasRows)
                    //    throw new Exception("Boks not found for title: " + title + " & price <= " + price);

                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        books.Add(book);
                    }
                    return books;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }


        // --2)Find the data using bookid, if it exst update the data else insert the new book record.
        public Book Insert_Update_Book(int bookId, BookModel bookModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_Insert_Update_Book", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    sqlCommand.Parameters.AddWithValue("@Title", bookModel.Title);
                    sqlCommand.Parameters.AddWithValue("@Author", bookModel.Author);
                    sqlCommand.Parameters.AddWithValue("@Description", bookModel.Description);
                    sqlCommand.Parameters.AddWithValue("@OriginalPrice", bookModel.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@DiscountPercentage", bookModel.DiscountPercentage);
                    sqlCommand.Parameters.AddWithValue("@Quantity", bookModel.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Image", bookModel.Image);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book book = new Book()
                        {
                            BookId = (int)dataReader["BookId"],
                            Title = (string)dataReader["Title"],
                            Author = (string)dataReader["Author"],
                            Description = (string)dataReader["Description"],
                            Rating = (decimal)dataReader["Rating"],
                            RatingCount = (int)dataReader["RatingCount"],
                            OriginalPrice = (int)dataReader["OriginalPrice"],
                            DiscountPercentage = (int)dataReader["DiscountPercentage"],
                            Price = (int)dataReader["Price"],
                            Quantity = (int)dataReader["Quantity"],
                            Image = (string)dataReader["Image"],
                            IsDeleted = (bool)dataReader["IsDeleted"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"],
                        };
                        return book;
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
