using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace DSPAssistant
{
    internal class TouchArea : MonoBehaviour
    {
        private static ConfigEntry<int> touchableAreaMultiplier;

        internal static float multiply;

        void Awake()
        {
            touchableAreaMultiplier = Bootstrap.Instance.Config.Bind("TouchArea", "touchable_area_multiplier", 250, new ConfigDescription("multiplier percentage. min=100(%) max=999(%). original behavior is 100", new AcceptableValueRange<int>(100, 999)));
            touchableAreaMultiplier.Value = Mathf.Clamp(touchableAreaMultiplier.Value, 100, 999);
            multiply = (float)touchableAreaMultiplier.Value / 100f;
            touchableAreaMultiplier.SettingChanged += (sender, args) =>
            {
                multiply = Mathf.Clamp(touchableAreaMultiplier.Value, 100, 999);
            };
            Harmony.CreateAndPatchAll(typeof(TouchArea));
        }

        [HarmonyPatch(typeof(PlayerAction_Inspect), "GetObjectSelectDistance")]
        public static class GetObjectSelectDistance_Patch
        {
            public static void Postfix(ref float __result)
            {
                __result = __result * multiply;
            }
        }

    }
}
