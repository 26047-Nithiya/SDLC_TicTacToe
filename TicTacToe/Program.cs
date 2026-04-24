using System.Text;
using TicTacToe.Controller;
using TicTacToe.Model;
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
            IJsonRepo<User> userRepo = new JsonRepo<User>("Users.json");
            IJsonRepo<Game> gameRepo = new JsonRepo<Game>("Games.json");
            IGameService gameService = new GameService(gameRepo);
            IAuthService<User> authService = new AuthService<User>(userRepo);
            ConsoleUI consoleUI = new ConsoleUI();
            Validation<User> validator = new Validation<User>(userRepo, consoleUI);
            AuthController controller = new AuthController(authService, consoleUI, validator);
            GameStarter(gameService, consoleUI, validator, controller);
        }

        /// <summary>
        /// Method to start the game application
        /// </summary>
        /// <param name="gameService"> Object references for the game service layer </param>
        /// <param name="consoleUI"> Object references for the console layer </param>
        /// <param name="validator"> Object reference for the validation layer </param>
        /// <param name="controller"> Object reference for the authentication controller </param>
        public static void GameStarter(IGameService gameService, ConsoleUI consoleUI, Validation<User> validator, AuthController controller)
        {
            while (true)
            {
                var session = controller.Run();
                if (session != null)
                {
                    Game game = new Game();
                    game.UserId = session.UserId;
                    GameController gameController = new GameController(session, game, consoleUI, gameService, validator, controller);
                    gameController.SelectMenuOption(game.UserId);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
