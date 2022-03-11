using BepInEx.Configuration;
using UnityEngine;

namespace TropicalStorm_Mod
{
    public static class Config
    {
        public static ConfigEntry<bool> useAsAnArtifact;
        public static ConfigEntry<float> initialMultiplier;
        public static ConfigEntry<float> loopMultiplier;


        public static void ReadConfig()
        {
            useAsAnArtifact = GetSetConfig("Tropical Storm", "Use as an artifact", false, "Makes Tropical Storm available as an artifact instead of a difficulty.");
            initialMultiplier = GetSetConfig("Tropical Storm", "Starting multiplier", 1.50f, "The initial multiplier applied to all the players stats at the start of a run. Bewteen 0 and 1 decreases the stats, over 1 increases it. Putting in 1 will not change the stats.");
            loopMultiplier = GetSetConfig("Tropical Storm", "Looping multiplier", 0.50f, "The multiplier applied to all the players stats for each loop done. Bewteen 0 and 1 decreases the stats, over 1 increases it. Putting in 1 will not change the stats.");            
        }

        internal static ConfigEntry<T> GetSetConfig<T>(string section, string key, T defaultValue, string description)
        {
            return TropicalStorm_ModPlugin.instance.Config.Bind<T>(new ConfigDefinition(section, key), defaultValue, new ConfigDescription(description));
        }
    }
}