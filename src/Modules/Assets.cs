using System.Reflection;
using UnityEngine;

namespace TropicalStorm_Mod
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        internal static void PopulateAssets()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TropicalStorm_Mod.tropicalstormbundle"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }            
        }
    }
}