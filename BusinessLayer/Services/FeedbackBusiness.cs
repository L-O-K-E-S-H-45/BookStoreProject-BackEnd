using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FeedbackBusiness : IFeedbackBusiness
    {
        private readonly IFeedbackRepository feedbackRepository;
        public FeedbackBusiness(IFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public Feedback GiveFeedback(int userId, FeedbackModel feedbackModel)
        {
            return feedbackRepository.GiveFeedback(userId, feedbackModel);
        }

        public List<Feedback> ViewAllFeedbacks()
        {
            return feedbackRepository.ViewAllFeedbacks();
        }

        public List<Feedback> ViewAllFeedbacksOfBook(int bookId)
        {
            return feedbackRepository.ViewAllFeedbacksOfBook(bookId);
        }

        public Feedback EditReview(int userId, EditFeedbackModel editFeedbackModel)
        {
            return feedbackRepository.EditReview(userId, editFeedbackModel);
        }

        public bool DeleteReview(int userId, int feedbackId)
        {
            return feedbackRepository.DeleteReview(userId, feedbackId);
        }
    }
}
