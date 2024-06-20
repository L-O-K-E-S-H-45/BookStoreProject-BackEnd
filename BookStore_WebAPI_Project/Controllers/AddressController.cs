using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Security.Claims;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBusiness addressBusiness;
        public AddressController(IAddressBusiness addressBusiness)
        {
            this.addressBusiness = addressBusiness;
        }

        [Authorize]
        [HttpPost("addAddress")]
        public IActionResult AddAddress(AddressModel addressModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var address = addressBusiness.AddAddress(userId, addressModel);
                return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "Address successfully added to user", Data = address });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add address to user", Data = ex.Message });
            }
        }

        [HttpGet("addresses")]
        public IActionResult GetAllAddresses()
        {
            try
            {
                var addresses = addressBusiness.GetAllAddresses().ToList();
                return Ok(new ResponseModel<List<AddressEntity>> { IsSuccess = true, Message = "Addresses successfully fetched", Data = addresses });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch addresses", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("addressesByUser")]
        public IActionResult GetAddressesByUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var addresses = addressBusiness.GetAddressesByUser(userId).ToList();
                return Ok(new ResponseModel<List<AddressEntity>> { IsSuccess = true, Message = "User addresses successfully fetched", Data = addresses });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch user addresses", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("getById/{addressId}")]
        public IActionResult UpdateAddress(int addressId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var address = addressBusiness.GetAddressById(userId, addressId);
                return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "Address successfully fetched", Data = address });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch Address", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateAddress(int addressId, AddressModel addressModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var address = addressBusiness.UpdateAddress(userId, addressId, addressModel);
                return Ok(new ResponseModel<AddressEntity> { IsSuccess = true, Message = "Address successfully updated", Data = address });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update Address", Data = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue("UserId"));

                var result = addressBusiness.DeleteAddress(userId, addressId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Address successfully deleted", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete address", Data = ex.Message });
            }
        }

    }
}
