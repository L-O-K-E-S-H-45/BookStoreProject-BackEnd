using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBusiness adminBusiness;
        public AdminController(IAdminBusiness adminBusiness)
        {
            this.adminBusiness = adminBusiness;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var result = adminBusiness.AdminLogin(loginModel);
                return Ok(new ResponseModel<AdminLoginResponseModel> { IsSuccess = true, Message = "Admin login successfull", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to login admin", Data = ex.Message });
            }
        }

    }
}
