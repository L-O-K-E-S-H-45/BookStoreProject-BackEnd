using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        User UserRegistration(UserModel userModel);

        List<User> GetAllUsers();

        string UserLogin(LoginModel loginModel);

        ForgotPasswordModel ForgotPassword(string email);

        bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);

        User GetUserById(int userId);

        User UpdateUser(int userId, UserModel userModel);

        bool DeleteUser(int userId);
    }
}
