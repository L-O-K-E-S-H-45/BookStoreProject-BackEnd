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
    public class AddressBusiness : IAddressBusiness
    {
        private readonly IAddressrepository addressrepository;
        public AddressBusiness(IAddressrepository addressrepository)
        {
            this.addressrepository = addressrepository;
        }
        public AddressEntity AddAddress(int userId, AddressModel addressModel)
        {
            return addressrepository.AddAddress(userId, addressModel);
        }

        public List<AddressEntity> GetAllAddresses()
        {
            return addressrepository.GetAllAddresses();
        }

        public List<AddressEntity> GetAddressesByUser(int userId)
        {
            return addressrepository.GetAddressesByUser(userId);
        }

        public AddressEntity GetAddressById(int userId, int addressId)
        {
            return addressrepository.GetAddressById(userId, addressId);
        }

        public AddressEntity UpdateAddress(int userId, int addressId, AddressModel addressModel)
        {
            return addressrepository.UpdateAddress(userId, addressId, addressModel);
        }

        public bool DeleteAddress(int userId, int addressId)
        {
            return addressrepository.DeleteAddress(userId, addressId);
        }
    }
}
