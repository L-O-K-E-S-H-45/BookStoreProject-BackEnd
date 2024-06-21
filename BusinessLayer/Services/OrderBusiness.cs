using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository orderRepository;
        public OrderBusiness(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public object PlaceOrder(int userId, int cartId)
        {
            return orderRepository.PlaceOrder(userId, cartId);
        }

        public List<Orders> ViewAllOrders()
        {
            return orderRepository.ViewAllOrders();
        }

        public List<Orders> ViewAllOrdersByUser(int userId)
        {
            return orderRepository.ViewAllOrdersByUser(userId);
        }

        public Orders GetOrderById(int orderId)
        {
            return orderRepository.GetOrderById(orderId);
        }

        public Orders CancelOrder(int userId, int orderId)
        {
            return orderRepository.CancelOrder(userId, orderId);
        }

    }
}
