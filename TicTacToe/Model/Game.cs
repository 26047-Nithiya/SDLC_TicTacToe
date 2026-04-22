namespace TicTacToe.Model
{
    /// <summary>
    /// Model game class
    /// </summary>
    public class Game
    {
        public Game(string userId, Mode gameMode, Hardness gameDifficulty, DateTime timeStamp, int score, string moves)
        {
            UserId = userId;
            GameMode = gameMode;
            GameDifficulty = gameDifficulty;
            TimeStamp = timeStamp;
            Score = score;
            Moves = moves;
        }

        /// <summary>
        /// Id of the user playing the game
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Mode of the game
        /// </summary>
        public Mode GameMode { get; set; }

        /// <summary>
        /// Difficulty of the game
        /// </summary>
        public Hardness GameDifficulty { get; set; }

        /// <summary>
        /// Timestamp of the game played
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Score of the game
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Moves that are made in a game
        /// </summary>
        public string Moves { get; set; }


    }
}
