using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ItemManager;
using PieceManager;

#pragma warning disable CS0618
namespace TotemsOfUndying
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    internal class Plugin : BaseUnityPlugin
    {
        #region variables
        public const string ModName = "TotemsOfUndying", ModVersion = "1.0.2", ModGUID = "com.Frogger." + ModName;
        private static Harmony harmony = new Harmony(ModGUID);
        public static Plugin _thistype;
        private AssetBundle _embeddedResourceBundle;
        #endregion

        #region Items
        public GameObject totemOfEikthyr;
        public GameObject totemOfTheElder;
        public GameObject totemOfBonemass;
        public GameObject totemOfModer;
        public GameObject totemOfYagluth;
        public GameObject fx_Eikthyr;
        public GameObject fx_TheElder;
        public GameObject fx_Bonemass;
        public GameObject fx_Moder;
        public GameObject fx_Yagluth;
        #endregion
        #region SE_Stats
        SE_Stats bafEikthyr;
        SE_Stats bafTheElder;
        SE_Stats bafBonemass;
        SE_Stats bafModer;
        SE_Stats bafYagluth;
        #endregion
        #region Totems
        Totem EikthyrTotem;
        Totem TheElderTotem;
        Totem BonemassTotem;
        Totem ModerTotem;
        Totem YagluthTotem;
        #endregion
        
        private void Awake()
        {
            _thistype = this;
            harmony.PatchAll();
            _embeddedResourceBundle = LoadAssetBundleFromResources("totems", Assembly.GetExecutingAssembly());
            LoadItems();

            Item eikthyrItem  = new("totems", "TotemOfEikthyr");
            eikthyrItem.Name.Russian("Тотем Эйктюра");
            eikthyrItem.Name.English("Totem Of Eikthyr");
            eikthyrItem.Description.Russian("");
            eikthyrItem.Description.English("");
            eikthyrItem.Crafting.Add("Altar", 1);
            eikthyrItem.RequiredItems.Add("GreydwarfEye", 10);
            eikthyrItem.RequiredItems.Add("SurtlingCore", 7);
            eikthyrItem.RequiredItems.Add("TrophyEikthyr", 1);
            eikthyrItem.CraftAmount = 1;
            eikthyrItem.Configurable = Configurability.Disabled;
            Item theElderItem = new("totems", "TotemOfTheElder");
            theElderItem.Name.Russian("Тотем Древнего");
            theElderItem.Name.English("Totem Of TheElder");
            theElderItem.Description.Russian("");
            theElderItem.Description.English("");
            theElderItem.Crafting.Add("Altar", 1);
            theElderItem.RequiredItems.Add("GreydwarfEye", 10);
            theElderItem.RequiredItems.Add("SurtlingCore", 7);
            theElderItem.RequiredItems.Add("TrophyTheElder", 1);
            theElderItem.CraftAmount = 1;
            theElderItem.Configurable = Configurability.Disabled;
            Item bonemassItem = new("totems", "TotemOfBonemass");
            bonemassItem.Name.Russian("Тотем Биомассы");
            bonemassItem.Name.English("Totem Of Bonemass");
            bonemassItem.Description.Russian("");
            bonemassItem.Description.English("T");
            bonemassItem.Crafting.Add("Altar", 1);
            bonemassItem.RequiredItems.Add("GreydwarfEye", 10);
            bonemassItem.RequiredItems.Add("SurtlingCore", 7);
            bonemassItem.RequiredItems.Add("TrophyBonemass", 1);
            bonemassItem.CraftAmount = 1;
            bonemassItem.Configurable = Configurability.Disabled;
            Item moderItem    = new("totems", "TotemOfModer");
            moderItem.Name.Russian("Тотем Матери");
            moderItem.Name.English("Totem Of Moder");
            moderItem.Description.Russian("");
            moderItem.Description.English("");
            moderItem.Crafting.Add("Altar", 1);
            moderItem.RequiredItems.Add("GreydwarfEye", 10);
            moderItem.RequiredItems.Add("SurtlingCore", 7);
            moderItem.RequiredItems.Add("TrophyDragonQueen", 1);
            moderItem.CraftAmount = 1;
            moderItem.Configurable = Configurability.Disabled;
            Item yagluthItem  = new("totems", "TotemOfYagluth");
            yagluthItem.Name.Russian("Тотем Яглута");
            yagluthItem.Name.English("Totem Of Yagluth");
            yagluthItem.Description.Russian("");
            yagluthItem.Description.English("");
            yagluthItem.Crafting.Add("Altar", 1);
            yagluthItem.RequiredItems.Add("GreydwarfEye", 10);
            yagluthItem.RequiredItems.Add("SurtlingCore", 7);
            yagluthItem.RequiredItems.Add("TrophyGoblinKing", 1);
            yagluthItem.CraftAmount = 1;
            yagluthItem.Configurable = Configurability.Disabled;


            BuildPiece altarBuildPiece  = new("totems", "Altar");
            altarBuildPiece.Name.English("Altar");
            altarBuildPiece.Name.Russian("Алтарь");
            altarBuildPiece.Description.English("");
            altarBuildPiece.Description.Russian("");
            altarBuildPiece.RequiredItems.Add("Stone", 20, true);
            altarBuildPiece.RequiredItems.Add("SurtlingCore", 10, true);
            altarBuildPiece.Category.Add(BuildPieceCategory.Crafting);
            altarBuildPiece.SpecialProperties.NoConfig = true;


            LoadTotems();
            LoadSE_Stats();
        }

        #region LoadingMethods
        private void LoadItems()
        {
            totemOfEikthyr = _embeddedResourceBundle.LoadAsset<GameObject>("TotemOfEikthyr");
            totemOfTheElder = _embeddedResourceBundle.LoadAsset<GameObject>("TotemOfTheElder");
            totemOfBonemass = _embeddedResourceBundle.LoadAsset<GameObject>("TotemOfBonemass");
            totemOfModer = _embeddedResourceBundle.LoadAsset<GameObject>("TotemOfModer");
            totemOfYagluth = _embeddedResourceBundle.LoadAsset<GameObject>("TotemOfYagluth");

            fx_Eikthyr = _embeddedResourceBundle.LoadAsset<GameObject>("fx_Eikthyr");
            fx_TheElder = _embeddedResourceBundle.LoadAsset<GameObject>("fx_TheElder");
            fx_Bonemass = _embeddedResourceBundle.LoadAsset<GameObject>("fx_Bonemass");
            fx_Moder = _embeddedResourceBundle.LoadAsset<GameObject>("fx_Moder");
            fx_Yagluth = _embeddedResourceBundle.LoadAsset<GameObject>("fx_Moder");
            Debug("All Items Loaded");
        }
        private void LoadSE_Stats()
        {
            bafEikthyr = new SE_Stats()
            {
                name = "TotemOfEikthyr",
                m_name = "TotemOfEikthyr",
                m_icon = EikthyrTotem.itemDrop.m_itemData.GetIcon(),
                m_ttl = 15,
                m_speedModifier = 1.05f,
                m_mods = new List<HitData.DamageModPair>()
                {
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Lightning,
                        m_modifier = HitData.DamageModifier.VeryResistant
                    }
                }
            };
            bafTheElder = new SE_Stats()
            {
                name = "TotemOfTheElder",
                m_name = "TotemOfTheElder",
                m_icon = TheElderTotem.itemDrop.m_itemData.GetIcon(),
                m_ttl = 15,
                m_speedModifier = 1.05f,
                m_modifyAttackSkill = Skills.SkillType.Axes,
                m_damageModifier = 1.2f,
                m_mods = new List<HitData.DamageModPair>()
                {
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Chop,
                        m_modifier = HitData.DamageModifier.Weak
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Pierce,
                        m_modifier = HitData.DamageModifier.Resistant
                    }
                }
            };
            bafBonemass = new SE_Stats()
            {
                name = "TotemOfBonemass",
                m_name = "TotemOfBonemass",
                m_icon = BonemassTotem.itemDrop.m_itemData.GetIcon(),
                m_ttl = 15,
                m_speedModifier = 1.05f,
                m_mods = new List<HitData.DamageModPair>() 
                {
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Poison,
                        m_modifier = HitData.DamageModifier.VeryResistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Blunt,
                        m_modifier = HitData.DamageModifier.VeryResistant
                    }
                }
            };
            bafModer = new SE_Stats()
            {
                name = "TotemOfModer",
                m_name = "TotemOfModer",
                m_icon = ModerTotem.itemDrop.m_itemData.GetIcon(),
                m_ttl = 15,
                m_speedModifier = 1.05f,
                m_mods = new List<HitData.DamageModPair>()
                {
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Frost,
                        m_modifier = HitData.DamageModifier.VeryResistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Fire,
                        m_modifier = HitData.DamageModifier.Weak
                    }
                }
            };
            bafYagluth = new SE_Stats()
            {
                name = "TotemOfYagluth",
                m_name = "TotemOfYagluth",
                m_speedModifier = 1.1f,
                m_icon = YagluthTotem.itemDrop.m_itemData.GetIcon(),
                m_ttl = 15,
                m_mods = new List<HitData.DamageModPair>()
                {
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Blunt,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Chop,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Elemental,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Fire,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Frost,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Lightning,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Physical,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Pickaxe,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Pierce,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Poison,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Slash,
                        m_modifier = HitData.DamageModifier.Resistant
                    },
                    new HitData.DamageModPair()
                    {
                        m_type = HitData.DamageType.Spirit,
                        m_modifier = HitData.DamageModifier.Resistant
                    }
                }
            };

            Debug("All SE_Stats Loaded");
        }
        private void LoadTotems()
        {
            EikthyrTotem = new Totem("TotemOfEikthyr", totemOfEikthyr, fx_Eikthyr, "TotemOfEikthyr", 25, 10, true);
            TheElderTotem = new Totem("TotemOfTheElder", totemOfTheElder, fx_TheElder, "TotemOfTheElder", 50, 25, 25, 5, Heightmap.Biome.BlackForest);
            BonemassTotem = new Totem("TotemOfBonemass", totemOfBonemass, fx_Bonemass, "TotemOfBonemass", 50, 30, 80, 20, Heightmap.Biome.Swamp);
            ModerTotem = new Totem("TotemOfModer", totemOfModer, fx_Moder, "TotemOfModer", 80, 39, 60, 45, Heightmap.Biome.Mountain, Heightmap.Biome.DeepNorth);
            YagluthTotem = new Totem("TotemOfYagluth", totemOfYagluth, fx_Yagluth, "TotemOfYagluth", 150, 111, true);
            Debug("All Totems Loaded");
        }
        #endregion

        private void AddStatusEffects()
        {
            if (ObjectDB.instance == null || ObjectDB.instance.m_items.Count == 0 || ObjectDB.instance.GetItemPrefab("Amber") == null) return;

            #region StatusEffects
            if (!ObjectDB.instance.m_StatusEffects.Contains(bafEikthyr)) ObjectDB.instance.m_StatusEffects.Add(bafEikthyr);
            if (!ObjectDB.instance.m_StatusEffects.Contains(bafTheElder))ObjectDB.instance.m_StatusEffects.Add(bafTheElder);
            if (!ObjectDB.instance.m_StatusEffects.Contains(bafBonemass))ObjectDB.instance.m_StatusEffects.Add(bafBonemass);
            if (!ObjectDB.instance.m_StatusEffects.Contains(bafModer))ObjectDB.instance.m_StatusEffects.Add(bafModer);
            if (!ObjectDB.instance.m_StatusEffects.Contains(bafYagluth))ObjectDB.instance.m_StatusEffects.Add(bafYagluth);
            #endregion

            Debug("Effects Added");
        }

        public void UseTotem(ItemDrop.ItemData itemData, string totemName)
        {
            if (totemName == "$item_TotemOfEikthyr")
            {
                EikthyrTotem.Use(itemData, Player.m_localPlayer.GetCurrentBiome());
                return;
            }
            else if (totemName == "$item_TotemOfTheElder")
            {
                TheElderTotem.Use(itemData, Player.m_localPlayer.GetCurrentBiome());
                return;
            }
            else if (totemName == "$item_TotemOfBonemass")
            {
                BonemassTotem.Use(itemData, Player.m_localPlayer.GetCurrentBiome());
                return;
            }
            else if (totemName == "$item_TotemOfModer")
            {
                ModerTotem.Use(itemData, Player.m_localPlayer.GetCurrentBiome());
                return;
            }
            else if (totemName == "$item_TotemOfYagluth")
            {
                YagluthTotem.Use(itemData, Player.m_localPlayer.GetCurrentBiome());
                return;
            }
            else
            {
                Debug("unknown totem");
            }
        }
        
        public AssetBundle LoadAssetBundleFromResources(string bundleName, Assembly resourceAssembly)
        {
            if (resourceAssembly == null)
            {
                throw new ArgumentNullException("Parameter resourceAssembly can not be null.");
            }
            string text = null;
            try
            {
                text = resourceAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(bundleName));
            }
            catch (Exception)
            {
            }
            if (text == null)
            {
                Logger.LogError("AssetBundle " + bundleName + " not found in assembly manifest");
                return null;
            }
            AssetBundle result;
            using (Stream manifestResourceStream = resourceAssembly.GetManifestResourceStream(text))
            {
                result = AssetBundle.LoadFromStream(manifestResourceStream);
            }
            return result;
        }
        public void Debug(string msg)
        {
            Logger.LogInfo(msg);
        }
        public void DebugError(string msg)
        {
            Logger.LogError(msg);
        }


        [HarmonyPatch] public static class Path
        {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(Character), "CheckDeath")]
            private static void CharacterDeathPatch(Character __instance)
            {
                bool isFocused = Application.isFocused;

                if (isFocused && __instance.IsPlayer() && !__instance.IsDead())
                {
                    Inventory inventory = Player.m_localPlayer.GetInventory();
                    if (__instance.GetHealth() <= 0f)
                    {
                        foreach (var item in inventory.GetAllItems())
                        {
                            if (item.m_shared.m_name.StartsWith("$item_TotemOf"))
                            {
                                _thistype.UseTotem(item, item.m_shared.m_name);
                                return;
                            }
                        }
                        foreach (var item in inventory.GetAllItems())
                        {
                            if (item == _thistype.EikthyrTotem.itemDrop.m_itemData && item == _thistype.TheElderTotem.itemDrop.m_itemData && item == _thistype.BonemassTotem.itemDrop.m_itemData && item == _thistype.ModerTotem.itemDrop.m_itemData && item == _thistype.YagluthTotem.itemDrop.m_itemData)
                            {
                                _thistype.UseTotem(item, item.m_shared.m_name);
                                return;
                            }
                        }
                    }
                }

            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(ObjectDB), "Awake")]
            [HarmonyPriority(Priority.Last)]
            private static void ObjectDBAwakePatch()
            {
                _thistype.Logger.LogInfo("ObjectDB Awake");
                _thistype.AddStatusEffects();
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
            [HarmonyPriority(Priority.Last)]
            private static void ObjectDBCopyOtherDBPatch()
            {
                _thistype.Logger.LogInfo("CopyOtherDB");
                _thistype.AddStatusEffects();
            }

        }
        public class Totem
        {
            #region Values
            public string name { get; }
            public int healthAfterDieInRightBiome { get; }
            public int staminaAfterDieInRightBiome { get; }
            public int healthAfterDieInWrongBiome { get; }
            public int staminaAfterDieInWrongBiome { get; }
            public Heightmap.Biome biomeMain { get; }
            public Heightmap.Biome biome1 { get; }
            public Heightmap.Biome biome2 { get; }
            public bool everyBiome { get; }
            public GameObject gameObject { get; }
            public ItemDrop itemDrop { get; }
            public GameObject fx { get; }
            public string bafName { get; }
            #endregion

            #region Сonstructors
            public Totem(string name, GameObject gameObject, GameObject fx, string bafName, int healthAfterDieInRightBiome, int healthAfterDieInWrongBiome, int staminaAfterDieInRightBiome, int staminaAfterDieInWrongBiome, Heightmap.Biome biomeMain)
            {
                this.name = name;
                this.gameObject = gameObject;
                this.fx = fx;
                this.bafName = bafName;
                this.healthAfterDieInRightBiome = healthAfterDieInRightBiome;
                this.healthAfterDieInWrongBiome = healthAfterDieInWrongBiome;
                this.staminaAfterDieInRightBiome = staminaAfterDieInRightBiome;
                this.staminaAfterDieInWrongBiome = staminaAfterDieInWrongBiome;
                this.biomeMain = biomeMain;

                itemDrop = gameObject.GetComponent<ItemDrop>();
            }
            public Totem(string name, GameObject gameObject, GameObject fx, string bafName, int healthAfterDie, int staminaAfterDie, bool everyBiome)
            {
                this.name = name;
                this.gameObject = gameObject;
                this.fx = fx;
                this.bafName = bafName;
                this.healthAfterDieInRightBiome = healthAfterDie;
                this.staminaAfterDieInRightBiome = staminaAfterDie;
                this.everyBiome = everyBiome;

                itemDrop = gameObject.GetComponent<ItemDrop>();
            }
            public Totem(string name, GameObject gameObject, GameObject fx, string bafName, int healthAfterDieInRightBiome, int healthAfterDieInWrongBiome, int staminaAfterDieInRightBiome, int staminaAfterDieInWrongBiome, Heightmap.Biome biome1, Heightmap.Biome biome2)
            {
                this.name = name;
                this.gameObject = gameObject;
                this.fx = fx;
                this.bafName = bafName;
                this.healthAfterDieInRightBiome = healthAfterDieInRightBiome;
                this.healthAfterDieInWrongBiome = healthAfterDieInWrongBiome;
                this.staminaAfterDieInRightBiome = staminaAfterDieInRightBiome;
                this.staminaAfterDieInWrongBiome = staminaAfterDieInWrongBiome;
                this.biome1 = biome1;
                this.biome2 = biome2;

                itemDrop = gameObject.GetComponent<ItemDrop>();
            }
            #endregion

            public void Use(ItemDrop.ItemData itemData, Heightmap.Biome biome)
            {
                Inventory inventory = Player.m_localPlayer.GetInventory();

                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, name);
                if ((biome == biomeMain || biome == biome1 || biome == biome2) || everyBiome)
                {
                    Player.m_localPlayer.SetHealth(healthAfterDieInRightBiome);
                    Player.m_localPlayer.AddStamina(staminaAfterDieInRightBiome);
                    Player.m_localPlayer.m_seman.AddStatusEffect(bafName, true);
                }
                else
                {
                    Player.m_localPlayer.SetHealth(healthAfterDieInWrongBiome);
                    Player.m_localPlayer.AddStamina(staminaAfterDieInWrongBiome);
                }

                Instantiate(fx, new Vector3(Player.m_localPlayer.transform.position.x, Player.m_localPlayer.transform.position.y + 1.4f, Player.m_localPlayer.transform.position.z), Quaternion.identity);
                inventory.RemoveItem(itemData, 1);
            }
        }
    }
}