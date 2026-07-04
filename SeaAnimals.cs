using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CreatureManager;
using HarmonyLib;
using HarmonyLibs;
using ItemManager;
using PieceManager;
using ServerSync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.UI.GridLayoutGroup;
using Random = UnityEngine.Random;

namespace SeaAnimals
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class SeaAnimalsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "SeaAnimals";
        internal const string ModVersion = "0.3.6";
        internal const string Author = "marlthon";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        private readonly Harmony _harmony = new(ModGUID);

        private static Dictionary<GameObject, GameObject> seaanimalscarnes = new();

        public static readonly ManualLogSource SeaAnimals =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        // DEUS SEJA LOUVADO!

        public void Awake()
        {
            HarmonyCore.Instance.Init("SeaAnimals");
            HarmonyCore.Instance.Ciano("Marlthon Mods");
            HarmonyCore.Instance.Verde("Download more mods at marlthon.com");

            // Needed for ServerSync to add locking of config toggle
            _serverConfigLocked = config("General", "Force Server Config", true, "Force Server Config");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            #region ORCA
            Creature SA_Orca = new Creature("mar_seaanimals", "SA_Orca")
                .ConfigureBiome(Heightmap.Biome.Ocean | Heightmap.Biome.DeepNorth)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureFoodItems("SA_BlueSharkMeat, SA_CrocoMeat, SA_HumboldMeat, SA_WhaleMeat, SA_SeaTurtleMeat, SA_SharkMeat, SA_TurtleMeat, SerpentMeat")
                .ConfigureForestSpawn(Forest.No)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledEikthyr)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 2f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(true)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(500f)
                .ConfigureRegenAllHpTime(3600f);

            SA_Orca.Localize()
                .Portuguese_Brazilian("Orca")
                .English("Orca");

            SA_Orca.ConfigureDrops();
            SA_Orca.Drops["SA_WhaleMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_Orca.Drops["SA_WhaleMeat"].DropChance = 100f;

            GameObject Orca_Attack_Bite1_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Orca_Attack_Bite1_marinelife");
            GameObject Orca_Attack_Jump_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Orca_Attack_Jump_marinelife");
            GameObject sfx_alert_orca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_alert_orca_marinelife");
            GameObject sfx_death_orca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_death_orca_marinelife");
            GameObject sfx_idle_orca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_idle_orca_marinelife");
            GameObject sfx_littleorca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_littleorca_marinelife");
            GameObject sfx_pet_orca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_pet_orca_marinelife");
            GameObject sfx_tamed_orca_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_tamed_orca_marinelife");
            GameObject sfx_orcasoundhit_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_orcasoundhit_marinelife");
            #endregion

            #region DOLPHIN
            Creature SA_Dolphin = new Creature("mar_seaanimals", "SA_Dolphin")
                .ConfigureBiome(Heightmap.Biome.Ocean | Heightmap.Biome.DeepNorth)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureFoodItems("SA_BlueSharkMeat, SA_CrocoMeat, SA_HumboldMeat, SA_WhaleMeat, SA_SeaTurtleMeat, SA_SharkMeat, SA_TurtleMeat, SerpentMeat")
                .ConfigureForestSpawn(Forest.No)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledEikthyr)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 3f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(true)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(400f)
                .ConfigureRegenAllHpTime(3600f);

            SA_Dolphin.Localize()
                .Portuguese_Brazilian("Golfinho")
                .English("Dolphin");

            SA_Dolphin.ConfigureDrops();
            SA_Dolphin.Drops["SA_WhaleMeat"].Amount = new CreatureManager.Range(1, 2);
            SA_Dolphin.Drops["SA_WhaleMeat"].DropChance = 100f;

            GameObject Attack_01_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_01_dolphin_marinelife");
            GameObject sfx_birth_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_birth_dolphin_marinelife");
            GameObject sfx_death_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_death_dolphin_marinelife");
            GameObject sfx_idle_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_idle_dolphin_marinelife");
            GameObject sfx_love_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_love_dolphin_marinelife");
            GameObject sfx_pet_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_pet_dolphin_marinelife");
            GameObject sfx_tamed_dolphin_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_tamed_dolphin_marinelife");
            #endregion

            #region CROCODILO
            Creature SA_Crocodile = new Creature("mar_seaanimals", "SA_Crocodile")
                .ConfigureBiome(Heightmap.Biome.BlackForest | Heightmap.Biome.Swamp)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1, 2f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureFoodItems("SA_BlueSharkMeat, SA_CrocoMeat, SA_HumboldMeat, SA_WhaleMeat, SA_SeaTurtleMeat, SA_SharkMeat, SA_TurtleMeat, SerpentMeat")
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledEikthyr)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(true)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(200f)
                .ConfigureRegenAllHpTime(3600f);

            SA_Crocodile.Localize()
                .Portuguese_Brazilian("Crocodilo")
                .English("Crocodile");

            SA_Crocodile.ConfigureDrops();
            SA_Crocodile.Drops["SA_CrocoMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_Crocodile.Drops["SA_CrocoMeat"].DropChance = 100f;

            GameObject Attack_Crocodile01_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_Crocodile01_marinelife");
            GameObject Attack_Crocodile02_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_Crocodile02_marinelife");
            GameObject Attack_Crocodile03_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_Crocodile03_marinelife");
            GameObject Attack_Crocodile04_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_Crocodile04_marinelife");
            GameObject sfx_alert_Crocodile_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_alert_Crocodile_marinelife");
            GameObject sfx_attack_crocodile_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_attack_crocodile_marinelife");
            GameObject sfx_death_crocodile_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_death_crocodile_marinelife");
            GameObject sfx_Idle_Crocodile_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_Idle_Crocodile_marinelife");
            #endregion

            #region WHITE SHARK
            Creature SA_WhiteShark = new Creature("mar_seaanimals", "SA_WhiteShark")
                .ConfigureBiome(Heightmap.Biome.Ocean | Heightmap.Biome.DeepNorth)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureFoodItems("SA_BlueSharkMeat, SA_CrocoMeat, SA_HumboldMeat, SA_WhaleMeat, SA_SeaTurtleMeat, SA_SharkMeat, SA_TurtleMeat, SerpentMeat")
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledBonemass)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(400f)
                .ConfigureRegenAllHpTime(3600f);

            SA_WhiteShark.Localize()
                .Portuguese_Brazilian("Tubarão Branco")
                .English("White Shark");

            SA_WhiteShark.ConfigureDrops();
            SA_WhiteShark.Drops["SA_SharkMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_WhiteShark.Drops["SA_SharkMeat"].DropChance = 100f;

            GameObject WhiteShark_Attack_Bite1_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "WhiteShark_Attack_Bite1_marinelife");
            // GameObject WhiteShark_Attack_Bite2_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "WhiteShark_Attack_Bite2_marinelife");
            GameObject WhiteShark_Attack_Jump_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "WhiteShark_Attack_Jump_marinelife");

            GameObject sfx_attack_whiteshark_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_attack_whiteshark_marinelife");
            GameObject sfx_pet_whiteshark_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_pet_whiteshark_marinelife");
            GameObject sfx_tamed_whiteshark_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_tamed_whiteshark_marinelife");
            #endregion

            #region HUMBOLDT SQUID
            Creature SA_HumboldtSquid = new Creature("mar_seaanimals", "SA_HumboldtSquid")
                .ConfigureBiome(Heightmap.Biome.Ocean)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 3f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(250f)
                .ConfigureRegenAllHpTime(3600f);

            SA_HumboldtSquid.Localize()
                .Portuguese_Brazilian("Luladrão")
                .English("Humboldt Squid");

            SA_HumboldtSquid.ConfigureDrops();
            SA_HumboldtSquid.Drops["SA_HumboldMeat"].Amount = new CreatureManager.Range(1, 2);
            SA_HumboldtSquid.Drops["SA_HumboldMeat"].DropChance = 100f;

            GameObject Attack_HumboldtSquid = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_HumboldtSquid");
            GameObject HumboldtSquid_Aoe = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "HumboldtSquid_Aoe");

            GameObject sfx_humboldtsquid_attack = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_humboldtsquid_attack");
            GameObject sfx_humboldtsquid_attack_hit = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_humboldtsquid_attack_hit");
            GameObject vfx_humboldtsquid_pray = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_humboldtsquid_pray");
            #endregion

            #region LEATHERBACK SEATURTLE
            Creature SA_LeatherbackSeaTurtle = new Creature("mar_seaanimals", "SA_LeatherbackSeaTurtle")
                .ConfigureBiome(Heightmap.Biome.Ocean)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 3f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(350f)
                .ConfigureRegenAllHpTime(3600f);

            SA_LeatherbackSeaTurtle.Localize()
                .Portuguese_Brazilian("Tartaruga de Couro")
                .English("Leatherback SeaTurtle");

            SA_LeatherbackSeaTurtle.ConfigureDrops();
            SA_LeatherbackSeaTurtle.Drops["SA_SeaTurtleMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_LeatherbackSeaTurtle.Drops["SA_SeaTurtleMeat"].DropChance = 100f;
            #endregion

            #region RIGHT WHALE
            Creature SA_RightWhale = new Creature("mar_seaanimals", "SA_RightWhale")
                .ConfigureBiome(Heightmap.Biome.Ocean | Heightmap.Biome.DeepNorth)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(500f)
                .ConfigureRegenAllHpTime(3600f);

            SA_RightWhale.Localize()
                .Portuguese_Brazilian("Baleia Franca")
                .English("Right Whale");

            SA_RightWhale.ConfigureDrops();
            SA_RightWhale.Drops["SA_WhaleMeat"].Amount = new CreatureManager.Range(2, 4);
            SA_RightWhale.Drops["SA_WhaleMeat"].DropChance = 100f;
            #endregion

            #region WHALE SHARK
            Creature SA_WhaleShark = new Creature("mar_seaanimals", "SA_WhaleShark")
                .ConfigureBiome(Heightmap.Biome.Ocean)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-1000f, 0f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(400f)
                .ConfigureRegenAllHpTime(3600f);

            SA_WhaleShark.Localize()
                .Portuguese_Brazilian("Tubarão Baleia")
                .English("Whale Shark");

            SA_WhaleShark.ConfigureDrops();
            SA_WhaleShark.Drops["SA_WhaleMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_WhaleShark.Drops["SA_WhaleMeat"].DropChance = 100f;
            #endregion

            #region BLUE SHARK
            Creature SA_BlueShark = new Creature("mar_seaanimals", "SA_BlueShark")
                .ConfigureBiome(Heightmap.Biome.Meadows)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-4, -1f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(30f)
                .ConfigureRegenAllHpTime(3600f);

            SA_BlueShark.Localize()
                .Portuguese_Brazilian("Tubarão Azul")
                .English("Blue Shark");

            SA_BlueShark.ConfigureDrops();
            SA_BlueShark.Drops["SA_BlueSharkMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_BlueShark.Drops["SA_BlueSharkMeat"].DropChance = 100f;

            GameObject Attack_BlueShark01_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_BlueShark01_marinelife");
            #endregion

            #region HAMMER HEAD SHARK
            Creature SA_HammerHeadShark = new Creature("mar_seaanimals", "SA_HammerHeadShark")
                .ConfigureBiome(Heightmap.Biome.Plains)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-5, -1f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledBonemass)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(300f)
                .ConfigureRegenAllHpTime(3600f);

            SA_HammerHeadShark.Localize()
                .Portuguese_Brazilian("Tubarão Martelo")
                .English("HammerHead Shark");

            SA_HammerHeadShark.ConfigureDrops();
            SA_HammerHeadShark.Drops["SA_SharkMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_HammerHeadShark.Drops["SA_SharkMeat"].DropChance = 100f;

            GameObject Attack_HammerHeadShark01_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_HammerHeadShark01_marinelife");
            GameObject Attack_HammerHeadShark02_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_HammerHeadShark02_marinelife");
            GameObject Attack_HammerHeadShark03_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_HammerHeadShark03_marinelife");
            #endregion

            #region TIGER SHARK
            Creature SA_TigerShark = new Creature("mar_seaanimals", "SA_TigerShark")
                .ConfigureBiome(Heightmap.Biome.Mistlands)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-10, -1f))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0f, 0f))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.KilledBonemass)
                .ConfigureSpawnChance(10f)
                .ConfigureGroupSize(new CreatureManager.Range(1f, 1f))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Day)
                .ConfigureMaximum(2)
                .EnableTaming(false, isConfigurable: false)
                .EnableStars(true)
                .EnableSpawning(true)
                .ConfigureHealth(350f)
                .ConfigureRegenAllHpTime(3600f);

            SA_TigerShark.Localize()
                .Portuguese_Brazilian("Tubarão Tigre")
                .English("Tiger Shark");

            SA_TigerShark.ConfigureDrops();
            SA_TigerShark.Drops["SA_SharkMeat"].Amount = new CreatureManager.Range(1, 3);
            SA_TigerShark.Drops["SA_SharkMeat"].DropChance = 100f;

            GameObject Attack_TigerShark01_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_TigerShark01_marinelife");
            GameObject Attack_TigerShark02_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_TigerShark02_marinelife");
            GameObject Attack_TigerShark03_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "Attack_TigerShark03_marinelife");
            #endregion

            #region TURTLES
            Creature SA_BlueTurtle = new Creature("mar_seaanimals", "SA_BlueTurtle")
                .ConfigureBiome(Heightmap.Biome.BlackForest)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-5, -1))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0, 0))
                .ConfigureRequiredWeather(Weather.ClearSkies | Weather.LightRain | Weather.ClearThunderStorm | Weather.BlackForestFog)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10)
                .ConfigureGroupSize(new CreatureManager.Range(1, 3))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Always)
                .ConfigureMaximum(3)
                .EnableTaming(false, isConfigurable: false)
                .EnableSpawning(true)
                .ConfigureHealth(80f)
                .ConfigureRegenAllHpTime(3600f);


            SA_BlueTurtle.Localize()
                .Portuguese_Brazilian("Tartaruga Azul")
                .English("Blue Turtle");

            SA_BlueTurtle.ConfigureDrops();
            SA_BlueTurtle.Drops["SA_TurtleMeat"].Amount = new CreatureManager.Range(1, 1);
            SA_BlueTurtle.Drops["SA_TurtleMeat"].DropChance = 100f;

            Creature SA_GreenTurtle = new Creature("mar_seaanimals", "SA_GreenTurtle")
                .ConfigureBiome(Heightmap.Biome.Meadows)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-5, -1))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0, 0))
                .ConfigureRequiredWeather(Weather.ClearSkies | Weather.LightRain | Weather.ClearThunderStorm | Weather.MeadowsClearSkies)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10)
                .ConfigureGroupSize(new CreatureManager.Range(1, 3))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Always)
                .ConfigureMaximum(3)
                .EnableSpawning(true)
                .ConfigureHealth(40f)
                .ConfigureRegenAllHpTime(3600f);

            SA_GreenTurtle.Localize()
                .Portuguese_Brazilian("Tartaruga Verde")
                .English("Green Turtle");

            SA_GreenTurtle.ConfigureDrops();
            SA_GreenTurtle.Drops["SA_TurtleMeat"].Amount = new CreatureManager.Range(1, 1);
            SA_GreenTurtle.Drops["SA_TurtleMeat"].DropChance = 100f;

            Creature SA_RedTurtle = new Creature("mar_seaanimals", "SA_RedTurtle")
                .ConfigureBiome(Heightmap.Biome.Plains)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-5, -1))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0, 0))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10)
                .ConfigureGroupSize(new CreatureManager.Range(1, 3))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Always)
                .ConfigureMaximum(3)
                .EnableTaming(false, isConfigurable: false)
                .EnableSpawning(true)
                .ConfigureHealth(160f)
                .ConfigureRegenAllHpTime(3600f);

            SA_RedTurtle.Localize()
                .Portuguese_Brazilian("Tartaruga Vermelha")
                .English("Red Turtle");

            SA_RedTurtle.ConfigureDrops();
            SA_RedTurtle.Drops["SA_TurtleMeat"].Amount = new CreatureManager.Range(1, 2);
            SA_RedTurtle.Drops["SA_TurtleMeat"].DropChance = 100f;

            Creature SA_YellowTurtle = new Creature("mar_seaanimals", "SA_YellowTurtle")
                .ConfigureBiome(Heightmap.Biome.Mistlands)
                .ConfigureSpawnArea(CreatureManager.SpawnArea.Everywhere)
                .ConfigureRequiredAltitude(new CreatureManager.Range(-5, -1))
                .ConfigureRequiredOceanDepth(new CreatureManager.Range(0, 0))
                .ConfigureRequiredWeather(Weather.None)
                .ConfigureForestSpawn(Forest.Both)
                .ConfigureRequiredGlobalKey(GlobalKey.None)
                .ConfigureSpawnChance(10)
                .ConfigureGroupSize(new CreatureManager.Range(1, 3))
                .ConfigureSpawnInterval(1000)
                .ConfigureSpawnTime(SpawnTime.Always)
                .ConfigureMaximum(3)
                .EnableTaming(false, isConfigurable: false)
                .EnableSpawning(true)
                .ConfigureHealth(250f)
                .ConfigureRegenAllHpTime(3600f);


            SA_YellowTurtle.Localize()
                .Portuguese_Brazilian("Tartaruga Amarela")
                .English("Yellow Turtle");

            SA_YellowTurtle.ConfigureDrops();
            SA_YellowTurtle.Drops["SA_TurtleMeat"].Amount = new CreatureManager.Range(1, 2);
            SA_YellowTurtle.Drops["SA_TurtleMeat"].DropChance = 100f;

            GameObject SA_blue_atk = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "SA_blue_atk");
            GameObject SA_green_atk = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "SA_green_atk");
            GameObject SA_red_atk = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "SA_red_atk");
            GameObject SA_yellow_atk = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "SA_yellow_atk");
            #endregion

            #region CARNES

            Item SA_BlueSharkMeat = new("mar_seaanimals", "SA_BlueSharkMeat");
            SA_BlueSharkMeat.Name.Portuguese_Brazilian("Carne de Tubarão Azul");
            SA_BlueSharkMeat.Name.English("Blue Shark Meat");
            SA_BlueSharkMeat.Description.Portuguese_Brazilian("Carne crua de Tubarão Azul.");
            SA_BlueSharkMeat.Description.English("Raw Blue Shark meat.");
            SA_BlueSharkMeat.Configurable = Configurability.Disabled;

            Item SA_RoastBlueSharkMeat = new("mar_seaanimals", "SA_RoastBlueSharkMeat");
            SA_RoastBlueSharkMeat.Name.Portuguese_Brazilian("Carne de Tubarão Azul Assada");
            SA_RoastBlueSharkMeat.Name.English("Roasted Blue Shark Meat");
            SA_RoastBlueSharkMeat.Description.Portuguese_Brazilian("Carne de Tubarão Azul assada.");
            SA_RoastBlueSharkMeat.Description.English("Roasted Blue Shark meat.");
            SA_RoastBlueSharkMeat.Configurable = Configurability.Stats;

            Item SA_CrocoMeat = new("mar_seaanimals", "SA_CrocoMeat");
            SA_CrocoMeat.Name.Portuguese_Brazilian("Carne de Crocodilo");
            SA_CrocoMeat.Name.English("Crocodile Meat");
            SA_CrocoMeat.Description.Portuguese_Brazilian("Carne crua de Crocodilo.");
            SA_CrocoMeat.Description.English("Raw Crocodile meat.");
            SA_CrocoMeat.Configurable = Configurability.Disabled;

            Item SA_CrocoMeatCook = new("mar_seaanimals", "SA_CrocoMeatCook");
            SA_CrocoMeatCook.Name.Portuguese_Brazilian("Carne de Crocodilo Assada");
            SA_CrocoMeatCook.Name.English("Roasted Crocodile Meat");
            SA_CrocoMeatCook.Description.Portuguese_Brazilian("Carne de Crocodilo assada.");
            SA_CrocoMeatCook.Description.English("Roasted Crocodile meat.");
            SA_CrocoMeatCook.Configurable = Configurability.Stats;

            Item SA_HumboldMeat = new("mar_seaanimals", "SA_HumboldMeat");
            SA_HumboldMeat.Name.Portuguese_Brazilian("Carne de Luladrão");
            SA_HumboldMeat.Name.English("Humboldt Squid Meat");
            SA_HumboldMeat.Description.Portuguese_Brazilian("Carne de Luladrão");
            SA_HumboldMeat.Description.English("Raw Humboldt Squid meat.");
            SA_HumboldMeat.Configurable = Configurability.Disabled;

            Item SA_HumboldMeatCooked = new("mar_seaanimals", "SA_HumboldMeatCooked");
            SA_HumboldMeatCooked.Name.Portuguese_Brazilian("Carne de Luladrão Assada");
            SA_HumboldMeatCooked.Name.English("Roasted Humboldt Squid Meat");
            SA_HumboldMeatCooked.Description.Portuguese_Brazilian("Carne de Luladrão assada.");
            SA_HumboldMeatCooked.Description.English("Roasted Humboldt Squid meat.");
            SA_HumboldMeatCooked.Configurable = Configurability.Stats;

            Item SA_WhaleMeat = new("mar_seaanimals", "SA_WhaleMeat");
            SA_WhaleMeat.Name.Portuguese_Brazilian("Carne de Baleia");
            SA_WhaleMeat.Name.English("Whale Meat");
            SA_WhaleMeat.Description.Portuguese_Brazilian("Carne crua de Baleia.");
            SA_WhaleMeat.Description.English("Raw Whale meat.");
            SA_WhaleMeat.Configurable = Configurability.Disabled;

            Item SA_RoastWhaleMeat = new("mar_seaanimals", "SA_RoastWhaleMeat");
            SA_RoastWhaleMeat.Name.Portuguese_Brazilian("Carne de Baleia Assada");
            SA_RoastWhaleMeat.Name.English("Roasted Whale Meat");
            SA_RoastWhaleMeat.Description.Portuguese_Brazilian("Carne de Baleia assada.");
            SA_RoastWhaleMeat.Description.English("Roasted Whale meat.");
            SA_RoastWhaleMeat.Configurable = Configurability.Stats;

            Item SA_SeaTurtleMeat = new("mar_seaanimals", "SA_SeaTurtleMeat");
            SA_SeaTurtleMeat.Name.Portuguese_Brazilian("Carne de Tartaruga Marinha");
            SA_SeaTurtleMeat.Name.English("Sea Turtle Meat");
            SA_SeaTurtleMeat.Description.Portuguese_Brazilian("Carne crua de Tartaruga Marinha.");
            SA_SeaTurtleMeat.Description.English("Raw Sea Turtle meat.");
            SA_SeaTurtleMeat.Configurable = Configurability.Disabled;

            Item SA_SeaTurtleMeatCooked = new("mar_seaanimals", "SA_SeaTurtleMeatCooked");
            SA_SeaTurtleMeatCooked.Name.Portuguese_Brazilian("Carne de Tartaruga Marinha Assada");
            SA_SeaTurtleMeatCooked.Name.English("Roasted Sea Turtle Meat");
            SA_SeaTurtleMeatCooked.Description.Portuguese_Brazilian("Carne de Tartaruga Marinha assada.");
            SA_SeaTurtleMeatCooked.Description.English("Roasted Sea Turtle meat.");
            SA_SeaTurtleMeatCooked.Configurable = Configurability.Stats;

            Item SA_SharkMeat = new("mar_seaanimals", "SA_SharkMeat");
            SA_SharkMeat.Name.Portuguese_Brazilian("Carne de Tubarão");
            SA_SharkMeat.Name.English("Shark Meat");
            SA_SharkMeat.Description.Portuguese_Brazilian("Carne crua de Tubarão.");
            SA_SharkMeat.Description.English("Raw Shark meat.");
            SA_SharkMeat.Configurable = Configurability.Disabled;

            Item SA_RoastSharkMeat = new("mar_seaanimals", "SA_RoastSharkMeat");
            SA_RoastSharkMeat.Name.Portuguese_Brazilian("Carne de Tubarão Assada");
            SA_RoastSharkMeat.Name.English("Roasted Shark Meat");
            SA_RoastSharkMeat.Description.Portuguese_Brazilian("Carne de Tubarão assada.");
            SA_RoastSharkMeat.Description.English("Roasted Shark meat.");
            SA_RoastSharkMeat.Configurable = Configurability.Stats;

            Item SA_TurtleMeat = new("mar_seaanimals", "SA_TurtleMeat");
            SA_TurtleMeat.Name.Portuguese_Brazilian("Carne de Tartaruga");
            SA_TurtleMeat.Name.English("Turtle Meat");
            SA_TurtleMeat.Description.Portuguese_Brazilian("Carne crua de Tartaruga.");
            SA_TurtleMeat.Description.English("Raw Turtle meat.");
            SA_TurtleMeat.Configurable = Configurability.Disabled;

            Item SA_TurtleMeatCook = new("mar_seaanimals", "SA_TurtleMeatCook");
            SA_TurtleMeatCook.Name.Portuguese_Brazilian("Carne de Tartaruga Assada");
            SA_TurtleMeatCook.Name.English("Roasted Turtle Meat");
            SA_TurtleMeatCook.Description.Portuguese_Brazilian("Carne de Tartaruga assada.");
            SA_TurtleMeatCook.Description.English("Roasted Turtle meat.");
            SA_TurtleMeatCook.Configurable = Configurability.Stats;

            // Adicionar itens à lista de conversão
            // O formato é De: Para:
            seaanimalscarnes.Add(SA_BlueSharkMeat.Prefab, SA_RoastBlueSharkMeat.Prefab);
            seaanimalscarnes.Add(SA_CrocoMeat.Prefab, SA_CrocoMeatCook.Prefab);
            seaanimalscarnes.Add(SA_HumboldMeat.Prefab, SA_HumboldMeatCooked.Prefab);
            seaanimalscarnes.Add(SA_WhaleMeat.Prefab, SA_RoastWhaleMeat.Prefab);
            seaanimalscarnes.Add(SA_SeaTurtleMeat.Prefab, SA_SeaTurtleMeatCooked.Prefab);
            seaanimalscarnes.Add(SA_SharkMeat.Prefab, SA_RoastSharkMeat.Prefab);
            seaanimalscarnes.Add(SA_TurtleMeat.Prefab, SA_TurtleMeatCook.Prefab);

            #endregion

            #region MARITIME SADLE

            Item SA_Sadle = new("mar_seaanimals", "SA_Sadle"); //assetbundle name, Asset Name
            SA_Sadle.Name.Portuguese_Brazilian("Sela Marítima");
            SA_Sadle.Name.English("Maritime Saddle");
            SA_Sadle.Description.Portuguese_Brazilian("Sela Marítima usada para montaria em Orcas, Golfinhos, Crocodilo e Tubarão Branco.");
            SA_Sadle.Description.English("Maritime Saddle used for riding in Orca, Dolphin, Crocodile and White Shark.");
            SA_Sadle.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
            SA_Sadle.RequiredItems.Add("Wood", 2);
            SA_Sadle.RequiredItems.Add("DeerHide", 5);
            SA_Sadle.RequiredItems.Add("LeatherScraps", 5);

            #endregion

            #region REGISTRO DE EFEITOS E PREFABS

            // EFEITOS COMUNS SFX
            GameObject sfx_build_hammer_wood_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_build_hammer_wood_marinelife");
            GameObject sfx_wood_destroyed_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_wood_destroyed_marinelife");
            GameObject sfx_seaturtle_attack_hit = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_seaturtle_attack_hit");

            // EFEITOS COMUNS FX
            GameObject fx_backstab_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "fx_backstab_marinelife");
            GameObject fx_crit_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "fx_crit_marinelife");
            GameObject fx_pet_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "fx_pet_marinelife");
            GameObject fx_tamed_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "fx_tamed_marinelife");

            // EFEITOS COMUNS VFX
            GameObject vfx_birth_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_birth_marinelife");
            GameObject vfx_corpse_medium_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_corpse_medium_marinelife");
            GameObject vfx_corpse_mini_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_corpse_mini_marinelife");
            GameObject vfx_corpse_small_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_corpse_small_marinelife");
            GameObject vfx_death_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_death_marinelife");
            GameObject vfx_hit_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_hit_marinelife");
            GameObject vfx_HitSparks_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_HitSparks_marinelife");
            GameObject vfx_love_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_love_marinelife");
            GameObject vfx_Place_wood_pole_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_Place_wood_pole_marinelife");
            GameObject vfx_SawDust_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_SawDust_marinelife");
            GameObject vfx_soothed_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_soothed_marinelife");
            GameObject vfx_wsurface_medium_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_wsurface_medium_marinelife");
            GameObject vfx_wsurface_small_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_wsurface_small_marinelife");
            GameObject vfx_hurt_marinelife = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_hurt_marinelife");
            GameObject sfx_death_seaanimals = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_death_seaanimals");
            GameObject vfx_death_seaanimals_big = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_death_seaanimals_big");
            GameObject vfx_death_seaanimals_medium = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "vfx_death_seaanimals_medium");
            
            GameObject sfx_hit_seamar = ItemManager.PrefabManager.RegisterPrefab("mar_seaanimals", "sfx_hit_seamar");


            #endregion

            SetupWatcher();
            _harmony.PatchAll();
        }

        #region ASSAR CARNES

        [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
        static class SeaAnimalsZNetScene_AwakePost_Patch
        {
            static void Postfix(ZNetScene __instance)
            {
                if (__instance == null || __instance.m_prefabs is not { Count: > 0 }) return;

                // Obtenha estações de cozinha do ZNetScene
                CookingStation cookingStation =
                    __instance.GetPrefab("piece_cookingstation").GetComponent<CookingStation>();
                CookingStation cookingStationIron =
                    __instance.GetPrefab("piece_cookingstation_iron").GetComponent<CookingStation>();

                foreach (KeyValuePair<GameObject, GameObject> carne in seaanimalscarnes)
                {
                    cookingStation.m_conversion.Add(new CookingStation.ItemConversion()
                    {
                        m_cookTime = 25f,
                        m_from = __instance.GetPrefab(carne.Key.name).GetComponent<ItemDrop>(),
                        m_to = __instance.GetPrefab(carne.Value.name).GetComponent<ItemDrop>(),
                    });

                    cookingStationIron.m_conversion.Add(new CookingStation.ItemConversion()
                    {
                        m_cookTime = 25f,
                        m_from = __instance.GetPrefab(carne.Key.name).GetComponent<ItemDrop>(),
                        m_to = __instance.GetPrefab(carne.Value.name).GetComponent<ItemDrop>(),
                    });
                }
            }
        }

        #endregion

        #region ConfigOptions

        private void OnDestroy()
        {
            Config.Save();

        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                SeaAnimals.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                SeaAnimals.LogError($"There was an issue loading your {ConfigFileName}");
                SeaAnimals.LogError("Please check your config entries for spelling and format!");
            }
        }

        private static ConfigEntry<bool>? _serverConfigLocked;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }
        private static int GetZDO(int prefabHash)
        {
            int prefabCount = 0;
            foreach (List<ZDO> zdoList in ZDOMan.instance.m_objectsBySector)
            {
                if (zdoList == null) continue;

                for (int index = 0; index < zdoList.Count; ++index)
                {
                    ZDO zdo2 = zdoList[index];
                    if (zdo2.GetPrefab() == prefabHash)
                    {
                        prefabCount++;
                    }
                }
            }

            return prefabCount;
        }


        private static int GetPrefabCount(int prefabHash)
        {
            int prefabCount = 0;
            foreach (List<ZDO> zdoList in ZDOMan.instance.m_objectsBySector)
            {
                if (zdoList == null) continue;

                for (int index = 0; index < zdoList.Count; ++index)
                {
                    ZDO zdo2 = zdoList[index];
                    if (zdo2.GetPrefab() == prefabHash)
                    {
                        prefabCount++;
                    }
                }
            }

            return prefabCount;
        }

        #endregion

    }
}