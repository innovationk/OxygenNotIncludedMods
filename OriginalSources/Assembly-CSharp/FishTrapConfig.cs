// Decompiled with JetBrains decompiler
// Type: FishTrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FishTrapConfig : IBuildingConfig
{
  public const string ID = "FishTrap";
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, TUNING.MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.AudioCategory = "Metal";
    buildingDef.Floodable = false;
    buildingDef.Deprecated = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = true;
    storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
    storage.sendOnStoreOnSpawn = true;
    TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
    trapTrigger.trappableCreatures = new Tag[1]
    {
      GameTags.Creatures.Swimmer
    };
    trapTrigger.trappedOffset = new Vector2(0.0f, 1f);
    go.AddOrGet<Trap>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Lure.Def def = go.AddOrGetDef<Lure.Def>();
    def.defaultLurePoints = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    def.radius = 32 /*0x20*/;
    def.initialLures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
  }
}
