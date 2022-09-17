﻿using AlbionDamageMetter.Albion.Models.NetworkModel;
using AlbionDamageMetter.Extensions;

namespace AlbionDamageMetter.Albion.Network.Events
{
    public class NewCharacterEvent
    {
        public long? ObjectId { get; }
        public Guid? Guid { get; }
        public string Name { get; }
        public string GuildName { get; }
        public float[] Position { get; }
        public CharacterEquipment CharacterEquipment { get; } = new();

        public NewCharacterEvent(Dictionary<byte, object> parameters)
        {
            if (parameters.ContainsKey(0)) ObjectId = parameters[0].ObjectToLong();

            if (parameters.ContainsKey(1)) Name = parameters[1].ToString();

            if (parameters.ContainsKey(7)) Guid = parameters[7].ObjectToGuid();

            if (parameters.ContainsKey(8)) GuildName = parameters[8].ToString();

            if (parameters.ContainsKey(12)) Position = (float[])parameters[12];

            if (parameters.ContainsKey(33))
            {
                var valueType = parameters[33].GetType();
                switch (valueType.IsArray)
                {
                    case true when typeof(byte[]).Name == valueType.Name:
                        {
                            var values = ((byte[])parameters[33]).ToDictionary();
                            CharacterEquipment = GetEquipment(values);
                            break;
                        }
                    case true when typeof(short[]).Name == valueType.Name:
                        {
                            var values = ((short[])parameters[33]).ToDictionary();
                            CharacterEquipment = GetEquipment(values);
                            break;
                        }
                    case true when typeof(int[]).Name == valueType.Name:
                        {
                            var values = ((int[])parameters[33]).ToDictionary();
                            CharacterEquipment = GetEquipment(values);
                            break;
                        }
                }
            }
        }

        private CharacterEquipment GetEquipment<T>(IReadOnlyDictionary<int, T> values)
        {
            return new CharacterEquipment
            {
                MainHand = values[0].ObjectToInt(),
                OffHand = values[1].ObjectToInt(),
                Head = values[2].ObjectToInt(),
                Chest = values[3].ObjectToInt(),
                Shoes = values[4].ObjectToInt(),
                Bag = values[5].ObjectToInt(),
                Cape = values[6].ObjectToInt(),
                Mount = values[7].ObjectToInt()
            };
        }
    }
}
