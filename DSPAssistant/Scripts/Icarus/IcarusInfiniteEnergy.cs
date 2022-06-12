using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace DSPAssistant
{
    internal class IcarusInfiniteEnergy: MonoBehaviour
    {
        void Update()
        {
            if (GameMain.data != null && GameMain.data.mainPlayer != null)
            {
                GameMain.data.mainPlayer.mecha.coreEnergy = GameMain.data.mainPlayer.mecha.coreEnergyCap;
            }
        }
    }
}
