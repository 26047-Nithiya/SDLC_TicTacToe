using System.Security.Cryptography;
using System.Text;
using TicTacToe.Model;
using TicTacToe.Repository;

namespace TicTacToe.Services
{
    /// <summary>
    /// Class for handling all the game logics
    /// </summary>
    public class GameService : IGameService
    {
        const string HUMAN = "❌";
        const string AI = "🔵";
        private IJsonRepo<Game> gameRepo;

        /// <summary>
        /// Constructor for instantiating the repository layer
        /// </summary>
        /// <param name="gameRepo"> Repository for games </param>
        public GameService(IJsonRepo<Game> gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        /// <summary>
        /// Method that contains the easy logic of the game
        /// </summary>
        /// <param name="gridInput"> Input to be mapped in the grid </param>
        /// <param name="playerInputs"> List of previous inputs from the player </param>
        /// <param name="board"> List of strings in the board </param>
        /// <param name="symbol"> Symbol to be written in the board </param>
        /// <returns> The list of computer inputs </returns>
        public List<int> EasyGameLogic(Game game, List<int> playerInputs, List<int> comInputs, string[] board, Symbols symbol)
        {
            Random random = new Random();
            int computerInput = 0;

            while (true)
            {
                computerInput = random.Next(1, 10);
                if (!comInputs.Contains(computerInput) && !playerInputs.Contains(computerInput))
                {
                    comInputs.Add(computerInput);
                    board = UpdateBoard(computerInput, board, symbol == Symbols.X ? Symbols.O : Symbols.X);
                    UpdateMoves(game, computerInput, symbol == Symbols.X ? Symbols.O : Symbols.X);
                    return comInputs;
                }
            }
        }

        /// <summary>
        /// Method to check if any moves left
        /// </summary>
        /// <param name="board"> Board of the tic tac toe </param>
        /// <returns> True if any moves left </returns>
        public bool IsMovesLeft(string[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != AI && board[i] != HUMAN)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Method to evaluate the winning patterns
        /// </summary>
        /// <param name="board"> Board of the tic tac toe </param>
        /// <returns> The score from the winning pattern </returns>
        public int Evaluate(string[] board)
        {
            int[,] winPatterns = {
                {0,1,2},{3,4,5},{6,7,8},
                {0,3,6},{1,4,7},{2,5,8},
                {0,4,8},{2,4,6}
            };

            for (int i = 0; i < 8; i++)
            {
                int a = winPatterns[i, 0];
                int b = winPatterns[i, 1];
                int c = winPatterns[i, 2];

                if (board[a] == board[b] && board[b] == board[c])
                {
                    if (board[a] == AI) return 10;
                    if (board[a] == HUMAN) return -10;
                }
            }
            return 0;
        }

        /// <summary> Evaluates the game board using the Minimax algorithm to determine the optimal move score.</summary>
        /// <param name="board">The current state of the game board represented as a string array.</param>
        /// <param name="depth">The current depth of recursion in the Minimax search tree.</param>
        /// <param name="isMax"> A boolean indicating whether the current move is for the computer (true) or the minimizing player (false).</param>
        /// <returns> The best score calculated recursively </returns>
        public int Minimax(string[] board, int depth, bool isMax)
        {
            int score = Evaluate(board);

            if (score == 10) return score - depth;
            if (score == -10) return score + depth;
            if (!IsMovesLeft(board)) return 0;

            if (isMax)
            {
                int best = int.MinValue;

                for (int i = 0; i < 9; i++)
                {
                    if (board[i] != AI && board[i] != HUMAN)
                    {
                        string temp = board[i];
                        board[i] = AI;

                        best = Math.Max(best, Minimax(board, depth + 1, false));

                        board[i] = temp;
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;

                for (int i = 0; i < 9; i++)
                {
                    if (board[i] != AI && board[i] != HUMAN)
                    {
                        string temp = board[i];
                        board[i] = HUMAN;

                        best = Math.Min(best, Minimax(board, depth + 1, true));

                        board[i] = temp;
                    }
                }
                return best;
            }
        }

        /// <summary>
        /// Method to get the best possible move 
        /// </summary>
        /// <param name="game"> Current playing game </param>
        /// <param name="board"> Tic tac toe game </param>
        /// <param name="symbol"> Symbol to be inserted </param>
        /// <returns> The best move </returns>
        public int HardGameLogic(Game game, string[] board, Symbols symbol)
        {
            int bestVal = int.MinValue;
            int bestMove = -1;

            for (int i = 0; i < 9; i++)
            {
                if (board[i] != AI && board[i] != HUMAN)
                {
                    string temp = board[i];
                    board[i] = AI;

                    int moveVal = Minimax(board, 0, false);

                    board[i] = temp;

                    if (moveVal > bestVal)
                    {
                        bestMove = i;
                        bestVal = moveVal;
                    }
                }
            }

            board = UpdateBoard(bestMove + 1, board, symbol == Symbols.X ? Symbols.O : Symbols.X);
            UpdateMoves(game, bestMove + 1, symbol == Symbols.X ? Symbols.O : Symbols.X);
            return bestMove + 1;
        }

        /// <summary>
        /// Method to check if the game is won
        /// </summary>
        /// <param name="inputs"> List of inputs </param>
        /// <returns> True if the game is won </returns>
        public bool CheckWin(List<int> inputs)
        {
            int[][] winPatterns = new int[][]
            {
                new[] {1, 2, 3 }, new[] {4, 5, 6 }, new[] {7, 8, 9 },
                new[] {1, 4, 7}, new[] {2, 5, 8}, new[] {3, 6, 9},
                new[] {1, 5, 9}, new[] {3, 5, 7}
            };

            for (int i = 0; i < winPatterns.Length; i++)
            {
                int count = 0;
                foreach (int pattern in winPatterns[i])
                {
                    if (inputs.Contains(pattern))
                    {
                        ++count;
                    }

                    if (count == 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Method to check if the match is draw
        /// </summary>
        /// <param name="board"> Board of the game </param>
        /// <returns> True if the match is draw </returns>
        public bool CheckDraw(string[] board)
        {
            foreach (string pattern in board)
            {
                if (int.TryParse(pattern.Trim(), out int gridNum))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to update the board with the symbol selected
        /// </summary>
        /// <param name="gridInput"> Input of the user </param>
        /// <param name="board"> List of strings in the board </param>
        /// <param name="symbol"> Symbol to be mapped </param>
        /// <returns> Updated list of strings in the board </returns>
        public string[] UpdateBoard(int gridInput, string[] board, Symbols symbol)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (symbol == Symbols.X)
            {
                board[gridInput - 1] = "❌";
            }
            else if (symbol == Symbols.O)
            {
                board[gridInput - 1] = "🔵";
            }
            return board;
        }

        public string[] GetGameBoard()
        {
            string[] board = { " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9" };
            return board;
        }
        
        /// <summary>
        /// Method to update the game info
        /// </summary>
        /// <typeparam name="T"> Generic type </typeparam>
        /// <param name="game"> Current game </param>
        /// <param name="info"> Info to be set </param>
        public void UpdateGameInfo<T>(Game game, T info)
        {
            if (info is Guid)
            {
                game.UserId = (Guid)Convert.ChangeType(info, typeof(T));
            }
            else if (info is Mode)
            {
                game.GameMode = (Mode)Convert.ChangeType(info, typeof(T));
            }
            else if (info is Hardness)
            {
                game.GameDifficulty = (Hardness)Convert.ChangeType(info, typeof(T)); ;
            }
            else if (info is Result)
            {
                game.TimeStamp = DateTime.Now;
                game.GameResult = (Result)Convert.ChangeType(info, typeof(T));
                if (game.GameResult == Result.Win)
                {
                    game.Score = 10;
                }
                else if (game.GameResult == Result.Draw)
                {
                    game.Score = 5;
                }
                else if (game.GameResult == Result.Loss)
                {
                    game.Score = 0;
                }
            }
        }
        
        /// <summary>
        /// Method to update the moves of the game
        /// </summary>
        /// <param name="game"> The current game </param>
        /// <param name="move"> Move of the game </param>
        /// <param name="symbol"> The symbol to be inserted </param>
        public void UpdateMoves(Game game, int move, Symbols symbol)
        {
            game.Moves[move] = symbol;
        }

        /// <summary>
        /// Method to save the game info
        /// </summary>
        /// <param name="game"> Current game </param>
        public void SaveGameInfo(Game game)
        {
            gameRepo.Add(game);
        }

        /// <summary>
        /// Method to get the game history
        /// </summary>
        /// <param name="id"> Id of the user </param>
        /// <returns> The list of games </returns>
        public IEnumerable<Game> GetGameHistory(Guid id)
        {
            var games = gameRepo.GetItems();
            var filteredGames = games.Where(x => x.UserId == id);
            return filteredGames;
        }
    }
}
