using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IAddressrepository
    {
        AddressEntity AddAddress(int userId, AddressModel addressModel);
        List<AddressEntity> GetAllAddresses();
        List<AddressEntity> GetAddressesByUser(int userId);
        AddressEntity GetAddressById(int userId, int addressId);
        AddressEntity UpdateAddress(int userId, int addressId, AddressModel addressModel);
        bool DeleteAddress(int userId, int addressId);

    }
}
