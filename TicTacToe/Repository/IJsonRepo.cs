
namespace TicTacToe.Repository
{
    public interface IJsonRepo<T>
    {
        /// <summary>
        /// Method to add an user to a list
        /// </summary>
        /// <param name="user"> User object </param>
        void Add(T item);

        /// <summary>
        /// Method to load all the items
        /// </summary>
        /// <returns> The list of users</returns>
        IReadOnlyList<T> GetItems();

        /// <summary>
        /// Method to save an item to the list
        /// </summary>
        /// <param name="items"> List of items </param>
        List<T> Load();

        /// <summary>
        /// Method to get all the items
        /// </summary>
        /// <returns> The list of items </returns>
        void Save<T>(List<T> items);
    }
}