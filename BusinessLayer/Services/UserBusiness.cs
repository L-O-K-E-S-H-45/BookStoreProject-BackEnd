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
    public class UserBusiness : IUserBusiness
    {

        private readonly IUserRepository userRepository;
        public UserBusiness(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User UserRegistration(UserModel userModel)
        {
            return userRepository.UserRegistration(userModel);
        }

        public List<User> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public string UserLogin(LoginModel loginModel)
        {
            return userRepository.UserLogin(loginModel);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepository.ForgotPassword(email);
        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            return userRepository.ResetPassword(email, resetPasswordModel);
        }

        public User GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }

        public User UpdateUser(int userId, UserModel userModel)
        {
            return userRepository.UpdateUser(userId, userModel);
        }
        public bool DeleteUser(int userId)
        {
            return userRepository.DeleteUser(userId);
        }

    }
}
