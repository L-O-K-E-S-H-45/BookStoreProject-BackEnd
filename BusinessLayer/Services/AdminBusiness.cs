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
    public class AdminBusiness : IAdminBusiness
    {
        private readonly IAdminRepository adminRepository;
        public AdminBusiness(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        public AdminLoginResponseModel AdminLogin(LoginModel loginModel)
        {
            return adminRepository.AdminLogin(loginModel);
        }
    }
}
