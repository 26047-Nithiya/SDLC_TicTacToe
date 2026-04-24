using TicTacToe.Model;

namespace TicTacToe.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Checks whether the game has ended in a draw based on the current board state.
        /// </summary>
        /// <param name="board">The current game board.</param>
        /// <returns>True if the game is a draw; otherwise, false.</returns>
        bool CheckDraw(string[] board);

        /// <summary>
        /// Determines whether the provided list of moves contains a winning combination.
        /// </summary>
        /// <param name="inputs">List of positions selected by a player.</param>
        /// <returns>True if a winning condition is met; otherwise, false.</returns>
        bool CheckWin(List<int> inputs);

        /// <summary>
        /// Executes the easy difficulty game logic for the computer player and returns the updated move sequence.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        /// <param name="playerInputs">List of positions chosen by the player.</param>
        /// <param name="comInputs">List of positions chosen by the computer.</param>
        /// <param name="board">The current game board.</param>
        /// <param name="symbol">The symbol assigned to the computer player.</param>
        /// <returns>A list of updated computer move positions.</returns>
        List<int> EasyGameLogic(Game game, List<int> playerInputs, List<int> comInputs, string[] board, Symbols symbol);

        /// <summary>
        /// Retrieves the initial or current game board state.
        /// </summary>
        /// <returns>A string array representing the game board.</returns>
        string[] GetGameBoard();

        /// <summary>
        /// Retrieves the game history for a specific player or game session.
        /// </summary>
        /// <param name="id">The unique identifier of the game or player.</param>
        /// <returns>A collection of previous game records.</returns>
        IEnumerable<Game> GetGameHistory(Guid id);

        /// <summary>
        /// Executes the hard difficulty game logic using a strategic algorithm (e.g., Minimax) to determine the best move.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        /// <param name="board">The current game board.</param>
        /// <param name="symbol">The symbol assigned to the computer player.</param>
        /// <returns>The calculated best move position.</returns>
        int HardGameLogic(Game game, string[] board, Symbols symbol);

        /// <summary>
        /// Saves the provided game information to persistent storage.
        /// </summary>
        /// <param name="game">The game object containing all relevant game data.</param>
        void SaveGameInfo(Game game);

        /// <summary>
        /// Updates the game board by placing a symbol at the specified grid position.
        /// </summary>
        /// <param name="gridInput">The selected grid position.</param>
        /// <param name="board">The current game board.</param>
        /// <param name="symbol">The symbol to place on the board.</param>
        /// <returns>The updated game board.</returns>
        string[] UpdateBoard(int gridInput, string[] board, Symbols symbol);

        /// <summary>
        /// Updates specific game-related information with the provided data.
        /// </summary>
        /// <typeparam name="T">The type of information being updated.</typeparam>
        /// <param name="game">The game instance to update.</param>
        /// <param name="info">The updated information.</param>
        void UpdateGameInfo<T>(Game game, T info);

        /// <summary>
        /// Records a move made during the game for the specified player symbol.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        /// <param name="move">The move position made by the player.</param>
        /// <param name="symbol">The symbol representing the player who made the move.</param>
        void UpdateMoves(Game game, int move, Symbols symbol);
    }
}