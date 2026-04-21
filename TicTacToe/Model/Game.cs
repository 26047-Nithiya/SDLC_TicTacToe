namespace TicTacToe.Model
{
    /// <summary>
    /// Model game class
    /// </summary>
    public class Game
    {
        public Game()
        {
            Moves = new Dictionary<int, Symbols>();
        }

        public Game(Guid userId, Mode gameMode, Hardness gameDifficulty, DateTime timeStamp, Result gameResult, int score, Dictionary<int, Symbols> moves)
        {
            this.UserId = userId;
            this.GameMode = gameMode;
            this.GameDifficulty = gameDifficulty;
            this.TimeStamp = timeStamp;
            this.GameResult = gameResult;
            this.Score = score;
            this.Moves = moves;
        }

        public Game(Mode gameMode, Hardness gameDifficulty, DateTime timeStamp, Result gameResult, int score, Dictionary<int, Symbols> moves)
        {
            this.GameMode = gameMode;
            this.GameDifficulty = gameDifficulty;
            this.TimeStamp = timeStamp;
            this.GameResult = gameResult;
            this.Score = score;
            this.Moves = moves;
        }

        /// <summary>
        /// Id of the user playing the game
        /// </summary>
        public Guid UserId { get; set; }

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

        public Result GameResult {  get; set; }

        /// <summary>
        /// Score of the game
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Moves that are made in a game
        /// </summary>
        public Dictionary<int, Symbols> Moves { get; set; }
    }
}
