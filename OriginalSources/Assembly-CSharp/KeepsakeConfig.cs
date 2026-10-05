// Decompiled with JetBrains decompiler
// Type: KeepsakeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class KeepsakeConfig : IMultiEntityConfig
{
  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    prefabs.Add(KeepsakeConfig.CreateKeepsake("MegaBrain", (string) UI.KEEPSAKES.MEGA_BRAIN.NAME, (string) UI.KEEPSAKES.MEGA_BRAIN.DESCRIPTION, "keepsake_mega_brain_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    prefabs.Add(KeepsakeConfig.CreateKeepsake("CritterManipulator", (string) UI.KEEPSAKES.CRITTER_MANIPULATOR.NAME, (string) UI.KEEPSAKES.CRITTER_MANIPULATOR.DESCRIPTION, "keepsake_critter_manipulator_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    prefabs.Add(KeepsakeConfig.CreateKeepsake("LonelyMinion", (string) UI.KEEPSAKES.LONELY_MINION.NAME, (string) UI.KEEPSAKES.LONELY_MINION.DESCRIPTION, "keepsake_lonelyminion_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    prefabs.Add(KeepsakeConfig.CreateKeepsake("FossilHunt", (string) UI.KEEPSAKES.FOSSIL_HUNT.NAME, (string) UI.KEEPSAKES.FOSSIL_HUNT.DESCRIPTION, "keepsake_fossil_dig_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    prefabs.Add(KeepsakeConfig.CreateKeepsake("HijackHeadquarters", (string) UI.KEEPSAKES.HIJACK_HEADQUARTERS.NAME, (string) UI.KEEPSAKES.HIJACK_HEADQUARTERS.DESCRIPTION, "keepsake_hijacked_hq_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    prefabs.Add(KeepsakeConfig.CreateKeepsake("GeothermalPlant", (string) UI.KEEPSAKES.GEOTHERMAL_PLANT.NAME, (string) UI.KEEPSAKES.GEOTHERMAL_PLANT.DESCRIPTION, "keepsake_geothermal_vent_kanim", "idle", "ui", DlcManager.DLC2, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature));
    GameObject keepsake1 = KeepsakeConfig.CreateKeepsake("MorbRoverMaker", (string) UI.KEEPSAKES.MORB_ROVER_MAKER.NAME, (string) UI.KEEPSAKES.MORB_ROVER_MAKER.DESCRIPTION, "keepsake_morb_tank_kanim", "idle", "ui", (string[]) null, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature);
    keepsake1.AddOrGetDef<MorbRoverMakerKeepsake.Def>();
    prefabs.Add(keepsake1);
    GameObject keepsake2 = KeepsakeConfig.CreateKeepsake("LargeImpactor", (string) UI.KEEPSAKES.VIEWMASTER.NAME, (string) UI.KEEPSAKES.VIEWMASTER.DESCRIPTION, "keepsake_demolior_kanim", "idle", "ui", DlcManager.DLC4, (string[]) null, (KeepsakeConfig.PostInitFn) null, SimHashes.Creature);
    if ((UnityEngine.Object) keepsake2 != (UnityEngine.Object) null)
    {
      keepsake2.GetComponent<KBoxCollider2D>().size = new Vector2(1f, 0.7f);
      keepsake2.AddOrGetDef<LargeImpactorKeepsake.Def>();
      prefabs.Add(keepsake2);
    }
    prefabs.RemoveAll((Predicate<GameObject>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
    return prefabs;
  }

  public static GameObject CreateKeepsake(
    string id,
    string name,
    string desc,
    string animFile,
    string initial_anim = "idle",
    string ui_anim = "ui",
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    KeepsakeConfig.PostInitFn postInitFn = null,
    SimHashes element = SimHashes.Creature)
  {
    if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
      return (GameObject) null;
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("keepsake_" + id.ToLower(), name, desc, 25f, true, Assets.GetAnim((HashedString) animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, isPickupable: true, sortOrder: SORTORDER.KEEPSAKES, element: element, additionalTags: new List<Tag>()
    {
      GameTags.MiscPickupable
    });
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(TUNING.DECOR.BONUS.TIER1);
    decorProvider.overrideName = looseEntity.GetProperName();
    looseEntity.AddOrGet<KSelectable>();
    looseEntity.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
    if (postInitFn != null)
      postInitFn(looseEntity);
    KPrefabID component1 = looseEntity.GetComponent<KPrefabID>();
    component1.AddTag(GameTags.PedestalDisplayable);
    component1.AddTag(GameTags.Keepsake);
    component1.AddTag(GameTags.Ornament);
    KPrefabID component2 = looseEntity.GetComponent<KPrefabID>();
    component2.requiredDlcIds = requiredDlcIds;
    component2.forbiddenDlcIds = forbiddenDlcIds;
    looseEntity.AddOrGet<CodexEntryRedirector>().CodexID = "REQUIREMENTCLASSORNAMENT";
    return looseEntity;
  }

  [Obsolete]
  public static GameObject CreateKeepsake(
    string id,
    string name,
    string desc,
    string animFile,
    string initial_anim = "idle",
    string ui_anim = "ui",
    string[] dlcIDs = null,
    KeepsakeConfig.PostInitFn postInitFn = null,
    SimHashes element = SimHashes.Creature)
  {
    DlcRestrictionsUtil.TemporaryHelperObject objectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIDs);
    return KeepsakeConfig.CreateKeepsake(id, name, desc, animFile, initial_anim, ui_anim, objectFromAllowList.requiredDlcIds, objectFromAllowList.forbiddenDlcIds, postInitFn, element);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public delegate void PostInitFn(GameObject gameObject);
}
