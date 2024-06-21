using BusinessLayer.Interfaces;
using BusinessLayer.Services;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderBusiness orderBusiness;
        public OrderController(IOrderBusiness orderBusiness)
        {
            this.orderBusiness = orderBusiness;
        }

        [Authorize]
        [HttpPost("placeorder")]
        public IActionResult PlaceOrder(int cartId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var order = orderBusiness.PlaceOrder(userId, cartId);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Order placed successfully", Data = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to place order", Data = ex.Message });
            }
        }

        [HttpGet("orders")]
        public IActionResult ViewAllOrders()
        {
            try
            {

                var orders = orderBusiness.ViewAllOrders().ToList();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Orders fetched successfully", Data = orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch orders", Data = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("ordersByUser")]
        public IActionResult ViewAllOrdersByUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var orders = orderBusiness.ViewAllOrdersByUser(userId).ToList();
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Orders fetched successfully for user id: " + userId, Data = orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch orders for user", Data = ex.Message });
            }
        }

        [HttpGet("getById/{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            try
            {
                var order = orderBusiness.GetOrderById(orderId);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Orders fetched successfully for order id: " +orderId, Data = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch order for order id: " + orderId, Data = ex.Message });
            }
        }

        [HttpGet("cancelorder/{orderId}")]
        public IActionResult CancelOrder(int orderId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var order = orderBusiness.CancelOrder(userId, orderId);
                return Ok(new ResponseModel<object> { IsSuccess = true, Message = "Orders canceled successfully for order id: " + orderId, Data = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to cancel order for order id: " + orderId, Data = ex.Message });
            }
        }

    }
}
