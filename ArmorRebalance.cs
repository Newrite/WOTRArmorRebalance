using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Cheats;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;

namespace ArmorDamageReduction
{
  internal class ArmorRebalance
  {
    private static readonly Dictionary<int, BlueprintArmorEnchantment> Enchants =
      new Dictionary<int, BlueprintArmorEnchantment>();

    private static readonly Dictionary<int, BlueprintFeature> Features =
      new Dictionary<int, BlueprintFeature>();

    private static readonly string[] EnchantsNameGuids =
    {
      "d2ddb8a6-8cf9-45e5-94a3-fb3b11839f93",
      "6d2b0489-c5cc-4293-86a3-e8e9ae40fb44",
      "6667f507-c3d2-46bd-b607-8fc8c0d96127",
      "cb7be8be-ec21-4235-85f8-a05af785a49b",
      "12fa84e1-79eb-44db-9035-b27d92ad9c0d",
      "19be2f3d-a0ce-4ca1-a489-a1b98387d2a6",
      "9de9f01b-3792-44a3-a358-d0c6f0761c2c",
      "101f58b2-89c4-447b-924d-2fa8f807497c",
      "54cc1ecd-8a29-45c0-8070-3fcb15231232",
      "3cdac21b-b8ae-420f-a487-269c1af1c6cb",
      "038247d1-3cce-4c70-9552-9d776818f6d7",
      "892bcf9c-356b-4b00-9580-76370b54c00a",
      "657e3ff9-c763-410a-a668-ca1be45a6b59",
      "93f82135-caf5-43e7-9445-6d2807862b88",
      "1594078b-6104-4ae2-b2fd-94216e168184",
      "502b9b14-d7fc-4026-957b-00f8dae877a8",
      "aa7dae01-7ffe-4f5d-bda5-fadafda3fee0",
      "7e918179-f52f-48ab-80d0-3ee13f00bdfa",
      "99060e53-acfa-4b9a-9c67-8dd6c4a580c2",
      "fd354d6f-0a3b-42f7-b37b-b08894bae30b",
      "4b9a11cc-ad97-48de-b29d-815e839184a8",
      "29958896-c738-4ffc-bd80-9e7a91abbb21",
      "58d681c4-1877-4ec2-b048-3b0838e998ed",
      "d4c150dc-51c9-4221-82ec-7f336970e5fb",
      "409e767f-96d3-4aef-ad4a-be08c406c09c",
      "5adb091f-3f90-47f2-90af-d04ae8d7eb71",
      "b64b596a-c609-4f34-870e-e3d63b982902",
      "6beecb27-c329-4781-ba28-895580bacc3d",
      "22aade65-48b6-4ae3-9eba-d5f74abad854",
      "b067f92c-618b-4e77-8727-31eea9b597a5",
      "c2d005ae-b07f-42bd-93f7-fdb7b7f713b5",
      "6557ee9a-1ec9-460a-bf65-28c960331e6a",
      "bffcdcf1-d46b-49ed-9415-693e6857187f",
      "506657e5-acff-48d2-ab91-47bc8d4bfe61",
      "213d2b81-d49d-4e0c-8114-2c642034328e",
      "15dc9cec-5d5e-4856-84ec-c3ddfdcc0fee",
      "b1206824-4a61-4a15-85a5-33f82ab7385f",
      "ac265754-8e6f-4a23-b4db-0459c6865076",
      "aa895ac5-ab1e-420a-971b-2401402bb017",
      "1cbfdc41-c2d5-4aa5-9027-e2b694686953",
      "15d0772d-c303-4ef9-b933-da54593839ad",
      "1ce7edcd-f36a-4798-8fe1-c74b6b01b6ca",
      "eae3e8b0-d007-404c-b35a-24a4fcb9affa",
      "494194f8-a0f8-482b-8eb1-d18e956987eb",
      "7781cb11-78ab-4d9d-8245-cd04a9d8dd2a",
      "361200c3-b0b6-4fc9-898b-bbb29ee5a3a0",
      "dbf8fb79-211e-4bdb-b855-c60a51f290d0",
      "7d0a9fa7-851d-4ad7-ae67-319e83895fcc",
      "c07961ed-2263-4701-9736-08102f0bc7f5",
      "d1e3af95-1a7b-4c8f-a84b-3f4fa22f696e"
    };

