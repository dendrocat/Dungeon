namespace DomainLogging
{
    using Debug = UnityEngine.Debug;
    using DomainDict = System.Collections.Generic.Dictionary<DomainType, bool>;
    using IReadOnlyDomainDict = System.Collections.Generic.IReadOnlyDictionary<DomainType, bool>;

    public enum DomainType
    {
        None,
        Director, Enemy,
        Agent, Training,
        StateMachine, State,
        Player,
        Room,
        Sound,
        UI,
        Weapon,
    };

    public static class DomainDebug
    {
        static IReadOnlyDomainDict m_Enabled = new DomainDict(){
        { DomainType.None, true },
        { DomainType.Director, true },
        { DomainType.Enemy, false },
        { DomainType.Agent, true },
        { DomainType.Training, true },
        { DomainType.StateMachine, false },
        { DomainType.State, false },
        { DomainType.Player, true },
        { DomainType.Room, true },
        { DomainType.Sound, false },
        { DomainType.UI, true },
        { DomainType.Weapon, true },
    };


        public static void Log(string msg, DomainType domain = DomainType.None)
        {
            if (!m_Enabled[domain]) return;
            Debug.Log($"[{domain}]: {msg}");
        }

        public static void LogWarning(string msg, DomainType domain = DomainType.None)
        {
            Debug.LogWarning($"[{domain}]: {msg}");
        }

        public static void LogError(string msg, DomainType domain = DomainType.None)
        {
            Debug.LogError($"[{domain}]: {msg}");
        }

        public static void LogAssertion(string msg, DomainType domain = DomainType.None)
        {
            Debug.LogAssertion($"[{domain}]: {msg}");
        }
    }
}
