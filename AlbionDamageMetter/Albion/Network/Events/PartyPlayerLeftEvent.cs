using AlbionDamageMetter.Extensions;

namespace AlbionDamageMetter.Albion.Network.Events
{
    public class PartyPlayerLeftEvent
    {
        public Guid? UserGuid { get; }

        public PartyPlayerLeftEvent(Dictionary<byte, object> parameters)
        {
            if (parameters.ContainsKey(1))
                UserGuid = parameters[1].ObjectToGuid();
        }
    }
}