    private static readonly string[] EnchantsDescGuids =
    {
      "43b5c508-6773-4915-98cc-b5e81e3f5def",
      "df0ec398-60eb-4dd1-a158-6cf45e7a4557",
      "242a5b8c-eb7c-47a2-bae6-0b31420c214f",
      "d7b7dd56-e3e5-4774-a16b-928cfe4a4c79",
      "8768dc47-8d09-42db-b652-b9d5c316dc60",
      "e286ff71-9c8a-4f54-ab83-fc9a3e24c4a9",
      "a0dadeb2-60c7-4225-be68-0ac1b470f531",
      "53f5aeac-5044-4a7a-aa9f-97bfcbcfe02a",
      "a2222804-c096-4f28-ba24-a6ba6a3e976e",
      "7e93ece7-15de-4e44-9fbc-9dd8a6a4288b",
      "0b84a4b6-1b62-40f4-bbb1-19fea0b12dfb",
      "ce1ea8aa-b1d6-4205-900a-cf62a1dabe28",
      "8ac17456-f129-4fed-9e41-f3f83b9dec3f",
      "2d138c0c-b5b6-4e1d-8a82-8a0f72b1b672",
      "1ddac608-1280-4eb4-98d9-7dea9617127d",
      "1b37d8d2-43f0-4f2a-9bc1-a2ce04b7fdd8",
      "e7021826-6072-4b1b-b582-1ce54922f41d",
      "7aebf8a5-80ef-4816-821e-9198933076cd",
      "8bf45171-c1ab-42d7-8d82-badf73405b3a",
      "310003dc-5f23-4d38-bee6-02e29560d3a7",
      "5e8592c1-ebc0-465c-92fe-13f1a97e120a",
      "c56561c3-6f88-46fe-bf74-2f4a399f21fe",
      "ba42cff6-9221-4bb4-9f70-2acff5b7fb89",
      "774f4244-fe20-4d3e-8ee5-947adca820bd",
      "931b50d2-f9cf-4d6e-bfc2-22d188517376",
      "960e5a7d-ad6b-479e-ab2c-1f7ba8c16d07",
      "0be44f6d-9d87-46de-ad6f-ef5b3afe8f81",
      "c0ef5366-4604-4ffd-9b50-0c6d89bf88dd",
      "bc7647e0-38b8-41b9-8c61-be3d9838e6a8",
      "fdc7d0a1-0071-434f-bccd-d7c047d2f59d",
      "fb1159e7-fc5f-4eda-9a66-56e1b0138008",
      "787f08eb-0b2a-473e-8f60-29c15a7b85b0",
      "0b9b75be-d67a-4ec9-92d5-395e4c29a838",
      "ae658c15-5f69-4afa-8457-2a9d27f015a2",
      "68feba25-2e21-4f30-a076-85bba908bc23",
      "041a9895-6947-4143-b459-2658b889087b",
      "394a5aa4-aa4e-4907-88c5-5dc13990aa57",
      "e40eace6-c385-4156-abec-527bc9810925",
      "e882eb05-30b7-4b42-b442-9224b8d2f3e6",
      "408818d0-3c56-48ac-8f96-ffa2dd6ca6ce",
      "c07aeecb-32fc-4136-a8eb-91833394cddb",
      "29987c2d-4f17-4a72-a385-e512de48f93c",
      "4d4af56d-2dda-4b44-9266-e98ac3630d4d",
      "90b2273f-2e59-4159-b6d3-ee3c0172e53b",
      "42eee487-dae9-486c-810d-48cb20d8fafe",
      "6875e07c-5b95-4e51-a6f6-a4d606c19c3c",
      "fcec52c4-58af-4466-90f2-9ba794b7a3c2",
      "2b417cfd-8731-47bf-8a7b-b1bb4d75cfe2",
      "cdd9d07d-a077-4b57-80de-5bdcf3f3a201",
      "16c60a7b-ffcc-46d9-8d5a-1abf4a78bf67"
    };

