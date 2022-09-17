using AlbionDamageMetter.Albion.Network;
using AlbionDamageMetter.Albion.Network.Events;
using AlbionDamageMetter.Albion.Network.Handler;
using PhotonPackageParser;

namespace AlbionDamageMetter.Albion
{
    public class AlbionPackageParser : PhotonParser
    {
        private readonly PartyChangedOrderEventHandler _partyChangedOrderEventHandler = new PartyChangedOrderEventHandler();
        private readonly PartyDisbandedEventHandler _partyDisbandedEventHandler = new PartyDisbandedEventHandler();
        private readonly PartyPlayerJoinedEventHandler _partyPlayerJoinedEventHandler = new PartyPlayerJoinedEventHandler();
        private readonly PartyPlayerLeftEventHandler _partyPlayerLeftEventHandler = new PartyPlayerLeftEventHandler();
        
        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            var eventCode = ParseEventCode(parameters);

            if (eventCode == EventCodes.Unused)
            {
                return;
            }

            Task.Run(async () =>
            {
                switch (eventCode)
                {
                    case EventCodes.PartyChangedOrder:
                        await _partyChangedOrderEventHandler.OnActionAsync(new PartyChangedOrderEvent(parameters));
                        return;
                    case EventCodes.PartyDisbanded:
                        await _partyDisbandedEventHandler.OnActionAsync(new PartyDisbandedEvent(parameters));
                        return;
                    case EventCodes.PartyPlayerJoined:
                        await _partyPlayerJoinedEventHandler.OnActionAsync(new PartyPlayerJoinedEvent(parameters));
                        return;
                    case EventCodes.PartyPlayerLeft:
                        await _partyPlayerLeftEventHandler.OnActionAsync(new PartyPlayerLeftEvent(parameters));
                        return;
                }
            });
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            throw new NotImplementedException();
        }

        private static EventCodes ParseEventCode(IReadOnlyDictionary<byte, object> parameters)
        {
            if (!parameters.TryGetValue(252, out var value))
            {
                return EventCodes.Unused;
            }

            return (EventCodes)Enum.ToObject(typeof(EventCodes), value);
        }
    }
}
