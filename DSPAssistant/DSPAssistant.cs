using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
// using xiaoye97;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DSPAssistant
{
    // [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tpxxn.plugin.Dyson.DSPAssistant", "DSPAssistant", "1.0")]
    public class DSPAssistant : BaseUnityPlugin
    {
        private static volatile DSPAssistant instance = null;
        private static object locker = new object();
        public static DSPAssistant Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new DSPAssistant();
                        }
                    }
                }
                return instance;
            }
        }


        internal static ManualLogSource logger;// "C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\LogOutput.log"
        public static int Count = 10;

        void Start()
        {
            //LDBTool.EditDataAction += Edit;
            logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(DSPAssistant), null);
            GodModeButton.Awake();
            Harmony.CreateAndPatchAll(typeof(GodModeButton), null);
            var the4DPocket = new GameObject(typeof(The4DPocket).FullName).AddComponent<The4DPocket>();
            var autoSorter = new GameObject(typeof(DSPAutoSorter).FullName).AddComponent<DSPAutoSorter>();
        }

        void Update()
        {
            if (GameMain.data != null && GameMain.data.mainPlayer != null)
            {
                GameMain.data.mainPlayer.mecha.coreEnergy = GameMain.data.mainPlayer.mecha.coreEnergyCap;
            }
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
                    StorageComponent.itemStackCount[dataArray[index].ID] = dataArray[index].StackSize * Count;
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
            logger.LogWarning("ArrivePlanet");
            if (GameMain.instance != null && GameMain.data != null && GameMain.data.factories != null)
            {
                foreach (var planetFactory in GameMain.data.factories)
                {
                    if (planetFactory != null)
                    {
                        //logger.LogWarning("planetFactory.index = " + planetFactory.index);
                        var factoryStorage = planetFactory.factoryStorage;
                        if (factoryStorage.storagePool != null)
                        {
                            foreach (var storageComponent in factoryStorage.storagePool)
                            {
                                if (storageComponent != null)
                                {
                                    //logger.LogWarning("storageComponent.id = " + storageComponent.id);
                                    storageComponent.Sort();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 建造范围
        /// </summary>
        [HarmonyPatch(typeof(Mecha), "Import")]
        private class Patch
        {
            private static void Postfix(Mecha __instance)
            {
                __instance.buildArea *= 2f;
                if (__instance.buildArea < 160.0)
                    __instance.buildArea = 160f;
                Debug.Log("机器人建造范围：" + __instance.buildArea.ToString());
            }
        }

        [HarmonyPatch(typeof(Mecha), "Export")]
        private class Patch2
        {
            private static void Prefix(Mecha __instance)
            {
                __instance.buildArea /= 2f;
                if (__instance.buildArea / 2.0 < 80.0)
                    __instance.buildArea = 80f;
                Debug.Log("机器人建造范围：" + __instance.buildArea.ToString());
            }
        }

        //void Edit(Proto proto)
        //{
        //    if (proto is RecipeProto && proto.ID == 67)
        //    {
        //        var recipe = proto as RecipeProto;
        //        recipe.Items[1] = 1113;
        //    }
        //}

    }
}
