using AlbionDamageMetter.Albion.Enums;
using AlbionDamageMetter.Albion.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace AlbionDamageMetter.Albion
{
    public static class AlbionWorldData
    {
        private static MapMarkerType GetMapMarkerType(string value)
        {
            return value switch
            {
                "Stone" => MapMarkerType.Stone,
                "Ore" => MapMarkerType.Ore,
                "Hide" => MapMarkerType.Hide,
                "Wood" => MapMarkerType.Wood,
                "Fiber" => MapMarkerType.Fiber,
                "roads_of_avalon_solo_pve" => MapMarkerType.RoadsOfAvalonSoloPve,
                "roads_of_avalon_group_pve" => MapMarkerType.RoadsOfAvalonGroupPve,
                "roads_of_avalon_raid_pve" => MapMarkerType.RoadsOfAvalonRaidPve,
                "dungeon_elite" => MapMarkerType.DungeonElite,
                "dungeon_group" => MapMarkerType.DungeonGroup,
                "dungeon_solo" => MapMarkerType.DungeonSolo,
                _ => MapMarkerType.Unknown
            };
        }

        public static Guid? GetMapGuid(string index)
        {
            // Cluster change event
            // @ISLAND@c640e642-5135-4203-89b5-0007e4215605
            // @RANDOMDUNGEON@fe968505-9771-4653-8ade-29a1bd6ddb56 
            // @HIDEOUT@2306@29c344b3-2138-421d-a97c-06e29d4759ec

            // Base vault info event
            // 4a82e5ed-8b64-40e3-926b-e0b020c4550a@@ISLAND@c640e642-5135-4203-89b5-0007e4215605
            // f56a368d-2f0b-4d01-a1ba-0079cf8b1fa9@4001

            try
            {
                var splitName = index.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);

                var mapTypeIndex = Array.FindIndex(splitName, indexString => indexString is "ISLAND" or "HIDEOUT");

                if (index.Contains("ISLAND") && mapTypeIndex > -1 && splitName.Length >= mapTypeIndex + 1)
                {
                    var mapGuid = new Guid(splitName[mapTypeIndex + 1]);
                    return mapGuid;
                }

                if (index.Contains("HIDEOUT") && mapTypeIndex > -1 && splitName.Length >= mapTypeIndex + 2)
                {
                    var mapGuid = new Guid(splitName[mapTypeIndex + 2]);
                    return mapGuid;
                }

                if (splitName.Length > 1 && index.ToLower().Contains('@'))
                {
                    var mapType = GetMapType(splitName[0]);
                    if (mapType is MapType.RandomDungeon or MapType.CorruptedDungeon or MapType.HellGate or MapType.Expedition && !string.IsNullOrEmpty(splitName[1]))
                    {
                        var mapGuid = new Guid(splitName[1]);
                        return mapGuid;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static string GetMapNameByMapType(MapType mapType)
        {
            return mapType switch
            {
                MapType.HellGate => "HELLGATE",
                MapType.RandomDungeon => "DUNGEON",
                MapType.CorruptedDungeon => "CORRUPTED_LAIR",
                MapType.Island => "ISLAND",
                MapType.Hideout => "HIDEOUT",
                MapType.Expedition => "EXPEDITION",
                MapType.Arena => "ARENA",
                _ => "UNKNOWN"
            };
        }

        public static MapType GetMapType(string index)
        {
            if (index.ToUpper().Contains("HELLCLUSTER")) return MapType.HellGate;

            if (index.ToUpper().Contains("RANDOMDUNGEON")) return MapType.RandomDungeon;

            if (index.ToUpper().Contains("CORRUPTEDDUNGEON")) return MapType.CorruptedDungeon;

            if (index.ToUpper().Contains("ISLAND")) return MapType.Island;

            if (index.ToUpper().Contains("HIDEOUT")) return MapType.Hideout;

            if (index.ToUpper().Contains("EXPEDITION")) return MapType.Expedition;

            if (index.ToUpper().Contains("ARENA")) return MapType.Arena;

            return MapType.Unknown;
        }
    }
}
