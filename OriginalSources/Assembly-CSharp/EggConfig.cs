// Decompiled with JetBrains decompiler
// Type: EggConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class EggConfig
{
  public static Dictionary<Tag, List<Tuple<Tag, float>>> CUSTOM_EGG_OUTPUTS = new Dictionary<Tag, List<Tuple<Tag, float>>>();

  [Obsolete("Mod compatibility: Use CreateEgg with requiredDlcIds and forbiddenDlcIds")]
  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate)
  {
    return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, (string[]) null, (string[]) null);
  }

  [Obsolete("Mod compatibility: Use CreateEgg with requiredDlcIds and forbiddenDlcIds")]
  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate,
    string[] dlcIds)
  {
    string[] requiredDlcIds;
    string[] forbiddenDlcIds;
    DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out requiredDlcIds, out forbiddenDlcIds);
    return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds);
  }

  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, false);
  }

  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds,
    bool preventEggDrops)
  {
    return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, preventEggDrops, mass);
  }

  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds,
    bool preventEggDrops,
    float eggMassToDrop)
  {
    return EggConfig.CreateEgg(id, name, desc, creature_id, anim, mass, egg_sort_order, base_incubation_rate, requiredDlcIds, forbiddenDlcIds, preventEggDrops, eggMassToDrop, 0.5f);
  }

  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds,
    bool preventEggDrops,
    float eggMassToDrop,
    float customEggShellRatio,
    bool allowCrackerRecipeCreation = true)
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(id, name, desc, mass, true, Assets.GetAnim((HashedString) anim), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true);
    looseEntity.AddOrGet<KBoxCollider2D>().offset = (Vector2) new Vector2f(0.0f, 0.36f);
    looseEntity.AddOrGet<Pickupable>().sortOrder = SORTORDER.EGGS + egg_sort_order;
    looseEntity.AddOrGet<Effects>();
    KPrefabID kprefabId = looseEntity.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.Egg);
    kprefabId.AddTag(GameTags.IncubatableEgg);
    kprefabId.AddTag(GameTags.PedestalDisplayable);
    kprefabId.requiredDlcIds = requiredDlcIds;
    kprefabId.forbiddenDlcIds = forbiddenDlcIds;
    IncubationMonitor.Def def = looseEntity.AddOrGetDef<IncubationMonitor.Def>();
    def.preventEggDrops = preventEggDrops;
    def.eggShellRatio = customEggShellRatio;
    def.spawnedCreature = creature_id;
    def.baseIncubationRate = base_incubation_rate;
    looseEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    UnityEngine.Object.Destroy((UnityEngine.Object) looseEntity.GetComponent<EntitySplitter>());
    Assets.AddPrefab(looseEntity.GetComponent<KPrefabID>());
    EggCrackerConfig.RegisterEgg((Tag) id, name, desc, eggMassToDrop, requiredDlcIds, forbiddenDlcIds, EggConfig.CUSTOM_EGG_OUTPUTS.ContainsKey(creature_id) ? EggConfig.CUSTOM_EGG_OUTPUTS[creature_id].ToArray() : (Tuple<Tag, float>[]) null, customEggShellRatio, allowCrackerRecipeCreation);
    return looseEntity;
  }
}
