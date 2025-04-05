using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HitScoreBloomReviver.HarmonyPatches
{
    [HarmonyPatch(typeof(EffectPoolsManualInstaller), nameof(EffectPoolsManualInstaller.ManualInstallBindings))]
    public static class FlyingScoreEffectMaterialPatch
    {
        private static TMP_FontAsset lazyBloomFont;

        [HarmonyPrefix]
        private static void Prefix(EffectPoolsManualInstaller __instance)
        {
            lazyBloomFont ??= GetBloomFont();

            if (lazyBloomFont == null)
            {
                Plugin.Logger.Warn("Bloom font not found, not updating hit score text.");
                return;
            }

            var text = __instance._flyingScoreEffectPrefab._text;
            text.font = lazyBloomFont;
        }

        /**
         * Bloom font creation logic copied from HitScoreVisualizer.
         */
        private static TMP_FontAsset GetBloomFont()
        {
            var tekoFontAsset = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                .FirstOrDefault(x => x.name == "Teko-Medium SDF");
            if (tekoFontAsset == null)
            {
                Plugin.Logger.Critical(
                    "Teko-Medium SDF not found, unable to create bloom font. This is likely because of a game update.");
                return null;
            }

            var bloomFontShader = Resources.FindObjectsOfTypeAll<Shader>()
                .FirstOrDefault(x => x.name == "TextMeshPro/Distance Field");
            if (bloomFontShader == null)
            {
                Plugin.Logger.Critical(
                    "Bloom font shader not found, unable to create bloom font. This is likely because of a game update.");
                return null;
            }

            var bloomTekoFont = tekoFontAsset.CopyFontAsset("Teko-Medium SDF (Bloom)");
            bloomTekoFont.material.shader = bloomFontShader;
            return bloomTekoFont;
        }

        private static TMP_FontAsset CopyFontAsset(this TMP_FontAsset original, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                newName = original.name;
            }

            var newFontAsset = Object.Instantiate(original);

            var texture = original.atlasTexture;

            var newTexture = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount, true)
            {
                name = $"{newName} Atlas"
            };

            Graphics.CopyTexture(texture, newTexture);

            var material = new Material(original.material) { name = $"{newName} Atlas Material" };
            material.SetTexture(Shader.PropertyToID("_MainTex"), newTexture);

            newFontAsset.m_AtlasTexture = newTexture;
            newFontAsset.name = newName;
            newFontAsset.atlasTextures = new[] { newTexture };
            newFontAsset.material = material;

            return newFontAsset;
        }
    }
}