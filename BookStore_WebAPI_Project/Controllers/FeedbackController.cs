using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using System.Security.Claims;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackBusiness feedbackBusiness;
        public FeedbackController(IFeedbackBusiness feedbackBusiness)
        {
            this.feedbackBusiness = feedbackBusiness;
        }

        [Authorize]
        [HttpPost("gevefeedback")]
        public IActionResult GiveFeedback(FeedbackModel feedbackModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var feedback = feedbackBusiness.GiveFeedback(userId, feedbackModel);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Feedback uploaded successfully", Data = feedback });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add feedback", Data = ex.Message });
            }
        }

        [HttpGet("feedbacks")]
        public IActionResult ViewAllFeedbacks()
        {
            try
            {
                var feedbacks = feedbackBusiness.ViewAllFeedbacks().ToList();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Feedbacks fetched successfully", Data = feedbacks });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch feedbacks", Data = ex.Message });
            }
        }

        [HttpGet("feedbacksByBook")]
        public IActionResult ViewAllFeedbacksOfBook(int bookId)
        {
            try
            {
                var feedbacks = feedbackBusiness.ViewAllFeedbacksOfBook(bookId).ToList();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Feeadbacks fetched successfully for book id: " + bookId, Data = feedbacks });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch feedbacks for book id: " + bookId, Data = ex.Message });
                }
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult EditReview(EditFeedbackModel editFeedbackModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var feedback = feedbackBusiness.EditReview(userId, editFeedbackModel);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Feedback updated successfully for feedback id: " + editFeedbackModel.FeedbackId, Data = feedback });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update feedback for feedback id: " + editFeedbackModel.FeedbackId, Data = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteFeedback(int feedbackId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var result = feedbackBusiness.DeleteReview(userId, feedbackId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Feedback successfully deleted", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete feedback", Data = ex.Message });
            }
        }


    }
}
