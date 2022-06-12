using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace DSPAssistant
{
    [HarmonyPatch]
    public class DSPAutoSorter : MonoBehaviour
    {
        static public DSPAutoSorter self;

        private static ConfigEntry<bool> enableForcedSort;
        private static ConfigEntry<bool> enableSortInInventry;
        private static ConfigEntry<bool> enableSortInStorage;
        private static ConfigEntry<bool> enableSortInFuelChamber;
        private static ConfigEntry<bool> enableSortInMiner;
        private static ConfigEntry<bool> enableSortInAssembler;

        //public static GameObject configButton;

        public static Sprite settingIconSprite;
        public static Sprite squareIconSprite;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            enableSortInInventry = Bootstrap.Instance.Config.Bind("AutoSorter General", "enableSortInInventry", true, "enable sort in normal inventry.");
            enableSortInStorage = Bootstrap.Instance.Config.Bind("AutoSorter General", "enableSortInStorage", true, "enable sort in storages.");
            enableSortInFuelChamber = Bootstrap.Instance.Config.Bind("AutoSorter General", "enableSortInFuelChamber", true, "enable sort in Mecha fuelchamber.");
            enableSortInMiner = Bootstrap.Instance.Config.Bind("AutoSorter General", "enableSortInMiner", true, "enable sort in inventry of miner.");
            enableSortInAssembler = Bootstrap.Instance.Config.Bind("AutoSorter General", "enableSortInAssembler", true, "enable sort in inventry of Assemblers or smelter.");
            enableForcedSort = Bootstrap.Instance.Config.Bind("AutoSorter ForcedSort", "enableForcedSort", true, "enable forced sort.");

            // Sprite[] sprites = (Sprite[])Resources.LoadAll<Sprite>("");
            //      //Bootstrap.Debug("----------------------------------------icon load");
            //foreach(Sprite sprite in sprites)
            // {
            //     if (sprite.name == "settings-icon")
            //     {
            //         settingIconSprite = sprite;
            //         //Bootstrap.Debug("-------------------------------------settingIconSprite name : " + settingIconSprite.name);
            //     } else if (sprite.name == "solid-bg")
            //     {
            //         squareIconSprite = sprite;
            //         //Bootstrap.Debug("-------------------------------------squareIconSprite name : " + squareIconSprite.name);
            //     }
            // }
            //settingIconSprite = Resources.Load<Sprite>("ui/textures/sprites/icons/settings-icon");
            //if (settingIconSprite == null )
            //{
            //    Bootstrap.Debug("-------------------------------------settingIconSprite is null");

            //}

            //squareIconSprite = Resources.Load<Sprite>("ui/textures/sprites/icons/solid-bg");
            //if (squareIconSprite == null)
            //{
            //    Bootstrap.Debug("-------------------------------------squareIconSprite is null");

            //}

        }


        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(StorageComponent), "NotifyStorageChange")]
        public static void StorageComponent_NotifyStorageChange_Postfix(StorageComponent __instance)
        {
            Bootstrap.Debug("+++++++++++++++++++++++++++++++++StorageComponent　NotifyStorageChange");
            //if (__instance.entityId > 0)
            //{
            //    Bootstrap.Debug("+++++++++++++++++++++++++++++++++StorageComponent　NotifyStorageChange　" + GameMain.data.localPlanet.factory.entityPool[__instance.entityId].assemblerId);
            //    __instance.Sort();
            //}
        }



        //強制１
        [HarmonyPrefix, HarmonyPatch(typeof(UIStorageGrid), "OnStorageContentChanged")]
        //[HarmonyPatch(typeof(UIStorageGrid), "HandPut")]
        public static void UIStorageGrid_OnStorageContentChanged_Prefix(UIStorageGrid __instance)
        {
            var storage = ReflectionUtil.GetPrivateField<StorageComponent>(__instance, "storage");
            if (storage == null || !enableForcedSort.Value)
            {
                return;
            }
            var parentObj = __instance.transform.parent.gameObject;
            if (enableSortInInventry.Value && parentObj.name == "Windows")
            {
                __instance.OnSort();
            }
            else if (enableSortInFuelChamber.Value && parentObj.name == "fuel-group")
            {
                __instance.OnSort();
            }
            else if (enableSortInStorage.Value && parentObj.name == "Storage Window")
            {
                __instance.OnSort();
            }
            else if (enableSortInMiner.Value && parentObj.name == "Miner Window")
            {
                __instance.OnSort();
            }
            else if (enableSortInAssembler.Value && parentObj.name == "Assembler Window")
            {
                __instance.OnSort();
            }
        }
        // fuel-storage
        // Player Inventory
        // storage
        // 

        //強制２
        //[HarmonyPostfix,HarmonyPatch(typeof(UIStorageGrid), "HandTake")]
        //public static void UIStorageGrid_HandTake_Postfix(UIStorageGrid __instance)
        //{
        //    if (enableSortInInventry.Value && enableForcedSort.Value && __instance.transform.parent.gameObject.name == "Windows")
        //    {
        //        __instance.OnSort();
        //    }
        //    else if (enableSortInFuelChamber.Value && enableForcedSort.Value && __instance.transform.parent.gameObject.name == "fuel-group")
        //    {
        //        __instance.OnSort();
        //    }
        //    else if (enableSortInStorage.Value && enableForcedSort.Value && __instance.transform.parent.gameObject.name == "Storage Window")
        //    {
        //        __instance.OnSort();
        //    }
        //    else if (enableSortInMiner.Value && enableForcedSort.Value && __instance.transform.parent.gameObject.name == "Miner Window")
        //    {
        //        Bootstrap.Debug("+++++++++++++++++++++++++++++++++HandTake　　" + __instance.transform.parent.gameObject.name);
        //        __instance.OnSort();
        //    }
        //    else if (enableSortInAssembler.Value && enableForcedSort.Value && __instance.transform.parent.gameObject.name == "Assembler Window")
        //    {
        //        Bootstrap.Debug("+++++++++++++++++++++++++++++++++HandTake　　" + __instance.transform.parent.gameObject.name);
        //        __instance.OnSort();
        //    }
        //}


        //インベントリ
        [HarmonyPostfix, HarmonyPatch(typeof(UIGame), "OpenPlayerInventory")]

        public static void UIGame_OpenPlayerInventory_Postfix(UIGame __instance)
        {

            if (enableForcedSort.Value)
            {
                __instance.inventory.OnSort();
            }
        }

        //インベントリ強制ソート
        //[HarmonyPatch(typeof(UIStorageGrid), "_OnUpdate")]
        public static class UIStorageGrid__OnUpdate_Postfix
        {
            [HarmonyPostfix]

            public static void Postfix(UIStorageGrid __instance)
            {
                if (enableSortInInventry.Value && enableForcedSort.Value && __instance.name == "Player Inventory")
                {
                    __instance.OnSort();
                    //Bootstrap.Debug("+++++++++++++++++++++++++++++++++" + __instance.name);
                }
            }
        }


        //メカ燃焼室
        [HarmonyPostfix, HarmonyPatch(typeof(UIMechaWindow), "_OnOpen")]
        public static void UIMechaWindow_OnOpen_Postfix(UIMechaWindow __instance)
        {


            if (enableSortInFuelChamber.Value)
            {
                GameMain.mainPlayer.mecha.reactorStorage.Sort(true);
            }

        }

        //メカ燃焼室強制
        //[HarmonyPatch(typeof(UIMechaWindow), "_OnUpdate")]
        public static class UIMechaWindow_OnUpdate_Postfix
        {
            [HarmonyPostfix]

            public static void Postfix(UIMechaWindow __instance)
            {


                if (enableSortInFuelChamber.Value && enableForcedSort.Value)
                {
                    GameMain.mainPlayer.mecha.reactorStorage.Sort(true);
                }

            }
        }

        //ストレージのオプションボタン作成
        //[HarmonyPatch(typeof(UIStorageWindow), "_OnInit")]
        //public static class UIStorageWindow_OnInit_Postfix
        //{
        //    [HarmonyPrefix]

        //    public static void Prefix()
        //    {
        //        Bootstrap.Debug("+++++++++++++++++++++++++++++++++ストレージのオプションボタン作成");
        //        GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box").GetComponent<RectTransform>().sizeDelta = new Vector2(85, GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box").GetComponent<RectTransform>().sizeDelta.y);　// 60，24
        //        GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box").transform.localPosition = new Vector3(219, GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box").transform.localPosition.y, 0);　// 234, 325, 0
        //        GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box/sort-btn").transform.localPosition = new Vector3(-27, 0, 0); // -15, 0, 0
        //        GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box/close-btn").transform.localPosition = new Vector3(30, 0, 0); // 15, 0. 0

        //        //ボタンの作成
        //        configButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box/sort-btn"), GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/btn-box").transform) as GameObject;
        //        configButton.name = "configButton";
        //        configButton.transform.localPosition = new Vector3(1.5f, 0, 0);
        //        configButton.transform.Find("x").localPosition = new Vector3(0, 0, 0);
        //        configButton.GetComponent<RectTransform>().sizeDelta = new Vector2(27, 18);
        //        //var sprite = Resources.Load<Sprite>("settings-icon");
        //        //Bootstrap.Debug("sprite.name" + sprite.name);

        //        //configButton.transform.Find("x").GetComponent<Image>().sprite = settingIconSprite;
        //        //Bootstrap.Debug("sprite.name 1 = " + configButton.GetComponent<Image>().sprite.name);
        //        //configButton.GetComponent<Image>().sprite = squareIconSprite;
        //        //Bootstrap.Debug("sprite.name 2 = " + configButton.GetComponent<Image>().sprite.name);
        //        //ボタンイベントの作成
        //        //configButton.GetComponent<UIButton>().button.onClick.AddListener(new UnityAction(OnSignButtonClick));

        //    }
        //}


        //ストレージ
        [HarmonyPostfix, HarmonyPatch(typeof(UIStorageWindow), "_OnOpen")]
        public static void UIStorageWindow_OnOpen_Postfix(UIStorageWindow __instance)
        {
            if (enableSortInStorage.Value)
            {
                __instance.OnSortClick();
            }
        }

        //ストレージ強制
        //[HarmonyPatch(typeof(UIStorageWindow), "_OnUpdate")]
        public static class UIStorageWindow_OnUpdate_Postfix
        {
            [HarmonyPostfix]

            public static void Postfix(UIStorageWindow __instance)
            {
                if (enableSortInStorage.Value && enableForcedSort.Value)
                {
                    __instance.OnSortClick();
                }
            }
        }



        //採掘機のインベントリ
        [HarmonyPostfix, HarmonyPatch(typeof(UIMinerWindow), "_OnOpen")]
        public static void UIMinerWindow_OnOpen_Postfix(UIMinerWindow __instance)
        {
            if (enableSortInMiner.Value)
            {
                //ref UIStorageGrid playerInventory = ref AccessTools.FieldRefAccess<UIMinerWindow, UIStorageGrid>(UIRoot.instance.uiGame.minerWindow, "playerInventory");
                if (__instance.playerInventory.active)
                {
                    ref StorageComponent Storage = ref AccessTools.FieldRefAccess<UIStorageGrid, StorageComponent>(__instance.playerInventory, "storage");
                    Storage.Sort();
                }
            }
        }

        //採掘機のインベントリ強制
        //[HarmonyPatch(typeof(UIMinerWindow), "_OnUpdate")]
        public static class UIMinerWindow__OnUpdate_Postfix
        {
            [HarmonyPostfix]
            public static void Postfix(UIMinerWindow __instance)
            {

                if (enableSortInMiner.Value && enableForcedSort.Value)
                {
                    if (__instance.playerInventory.active)
                    {
                        ref StorageComponent Storage = ref AccessTools.FieldRefAccess<UIStorageGrid, StorageComponent>(__instance.playerInventory, "storage");
                        Storage.Sort();
                    }
                }
            }
        }



        //組立機のインベントリ
        [HarmonyPostfix, HarmonyPatch(typeof(UIAssemblerWindow), "_OnOpen")]
        public static void UIAssemblerWindow_OnOpen_Postfix(UIAssemblerWindow __instance)
        {
            if (enableSortInAssembler.Value)
            {
                //ref UIStorageGrid playerInventory = ref AccessTools.FieldRefAccess<UIAssemblerWindow, UIStorageGrid>(UIRoot.instance.uiGame.assemblerWindow, "playerInventory");
                if (__instance.playerInventory.active)
                {
                    ref StorageComponent Storage = ref AccessTools.FieldRefAccess<UIStorageGrid, StorageComponent>(__instance.playerInventory, "storage");
                    Storage.Sort();
                }

            }
        }

        //組立機のインベントリ強制
        //[HarmonyPatch(typeof(UIAssemblerWindow), "_OnUpdate")]
        public static class UIAssemblerWindow_OnUpdate_Postfix
        {
            [HarmonyPostfix]

            public static void Postfix(UIAssemblerWindow __instance)
            {
                if (enableSortInAssembler.Value && enableForcedSort.Value)
                {
                    //ref UIStorageGrid playerInventory = ref AccessTools.FieldRefAccess<UIAssemblerWindow, UIStorageGrid>(UIRoot.instance.uiGame.assemblerWindow, "playerInventory");
                    if (__instance.playerInventory.active)
                    {
                        ref StorageComponent Storage = ref AccessTools.FieldRefAccess<UIStorageGrid, StorageComponent>(__instance.playerInventory, "storage");
                        Storage.Sort();
                    }

                }
            }
        }

    }

}