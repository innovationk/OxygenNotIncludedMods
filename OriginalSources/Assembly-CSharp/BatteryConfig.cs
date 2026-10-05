// Decompiled with JetBrains decompiler
// Type: BatteryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class BatteryConfig : BaseBatteryConfig
{
  public const string ID = "Battery";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = this.CreateBuildingDef("Battery", 1, 2, 30, "batterysm_kanim", 30f, tieR3, allMetals, 800f, 0.25f, 1f, tieR1, noise);
    buildingDef.Breakable = true;
    SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Battery battery = go.AddOrGet<Battery>();
    battery.capacity = 10000f;
    battery.joulesLostPerSecond = 1.66666663f;
    base.DoPostConfigureComplete(go);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    base.ConfigureBuildingTemplate(go, prefab_tag);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
  }
}
