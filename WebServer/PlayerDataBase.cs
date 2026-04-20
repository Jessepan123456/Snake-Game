namespace WebServerDemo
{
    public class PlayerDataBase
    {
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public int MaxScore { get; set; }
        public string? EnterTime { get; set; }
        public string? EndTime { get; set; }

        public PlayerDataBase()
        {
            PlayerId = 0;
            PlayerName = "";
            MaxScore = 0;
            EnterTime = "";
            EndTime = "";
        
        }

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