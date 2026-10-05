// Decompiled with JetBrains decompiler
// Type: BedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class BedConfig : IBuildingConfig
{
  public const string ID = "Bed";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMineralsOrWood = TUNING.MATERIALS.RAW_MINERALS_OR_WOOD;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Bed", 2, 2, "bedlg_kanim", 10, 10f, tieR3, rawMineralsOrWood, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BED);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Bed bed = go.AddOrGet<Bed>();
    bed.effects = new string[2]{ "BedStamina", "BedHealth" };
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
