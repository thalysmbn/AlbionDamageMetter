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
        private readonly HealthUpdateEventHandler _healthUpdateEventHandler;
        private readonly JoinResponseHandler _joinResponseHandler;
        private readonly NewCharacterEventHandler _newCharacterEventHandler;
        private readonly PartyChangedOrderEventHandler _partyChangedOrderEventHandler;
        private readonly PartyDisbandedEventHandler _partyDisbandedEventHandler;
        private readonly PartyPlayerJoinedEventHandler _partyPlayerJoinedEventHandler;
        private readonly PartyPlayerLeftEventHandler _partyPlayerLeftEventHandler;
        
        public AlbionPackageParser(AlbionClusterData albionClusterData,
            AlbionEntityData albionEntityData)
        {
            _changeClusterResponseHandler = new ChangeClusterResponseHandler(albionClusterData, albionEntityData);
            _healthUpdateEventHandler = new HealthUpdateEventHandler(albionEntityData);
            _joinResponseHandler = new JoinResponseHandler(albionClusterData, albionEntityData);
            _newCharacterEventHandler = new NewCharacterEventHandler(albionEntityData);
            _partyChangedOrderEventHandler = new PartyChangedOrderEventHandler(albionEntityData);
            _partyDisbandedEventHandler = new PartyDisbandedEventHandler(albionEntityData);
            _partyPlayerJoinedEventHandler = new PartyPlayerJoinedEventHandler(albionEntityData);
            _partyPlayerLeftEventHandler = new PartyPlayerLeftEventHandler(albionEntityData);
        }

        protected override async void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            var eventCode = ParseEventCode(parameters);

            if (eventCode == EventCodes.Unused)
            {
                return;
            }
            switch (eventCode)
            {
                case EventCodes.HealthUpdate:
                    await _healthUpdateEventHandler.OnActionAsync(new HealthUpdateEvent(parameters));
                    return;
                case EventCodes.NewCharacter:
                    await _newCharacterEventHandler.OnActionAsync(new NewCharacterEvent(parameters));
                    return;
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
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
        }

        protected override async void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            var opCode = ParseOperationCode(parameters);

            if (opCode == OperationCodes.Unused)
            {
                return;
            }
            Console.WriteLine($"code: {opCode}");
            switch (opCode)
            {
                case OperationCodes.Join:
                    await _joinResponseHandler.OnActionAsync(new JoinResponse(parameters));
                    return;
                case OperationCodes.ChangeCluster:
                    await _changeClusterResponseHandler.OnActionAsync(new ChangeClusterResponse(parameters));
                    return;
            }
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
