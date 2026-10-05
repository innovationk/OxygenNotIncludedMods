// Decompiled with JetBrains decompiler
// Type: GraveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GraveConfig : IBuildingConfig
{
  public const string ID = "Grave";
  public const string AnimFile = "gravestone_kanim";
  private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;
  private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[1]
  {
    (HashedString) "working_pre"
  };
  private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;
  private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Grave", 1, 2, "gravestone_kanim", 30, 120f, tieR5, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bury_dupe_kanim")
    };
    GraveStorage graveStorage = go.AddOrGet<GraveStorage>();
    graveStorage.showInUI = true;
    graveStorage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
    graveStorage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
    graveStorage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
    graveStorage.workingPstComplete = new HashedString[1]
    {
      GraveConfig.STORAGE_PST_ANIM
    };
    graveStorage.synchronizeAnims = false;
    graveStorage.useGunForDelivery = false;
    graveStorage.workAnimPlayMode = KAnim.PlayMode.Once;
    go.AddOrGet<Grave>();
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().prefabInitFn += new KPrefabID.PrefabFn(this.OnInit);
  }

  private void OnInit(GameObject go)
  {
    GraveStorage graveStorage = go.AddOrGet<GraveStorage>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bury_dupe_kanim")
    };
    graveStorage.workerTypeOverrideAnims.Add((Tag) MinionConfig.ID, kanimFileArray);
    graveStorage.workerTypeOverrideAnims.Add((Tag) BionicMinionConfig.ID, new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bionic_bury_dupe_kanim")
    });
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
