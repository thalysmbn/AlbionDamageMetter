using AlbionDamageMetter.Extensions;

namespace AlbionDamageMetter.Albion.Network.Events
{
    public class PartyPlayerJoinedEvent
    {
        public Guid? UserGuid { get; }
        public string Username { get; }

        public PartyPlayerJoinedEvent(Dictionary<byte, object> parameters)
        {
            if (parameters.ContainsKey(1))
                UserGuid = parameters[1].ObjectToGuid();
            if (parameters.ContainsKey(2))
                Username = string.IsNullOrEmpty(parameters[2].ToString()) ? string.Empty : parameters[2].ToString();
        }
    }
}
