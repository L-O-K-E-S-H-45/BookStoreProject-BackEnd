using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IFeedbackRepository
    {
        Feedback GiveFeedback(int userId, FeedbackModel feedbackModel);
        List<Feedback> ViewAllFeedbacks();
        List<Feedback> ViewAllFeedbacksOfBook(int bookId);
        Feedback EditReview(int userId, EditFeedbackModel editFeedbackModel);
        bool DeleteReview(int userId, int feedbackId);

    }
}
