using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Security.Claims;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistBusiness wishlistBusiness;
        public WishlistController(IWishlistBusiness wishlistBusiness)
        {
            this.wishlistBusiness = wishlistBusiness;
        }

        [HttpPost("addBookToWishlist")]
        public IActionResult AddBookToWishlist(Cart_WishListModel? cart_WishListModel)
        {
            try
            {
                if (cart_WishListModel.UserId == null || cart_WishListModel.UserId == 0)
                {
                    var userId = int.Parse(User.FindFirstValue("UserId"));
                    cart_WishListModel.UserId = userId;
                }

                var wishlist = wishlistBusiness.AddBookToWishlist(cart_WishListModel);
                return Ok(new ResponseModel<Wishlist> { IsSuccess = true, Message = "Book successfully added to wishlist", Data = wishlist });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add book to wishlist", Data = ex.Message });
            }
        }

        [HttpGet("wishlists")]
        public IActionResult ViewAllWishlists()
        {
            try
            {
                var wishlists = wishlistBusiness.ViewAllWishlist().ToList();
                return Ok(new ResponseModel<List<Wishlist>> { IsSuccess = true, Message = "Wishlists successfully fetched", Data = wishlists });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch wishlists", Data = ex.Message });
            }
        }

        [HttpGet("wishlistByUser")]
        public IActionResult ViewWishlistByUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var wishlists = wishlistBusiness.ViewWhishlistByUser(userId);
                return Ok(new ResponseModel<List<Wishlist>> { IsSuccess = true, Message = "User wishlists successfully fetched", Data = wishlists });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch user wishlists", Data = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult RemoveBookFromWishlist(int wishlistId)
        {
            try
            {
                var result = wishlistBusiness.RemoveBookFromWishlist(wishlistId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Wishlist successfully deleted", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete wishlist", Data = ex.Message });
            }
        }

        [HttpGet("countBooksForUser")]
        public IActionResult NoOfBooksInUserWishlist()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var result = wishlistBusiness.NoOfBooksInUserWishlist(userId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User wishlists count is successfull", Data = "Number of books in wishlist of user id: " + userId + " is: " + result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to get count of user wishlists", Data = ex.Message });
            }
        }
    }
}
