using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Security.Claims;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IBus bus;
        public UserController(IUserBusiness userBusiness, IBus bus)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
        }

        [HttpPost("register")]
        public IActionResult UserRegistration(UserModel userModel)
        {
            try
            {
                var user = userBusiness.UserRegistration(userModel);
                return Ok(new ResponseModel<User> { IsSuccess = true, Message = "User registration successfull", Data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User registration failed", Data = ex.Message});
            }
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            try
            {
                List<User> users = userBusiness.GetAllUsers();
                return Ok(new ResponseModel<List<User>> { IsSuccess = true, Message = "Successfully fetched users", Data = users });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to find users", Data = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult UserLogin(LoginModel loginModel)
        {
            try
            {
                var result = userBusiness.UserLogin(loginModel);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User login successfull", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to login user", Data = ex.Message });
            }
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                ForgotPasswordModel forgotPasswordModel = userBusiness.ForgotPassword(email);
                Send send = new Send();
                send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq://localhost/BookStoreEmailQueue");
                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail sent successfully", Data = "Token has been sent to your mail to reset password"});
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to send mail, Please provide valid email!!!", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("resetpassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var email = User.FindFirstValue("Email");
                if (email != null)
                {
                    var result = userBusiness.ResetPassword(email, resetPasswordModel);
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Reset password successfull", Data = "successfully password is reset" });
                }
                else throw new Exception("Email is null");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to reset password", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("getUserById")]
        public IActionResult GetUserById()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));
                if (userId != null)
                {
                    var user = userBusiness.GetUserById(userId);
                    return Ok(new ResponseModel<User> { IsSuccess = true, Message = "User fetched successfully", Data = user });
                }
                else throw new Exception("User id is null");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch user", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateUser(UserModel userModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));
                if (userId != null)
                {
                    var user = userBusiness.UpdateUser(userId, userModel);
                    return Ok(new ResponseModel<User> { IsSuccess = true, Message = "User update is successfull", Data = user });
                }
                else throw new Exception("User id is null");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update user", Data = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                var result = userBusiness.DeleteUser(userId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "User deleted successfully", Data=result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete user", Data = ex.Message });
            }
        }


    }
}
