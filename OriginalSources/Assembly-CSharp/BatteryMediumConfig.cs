// Decompiled with JetBrains decompiler
// Type: BatteryMediumConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class BatteryMediumConfig : BaseBatteryConfig
{
  public const string ID = "BatteryMedium";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = this.CreateBuildingDef("BatteryMedium", 2, 2, 30, "batterymed_kanim", 60f, tieR4, allMetals, 800f, 0.25f, 1f, tieR2, noise);
    SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Battery battery = go.AddOrGet<Battery>();
    battery.capacity = 40000f;
    battery.joulesLostPerSecond = 3.33333325f;
    base.DoPostConfigureComplete(go);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    base.ConfigureBuildingTemplate(go, prefab_tag);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
  }
}
