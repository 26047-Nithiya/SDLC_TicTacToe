using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class GameDTO
    {
        public GameDTO(Mode gameMode, Hardness gameDifficulty, DateTime timeStamp, Result gameResult, int score)
        {
            this.GameMode = gameMode;
            this.GameDifficulty = gameDifficulty;
            this.TimeStamp = timeStamp;
            this.GameResult = gameResult;
            this.Score = score;
        }

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
        /// Result of the game played
        /// </summary>
        public Result GameResult { get; set; }

        /// <summary>
        /// Score of the game
        /// </summary>
        public int Score { get; set; }
    }
}
