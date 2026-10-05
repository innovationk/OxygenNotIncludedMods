// Decompiled with JetBrains decompiler
// Type: RemoteWorkTerminalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class RemoteWorkTerminalConfig : IBuildingConfig
{
  public static string ID = "RemoteWorkTerminal";
  public static readonly Tag INPUT_MATERIAL = DatabankHelper.TAG;
  public const float INPUT_CAPACITY = 10f;
  public const float INPUT_CONSUMPTION_RATE_PER_S = 0.0133333337f;
  public const float INPUT_REFILL_RATIO = 0.5f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = RemoteWorkTerminalConfig.ID;
    float[] tieR2_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues tieR2_2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 3, 3, "remote_work_terminal_kanim", 30, 60f, tieR2_1, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR2_2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
    go.AddComponent<RemoteWorkTerminalSM>();
    go.AddOrGet<Operational>();
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 100f;
    storage.showInUI = true;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Insulate
    });
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = RemoteWorkTerminalConfig.INPUT_MATERIAL;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
    manualDeliveryKg.operationalRequirement = Operational.State.Functional;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(RemoteWorkTerminalConfig.INPUT_MATERIAL, 0.0133333337f)
    };
    elementConverter.showDescriptors = false;
    go.AddOrGet<ElementConverterOperationalRequirement>();
    Prioritizable.AddRef(go);
  }

  public override string[] GetRequiredDlcIds()
  {
    return new string[1]{ "DLC3_ID" };
  }
}
