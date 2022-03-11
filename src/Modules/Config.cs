using BepInEx.Configuration;
using UnityEngine;

namespace TropicalStorm_Mod
{
    public static class Config
    {
        public static ConfigEntry<bool> useAsAnArtifact;
        public static ConfigEntry<float> initialMultiplier;
        public static ConfigEntry<float> loopIncreaseMultiplier;
        public static void ReadConfig()
        {
            useAsAnArtifact = GetSetConfig("Tropical Storm", "Use as an artifact", false, "Makes Tropical Storm available as an artifact instead of a difficulty.");
            initialMultiplier = GetSetConfig("Tropical Storm", "Initial stats multiplier", -0.30f, "The initial multiplier applied too all the players stats at the start of a run.");
            loopIncreaseMultiplier = GetSetConfig("Tropical Storm", "Loop stats multiplier", 0.10f, "The multiplier applied too all the players stats for each loop done.");
        }

        internal static ConfigEntry<T> GetSetConfig<T>(string section, string key, T defaultValue, string description)
        {
            return TropicalStorm_ModPlugin.instance.Config.Bind<T>(new ConfigDefinition(section, key), defaultValue, new ConfigDescription(description));
        }
    }
}