    private static readonly string[] EnchantsGuids =
    {
      "8e9f2b54-e2ad-4b5b-81a7-9a5df8343158",
      "ceab3d50-cf93-45cf-aae4-9026d53cc892",
      "7f3a63f8-9709-4a25-b8bf-cfe270af654b",
      "f7f0cad8-c47c-4ac1-91ac-4d8143e20b03",
      "d9494cc3-f931-49ba-9f62-694c51b3ff82",
      "6e1feab3-7a23-4fde-942b-08d24d860106",
      "d30c24cd-2bfe-4fb4-b264-0507910652dc",
      "dbc5edf8-4809-4703-be9f-8ba725dd1286",
      "79d23cec-abeb-43a2-8ed9-5139eaac5d18",
      "7f4efe68-ed77-47cc-9f21-da5acc1fe775",
      "2a60a69f-25da-42e2-9707-58e5d2035df1",
      "5190ff03-932c-4649-8978-e56385ef498c",
      "57684f50-520c-4ead-a3f9-99d475893f89",
      "a301af7d-1d12-43cf-b513-cd30a862b81d",
      "c79f4400-be8c-43ac-aea8-b02cfb9fd21b",
      "34066cf7-3e17-4586-8682-d8ed84cbf4e6",
      "028c85e6-dc6e-409d-817e-b75f21d23d22",
      "108ddb3f-c45a-4801-a1b1-6b1451375a80",
      "8b6bba05-1eb7-400a-bd52-c2223fecdaa6",
      "ef43f88e-5d8f-41bc-9363-bf58d8c65f23",
      "7242a5b8-2a7d-480c-b5e7-ae3475d06c71",
      "a5e62ce5-b222-4ed7-83ad-356a450dcbcb",
      "ada13801-5bc0-46f5-92d7-caf320386086",
      "8baec0d0-6041-4262-9de5-c2b56577f0ab",
      "dc0d4017-e397-45ea-90f5-7988bb916ebc",
      "92bdf64a-ed68-49c7-acbb-5398bbfdbe4a",
      "1f7e58a8-8f37-4494-a0a4-ecf5523a8d67",
      "064da4d8-bec2-4d35-9768-f9fb814e82ab",
      "15c975f6-b1dc-4bf7-b898-30057f9b90a2",
      "1d21a497-89e1-4a6b-9eed-67f8f7676d02",
      "359c58dd-616f-4809-8b1d-5dcd7989a7e2",
      "a75a67ab-2729-4670-929b-4a2862be0a5f",
      "f61a13aa-d0d0-4d40-9ecc-1a40a73ace14",
      "694e61bb-ede7-4614-8422-66e0a86a355e",
      "016f1850-b005-46aa-846e-13befd6d6dc0",
      "21337ad3-f4d5-40de-8aea-9c734c8fe4ec",
      "ef3a39fc-4294-4ee7-8986-9391f0626235",
      "b2d4be7f-4efd-471b-bcfa-293678560124",
      "196f450d-2690-46e5-a075-cc4b7a743e6d",
      "50b4c840-12d9-47a7-8bb2-6457fb540a6c",
      "a99ff36c-eedd-496d-95b2-2ae496d49fb4",
      "11517980-833b-46ab-99d7-4b9340c9bafa",
      "55956cbc-3512-4361-8921-fac18fcac51d",
      "b8bf4ace-9957-43dd-a753-cbf84022bfac",
      "bd3eeeff-8900-4148-8e2e-11d893e10bd7",
      "8453b9ed-1a52-4e9a-93a9-d04c4ef26bc4",
      "039391ea-629b-40a8-988f-cd15a2e0d859",
      "66d6047b-3f26-4100-9545-6d5ad1996a7d",
      "e4f04e99-1556-4876-9f0d-35277b44b769",
      "3605806d-c018-47cf-9be7-9c41fe290d53"
    };

