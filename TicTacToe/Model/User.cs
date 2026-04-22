namespace TicTacToe.Model
{
    /// <summary>
    /// Model of an user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Constructor for initializing the user object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        public User(Guid id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        /// <summary>
        /// Unique Id for each user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Password for the user
        /// </summary>
        public string Password { get; set; }
    }
}
