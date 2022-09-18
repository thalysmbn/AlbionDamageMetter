using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Network.Time;
using AlbionDamageMetter.Extensions;

namespace AlbionDamageMetter.Albion.Network.Events
{
    public class HealthUpdateEvent
    {
        public long CauserId { get; private set; }
        public int CausingSpellType { get; private set; }
        public EffectOrigin EffectOrigin { get; private set; }
        public EffectType EffectType { get; private set; }
        public double HealthChange { get; private set; }
        public double NewHealthValue { get; private set; }

        public long ObjectId { get; private set; }
        public GameTimeStamp TimeStamp { get; private set; }

        public HealthUpdateEvent(Dictionary<byte, object> parameters)
        {
            if (parameters.ContainsKey(0))
            {
                ObjectId = parameters[0].ObjectToLong() ?? throw new ArgumentNullException();
            }

            if (parameters.ContainsKey(1))
            {
                TimeStamp = new GameTimeStamp(parameters[1].ObjectToLong() ?? 0);
            }

            if (parameters.ContainsKey(2))
            {
                HealthChange = parameters[2].ObjectToDouble();
            }

            if (parameters.ContainsKey(3))
            {
                NewHealthValue = parameters[3].ObjectToDouble();
            }

            if (parameters.ContainsKey(4))
            {
                EffectType = (EffectType)(parameters[4] as byte? ?? 0);
            }

            if (parameters.ContainsKey(5))
            {
                EffectOrigin = (EffectOrigin)(parameters[5] as byte? ?? 0);
            }

            if (parameters.ContainsKey(6))
            {
                CauserId = parameters[6].ObjectToLong() ?? throw new ArgumentNullException();
            }

            if (parameters.ContainsKey(7))
            {
                CausingSpellType = parameters[7].ObjectToShort();
            }
        }
    }
}
