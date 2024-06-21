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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookContext bookContext;
        private readonly SqlConnection sqlConnection = null;
        public FeedbackRepository(BookContext bookContext)
        {
            this.bookContext = bookContext;
            this.sqlConnection = (SqlConnection?)bookContext.GetDbConnection();
        }

        public Feedback GiveFeedback(int userId, FeedbackModel feedbackModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_GiveFeedback", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@BookId", feedbackModel.BookId);
                    sqlCommand.Parameters.AddWithValue("@Rating", feedbackModel.Rating);
                    sqlCommand.Parameters.AddWithValue("@Review", feedbackModel.Review);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)dataReader["FeedbackId"],
                            UserId = (int)dataReader["UserId"],
                            UserName = (string)dataReader["UserName"],
                            BookId = (int)dataReader["BookId"],
                            Rating = (int)dataReader["Rating"],
                            Review = (string)dataReader["Review"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"]
                        };
                        return feedback;
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

        public List<Feedback> ViewAllFeedbacks()
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Feedback> feedbacks = new List<Feedback>();

                    SqlCommand sqlCommand = new SqlCommand("exec usp_ViewAllFeedbacks", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)dataReader["FeedbackId"],
                            UserId = (int)dataReader["UserId"],
                            UserName = (string)dataReader["UserName"],
                            BookId = (int)dataReader["BookId"],
                            Rating = (int)dataReader["Rating"],
                            Review = (string)dataReader["Review"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"]
                        };
                        feedbacks.Add(feedback);
                    }
                    return feedbacks;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public List<Feedback> ViewAllFeedbacksOfBook(int bookId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    List<Feedback> feedbacks = new List<Feedback>();

                    SqlCommand sqlCommand = new SqlCommand("usp_ViewAllFeedbacksOfBook", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)dataReader["FeedbackId"],
                            UserId = (int)dataReader["UserId"],
                            UserName = (string)dataReader["UserName"],
                            BookId = (int)dataReader["BookId"],
                            Rating = (int)dataReader["Rating"],
                            Review = (string)dataReader["Review"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"]
                        };
                        feedbacks.Add(feedback);
                    }
                    return feedbacks;
                }
                else throw new Exception("SqlConnection is not established");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { sqlConnection.Close(); }
        }

        public Feedback EditReview(int userId, EditFeedbackModel editFeedbackModel)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_EditReview", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@FeedbackId", editFeedbackModel.FeedbackId);
                    sqlCommand.Parameters.AddWithValue("@Rating", editFeedbackModel.Rating);
                    sqlCommand.Parameters.AddWithValue("@Review", editFeedbackModel.Review);

                    sqlConnection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Feedback feedback = new Feedback()
                        {
                            FeedbackId = (int)dataReader["FeedbackId"],
                            UserId = (int)dataReader["UserId"],
                            UserName = (string)dataReader["UserName"],
                            BookId = (int)dataReader["BookId"],
                            Rating = (int)dataReader["Rating"],
                            Review = (string)dataReader["Review"],
                            CreatedAt = (DateTime)dataReader["CreatedAt"],
                            UpdatedAt = (DateTime)dataReader["UpdatedAt"]
                        };
                        return feedback;
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

        public bool DeleteReview(int userId, int feedbackId)
        {
            try
            {
                if (sqlConnection != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_DeleteReview", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    sqlCommand.Parameters.AddWithValue("@FeedbackId", feedbackId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
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
