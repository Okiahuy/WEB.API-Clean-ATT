using APPLICATIONCORE.Domain.Momo.MomoDtos;
using APPLICATIONCORE.Models;
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
        Task<string> CreatePaymentMomoAsync(OrderViewModel orderViewModel, string orderId);
        Task<IEnumerable<object>> GetAllOrders();
        Task<OrderModel> GetOrderById(int Id);
        Task<MomoDTO> GetMomoBycode_orderId(string code_order);
        Task<List<MomodetailDTO>> GetMomoDetailBycode_order(string code_order);
        Task<List<OrderModel>> GetOrdersByAccountID(int accountID);
        Task<List<OrderDetailModel>> GetOrderDetailsByOrderID(int Id);
        Task<Result> UpdateOrderAddressAsync(int orderID, int accountID, int addressID);
        Task UpdateOrderStatusAsync(int orderID, int status_order);
        Task<object> GetOrderDetailsByOrderIDPrintInvoice(int orderID);
    }
}
