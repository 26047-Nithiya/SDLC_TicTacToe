using System.Text;
using TicTacToe.Controller;
using TicTacToe.Repository;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe
{
    /// <summary>
    /// Main program of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that acts as the entry-point of the application
        /// </summary>
        /// <param name="args">An array of command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            UserRepo userRepo = new UserRepo("Users.json"); 
            GameService gameService = new GameService();
            AuthService authService = new AuthService(userRepo);
            ConsoleUI consoleUI = new ConsoleUI();
            Validation validator = new Validation(userRepo, consoleUI);
            GameController gameController = new GameController(consoleUI, gameService, validator);
            AuthController controller = new AuthController(gameController, authService, consoleUI, validator);
            bool isLoginSuccess = controller.Run();
            if(isLoginSuccess)
            {
                gameController.StartGame();
            }
        }
    }
}
