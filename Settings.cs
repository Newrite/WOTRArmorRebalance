using BlueprintCore.Utils;
using ModMenu.Settings;

namespace ArmorDamageReduction
{
  internal class Settings
  {
    private const string Root = "ArmorDamageReduction.Settings";
    private static readonly string RootKey = Root.ToLower();
    private static readonly LogWrapper Logger = LogWrapper.Get(Root);

    private static string GetKey(string partialKey, string separator = ".")
    {
      return $"{RootKey}{separator}{partialKey}";
    }

    #region Strings

    private static readonly string title = "Settings.Title";
    private static readonly string main = "Settings.Main";
    private static readonly string updateResource = "Settings.UpdateResource";
    private static readonly string updateResourceLong = "Settings.UpdateResourceLong";
    private static readonly string updateSpellBook = "Settings.UpdateSpellBook";
    private static readonly string updateSpellBookLong = "Settings.UpdateSpellBookLong";

    #endregion

    #region Bools

    public bool UpdateResource => ModMenu.ModMenu.GetSettingValue<bool>(GetKey("updateResource"));
    public bool UpdateSpellBook => ModMenu.ModMenu.GetSettingValue<bool>(GetKey("updateSpellBook"));

    #endregion

    internal void Initialize()
    {
      ModMenu.ModMenu.AddSettings(
        SettingsBuilder
          .New(GetKey("title"), LocalizationTool.GetString(title))
          .AddSubHeader(LocalizationTool.GetString(main), startExpanded: true)
          .AddToggle(Toggle
            .New(GetKey("updateResource"), defaultValue: true, LocalizationTool.GetString(updateResource))
            .WithLongDescription(LocalizationTool.GetString(updateResourceLong)))
          .AddToggle(Toggle
            .New(GetKey("updateSpellBook"), defaultValue: true, LocalizationTool.GetString(updateSpellBook))
            .WithLongDescription(LocalizationTool.GetString(updateSpellBookLong))));

      Logger.Info("Initialized.");
    }
  }
}