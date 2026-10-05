// Decompiled with JetBrains decompiler
// Type: LuxuryBedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class LuxuryBedConfig : IBuildingConfig
{
  public const string ID = "LuxuryBed";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LuxuryBed", 4, 2, "elegantbed_kanim", 10, 10f, tieR3, plastics, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BED);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LuxuryBedType);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Bed bed = go.AddOrGet<Bed>();
    bed.effects = new string[2]
    {
      "LuxuryBedStamina",
      "BedHealth"
    };
    bed.workLayer = Grid.SceneLayer.BuildingFront;
    Sleepable sleepable = go.AddOrGet<Sleepable>();
    sleepable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_sleep_bed_kanim")
    };
    sleepable.workLayer = Grid.SceneLayer.BuildingFront;
    if (DlcManager.IsContentSubscribed("DLC3_ID"))
    {
      DefragmentationZone defragmentationZone = go.AddOrGet<DefragmentationZone>();
      defragmentationZone.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_bionic_kanim")
      };
      defragmentationZone.workLayer = Grid.SceneLayer.BuildingFront;
    }
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }
}
