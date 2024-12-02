﻿using APPLICATIONCORE.Models;
using APPLICATIONCORE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Order
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(OrderViewModel orderViewModel);
        Task<IEnumerable<OrderModel>> GetAllOrders();
        Task<OrderModel> GetOrderById(int Id);
        Task<List<OrderModel>> GetOrdersByAccountID(int accountID);
    }
}
