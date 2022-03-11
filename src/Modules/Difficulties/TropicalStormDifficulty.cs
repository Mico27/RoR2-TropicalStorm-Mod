using R2API;
using RoR2;
using UnityEngine;

namespace TropicalStorm_Mod
{
    public static class TropicalStormDifficulty
    {
        public static void CreateDifficulty()
        {
            initialMult = Config.initialMultiplier.Value;
            loopIncreaseMult = Config.loopIncreaseMultiplier.Value;
            string prefix = TropicalStorm_ModPlugin.developerPrefix;
            var nameToken = prefix + "_DIFFICULTY_TROPICALSTORM_NAME";
            var descriptionToken = prefix + "_DIFFICULTY_TROPICALSTORM_DESCRIPTION";
            LanguageAPI.Add(nameToken, "Tropical Storm");
            string description = "An extremely hard difficulty that rewards longer runs. Become a legend or die trying.\r\n\r\n<style=cStack>>All player stats starts ";
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
            description += "every loop.\r\n>Difficulty scaling: <style=cIsHealth>+75%</style></style>";
            LanguageAPI.Add(descriptionToken, description);
            
            DifficultyDef difficultyDef = new DifficultyDef(3.5f, nameToken, "@TropicalStorm:Assets/TropicalStormIcon.png", descriptionToken, new Color(0.25f, 0.85f, 0.95f), "ts", true);
            difficultyDef.iconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("TropicalStormIcon");
            difficultyDef.foundIconSprite = true;
            TropicalStormDifficulty.difficultyIndex = DifficultyAPI.AddDifficulty(difficultyDef);
            Run.onPlayerFirstCreatedServer += Run_onPlayerFirstCreatedServer;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void Run_onPlayerFirstCreatedServer(Run arg1, PlayerCharacterMasterController arg2)
        {
            if (arg1 != null && arg1.selectedDifficulty == TropicalStormDifficulty.difficultyIndex &&
                arg2 != null && arg2.master != null && arg2.master.inventory != null)
            {
                arg2.master.inventory.RemoveItem(RoR2Content.Items.MonsoonPlayerHelper);
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (Run.instance != null && Run.instance.selectedDifficulty == TropicalStormDifficulty.difficultyIndex &&
                self.teamComponent != null && self.teamComponent.teamIndex == TeamIndex.Player)
            {
                var loopClearCount = Run.instance.loopClearCount;
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
            if (Run.instance != null && Run.instance.selectedDifficulty == TropicalStormDifficulty.difficultyIndex &&
                sender.teamComponent != null && sender.teamComponent.teamIndex == TeamIndex.Player)
            {
                var loopClearCount = Run.instance.loopClearCount;
                var multiplier = (1f + initialMult) + (loopClearCount * loopIncreaseMult);
                args.healthMultAdd *= multiplier;
            };
        }

        public static DifficultyIndex difficultyIndex;
        public static float initialMult = -0.3f;
        public static float loopIncreaseMult = 0.1f;
    }
}
