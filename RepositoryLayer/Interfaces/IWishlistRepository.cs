using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IWishlistRepository
    {
        Wishlist AddBookToWishlist(Cart_WishListModel cart_WishListModel);
        List<Wishlist> ViewAllWishlist();
        List<Wishlist> ViewWhishlistByUser(int userId);
        bool RemoveBookFromWishlist(int cartId);
        int NoOfBooksInUserWishlist(int userId);
    }
}
