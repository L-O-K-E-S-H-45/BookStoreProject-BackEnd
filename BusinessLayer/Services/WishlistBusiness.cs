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
    public class WishlistBusiness : IWishlistBusiness
    {
        private readonly IWishlistRepository wishlistRepository;
        public WishlistBusiness(IWishlistRepository wishlistRepository)
        {
            this.wishlistRepository = wishlistRepository;
        }

        public Wishlist AddBookToWishlist(Cart_WishListModel cart_WishListModel)
        {
            return wishlistRepository.AddBookToWishlist(cart_WishListModel);
        }

        public List<Wishlist> ViewAllWishlist()
        {
            return wishlistRepository.ViewAllWishlist();
        }

        public List<Wishlist> ViewWhishlistByUser(int userId)
        {
            return wishlistRepository.ViewWhishlistByUser(userId);
        }
        public bool RemoveBookFromWishlist(int wishlistId)
        {
            return wishlistRepository.RemoveBookFromWishlist(wishlistId);
        }

        public int NoOfBooksInUserWishlist(int userId)
        {
            return wishlistRepository.NoOfBooksInUserWishlist(userId);
        }

    }
}
