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
    private static readonly string updateresource = "Settings.UpdateResource";
    private static readonly string updateresourcelong = "Settings.UpdateResourceLong";
    private static readonly string updatespellbook = "Settings.UpdateSpellBook";
    private static readonly string updatespellbooklong = "Settings.UpdateSpellBookLong";

    #endregion

    #region Bools

    public bool UpdateResource => ModMenu.ModMenu.GetSettingValue<bool>(GetKey("updateresource"));
    public bool UpdateSpellBook => ModMenu.ModMenu.GetSettingValue<bool>(GetKey("updatespellbook"));

    #endregion

    internal void Initialize()
    {
      ModMenu.ModMenu.AddSettings(
        SettingsBuilder
          .New(GetKey("title"), LocalizationTool.GetString(title))
          .AddSubHeader(LocalizationTool.GetString(main), startExpanded: true)
          .AddToggle(Toggle
            .New(GetKey("updateresource"), defaultValue: true, LocalizationTool.GetString(updateresource))
            .WithLongDescription(LocalizationTool.GetString(updateresourcelong)))
          .AddToggle(Toggle
            .New(GetKey("updatespellbook"), defaultValue: true, LocalizationTool.GetString(updatespellbook))
            .WithLongDescription(LocalizationTool.GetString(updatespellbooklong))));

      Logger.Info("Initialized.");
    }
  }
}