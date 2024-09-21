using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShowBooking.Interface;
using ShowBooking.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShowBooking.Repository
{
    public class UserRepository : IUser
    {
        private readonly ShowContext _context;
        private readonly SymmetricSecurityKey _key;

        public UserRepository(ShowContext context, IConfiguration configuration)
        {
            _context = context;
            _key = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(configuration["Key"]!));
        }

        public async Task ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (!VerifyPasswordHash(oldPassword, user.PasswordHash))
            {
                throw new Exception("The old password is incorrect.");
            }

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            user.PasswordHash = newPasswordHash;

            await _context.SaveChangesAsync();
        }

        public async Task CreateOrganizer(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email already exists.");
                }

                user.RoleID = 2;

                var tempPassword = GenerateTemporaryPassword();
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword);

                user.IsActive = true;
                user.CreatedAt = DateTime.Now;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await SendPasswordResetEmail(user.Email, tempPassword);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the organizer: {ex.Message}");
            }
        }

        public Task Deactivation(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .Where(u => u.RoleID == 2 && _context.Theatres.Any(t => t.UserID == u.UserID)) // Check RoleID and Theatre mapping
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetOrganizers()
        {
            
           List<User> users =  await _context.Users
                .Where(u => u.RoleID == 2).ToListAsync();
            var organizersWithMovies = new List<object>();

            foreach (User user in users)
            {
                
                var userTheatre = await _context.Theatres.FirstOrDefaultAsync(t => t.UserID == user.UserID);

                if (userTheatre != null)
                {
                    var moviesNotMappedToUserTheatre = await _context.Movies
                        .Where(m => !_context.MoviesInTheatre
                            .Where(mit => mit.TheatreID == userTheatre.TheatreID)
                            .Select(mit => mit.MovieID)
                            .Contains(m.MovieID))
                        .Select(m => new
                        {
                            m.MovieID,
                            m.MovieName
                        })
                        .ToListAsync();


                    organizersWithMovies.Add(new
                    {
                        UserID = user.UserID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        theatreID = userTheatre.TheatreID,
                        HasTheatre = userTheatre != null ? true : false,
                        TheatreName = userTheatre != null ? userTheatre.TheatreName : "No theatre mapped",
                        MoviesNotMapped = moviesNotMappedToUserTheatre
                    });

                }
                else
                {
                    organizersWithMovies.Add(new
                    {
                        UserID = user.UserID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        HasTheatre = userTheatre != null ? true : false,
                        TheatreName = userTheatre != null ? userTheatre.TheatreName : "No theatre mapped",
                        MoviesNotMapped = new List<object>()
                    });
                }

            }
            return organizersWithMovies;
        }

        public async Task<User> GetDetails(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id) ?? null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> Login(string email, string password)
        {
            string token = string.Empty;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return string.Empty;
            }

            bool isPasswordValid = VerifyPasswordHash(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return string.Empty;
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.FirstName + " " + user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, Convert.ToString(user.RoleID))
            };
            claims.Add(new Claim("UserId", user.UserID.ToString()));

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(createToken);

            return token;
        }

        public async Task Register(User user)
        {
            if (!IsValidEmail(user.Email))
            {
                throw new ArgumentException("Invalid email format");
            }
            user.RoleID = 3;

            string rawPassword = user.PasswordHash;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task ResetPassword(int userId, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateDetails(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserID);

                if (existingUser == null)
                {
                    return null;
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user details: {ex.Message}");
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private async Task SendPasswordResetEmail(string email, string tempPassword)
        {
            var fromAddress = new MailAddress("showspot.booking@gmail.com", "ShowSpot");
            var toAddress = new MailAddress(email);
            const string fromPassword = "whiw jkvl zboa xkkp";
            const string subject = "Account Creation & Password Reset Notification";

            var body = $"Dear Organizer,\n\n" +
                           $"Your account has been created. Please use the temporary password below to log in and reset your password.\n" +
                           $"Temporary Password: {tempPassword}\n" +
                           $"Kindly login to reset your password\n\n" +
                           $"Best Regards,\nShowSpot Team";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }



        private string GenerateTemporaryPassword()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            char[] password = new char[10];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[password.Length];

                rng.GetBytes(randomBytes);

                for (int i = 0; i < password.Length; i++)
                {
                    int randomIndex = randomBytes[i] % validChars.Length;
                    password[i] = validChars[randomIndex];
                }
            }

            return new string(password);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use System.Net.Mail.MailAddress for email format validation
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}
