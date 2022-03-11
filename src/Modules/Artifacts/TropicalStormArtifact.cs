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
            initialMult = Math.Max(0f, Config.initialMultiplier.Value);
            loopIncreaseMult = Math.Max(0f, Config.loopMultiplier.Value);
            string prefix = TropicalStorm_ModPlugin.developerPrefix;
            var nameToken = prefix + "_ARTIFACT_TROPICALSTORM_NAME";
            var descriptionToken = prefix + "_ARTIFACT_TROPICALSTORM_DESCRIPTION";
            LanguageAPI.Add(nameToken, "Tropical Storm");
            string description = "All player stats starts ";
            if (initialMult < 1f)
            {
                description += $"decreased by <style=cIsHealth>{100f * (1 - initialMult)}%</style> and ";
            }
            else if (initialMult > 1f)
            {
                description += $"increased by <style=cIsHealing>{100f * (initialMult - 1)}%</style> and ";
            } 
            else
            {
                description += $"at their normal values and ";
            }
            if (loopIncreaseMult < 1f)
            {
                description += $"decrease by <style=cIsHealth>{100f * (1 - loopIncreaseMult)}%</style> ";
            }
            else if (loopIncreaseMult > 1f)
            {
                description += $"increase by <style=cIsHealing>{100f * (loopIncreaseMult - 1)}%</style> ";
            } 
            else
            {
                description += $"stay the same ";
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
                var multiplier = initialMult * (float)Math.Pow(loopIncreaseMult, loopClearCount);
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
                var multiplier = initialMult * (float)Math.Pow(loopIncreaseMult, loopClearCount);
                args.healthMultAdd *= multiplier;
            };
        }

        public static ArtifactDef artifactDef;
        public static float initialMult = -0.3f;
        public static float loopIncreaseMult = 0.1f;
    }
}
