using Microsoft.EntityFrameworkCore;
using ShowBooking.Interface;
using ShowBooking.Models;

namespace ShowBooking.Service
{
    public class UserService
    {
        private readonly IUser _userRepo;

        public UserService(IUser userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task ChangePassword(int userId, string oldPassword, string newPassword)
        {
            await _userRepo.ChangePassword(userId, oldPassword, newPassword);
        }

        public async Task CreateOrganizer(User user)
        {
            await  _userRepo.CreateOrganizer(user);
        }

        public async Task Deactivation(int userId)
        {
            await _userRepo.Deactivation(userId);
        }

        public async Task<IEnumerable<object>> GetAllUsers()
        {
            return await  _userRepo.GetAllUsers();
        }

        public async Task<User> GetDetails(int id)
        {
            return await  _userRepo.GetDetails(id);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepo.GetUserByEmail(email);
        }

        public async Task<string> Login(string email, string password)
        {
            return await _userRepo.Login(email, password);
        }

        public async Task Register(User user)
        {
            await _userRepo.Register(user);
        }

        public async Task<User> UpdateDetails(User user)
        {
            return await _userRepo.UpdateDetails(user);
        }

        public async Task ResetPassword(int userId, string password)
        {
            await _userRepo.ResetPassword(userId, password);
        }

        public async Task<IEnumerable<object>> GetOrganizers()
        {
            return await _userRepo.GetOrganizers();
        }
    }
}
