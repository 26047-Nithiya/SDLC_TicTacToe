using TicTacToe.Model;
using TicTacToe.Repository;

namespace TicTacToe.Services
{
    /// <summary>
    /// Services for handling authentication
    /// </summary>
    public class AuthService<T> : IAuthService<T>
    {
        private IJsonRepo<User> userRepo;

        /// <summary>
        /// Constructor to initialize other layer references
        /// </summary>
        /// <param name="userRepo"> Object reference for the repository layer </param>
        public AuthService(IJsonRepo<User> userRepo)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// Method to register a new user
        /// </summary>
        /// <param name="userName"> Username of user</param>
        /// <param name="password"> Password of the user </param>
        /// <returns> True if registration successful </returns>
        public bool Register(string userName, string password)
        {
            Guid userId = Guid.NewGuid();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            User user = new User(userId, userName, hashedPassword);
            if (user != null)
            {
                userRepo.Add(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to login to an existing account
        /// </summary>
        /// <param name="userName"> Name of the user </param>
        /// <param name="password"> Password of the user </param>
        /// <returns> True if login successful </returns>
        public Session Login(string userName, string password)
        {
            var users = userRepo.GetItems();
            var user = users.FirstOrDefault(x => x.Name == userName);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new Session(user.Id, userName);
            }
            return null;
        }
    }
}
