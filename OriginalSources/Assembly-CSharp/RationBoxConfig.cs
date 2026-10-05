// Decompiled with JetBrains decompiler
// Type: RationBoxConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class RationBoxConfig : IBuildingConfig
{
  public const string ID = "RationBox";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RationBox", 2, 2, "rationbox_kanim", 10, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    SoundEventVolumeCache.instance.AddVolume("rationbox_kanim", "RationBox_open", NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("rationbox_kanim", "RationBox_close", NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 150f;
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.FOOD;
    storage.allowItemRemoval = true;
    storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    storage.showCapacityStatusItem = true;
    storage.showCapacityAsMainStatus = true;
    go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = (string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON_EDIBLES;
    go.AddOrGet<RationBox>();
    go.AddOrGet<UserNameable>();
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
  }
}
