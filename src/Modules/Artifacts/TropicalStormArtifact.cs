using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TropicalStorm_Mod
{
    public static class TropicalStormArtifact
    {
        public static void CreateArtifact()
        {
            initialMult = Config.initialMultiplier.Value;
            loopIncreaseMult = Config.loopIncreaseMultiplier.Value;
            string prefix = TropicalStorm_ModPlugin.developerPrefix;
            var nameToken = prefix + "_ARTIFACT_TROPICALSTORM_NAME";
            var descriptionToken = prefix + "_ARTIFACT_TROPICALSTORM_DESCRIPTION";
            LanguageAPI.Add(nameToken, "Tropical Storm");
            string description = "All player stats starts ";
            if (initialMult < 0)
            {
                description += $"decreased by <style=cIsHealth>{100f * initialMult}%</style> and ";
            }
            else
            {
                description += $"increased by <style=cIsHealing>{100f * initialMult}%</style> and ";
            }
            if (loopIncreaseMult < 0)
            {
                description += $"decrease by <style=cIsHealth>{100f * loopIncreaseMult}%</style> ";
            }
            else
            {
                description += $"increase by <style=cIsHealing>{100f * loopIncreaseMult}%</style> ";
            }
            description += "every loop.";
            LanguageAPI.Add(descriptionToken, description);

            artifactDef = ScriptableObject.CreateInstance<ArtifactDef>();
            artifactDef.nameToken = nameToken;
            artifactDef.descriptionToken = descriptionToken;
            artifactDef.smallIconSelectedSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("TropicalStormArtifactEnabledIcon");
            artifactDef.smallIconDeselectedSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("TropicalStormArtifactDisabledIcon");
            ContentAddition.AddArtifactDef(artifactDef);
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }
        
        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (RunArtifactManager.instance.IsArtifactEnabled(artifactDef) &&
                self.teamComponent != null && self.teamComponent.teamIndex == TeamIndex.Player)
            {
                var loopClearCount = (Run.instance) ? Run.instance.loopClearCount : 0;
                var multiplier = (1f + initialMult) + (loopClearCount * loopIncreaseMult);
                self.acceleration *= multiplier;
                self.armor *= multiplier;
                self.attackSpeed *= multiplier;
                self.crit *= multiplier;
                self.critHeal *= multiplier;
                self.damage *= multiplier;
                self.moveSpeed *= multiplier;
                self.regen *= multiplier;
            };
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(artifactDef) &&
                sender.teamComponent != null && sender.teamComponent.teamIndex == TeamIndex.Player)
            {
                var loopClearCount = (Run.instance)? Run.instance.loopClearCount: 0;
                var multiplier = (1f + initialMult) + (loopClearCount * loopIncreaseMult);
                args.healthMultAdd *= multiplier;
            };
        }

        public static ArtifactDef artifactDef;
        public static float initialMult = -0.3f;
        public static float loopIncreaseMult = 0.1f;
    }
}
