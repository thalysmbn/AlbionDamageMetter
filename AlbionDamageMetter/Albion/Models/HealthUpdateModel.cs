using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Network.Time;

namespace AlbionDamageMetter.Albion.Models
{
    public class HealthUpdateModel
    {
        public long CauserId { get; set; }
        public int CausingSpellType { get; set; }
        public EffectOrigin EffectOrigin { get; set; }
        public EffectType EffectType { get; set; }
        public double HealthChange { get; set; }
        public double NewHealthValue { get; set; }

        public long ObjectId { get; set; }
        public GameTimeStamp? TimeStamp { get; set; }
    }
}
