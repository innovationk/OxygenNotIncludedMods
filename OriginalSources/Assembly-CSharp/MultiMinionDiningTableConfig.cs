// Decompiled with JetBrains decompiler
// Type: MultiMinionDiningTableConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class MultiMinionDiningTableConfig : IBuildingConfig
{
  public const string ID = "MultiMinionDiningTable";
  public static readonly MultiMinionDiningTableConfig.Seat[] seats = new MultiMinionDiningTableConfig.Seat[3]
  {
    new MultiMinionDiningTableConfig.Seat((HashedString) "anim_eat_table_kanim", (HashedString) "anim_bionic_eat_table_kanim", (HashedString) "saltshaker", new CellOffset(0, 0)),
    new MultiMinionDiningTableConfig.Seat((HashedString) "anim_eat_table_L_kanim", (HashedString) "anim_bionic_eat_table_L_kanim", (HashedString) "saltshaker_L", new CellOffset(-1, 0)),
    new MultiMinionDiningTableConfig.Seat((HashedString) "anim_eat_table_R_kanim", (HashedString) "anim_bionic_eat_table_R_kanim", (HashedString) "saltshaker_R", new CellOffset(1, 0))
  };

  public static int SeatCount => MultiMinionDiningTableConfig.seats.Length;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] woods = TUNING.MATERIALS.WOODS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MultiMinionDiningTable", 5, 1, "multi_dupe_table_kanim", 10, 10f, tieR4, woods, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.WorkTime = 20f;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.DINING);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DiningTableType);
    go.AddOrGetDef<RocketUsageRestriction.Def>();
    go.AddOrGet<MultiMinionDiningTable>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = (TableSaltTuning.SALTSHAKERSTORAGEMASS + CaviarTuning.STORAGEMASS) * (float) MultiMinionDiningTableConfig.SeatCount;
    ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg1.SetStorage(defaultStorage);
    manualDeliveryKg1.RequestedItemTag = TableSaltConfig.ID.ToTag();
    manualDeliveryKg1.capacity = TableSaltTuning.SALTSHAKERSTORAGEMASS * (float) MultiMinionDiningTableConfig.SeatCount;
    manualDeliveryKg1.refillMass = TableSaltTuning.CONSUMABLE_RATE * (float) MultiMinionDiningTableConfig.SeatCount;
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
    manualDeliveryKg1.ShowStatusItem = false;
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.SetStorage(defaultStorage);
    manualDeliveryKg2.RequestedItemTag = CaviarConfig.TAG;
    manualDeliveryKg2.capacity = CaviarTuning.STORAGEMASS * (float) MultiMinionDiningTableConfig.SeatCount;
    manualDeliveryKg2.refillMass = CaviarTuning.CONSUMABLE_RATE * (float) MultiMinionDiningTableConfig.SeatCount;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
    manualDeliveryKg2.ShowStatusItem = false;
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }

  public struct Seat(
    HashedString eatAnim,
    HashedString reloadElectrobankAnim,
    HashedString saltSymbol,
    CellOffset tableRelativeLocation)
  {
    private readonly HashedString eatAnim = eatAnim;
    private readonly HashedString reloadElectrobankAnim = reloadElectrobankAnim;
    private readonly HashedString saltSymbol = saltSymbol;
    private CellOffset tableRelativeLocation = tableRelativeLocation;

    public readonly HashedString EatAnim => this.eatAnim;

    public readonly HashedString ReloadElectrobankAnim => this.reloadElectrobankAnim;

    public readonly HashedString SaltSymbol => this.saltSymbol;

    public readonly CellOffset TableRelativeLocation => this.tableRelativeLocation;
  }
}
