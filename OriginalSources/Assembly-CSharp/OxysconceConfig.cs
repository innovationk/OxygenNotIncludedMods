// Decompiled with JetBrains decompiler
// Type: OxysconceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class OxysconceConfig : IBuildingConfig
{
  public const string ID = "Oxysconce";
  private const float OXYLITE_STORAGE = 240f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR0_3 = BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = tieR0_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Oxysconce", 1, 1, "oxy_sconce_kanim", 10, 3f, tieR0_1, allMetals, 800f, BuildLocationRule.Anywhere, tieR0_3, noise);
    buildingDef.RequiresPowerInput = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.Breakable = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    CellOffset cellOffset = new CellOffset(0, 0);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 240f;
    storage.showInUI = true;
    storage.showCapacityStatusItem = true;
    storage.showCapacityAsMainStatus = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = SimHashes.OxyRock.CreateTag();
    manualDeliveryKg.capacity = 240f;
    manualDeliveryKg.refillMass = 96f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    go.AddOrGet<StorageMeter>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object => Tutorial.Instance.oxygenGenerators.Add(game_object));
  }
}
