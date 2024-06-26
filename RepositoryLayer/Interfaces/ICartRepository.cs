﻿using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepository
    {
        Cart AddBookToCart(Cart_WishListModel cart_WishListModel);
        List<Cart> ViewAllCarts();
        List<Cart> ViewCartByUser(int userId);
        Cart UpdateCart(int cartId, int quantity);
        bool RemoveBookFromCart(int cartId);
        int NoOfBooksInUserCart(int userId);

        // review Task
        object GetCartDetailsWithUser();

    }
}
