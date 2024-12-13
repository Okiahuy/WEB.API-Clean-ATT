using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Address
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressModel>> GetAllAddresses();
        Task<AddressModel> GetAddressById(int addressID);
        Task<List<AddressModel>> GetAddressByAccountIDAsync(int accountID);
        Task AddAddress(AddressModel address);
        Task<AddressModel> UpdateAddressAsync(int addressID, AddressModel address);
        Task DeleteAddress(int addressID);
        Task<IEnumerable<AddressModel>> SearchAddresses(string addressName);

        Task<IEnumerable<AddressModel>> FindById(int addressID);

        Task<IEnumerable<OrderModel>> FindAddressById(int id);
    }
}