    private static readonly string[] FeaturesGuids =
    {
      "18c29862-7033-45f5-a753-d84e98345cd6",
      "f18e61f6-e523-4acc-82d3-20b708222762",
      "dca46422-6041-42e9-854b-0133f883ca82",
      "edb51f8e-fe99-4e60-a367-056976adb775",
      "89bc9a6a-a538-455f-87e9-10e1577a0b24",
      "4502a84c-3170-4abf-b041-504a2f8319fa",
      "ad856e3a-59f3-4b8d-ae2c-29797f56fce0",
      "674ccbf3-76d3-4f9e-a814-afecd159819d",
      "b7426319-6281-49a1-a55f-2f07938c572d",
      "f53ad7d6-4d70-4c47-a49b-571cb1a092f3",
      "a9acb3d8-995c-4b84-9d50-ed4c5b2e7091",
      "a4cfb4fd-9c15-4e91-8233-8f01d4daec81",
      "c11ca333-0b5d-44a5-ac1d-eede6b00f0de",
      "2cb4ab04-3754-4cf8-8818-7a52f3bad13e",
      "ff22b964-3ec5-4f85-89a2-76ad402b1753",
      "a4a6736c-dd43-47c6-9af8-9b7703239786",
      "a24e0ea4-d032-47cb-9dc4-5f0f40387f23",
      "170cbffe-66bd-44a1-b253-ea4bace5723b",
      "1297489a-a047-4b76-8eb7-898ca2720405",
      "35bb8985-5760-4ab7-b486-01beec1168e9",
      "dfdc19f8-d7f6-42e7-be97-e73131dd93f5",
      "06fd5c6c-f33f-4429-96ce-f83a73096a18",
      "f3337449-3e2c-4f05-abba-ce142ac0ca6f",
      "f6f21bb1-51d3-42d4-8237-a198ddb5994e",
      "5e310ba9-39f7-4f01-9170-e7f07818a57a",
      "90a51ea3-82df-45f9-b9de-50b1d766a99a",
      "4c539f6a-c03e-4f76-b3f1-d89f98d6ff25",
      "bb876ce5-2128-4291-a5d3-fde158eb2bed",
      "9aa135aa-d9b5-40f7-9a79-5ebe35e7a146",
      "dc5c1301-0c25-478c-ab10-192e20596c93",
      "e9107551-a6ee-4071-8155-7fea1b436924",
      "238890d2-b7e2-42e7-b5a7-6c077140a07f",
      "be8ce52f-f247-4f3d-a86a-6620c9c25477",
      "19440d2a-d43c-486d-81bd-ce27e144303e",
      "1121b2a4-5a57-41fc-b2a3-47957c73df21",
      "58ad8daf-aedd-44ec-a725-4405e69a0e88",
      "36ea8b37-2b14-4aa6-b44d-e07a290788bc",
      "0c9e4d61-597c-406b-88c7-5afaf0caa318",
      "b5ca3851-3e72-417d-afc7-4ccb7ecef6ab",
      "12c770fe-f9c7-476e-94ad-7bece0fc249a",
      "629eb2f6-e2ee-4871-adf4-38a94fa7d3da",
      "fa58526c-5d14-4978-a91e-514084531fb8",
      "1e67bb98-d8ad-4249-8f17-6e6a9f562d47",
      "1c99dfe6-9686-4bbb-a685-92d9cdebce98",
      "2ff95589-bd45-4795-83cd-2485aa199865",
      "fae109f9-2840-4acd-ba45-d34a3ef1a051",
      "1a54781d-a2db-4f54-bcf9-fdf61529cb01",
      "ed957dd5-bf63-423d-8034-5d8bfe079fbc",
      "1eb70f33-e385-42c4-8ff1-2c552847676d",
      "b10712b7-d856-4044-b72d-b6da4f945d64"
    };

    private static int GetEnchantmentArmorBonus(List<BlueprintItemEnchantment> enchantments)
    {
      var armorBonus = 0;
      foreach (var enchantment in enchantments)
      {
        foreach (var component in enchantment.Components)
        {
          if (component is ArmorEnhancementBonus armorEnchant)
          {
            armorBonus += armorEnchant.EnhancementValue;
          }
        }
      }

      return armorBonus;
    }

    private static int CalculateArmorDamageReduction(BlueprintItemArmor armor)
    {
      var enchantmentArmorBonus = GetEnchantmentArmorBonus(armor.Enchantments);
      if (enchantmentArmorBonus < 0)
      {
        enchantmentArmorBonus = 0;
      }

      var dexterityBonus = armor.MaxDexterityBonus;
      if (dexterityBonus > 10)
      {
        dexterityBonus = 10;
      }

      if (dexterityBonus < 0)
      {
        dexterityBonus = 0;
      }

      var dexterityMultiplyBaseArmor = 1.0 + (((10.0 - dexterityBonus) / 10.0) / 2.0);
      var dexterityMultiplyEnchantmentArmor = 3.0 + ((10.0 - dexterityBonus) / 10.0);

      var damageReduction = (int) ((enchantmentArmorBonus * dexterityMultiplyEnchantmentArmor) +
                                   (armor.ArmorBonus * dexterityMultiplyBaseArmor));

      Mod.Logger.Log(
        $"{armor.Name}: DXB {dexterityBonus} EAB {enchantmentArmorBonus} AB {armor.ArmorBonus} DXMBA {dexterityMultiplyBaseArmor} DXMEA {dexterityMultiplyEnchantmentArmor} DR {damageReduction}");

      return damageReduction;
    }

    private static BlueprintFeature GetOrCreateDamageReductionFeature(int reductionValue)
    {
      if (Features.ContainsKey(reductionValue))
      {
        return Features[reductionValue];
      }

      var name = $"DamageReductionFeature{reductionValue}";
      var guid = FeaturesGuids[reductionValue];

      var feature = FeatureConfigurator.New(name, guid)
        .AddDamageResistancePhysical(value: reductionValue, isStackable: true).Configure();
      Features.Add(reductionValue, feature);

      return Features[reductionValue];
    }

