using TicTacToe.Model;

namespace TicTacToe.Services
{
    public interface IAuthService<T>
    {
        Session Login(string userName, string password);
        bool Register(string userName, string password);
    }
}