using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IOrderBusiness
    {
        object PlaceOrder(int userId, int cartId, int addressId);
        List<Orders> ViewAllOrders();
        List<Orders> ViewAllOrdersByUser(int userId);
        Orders GetOrderById(int orderId);
        Orders CancelOrder(int userId, int orderId);
    }
}
