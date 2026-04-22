using System.Security.Cryptography;
using System.Text;
using TicTacToe.Model;

namespace TicTacToe.Services
{
    /// <summary>
    /// Class for handling all the game logics
    /// </summary>
    public class GameService
    {
        const string HUMAN = "❌";
        const string AI = "🔵";

        /// <summary>
        /// Method that contains the easy logic of the game
        /// </summary>
        /// <param name="gridInput"> Input to be mapped in the grid </param>
        /// <param name="playerInputs"> List of previous inputs from the player </param>
        /// <param name="board"> List of strings in the board </param>
        /// <param name="symbol"> Symbol to be written in the board </param>
        /// <returns> The list of computer inputs </returns>
        public List<int> EasyGameLogic(List<int> playerInputs, List<int> comInputs, string[] board, Symbols symbol)
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
                    return comInputs;
                }
            }
        }

        static bool IsMovesLeft(string[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != AI && board[i] != HUMAN)
                    return true;
            }
            return false;
        }

        static int Evaluate(string[] board)
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

        static int Minimax(string[] board, int depth, bool isMax)
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

        public int HardGameLogic(string[] board, Symbols symbol)
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

            for(int i = 0; i < winPatterns.Length; i++)
            {
                int count = 0;
                foreach(int pattern in winPatterns[i])
                {
                    if (inputs.Contains(pattern))
                    {
                        ++count;
                    }

                    if(count == 3)
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
            if(symbol == Symbols.X)
            {
                board[gridInput - 1] = "❌";
            }
            else if(symbol == Symbols.O)
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
    }
}
