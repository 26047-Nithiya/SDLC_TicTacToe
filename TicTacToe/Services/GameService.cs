using System.Text;
using TicTacToe.Model;

namespace TicTacToe.Services
{
    /// <summary>
    /// Class for handling all the game logics
    /// </summary>
    public class GameService
    {
        /// <summary>
        /// Method that contains the easy logic of the game
        /// </summary>
        /// <param name="gridInput"> Input to be mapped in the grid </param>
        /// <param name="playerInputs"> List of previous inputs from the player </param>
        /// <param name="board"> List of strings in the board </param>
        /// <param name="symbol"> Symbol to be written in the board </param>
        /// <returns> The list of computer inputs </returns>
        public List<int> EasyGameLogic(int gridInput, List<int> playerInputs, string[] board, Symbols symbol)
        {
            List<int> comInputs = new List<int>();
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
                if (int.TryParse(pattern, out int gridNum))
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
    }
}
