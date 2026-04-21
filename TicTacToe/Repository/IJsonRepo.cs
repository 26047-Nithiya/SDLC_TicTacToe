
namespace TicTacToe.Repository
{
    public interface IJsonRepo<T>
    {
        void Add(T item);
        IReadOnlyList<T> GetItems();
        List<T> Load();
        void Save<T>(List<T> items);
    }
}