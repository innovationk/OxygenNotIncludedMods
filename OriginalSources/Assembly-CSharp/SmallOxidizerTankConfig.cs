// Decompiled with JetBrains decompiler
// Type: SmallOxidizerTankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SmallOxidizerTankConfig : IBuildingConfig
{
  public const string ID = "SmallOxidizerTank";
  public const float FuelCapacity = 450f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] denseTieR0 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SmallOxidizerTank", 3, 2, "rocket_oxidizer_tank_small_kanim", 1000, 30f, denseTieR0, allMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "grounded";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, (AttachableBuilding) null)
    };
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 450f;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    FlatTagFilterable flatTagFilterable = go.AddOrGet<FlatTagFilterable>();
    flatTagFilterable.tagOptions = new List<Tag>()
    {
      SimHashes.OxyRock.CreateTag(),
      SimHashes.Fertilizer.CreateTag()
    };
    flatTagFilterable.headerText = (string) STRINGS.BUILDINGS.PREFABS.OXIDIZERTANK.UI_FILTER_CATEGORY;
    OxidizerTank oxidizerTank = go.AddOrGet<OxidizerTank>();
    oxidizerTank.consumeOnLand = !DlcManager.FeatureClusterSpaceEnabled();
    oxidizerTank.storage = storage;
    oxidizerTank.targetFillMass = 450f;
    oxidizerTank.maxFillMass = 450f;
    oxidizerTank.supportsMultipleOxidizers = true;
    go.AddOrGet<Prioritizable>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<DropToUserCapacity>();
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MINOR);
  }
}
