using TicTacToe.Model;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe.Controller
{
    public class AppController
    {
        private AuthService authService;
        private ConsoleUI consoleUI;
        private GameService gameService;
        private Validation validator;

        /// <summary>
        /// Constructor for initializing objects of other layers
        /// </summary>
        /// <param name="authService"> Object reference of the authentication services</param>
        /// <param name="consoleUI"> Object reference for the console UI layer</param>
        /// <param name="gameService">Object reference for the console game service layer</param>
        /// <param name="validator">Object reference for the validation layer</param>
        public AppController(AuthService authService, ConsoleUI consoleUI, GameService gameService, Validation validator)
        {
            this.authService = authService;
            this.consoleUI = consoleUI;
            this.gameService = gameService;
            this.validator = validator;
        }

        /// <summary>
        /// Method to run once the application starts
        /// </summary>
        public void Run()
        {
            while (true)
            {
                int userInput = consoleUI.GetUserInput();
                switch (userInput)
                {
                    case 1:
                        string userName, password;
                        RegistrationHandler(out userName, out password);
                        break;
                    case 2:
                        while (true)
                        {
                            userName = consoleUI.GetStringInput("User name");
                            password = consoleUI.GetStringInput("Password");
                            if (authService.Login(userName, password))
                            {
                                consoleUI.PrintSuccessMessage("Login Successful\n");
                                StartGame();
                            }
                            else
                            {
                                consoleUI.PrintErrorMessage("User name and password not found\n");
                            }
                        }

                        break;
                    case 3:
                        return;
                    default:
                        consoleUI.PrintErrorMessage("Invalid option selection... Try again");
                        break;
                }
            }
        }

        /// <summary>
        /// Method tha handles the registration
        /// </summary>
        /// <param name="userName"> Username of the user </param>
        /// <param name="password"> Password of the user </param>
        private void RegistrationHandler(out string userName, out string password)
        {
            while (true)
            {
                userName = consoleUI.GetStringInput("User name");
                if (validator.ValidateUserName(userName))
                {
                    password = consoleUI.GetStringInput("Password");
                    if (validator.ValidatePassword(password))
                    {
                        if (authService.Register(userName, password))
                        {
                            consoleUI.PrintSuccessMessage("Sign-up successful\n");
                            break;
                        }
                        else
                        {
                            consoleUI.PrintErrorMessage("Sign-up not successful\n");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that starts the game
        /// </summary>
        public void StartGame()
        {
            string[] board = { " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9" };
            consoleUI.PrintGameInfo("Select Game Mode");
            Mode mode = consoleUI.SelectGameCategory<Mode>();
            switch (mode)
            {
                case Mode.SinglePlayer:
                    Symbols symbol;
                    SinglePlayerMode(board);
                    break;
                case Mode.Multiplayer:
                    MultiPlayerMode(board);
                    break;
                case Mode.Logout:
                    consoleUI.PrintErrorMessage("Logout Successful");
                    Run();
                    break;
            }
        }

        /// <summary>
        /// Method handling the multiplayer mode of the game
        /// </summary>
        /// <param name="board"> Board of the tic tac game </param>
        private void MultiPlayerMode(string[] board)
        {
            while(true)
            {
                Symbols symbol = consoleUI.SelectSymbol();
                List<int> playerOneInputs = new List<int>();
                List<int> playerTwoInputs = new List<int>();
                consoleUI.PrintGameBoard(board);

                for (int i = 0; i < board.Length; i++)
                {
                    int userOneInput = consoleUI.GetGridInput();
                    if(validator.ValidateMove(userOneInput, board))
                    {
                        playerOneInputs.Add(userOneInput);
                        board = gameService.UpdateBoard(userOneInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool win = gameService.CheckWin(playerOneInputs);
                        WinHandler(win, "Player 1 Won");
                        int userTwoInput = consoleUI.GetGridInput();
                        if(validator.ValidateMove(userTwoInput, board))
                        {
                            playerTwoInputs.Add(userTwoInput);
                            board = gameService.UpdateBoard(userTwoInput, board, symbol == Symbols.X ? Symbols.O : Symbols.X);
                            consoleUI.PrintGameBoard(board);
                            win = gameService.CheckWin(playerOneInputs);
                            WinHandler(win, "Player 2 Won");
                            bool draw = gameService.CheckDraw(board);
                            DrawHandler(draw);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Method that handles the single player mode of the game
        /// </summary>
        /// <param name="board"> Board of the game </param>
        private void SinglePlayerMode(string[] board)
        {
            while (true)
            {
                Symbols symbol = consoleUI.SelectSymbol();
                List<int> playerInputs = new List<int>();
                consoleUI.PrintGameBoard(board);
                for (int i = 0; i < board.Length; i++)
                {
                    int userInput = consoleUI.GetGridInput();
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool win = gameService.CheckWin(playerInputs);
                        WinHandler(win, "Player Won");
                        var computerInputs = gameService.EasyGameLogic(userInput, playerInputs, board, symbol);
                        consoleUI.PrintGameInfo("Computer Thinking...");
                        Thread.Sleep(3000);
                        consoleUI.PrintGameBoard(board);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        win = gameService.CheckWin(computerInputs);
                        WinHandler(win, "Computer Won");
                        bool draw = gameService.CheckDraw(board);
                        DrawHandler(draw);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Method that checks for draw
        /// </summary>
        /// <param name="draw"> True if the match is draw </param>
        private void DrawHandler(bool draw)
        {
            if (draw)
            {
                consoleUI.PrintErrorMessage("MATCH IS DRAW");
                bool toContinue = consoleUI.GetInputToContinue();
                if (toContinue)
                {
                    string[] newBoard = { " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9" };
                    SinglePlayerMode(newBoard);
                }
                else
                {
                    StartGame();
                }
            }
        }

        /// <summary>
        /// Method that handles the win logic
        /// </summary>
        /// <param name="win"> True if the match is won </param>
        /// <param name="message"> Message to be displayed </param>
        private void WinHandler(bool win, string message)
        {
            if (win)
            {
                consoleUI.PrintSuccessMessage(message);
                bool toContinue = consoleUI.GetInputToContinue();
                if (toContinue)
                {
                    string[] newBoard = { " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9" };
                    SinglePlayerMode(newBoard);
                }
                else
                {
                    StartGame();
                }
            }
        }
    }
}
