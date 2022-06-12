using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;

namespace DSPAssistant
{
    internal class ItemStack : MonoBehaviour
    {
        private static ConfigEntry<int> Count;

        void Awake()
        {
            Count = Bootstrap.Instance.Config.Bind("ItemStack", "item_stack_multiplier", 10, new ConfigDescription("buildArea. min=1 max=999. original behavior is 10", new AcceptableValueRange<int>(1, 999)));
            Harmony.CreateAndPatchAll(typeof(ItemStack));
        }

        /// <summary>
        /// 物品堆叠
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StorageComponent), "LoadStatic")]
        public static bool StorageComponentLoadStatic(StorageComponent __instance)
        {
            if (!StorageComponent.staticLoaded)
            {
                StorageComponent.itemIsFuel = new bool[12000];
                StorageComponent.itemStackCount = new int[12000];
                for (int index = 0; index < 12000; ++index)
                {
                    StorageComponent.itemStackCount[index] = 1000;
                }
                ItemProto[] dataArray = LDB.items.dataArray;
                for (int index = 0; index < dataArray.Length; ++index)
                {
                    StorageComponent.itemIsFuel[dataArray[index].ID] = dataArray[index].HeatValue > 0L;
                    StorageComponent.itemStackCount[dataArray[index].ID] = dataArray[index].StackSize * Count.Value;
                }
                StorageComponent.staticLoaded = true;
            }
            return false;
        }

        /// <summary>
        /// 整理所有箱子
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameMain), "Begin")]
        private static void GameMain_Begin()
        {
            Bootstrap.Debug("ArrivePlanet");
            if (GameMain.instance != null && GameMain.data != null && GameMain.data.factories != null)
            {
                foreach (var planetFactory in GameMain.data.factories)
                {
                    if (planetFactory != null)
                    {
                        //Bootstrap.Debug("planetFactory.index = " + planetFactory.index);
                        var factoryStorage = planetFactory.factoryStorage;
                        if (factoryStorage.storagePool != null)
                        {
                            foreach (var storageComponent in factoryStorage.storagePool)
                            {
                                if (storageComponent != null)
                                {
                                    //Bootstrap.Debug("storageComponent.id = " + storageComponent.id);
                                    storageComponent.Sort();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
