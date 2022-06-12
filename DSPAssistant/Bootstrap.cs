using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace DSPAssistant
{
    // [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tpxxn.plugin.Dyson.DSPAssistant", "DSPAssistant", _VERSION)]
    public class Bootstrap : BaseUnityPlugin
    {

        public const string _VERSION = "1.0.1";
        private static volatile Bootstrap instance = null;
        private static object locker = new object();
        public new static ManualLogSource Logger;// "C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\LogOutput.log"
        public static Queue buffer = new Queue();


        public static Bootstrap Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new Bootstrap();
                        }
                    }
                }
                return instance;
            }
        }

        void Start()
        {
            InitializeLogger();
            ApplyHarmonyPatches();
            //LDBTool.EditDataAction += Edit;

        }
        private void InitializeLogger()
        {
            Logger = new ManualLogSource("DSPAssistant");
            BepInEx.Logging.Logger.Sources.Add(Logger);
            DSPAssistant.ConsoleSplash();
        }

        public static ConfigEntry<bool> enableGodModeButton;
        public static ConfigEntry<bool> enableThe4DPocket;
        public static ConfigEntry<bool> enableAutoSorter;
        public static ConfigEntry<bool> enableBuildArea;
        public static ConfigEntry<bool> enableExpandTouchableRange;
        public static ConfigEntry<bool> enableIcarusInfiniteEnergy;
        public static ConfigEntry<bool> enableItemStack;

        private void ApplyHarmonyPatches()
        {
            enableGodModeButton = Instance.Config.Bind("General", "enableGodModeButton", true, new ConfigDescription("enable GodModeButton feature"));
            enableThe4DPocket = Instance.Config.Bind("General", "enableThe4DPocket", true, new ConfigDescription("enable the4DPocket feature"));
            enableAutoSorter = Instance.Config.Bind("General", "enableAutoSorter", true, new ConfigDescription("enable autoSorter feature"));
            enableBuildArea = Instance.Config.Bind("General", "enableBuildArea", true, new ConfigDescription("enable buildArea feature"));
            enableExpandTouchableRange = Instance.Config.Bind("General", "enableExpandTouchableRange", true, new ConfigDescription("enable expandTouchableRange feature"));
            enableIcarusInfiniteEnergy = Instance.Config.Bind("General", "enableIcarusInfiniteEnergy", true, new ConfigDescription("enable enableIcarusInfiniteEnergy feature"));
            enableItemStack = Instance.Config.Bind("General", "enableItemStack", true, new ConfigDescription("enable enableItemStack feature"));
            var harmony = new Harmony("dsp.assistant");
            harmony.PatchAll(typeof(Bootstrap));
            if (enableGodModeButton.Value)
            {
                var godModeButton = new GameObject("DSPAssistant/GodModeButton").AddComponent<GodModeButton>();
            }
            
            if (enableThe4DPocket.Value)
            {
                var the4DPocket = new GameObject("DSPAssistant/The4DPocket").AddComponent<The4DPocket>();
            }
            if (enableAutoSorter.Value)
            {
                var autoSorter = new GameObject("DSPAssistant/DSPAutoSorter").AddComponent<DSPAutoSorter>();
            }
            if (enableBuildArea.Value)
            {
                var buildArea = new GameObject("DSPAssistant/Icarus/BuildArea").AddComponent<BuildArea>();
            }
            if (enableExpandTouchableRange.Value)
            {
                var expandTouchableRange = new GameObject("DSPAssistant/Icarus/TouchArea").AddComponent<TouchArea>();
            }
            if (enableIcarusInfiniteEnergy.Value)
            {
                var icarusInfiniteEnergy = new GameObject("DSPAssistant/Icarus/IcarusInfiniteEnergy").AddComponent<IcarusInfiniteEnergy>();
            }
            if (enableItemStack.Value)
            {
                var icarusInfiniteEnergy = new GameObject("DSPAssistant/ItemStack").AddComponent<ItemStack>();
            }
        }

        public static void Debug(object data, LogLevel logLevel, bool isActive)
        {
            if (isActive && Logger != null)
            {
                while (buffer.Count > 0)
                {
                    var o = buffer.Dequeue();
                    var l = ((object data, LogLevel loglevel, bool isActive))o;
                    if (l.isActive) Logger.Log(l.loglevel, "Q:" + l.data);
                }

                Logger.Log(logLevel, data);
            }
            else
            {
                buffer.Enqueue((data, logLevel, true));
            }
        }

        public static void Debug(object data)
        {
            Debug(data, LogLevel.Message, true);
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
