using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRepository
    {
        object PlaceOrder(int userId, int cartId);
        List<Orders> ViewAllOrders();
        List<Orders> ViewAllOrdersByUser(int userId);
        Orders GetOrderById(int orderId);
        Orders CancelOrder(int userId, int orderId);

    }
}
