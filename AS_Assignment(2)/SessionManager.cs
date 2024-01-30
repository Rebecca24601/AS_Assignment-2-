namespace AS_Assignment_2_
{
    public class SessionManager
    {
        private readonly Dictionary<string, List<string>> userSessions = new Dictionary<string, List<string>>();

        public void AddSession(string userId, string sessionId)
        {
            if (!userSessions.ContainsKey(userId))
            {
                userSessions[userId] = new List<string>();
            }

            userSessions[userId].Add(sessionId);
        }

        public void RemoveSession(string userId, string sessionId)
        {
            if (userSessions.ContainsKey(userId))
            {
                userSessions[userId].Remove(sessionId);

                if (userSessions[userId].Count == 0)
                {
                    userSessions.Remove(userId);
                }
            }
        }

        public bool IsUserLoggedInFromAnotherDevice(string userId, string currentSessionId)
        {
            return userSessions.ContainsKey(userId) && userSessions[userId].Any(sessionId => sessionId != currentSessionId);
        }
    }
}
