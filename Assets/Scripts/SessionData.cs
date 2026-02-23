// Scripts/Data/SessionData.cs
using System.Collections.Generic;

namespace ARSurvivalShooter
{
    [System.Serializable]
    public class SessionData
    {
        public int score;
        public int enemiesKilled;
        public float timeSurvived;
        public string date;
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public List<SessionData> sessions = new List<SessionData>();
    }
}