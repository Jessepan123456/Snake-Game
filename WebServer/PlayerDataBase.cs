namespace WebServer
{
    public class PlayerDataBase
    {
        /// <summary>
        ///     Stores the PlayerId from Database
        /// </summary>
        public int PlayerId { get; set; }
        
        /// <summary>
        ///     Stores the PlayerName from Database
        /// </summary>
        public string? PlayerName { get; set; }
        
        /// <summary>
        ///     Stores the MaxScore from Database
        /// </summary>
        public int MaxScore { get; set; }
        
        /// <summary>
        ///     Stores the EnterTime from Database
        /// </summary>
        public string? EnterTime { get; set; }
        
        /// <summary>
        ///     Stores the EndTime from Database
        /// </summary>
        public string? EndTime { get; set; }

        /// <summary>
        ///     Default Constructor for Player DataBase
        /// </summary>
        public PlayerDataBase()
        {
            PlayerId = 0;
            PlayerName = "";
            MaxScore = 0;
            EnterTime = "";
            EndTime = "";
        
        }

        /// <summary>
        ///     Constructor for Player Database
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerName"></param>
        /// <param name="maxScore"></param>
        /// <param name="enterTime"></param>
        /// <param name="endTime"></param>
        public PlayerDataBase(int playerId, string playerName, int maxScore, string enterTime, string endTime )
        {
            PlayerId = playerId;
            PlayerName = playerName;
            MaxScore = maxScore;
            EnterTime = enterTime;
            EndTime = endTime;
        }
    
    }
}