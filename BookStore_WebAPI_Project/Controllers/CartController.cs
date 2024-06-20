using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Security.Claims;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartBusiness cartBusiness;
        public CartController(ICartBusiness cartBusiness)
        {
            this.cartBusiness = cartBusiness;
        }

        [HttpPost("addBookToCart")]
        public IActionResult AddBookToCart(Cart_WishListModel? cart_WishListModel)
        {
            try
            {
                if (cart_WishListModel.UserId == null || cart_WishListModel.UserId == 0)
                {
                    var userId = int.Parse(User.FindFirstValue("UserId"));
                    cart_WishListModel.UserId = userId;
                }

                var cart = cartBusiness.AddBookToCart(cart_WishListModel);
                return Ok(new ResponseModel<Cart> { IsSuccess = true, Message = "Book successfully added to cart", Data = cart });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add book to cart", Data = ex.Message });
            }
        }

        [HttpGet("carts")]
        public IActionResult ViewAllCarts()
        {
            try
            {
                var carts = cartBusiness.ViewAllCarts().ToList();
                return Ok(new ResponseModel<List<Cart>> { IsSuccess = true, Message = "carts successfully fetched", Data = carts });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch carts", Data = ex.Message });
            }
        }

        [HttpGet("cartByUser")]
        public IActionResult ViewCartByUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var carts = cartBusiness.ViewCartByUser(userId);
                return Ok(new ResponseModel<List<Cart>> { IsSuccess = true, Message = "User carts successfully fetched", Data = carts });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch user carts", Data = ex.Message });
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateCart(int cartId, int quantity)
        {
            try
            {
                var cart = cartBusiness.UpdateCart(cartId, quantity);
                return Ok(new ResponseModel<Cart> { IsSuccess = true, Message = "Cart successfully updated", Data = cart });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update cart", Data = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult RemoveBookFromCart(int cartId)
        {
            try
            {
                var result = cartBusiness.RemoveBookFromCart(cartId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Cart successfully deleted", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete cart", Data = ex.Message });
            }
        }

        [HttpGet("countBooksForUser")]
        public IActionResult NoOfBooksInUserCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var result = cartBusiness.NoOfBooksInUserCart(userId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User carts count is successfull", Data = "Number of books in cart of user id: " + userId + " is: " + result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to get count of user carts", Data = ex.Message });
            }
        }


    }
}
