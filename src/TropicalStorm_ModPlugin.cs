using BepInEx;
using R2API;
using R2API.Utils;
using System;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace TropicalStorm_Mod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(DifficultyAPI), nameof(RecalculateStatsAPI))]
    public sealed class TropicalStorm_ModPlugin : BaseUnityPlugin
    {
        public const string
            MODNAME = "TropicalStorm_Mod",
            MODAUTHOR = "Mico27",
            MODUID = "com." + MODAUTHOR + "." + MODNAME,
            MODVERSION = "2.0.0";
        // a prefix for name tokens to prevent conflicts
        public const string developerPrefix = MODAUTHOR;
        public void Awake()
        {
            instance = this;
            try
            {
                TropicalStorm_Mod.Config.ReadConfig();
                Assets.PopulateAssets();
                if (TropicalStorm_Mod.Config.useAsAnArtifact.Value)
                {
                    TropicalStormArtifact.CreateArtifact();
                }
                else
                {
                    TropicalStormDifficulty.CreateDifficulty();
                }                
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + " - " + e.StackTrace);
            }
        }

        public static TropicalStorm_ModPlugin instance;
    }
}
