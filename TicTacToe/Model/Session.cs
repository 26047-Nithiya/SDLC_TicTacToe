namespace TicTacToe.Model
{
    public class Session
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class with the specified user details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The name of the user associated with the session.</param>
        public Session(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the user for this session.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the username associated with this session.
        /// </summary>
        public string UserName { get; set; }
    }
}
