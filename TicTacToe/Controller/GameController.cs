using TicTacToe.Model;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe.Controller
{
    public class GameController
    {
        private Session session;
        private Game game;
        private ConsoleUI consoleUI;
        private IGameService gameService;
        private Validation<User> validator;
        private AuthController controller;

        /// <summary>
        /// Controller to instantiate the references for object references
        /// </summary>
        /// <param name="session"> Object references for the session </param>
        /// <param name="game"> Object references for the game </param>
        /// <param name="consoleUI"> Object references for the console layer </param>
        /// <param name="gameService"> Object references for the game service layer </param>
        /// <param name="validator"> Object reference for the validation layer </param>
        /// <param name="controller"> Object reference for the authentication controller </param>
        public GameController(Session session, Game game, ConsoleUI consoleUI, IGameService gameService, Validation<User> validator, AuthController controller)
        {
            this.session = session;
            this.game = game;
            this.consoleUI = consoleUI;
            this.gameService = gameService;
            this.validator = validator;
            this.controller = controller;
        }

        /// <summary>
        /// Method to select the menu
        /// </summary>
        /// <param name="userId"> Id of the current user </param>
        public void SelectMenuOption(Guid userId)
        {
            while (true)
            {
                gameService.UpdateGameInfo(game, userId);
                Menu menuOption = consoleUI.SelectGameCategory<Menu>(session.UserName);
                switch (menuOption)
                {
                    case Menu.PlayGame:
                        StartGame();
                        break;
                    case Menu.ShowHistory:
                        var games = gameService.GetGameHistory(userId);
                        consoleUI.ShowGameHistory(games);
                        break;
                    case Menu.ReplayGame:
                        ReplayGame(userId);
                        break;
                    case Menu.Logout:
                        consoleUI.PrintErrorMessage("Log out successful");
                        Program.GameStarter(gameService, consoleUI, validator, controller);
                        break;
                    default:
                        consoleUI.PrintErrorMessage("Enter a valid option");
                        consoleUI.ReadKeyPressToContinue();
                        break;
                }
            }
        }

        /// <summary>
        /// Method that starts the game
        /// </summary>
        public void StartGame()
        {
            var board = gameService.GetGameBoard();
            consoleUI.PrintGameInfo("Select Game Mode");
            Mode mode = consoleUI.SelectGameCategory<Mode>(session.UserName);
            switch (mode)
            {
                case Mode.SinglePlayer:
                    gameService.UpdateGameInfo(game, mode);
                    SinglePlayerMode(board);
                    break;
                case Mode.Multiplayer:
                    gameService.UpdateGameInfo(game, mode);
                    int playerOneScore = 0;
                    int playerTwoScore = 0;
                    MultiPlayerMode(board, playerOneScore, playerTwoScore);
                    break;
                case Mode.MainMenu:
                    SelectMenuOption(game.UserId);
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
            Hardness difficulty = consoleUI.SelectGameCategory<Hardness>(session.UserName);
            gameService.UpdateGameInfo(game, difficulty);
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

        /// <summary>
        /// Method to replay the game
        /// </summary>
        /// <param name="userId"> Id of the user </param>
        /// <returns></returns>
        private void ReplayGame(Guid userId)
        {
            IEnumerable<Game> games = gameService.GetGameHistory(userId);
            consoleUI.ShowGameHistory(games);
            int gameId = consoleUI.SelectGameToReplay(games);

            if(gameId <= games.Count())
            {
                var board = gameService.GetGameBoard();
                var gameToBeReplayed = games.ToList()[gameId - 1];
                consoleUI.PrintGameBoard(board);
                foreach (var moves in gameToBeReplayed.Moves)
                {
                    consoleUI.PrintGameInfo("REPLAYING GAME");
                    board = gameService.UpdateBoard(moves.Key, board, (Symbols)moves.Value);
                    Thread.Sleep(2000);
                    consoleUI.PrintGameBoard(board);
                }
                if (gameToBeReplayed.GameResult == Result.Win)
                {
                    consoleUI.PrintSuccessMessage("PLAYER WON");
                    consoleUI.ReadKeyPressToContinue();
                }
                else if (gameToBeReplayed.GameResult == Result.Loss)
                {
                    consoleUI.PrintSuccessMessage("PLAYER LOST");
                    consoleUI.ReadKeyPressToContinue();
                }
                else
                {
                    consoleUI.PrintSuccessMessage("MATCH DRAW");
                    consoleUI.ReadKeyPressToContinue();
                }
            }
            else
            {
                consoleUI.PrintErrorMessage("Game id does not exist");
                consoleUI.ReadKeyPressToContinue();
                return;
            }
        }

        /// <summary>
        /// Method to handle the hard mode game
        /// </summary>
        /// <param name="board"> Game board of tic tac toe</param>
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
                    if(userInput == -1)
                    {
                        return;
                    }
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        gameService.UpdateMoves(game, userInput, symbol);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => HardMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        bool isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isWin, "Match is Draw", () => HardMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Method to process the hard moves 
        /// </summary>
        /// <param name="board"> Board of tic tac toe </param>
        /// <param name="computerInputs"> Inputs by the computer algorithm </param>
        /// <param name="symbol"> Symbol to be inserted </param>
        /// <returns> The list of computer inputs </returns>
        private List<int> ProcessHardComputerMove(string[] board, List<int> computerInputs, Symbols symbol)
        {
            int bestMove = gameService.HardGameLogic(game, board, symbol);
            computerInputs.Add(bestMove); consoleUI.PrintGameInfo("Computer Thinking...");
            Thread.Sleep(3000);
            consoleUI.PrintGameBoard(board);
            bool isWin = gameService.CheckWin(computerInputs);
            WinOrDrawHandler(Result.Loss, isWin, "Computer Won", () => HardMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            bool isDraw = gameService.CheckDraw(board);
            WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => HardMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            return computerInputs;
        }

        /// <summary>
        /// Method that handles the easy mode
        /// </summary>
        /// <param name="board"> Board of tic tac toe </param>
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
                    if (userInput == -1)
                    {
                        return;
                    }
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        gameService.UpdateMoves(game, userInput, symbol);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        computerInputs = ProcessEasyComputerLogic(board, symbol, playerInputs, computerInputs);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Method to handle the easy computer logic
        /// </summary>
        /// <param name="board"> Board of tic tac toe </param>
        /// <param name="symbol"> Symbol to be inserted </param>
        /// <param name="playerInputs"> Inputs by the user </param>
        /// <param name="computerInputs"> Inputs by the computer </param>
        /// <returns>The list of easy logic inputs </returns>
        private List<int> ProcessEasyComputerLogic(string[] board, Symbols symbol, List<int> playerInputs, List<int> computerInputs)
        {
            computerInputs = gameService.EasyGameLogic(game, playerInputs, computerInputs, board, symbol);
            consoleUI.PrintGameInfo("Computer Thinking...");
            Thread.Sleep(3000);
            consoleUI.PrintGameBoard(board);
            bool isWin = gameService.CheckWin(computerInputs);
            WinOrDrawHandler(Result.Loss, isWin, "Computer Won", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            bool isDraw = gameService.CheckDraw(board);
            WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => EasyMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
            return computerInputs;
        }

        /// <summary>
        /// Method that handles the medium mode
        /// </summary>
        /// <param name="board"> Board of tic tac toe </param>
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
                    if (userInput == -1)
                    {
                        return;
                    }
                    bool isValid = validator.ValidateMove(userInput, board);
                    if (isValid)
                    {
                        playerInputs.Add(userInput);
                        gameService.UpdateMoves(game, userInput, symbol);
                        board = gameService.UpdateBoard(userInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        bool isWin = gameService.CheckWin(playerInputs);
                        WinOrDrawHandler(Result.Win, isWin, "Player Won", () => MediumMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
                        bool isDraw = gameService.CheckDraw(board);
                        WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => MediumMode(gameService.GetGameBoard()), () => SinglePlayerMode(gameService.GetGameBoard()));
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
        private void MultiPlayerMode(string[] board, int playerOneScore, int playerTwoScore)
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
                    if (userOneInput == -1)
                    {
                        return;
                    }
                    if (validator.ValidateMove(userOneInput, board))
                    {
                        playerOneInputs.Add(userOneInput);
                        gameService.UpdateMoves(game, userOneInput, symbol);
                        board = gameService.UpdateBoard(userOneInput, board, symbol);
                        consoleUI.PrintGameBoard(board);
                        consoleUI.PrintMultiPlayerScores(playerOneScore, playerTwoScore);
                        bool isWin = gameService.CheckWin(playerOneInputs);
                        if(isWin)
                        {
                            playerOneScore += 10;
                            playerTwoScore += 0;
                        }
                        WinOrDrawHandler(Result.Win, isWin, "Player 1 Won", () => MultiPlayerMode(gameService.GetGameBoard(), playerOneScore, playerTwoScore), StartGame);
                        bool isDraw = gameService.CheckDraw(board);
                        if (isDraw)
                        {
                            playerOneScore += 5;
                            playerTwoScore += 5;
                        }
                        WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard(), playerOneScore, playerTwoScore), StartGame);
                        int userTwoInput = consoleUI.GetGridInput();
                        if (userTwoInput == -1)
                        {
                            return;
                        }
                        if (validator.ValidateMove(userTwoInput, board))
                        {
                            playerTwoInputs.Add(userTwoInput);
                            gameService.UpdateMoves(game, userOneInput, symbol);
                            board = gameService.UpdateBoard(userTwoInput, board, symbol == Symbols.X ? Symbols.O : Symbols.X);
                            consoleUI.PrintGameBoard(board);
                            consoleUI.PrintMultiPlayerScores(playerOneScore, playerTwoScore);
                            isWin = gameService.CheckWin(playerTwoInputs);
                            WinOrDrawHandler(Result.Loss, isWin, "Player 2 Won", () => MultiPlayerMode(gameService.GetGameBoard(), playerOneScore, playerTwoScore), StartGame);
                            isDraw = gameService.CheckDraw(board);
                            if (isWin)
                            {
                                playerOneScore += 0;
                                playerTwoScore += 10;
                            }
                            if (isDraw)
                            {
                                playerOneScore += 5;
                                playerTwoScore += 5;
                            }
                            WinOrDrawHandler(Result.Draw, isDraw, "Match is Draw", () => MultiPlayerMode(gameService.GetGameBoard(), playerOneScore, playerTwoScore), StartGame);
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
                gameService.UpdateGameInfo(game, result);
                gameService.SaveGameInfo(game);
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