    private static void CreateString(LocalizationPack pack, string key, string text)
    {
      pack.PutString(key, text);
    }

    private static BlueprintArmorEnchantment GetOrCreateDamageReductionArmorEnchantment(int reductionValue,
      BlueprintFeature drFeature)
    {
      if (Enchants.ContainsKey(reductionValue))
      {
        return Enchants[reductionValue];
      }

      var descriptionLocalKey = EnchantsDescGuids[reductionValue];
      var description = $"Уменьшает получаемый физический урон на {reductionValue} единиц.";
      var enchantmentLocalKey = EnchantsNameGuids[reductionValue];
      var enchantmentName = $"Вычет урона от брони {reductionValue}/-";

      CreateString(LocalizationManager.CurrentPack, descriptionLocalKey, description);
      CreateString(LocalizationManager.CurrentPack, enchantmentLocalKey, enchantmentName);

      var name = $"DamageReductionArmorEnchantment{reductionValue}";
      var guid = EnchantsGuids[reductionValue];
      var armorEnchantment = ArmorEnchantmentConfigurator.New(name,
          guid)
        .AddUnitFeatureEquipment(drFeature)
        .SetDescription(descriptionLocalKey)
        .SetEnchantName(enchantmentLocalKey)
        .Configure();
      Enchants.Add(reductionValue, armorEnchantment);

      return Enchants[reductionValue];
    }

    private static void PatchingBlueprintArmor(BlueprintItemArmor blueprintArmor)
    {
      var reductionValue = CalculateArmorDamageReduction(blueprintArmor);
      var feature = GetOrCreateDamageReductionFeature(reductionValue);
      var enchantment = GetOrCreateDamageReductionArmorEnchantment(reductionValue, feature);

      blueprintArmor.Enchantments.Add(enchantment);
      Mod.Logger.Log($"DR {reductionValue}/- enchantment attached to {blueprintArmor.Name}");
    }

    private static bool AllowedArmorProficiency(ArmorProficiencyGroup proficiency)
    {
      switch (proficiency)
      {
        case ArmorProficiencyGroup.Light:
        case ArmorProficiencyGroup.Medium:
        case ArmorProficiencyGroup.Heavy:
        case ArmorProficiencyGroup.LightBarding:
        case ArmorProficiencyGroup.MediumBarding:
        case ArmorProficiencyGroup.HeavyBarding:
          return true;
        case ArmorProficiencyGroup.Buckler:
        case ArmorProficiencyGroup.LightShield:
        case ArmorProficiencyGroup.HeavyShield:
        case ArmorProficiencyGroup.TowerShield:
        case ArmorProficiencyGroup.None:
        default:
          return false;
      }
    }

    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class BlueprintPatcher
    {
      private static bool _loaded = false;
      private const string TypeName = "Kingmaker.Blueprints.Items.Armors.BlueprintItemArmor";

      [HarmonyPriority(Priority.Last)]
      private static void Postfix()
      {
        if (_loaded)
        {
          Mod.Logger.Log("Blueprints already loaded");
          return;
        }

        _loaded = true;

        Mod.Settings.Initialize();

        Mod.Logger.Log("Start iterate blueprints");
        var blueprints = Utilities.GetAllBlueprints();
        Mod.Logger.Log($"Loaded entries: {blueprints.Entries.Count}");

        var entries = blueprints.Entries.Where(e => e != null && e.TypeFullName.Equals(TypeName));

        foreach (var entry in entries)
        {
          var blueprintArmor =
            Utilities.GetBlueprintByGuid<BlueprintItemArmor>(entry.Guid);

          if (blueprintArmor == null || !AllowedArmorProficiency(blueprintArmor.ProficiencyGroup)) continue;
          Mod.Logger.Log($"Patching: {blueprintArmor.Name}");
          PatchingBlueprintArmor(blueprintArmor);
        }
      }
    }
    
    [HarmonyPatch(typeof(AddDamageResistancePhysical), nameof(AddDamageResistancePhysical.IsStackable),
      MethodType.Getter)]
    internal static class AddDamageResistancePhysicalMod
    {
      // ReSharper disable once InconsistentNaming
      private static void Postfix(ref bool __result)
      {
        Mod.Logger.Log($"DamageResist old result {__result}");
        __result = true;
      }
    }
    
  }
}