using TicTacToe.Controller;
using TicTacToe.Repository;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UserRepo userRepo = new UserRepo("Users.json"); 
            GameService gameService = new GameService();
            AuthService authService = new AuthService(userRepo);
            ConsoleUI consoleUI = new ConsoleUI();
            Validation validator = new Validation(userRepo, consoleUI);
            AppController controller = new AppController(authService, consoleUI, gameService, validator);
            controller.Run();
        }
    }
}
