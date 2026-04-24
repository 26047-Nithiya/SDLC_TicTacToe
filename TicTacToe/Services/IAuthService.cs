using TicTacToe.Model;

namespace TicTacToe.Services
{
    /// <summary>
    /// Interface of the authentication services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthService<T>
    {
        /// <summary>
        /// Method to register a new user
        /// </summary>
        /// <param name="userName"> Username of user</param>
        /// <param name="password"> Password of the user </param>
        /// <returns> True if registration successful </returns>
        Session Login(string userName, string password);

        /// <summary>
        /// Method to login to an existing account
        /// </summary>
        /// <param name="userName"> Name of the user </param>
        /// <param name="password"> Password of the user </param>
        /// <returns> True if login successful </returns>
        bool Register(string userName, string password);
    }
}