using TicTacToe.Model;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe.Controller
{
    public class GameController
    {
        private ConsoleUI consoleUI;
        private GameService gameService;
        private Validation validator;

        public GameController(ConsoleUI consoleUI, GameService gameService, Validation validator)
        {
            this.consoleUI = consoleUI;
            this.gameService = gameService;
            this.validator = validator;
        }

        /// <summary>
        /// Method that starts the game
        /// </summary>
        public void StartGame()
        {
            var board = gameService.GetGameBoard();
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
                    break;
            }
        }

        /// <summary>
        /// Method that handles the single player mode of the game
        /// </summary>
        /// <param name="board"> Board of the game </param>
        private void SinglePlayerMode(string[] board)
        {
            consoleUI.PrintGameInfo("\nSelect game difficulty");
            Hardness difficulty = consoleUI.SelectGameCategory<Hardness>();
            switch (difficulty)
            {
                case Hardness.Easy:
                    EasyMode(board);
                    break;
                case Hardness.Medium:
                    MediumMode(board);
                    break;
                case Hardness.Impossible:
                    HardMode(board);
                    break;
                case Hardness.MainMenu:
                    StartGame();
                    return;
            }
        }

        public void HardMode(string[] board)
        {
            List<int> computerInputs = new List<int>();
            List<int> playerInputs = new List<int>();
            Symbols symbol = consoleUI.SelectSymbol();
            while (true)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    consoleUI.PrintGameBoard(board);
                    bool isWin;
                    computerInputs = ProcessHardComputerMove(board, computerInputs, symbol);
                    int userInput = consoleUI.GetGridInput();
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => MultiPlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        bool isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        private List<int> ProcessHardComputerMove(string[] board, List<int> computerInputs, Symbols symbol)
        {
            int bestMove = gameService.HardGameLogic(board, symbol);
            computerInputs.Add(bestMove); consoleUI.PrintGameInfo("Computer Thinking...");
            Thread.Sleep(3000);
            consoleUI.PrintGameBoard(board);
            bool isWin = gameService.CheckWin(computerInputs);
            WinOrDrawHandler(Result.Loss, isWin, "Computer Won", () => HardMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            bool isDraw = gameService.CheckDraw(board);
            WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            return computerInputs;
        }

        private void EasyMode(string[] board)
        {
            while (true)
            {
                Symbols symbol = consoleUI.SelectSymbol();
                List<int> playerInputs = new List<int>();
                List<int> computerInputs = new List<int>();
                consoleUI.PrintGameBoard(board);
                for (int i = 0; i < board.Length; i++)
                {
                    consoleUI.PrintGameBoard(board);
                    bool isDraw = gameService.CheckDraw(board);
                    WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                    int userInput = consoleUI.GetGridInput();
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => SinglePlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        computerInputs = ProcessEasyComputerLogic(board, symbol, playerInputs, computerInputs);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        private List<int> ProcessEasyComputerLogic(string[] board, Symbols symbol, List<int> playerInputs, List<int> computerInputs)
        {
            computerInputs = gameService.EasyGameLogic(playerInputs, computerInputs, board, symbol);
            consoleUI.PrintGameInfo("Computer Thinking...");
            Thread.Sleep(3000);
            consoleUI.PrintGameBoard(board);
            bool isWin = gameService.CheckWin(computerInputs);
            WinOrDrawHandler(Result.Loss, isWin, "Computer Won", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            bool isDraw = gameService.CheckDraw(board);
            WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => SinglePlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            return computerInputs;
        }

        public void MediumMode(string[] board)
        {
            List<int> computerInputs = new List<int>();
            List<int> playerInputs = new List<int>();
            Symbols symbol = consoleUI.SelectSymbol();
            while (true)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    consoleUI.PrintGameBoard(board);
                    if(computerInputs.Count < 3)
                    {
                        computerInputs = ProcessHardComputerMove(board, computerInputs, symbol);
                    }
                    else
                    {
                        computerInputs = ProcessEasyComputerLogic(board, symbol, playerInputs, computerInputs);
                    }
                    int userInput = consoleUI.GetGridInput();
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => MultiPlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        bool isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Method handling the multiplayer mode of the game
        /// </summary>
        /// <param name="board"> Board with numberings </param>
        private void MultiPlayerMode(string[] board)
        {
            while (true)
            {
                Symbols symbol = consoleUI.SelectSymbol();
                List<int> playerOneInputs = new List<int>();
                List<int> playerTwoInputs = new List<int>();
                consoleUI.PrintGameBoard(board);

                for (int i = 0; i < board.Length; i++)
                {
                    int userOneInput = consoleUI.GetGridInput();
                    if (validator.ValidateMove(userOneInput, board))
                    {
                        playerOneInputs.Add(userOneInput);
                        board = gameService.UpdateBoard(userOneInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool isWin = gameService.CheckWin(playerOneInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player 1 Won", () => MultiPlayerMode(gameService.GetGameBoard()), StartGame);
                        bool draw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard()), StartGame);
                        int userTwoInput = consoleUI.GetGridInput();
                        if (validator.ValidateMove(userTwoInput, board))
                        {
                            playerTwoInputs.Add(userTwoInput);
                            board = gameService.UpdateBoard(userTwoInput, board, symbol == Symbols.X ? Symbols.O : Symbols.X);
                            consoleUI.PrintGameBoard(board);
                            isWin = gameService.CheckWin(playerOneInputs);
                            WinOrDrawHandler(Result.Loss, isWin, "Player 2 Won", () => MultiPlayerMode(gameService.GetGameBoard()), StartGame);
                            draw = gameService.CheckDraw(board);
                            WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard()), StartGame);
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
        /// Method that handles the win logic
        /// </summary>
        /// <param name="message"> Message to be displayed </param>
        private void WinOrDrawHandler(Result result, bool isWinOrDraw, string message, Action restartSameMode, Action previousMode)
        {
            if (isWinOrDraw)
            {
                consoleUI.PrintSuccessMessage(message);
                bool toContinue = consoleUI.GetInputToContinue();
                if (toContinue)
                {
                    restartSameMode();
                }
                else
                {
                    previousMode();
                }
            }
        }
    }
}