using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;

namespace ArmorDamageReduction
{
  internal static class UpdateResourcesAndSpell
  {
    private static void UpdateResources(UnitEntityData unit)
    {
      Mod.Logger.Log($"Restore resources for {unit.CharacterName}");
      foreach (var resource in unit.Resources)
      {
        Mod.Logger.Log($"Restore: {resource.NameSafe()}");
        unit.Resources.Restore(resource);
      }
    }

    private static void UpdateSpellBook(UnitEntityData unit, Spellbook spellBook)
    {
      var maxSpellLevel = spellBook.MaxSpellLevel;
      var mythicLevel = unit.Progression.MythicLevel;
      if (maxSpellLevel > mythicLevel)
      {
        maxSpellLevel = mythicLevel;
      }

      for (var spellLevel = 1; spellLevel <= maxSpellLevel; ++spellLevel)
      {
        spellBook.UpdateSlotsSize(spellLevel, true);
        if (spellBook.Blueprint.Spontaneous)
        {
          spellBook.m_SpontaneousSlots[spellLevel] = spellBook.GetSpellsPerDay(spellLevel);
        }
      }


      if (spellBook.Blueprint.MemorizeSpells)
      {
        spellBook.m_MemorizedSpells.SelectMany(list => list).ForEach(spellSlot =>
        {
          if (spellSlot.SpellLevel <= maxSpellLevel)
          {
            spellSlot.Available = true;
          }
        });
      }

      spellBook.RemoveMemorizedSpells.Clear();
    }

    public static void UpdateSpellsAndResources(UnitEntityData unit)
    {
      if (Mod.Settings.UpdateResource)
      {
        UpdateResources(unit);
      }

      if (!Mod.Settings.UpdateSpellBook) return;

      Mod.Logger.Log($"Restore spellBooks for {unit.CharacterName}");
      foreach (var spellBook in unit.Spellbooks)
      {
        UpdateSpellBook(unit, spellBook);
      }
    }
    
    [HarmonyPatch(typeof(GameHistoryLog), nameof(GameHistoryLog.HandlePartyCombatStateChanged))]
    internal static class GameHistoryLogModCombat
    {
      private static void Postfix(ref bool inCombat)
      {
        Mod.Logger.Log($"GameHistoryLogModCombat postfix, combat is {inCombat}");
        if (inCombat || !Mod.Enable) return;
        var partyMembers = Game.Instance.Player.PartyAndPets;
        foreach (var member in partyMembers)
        {
          UpdateSpellsAndResources(member);
        }
      }
    }
    
  }
}