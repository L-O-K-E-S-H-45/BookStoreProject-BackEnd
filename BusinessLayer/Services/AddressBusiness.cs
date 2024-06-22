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
        private readonly IAddressRepository addressRepository;
        public AddressBusiness(IAddressRepository addressrepository)
        {
            this.addressRepository = addressrepository;
        }
        public AddressEntity AddAddress(int userId, AddressModel addressModel)
        {
            return addressRepository.AddAddress(userId, addressModel);
        }

        public List<AddressEntity> GetAllAddresses()
        {
            return addressRepository.GetAllAddresses();
        }

        public List<AddressEntity> GetAddressesByUser(int userId)
        {
            return addressRepository.GetAddressesByUser(userId);
        }

        public AddressEntity GetAddressById(int userId, int addressId)
        {
            return addressRepository.GetAddressById(userId, addressId);
        }

        public AddressEntity UpdateAddress(int userId, int addressId, AddressModel addressModel)
        {
            return addressRepository.UpdateAddress(userId, addressId, addressModel);
        }

        public bool DeleteAddress(int userId, int addressId)
        {
            return addressRepository.DeleteAddress(userId, addressId);
        }
    }
}
