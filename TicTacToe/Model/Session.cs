namespace TicTacToe.Model
{
    public class Session
    {
        public Session(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }
}
