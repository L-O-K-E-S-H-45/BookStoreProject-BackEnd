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
    public class CartBusiness : ICartBusiness
    {
        private readonly ICartRepository cartRepository;
        public CartBusiness(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        public Cart AddBookToCart(Cart_WishListModel cart_WishListModel)
        {
            return cartRepository.AddBookToCart(cart_WishListModel);
        }
        public List<Cart> ViewAllCarts()
        {
            return cartRepository.ViewAllCarts();
        }

        public List<Cart> ViewCartByUser(int userId)
        {
            return cartRepository.ViewCartByUser(userId);
        }
        public Cart UpdateCart(int cartId, int quantity)
        {
            return cartRepository.UpdateCart(cartId, quantity);
        }

        public bool RemoveBookFromCart(int cartId)
        {
            return cartRepository.RemoveBookFromCart(cartId);
        }


        public int NoOfBooksInUserCart(int userId)
        {
            return cartRepository.NoOfBooksInUserCart(userId);
        }
    }
}
