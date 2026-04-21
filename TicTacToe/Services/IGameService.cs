using TicTacToe.Model;

namespace TicTacToe.Services
{
    public interface IGameService
    {
        bool CheckDraw(string[] board);
        bool CheckWin(List<int> inputs);
        List<int> EasyGameLogic(Game game, List<int> playerInputs, List<int> comInputs, string[] board, Symbols symbol);
        string[] GetGameBoard();
        IEnumerable<Game> GetGameHistory(Guid id);
        int HardGameLogic(Game game, string[] board, Symbols symbol);
        void SaveGameInfo(Game game);
        string[] UpdateBoard(int gridInput, string[] board, Symbols symbol);
        void UpdateGameInfo<T>(Game game, T info);
        void UpdateMoves(Game game, int move, Symbols symbol);
    }
}