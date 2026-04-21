using System.Linq;
using System.Text.Json;
using TicTacToe.Model;

namespace TicTacToe.Repository
{
    /// <summary>
    /// Repository class for handling json operations
    /// </summary>
    public class JsonRepo<T> : IJsonRepo<T>
    {
        private string filePath;
        private List<T> items = new List<T>();

        /// <summary>
        /// Constructor to initialize the file path and load all the users in a list
        /// </summary>
        /// <param name="filepath"></param>
        public JsonRepo(string filepath)
        {
            filePath = filepath;
            items = Load();
        }

        /// <summary>
        /// Method to add an user to a list
        /// </summary>
        /// <param name="user"> User object </param>
        public void Add(T item)
        {
            items.Add(item);
            Save<T>(items);
        }

        /// <summary>
        /// Method to load all the items
        /// </summary>
        /// <returns> The list of users</returns>
        public List<T> Load()
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    return JsonSerializer.Deserialize<List<T>>(jsonContent) ?? new List<T>();
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (JsonException ex)
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Method to save an item to the list
        /// </summary>
        /// <param name="items"> List of items </param>
        public void Save<T>(List<T> items)
        {
            string jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Method to get all the items
        /// </summary>
        /// <returns> The list of items </returns>
        public IReadOnlyList<T> GetItems()
        {
            return items;
        }
    }
}
