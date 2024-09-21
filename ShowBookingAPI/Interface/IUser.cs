using ShowBooking.Models;

namespace ShowBooking.Interface
{
    public interface IUser
    {
        Task Register(User user);
        Task<string> Login(string email, string password);
        Task CreateOrganizer(User user);
        Task<User> GetDetails(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> UpdateDetails(User user);
        Task<IEnumerable<User>> GetAllUsers();
        Task ResetPassword(int userId, string password);
        Task ChangePassword(int userId, string oldPassword, string newPassword);
        Task Deactivation(int userId);
        Task<IEnumerable<object>> GetOrganizers();
    }
}
