using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using BepInEx.Configuration;
using HarmonyLib;
using StationeersMods.Interface;

namespace StackSizeMod
{
    [StationeersMod("StackSizeMod","[JIX] StackSizeMod [StationeersMods]","0.2.5370.24111.1")]
    class StackSizeMod : ModBehaviour
    {
        Dictionary<Stackable, ConfigEntry<int>> stackables = new Dictionary<Stackable, ConfigEntry<int>>();
        
        public override void OnLoaded(ContentHandler contentHandler)
        {
            Prefab.OnPrefabsLoaded += () =>
            {
                foreach (var prefab in Prefab.AllPrefabs)
                {
                    Stackable stackable =  prefab.GetComponent<Stackable>();
                    if (stackable)
                    {
                        DynamicThing dynamicThing =  prefab.GetComponent<DynamicThing>();
                        string section = dynamicThing ? dynamicThing.SortingClass.ToString() : "Other";
                        
                        ConfigEntry<int> configStackSize = Config.Bind(section,
                            prefab.name + " Stack Size",
                            stackable.MaxQuantity,
                            new ConfigDescription("Size of the stack for " + prefab.name + "\nDefault: " + stackable.MaxQuantity, new AcceptableValueRange<int>(1, 500)));
                        configStackSize.SettingChanged += (sender, args) =>
                        {
                            if(configStackSize.Value > 0)
                                stackable.MaxQuantity = configStackSize.Value;
                        };
                        
                        stackables.Add(stackable, configStackSize);
                    }
                }
            };
            
            UnityEngine.Debug.Log("StackSizeMod Loaded!");
        }
    }
}