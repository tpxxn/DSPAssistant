using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace DSPAssistant
{
    internal class BuildArea : MonoBehaviour
    {
        private static ConfigEntry<int> touchableRangeMultiplier;

        internal static float buildArea;

        void Awake()
        {
            touchableRangeMultiplier = Bootstrap.Instance.Config.Bind("BuildArea", "build_area", 160, new ConfigDescription("buildArea. min=80 max=999. original behavior is 80", new AcceptableValueRange<int>(80, 999)));
            touchableRangeMultiplier.Value = Mathf.Clamp(touchableRangeMultiplier.Value, 80, 999);
            buildArea = touchableRangeMultiplier.Value;
            touchableRangeMultiplier.SettingChanged += (sender, args) =>
            {
                buildArea = Mathf.Clamp(touchableRangeMultiplier.Value, 80, 999);
            };
            Harmony.CreateAndPatchAll(typeof(BuildArea));
        }

        /// <summary>
        /// 建造范围
        /// </summary>
        [HarmonyPatch(typeof(Mecha), "Import")]
        private class Patch
        {
            private static void Postfix(Mecha __instance)
            {
                __instance.buildArea = buildArea;
                Bootstrap.Debug("机器人建造范围：" + __instance.buildArea.ToString());
            }
        }

        [HarmonyPatch(typeof(Mecha), "Export")]
        private class Patch2
        {
            private static void Prefix(Mecha __instance)
            {
                __instance.buildArea = 80;
                Bootstrap.Debug("机器人建造范围：" + __instance.buildArea.ToString());
            }
        }

    }
}
