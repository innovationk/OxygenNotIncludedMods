// Decompiled with JetBrains decompiler
// Type: HijackedHeadquartersConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class HijackedHeadquartersConfig : IBuildingConfig
{
  public const string ID = "HijackedHeadquarters";
  private const int WIDTH = 5;
  private const int HEIGHT = 5;
  public const int DEFAULT_DATABANK_PRINT_COST = 25;
  public const int COST_INCREASE_PER_PRINT = 25;
  public const int MAX_COST_INCREASES_PER_PRINT = 10;
  private static Dictionary<Tag, int> PrintableCostOverrides = new Dictionary<Tag, int>();

  public static int GetDataBankCost(Tag printableTag, int printCount = 0)
  {
    return HijackedHeadquartersConfig.PrintableCostOverrides.ContainsKey(printableTag) ? HijackedHeadquartersConfig.PrintableCostOverrides[printableTag] : 25 + Math.Min(printCount, 10) * 25;
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HijackedHeadquarters", 5, 5, "hijacked_hq_kanim", 250, 120f, tieR5_1, construction_materials, 3200f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.Floodable = false;
    buildingDef.Entombable = true;
    buildingDef.Overheatable = false;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "medium";
    buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Steel);
    component.Temperature = 294.15f;
    BuildingTemplates.ExtendBuildingToGravitas(go);
    Storage storage = go.AddComponent<Storage>();
    storage.capacityKg = 275f;
    Activatable activatable = go.AddComponent<Activatable>();
    activatable.synchronizeAnims = false;
    activatable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_remote_kanim")
    };
    activatable.SetWorkTime(30f);
    go.AddOrGetDef<HijackedHeadquarters.Def>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = (Tag) DatabankHelper.ID;
    manualDeliveryKg.MinimumMass = 0.0f;
    manualDeliveryKg.refillMass = 25f;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
    manualDeliveryKg.operationalRequirement = Operational.State.Operational;
    manualDeliveryKg.ShowStatusItem = false;
    manualDeliveryKg.RoundFetchAmountToInt = true;
    manualDeliveryKg.FillToCapacity = true;
    go.AddComponent<DropToUserCapacity>();
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object =>
    {
      game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight);
      StoryManager.Instance.ForceCreateStory(Db.Get().Stories.HijackedHeadquarters, game_object.GetMyWorldId());
    });
  }
}
