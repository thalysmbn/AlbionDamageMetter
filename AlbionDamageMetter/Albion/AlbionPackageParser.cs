using AlbionDamageMetter.Albion.Network;
using AlbionDamageMetter.Albion.Network.Events;
using AlbionDamageMetter.Albion.Network.Handler;
using AlbionDamageMetter.Albion.Network.Response;
using PhotonPackageParser;

namespace AlbionDamageMetter.Albion
{
    public class AlbionPackageParser : PhotonParser
    {
        private readonly ChangeClusterResponseHandler _changeClusterResponseHandler;
        private readonly PartyChangedOrderEventHandler _partyChangedOrderEventHandler;
        private readonly PartyDisbandedEventHandler _partyDisbandedEventHandler;
        private readonly PartyPlayerJoinedEventHandler _partyPlayerJoinedEventHandler;
        private readonly PartyPlayerLeftEventHandler _partyPlayerLeftEventHandler;
        
        public AlbionPackageParser(AlbionClusterData clusterDataController)
        {
            _changeClusterResponseHandler = new ChangeClusterResponseHandler(clusterDataController);
            _partyChangedOrderEventHandler = new PartyChangedOrderEventHandler();
            _partyDisbandedEventHandler = new PartyDisbandedEventHandler();
            _partyPlayerJoinedEventHandler = new PartyPlayerJoinedEventHandler();
            _partyPlayerLeftEventHandler = new PartyPlayerLeftEventHandler();
        }

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
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            var opCode = ParseOperationCode(parameters);

            if (opCode == OperationCodes.Unused)
            {
                return;
            }

            Task.Run(async () =>
            {
                switch (opCode)
                {
                    case OperationCodes.ChangeCluster:
                        await _changeClusterResponseHandler.OnActionAsync(new ChangeClusterResponse(parameters));
                        return;
                    case OperationCodes.PartyMakeLeader:
                        return;
                }
            });
        }

        private static EventCodes ParseEventCode(IReadOnlyDictionary<byte, object> parameters)
        {
            if (!parameters.TryGetValue(252, out var value))
            {
                return EventCodes.Unused;
            }

            return (EventCodes)Enum.ToObject(typeof(EventCodes), value);
        }

        private OperationCodes ParseOperationCode(IReadOnlyDictionary<byte, object> parameters)
        {
            if (!parameters.TryGetValue(253, out var value))
            {
                return OperationCodes.Unused;
            }

            return (OperationCodes)Enum.ToObject(typeof(OperationCodes), value);
        }
    }
}
