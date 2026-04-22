using System.Text.Json;
using TicTacToe.Model;

namespace TicTacToe.Repository
{
    /// <summary>
    /// Repository class for handling json operations
    /// </summary>
    public class UserRepo
    {
        private string filePath;
        private List<User> users = new List<User>();

        /// <summary>
        /// Constructor to initialize the filepath and load all the users in a list
        /// </summary>
        /// <param name="filepath"></param>
        public UserRepo(string filepath)
        {
            filePath = filepath;
            users = LoadUser();
        }

        /// <summary>
        /// Method to add an user to a list
        /// </summary>
        /// <param name="user"> User object </param>
        public void AddUser(User user)
        {
            users.Add(user);
            SaveUser(users);
        }

        /// <summary>
        /// Method to load all the users
        /// </summary>
        /// <returns> The list of users</returns>
        public List<User> LoadUser()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            string jsonContent = File.ReadAllText(filePath);
            if(!string.IsNullOrWhiteSpace(jsonContent))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonContent) ?? new List<User>();
            }
            return users;
        }

        /// <summary>
        /// Method to save an user to the list
        /// </summary>
        /// <param name="users"> List of users </param>
        public void SaveUser(List<User> users)
        {
            string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Method to get all the users
        /// </summary>
        /// <returns> The list of users </returns>
        public IReadOnlyList<User> GetUsers()
        {
            return users;
        }
    }
}
