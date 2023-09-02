using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using Kingmaker.UnitLogic.FactLogic;

namespace ArmorDamageReduction
{
  public static class Mod
  {
    public static UnityModManager.ModEntry.ModLogger Logger;
    private static Harmony _harmony;
    public static bool Enable;
    internal static Settings Settings;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
      Logger = modEntry.Logger;
      Settings = new Settings();
      _harmony = new Harmony(modEntry.Info.Id);
      Logger.Log("Load mod ArmorDamageReduction");
      _harmony.PatchAll(Assembly.GetExecutingAssembly());
      Logger.Log("Finish patch ArmorDamageReduction");
      modEntry.OnToggle = OnToggle;

      return true;
    }

    private static bool OnToggle(UnityModManager.ModEntry modEntry, bool status)
    {
      Logger.Log($"Toggled {modEntry.Info.DisplayName}: now {(status ? "on" : "off")}");
      Enable = status;
      return true;
    }
  }
  
}