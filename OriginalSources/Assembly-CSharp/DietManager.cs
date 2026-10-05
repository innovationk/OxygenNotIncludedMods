// Decompiled with JetBrains decompiler
// Type: DietManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
  private Dictionary<Tag, Diet> diets;
  public static DietManager Instance;

  public static void DestroyInstance() => DietManager.Instance = (DietManager) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.diets = DietManager.CollectSaveDiets((Tag[]) null);
    DietManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
      this.Discover(tag);
    foreach (KeyValuePair<Tag, Diet> diet in this.diets)
    {
      foreach (Diet.Info info in diet.Value.infos)
      {
        foreach (Tag consumedTag in info.consumedTags)
        {
          if ((UnityEngine.Object) Assets.GetPrefab(consumedTag) == (UnityEngine.Object) null)
            Debug.LogError((object) $"Could not find prefab {consumedTag}, required by diet for {diet.Key}");
        }
      }
    }
    DiscoveredResources.Instance.OnDiscover += new Action<Tag, Tag>(this.OnWorldInventoryDiscover);
  }

  private void Discover(Tag tag)
  {
    foreach (KeyValuePair<Tag, Diet> diet in this.diets)
    {
      if (diet.Value.GetDietInfo(tag) != null)
        DiscoveredResources.Instance.Discover(tag, diet.Key);
    }
  }

  private void OnWorldInventoryDiscover(Tag category_tag, Tag tag) => this.Discover(tag);

  public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
  {
    Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
    foreach (KPrefabID prefab in Assets.Prefabs)
    {
      CreatureCalorieMonitor.Def def1 = prefab.GetDef<CreatureCalorieMonitor.Def>();
      BeehiveCalorieMonitor.Def def2 = prefab.GetDef<BeehiveCalorieMonitor.Def>();
      Diet diet = (Diet) null;
      if (def1 != null)
        diet = def1.diet;
      else if (def2 != null)
        diet = def2.diet;
      if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, prefab.GetComponent<CreatureBrain>().species) >= 0))
        dictionary[prefab.PrefabTag] = diet;
    }
    return dictionary;
  }

  public static Dictionary<Tag, Diet> CollectSaveDiets(Tag[] target_species)
  {
    Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
    foreach (KPrefabID prefab in Assets.Prefabs)
    {
      CreatureCalorieMonitor.Def def1 = prefab.GetDef<CreatureCalorieMonitor.Def>();
      BeehiveCalorieMonitor.Def def2 = prefab.GetDef<BeehiveCalorieMonitor.Def>();
      Diet diet = (Diet) null;
      if (def1 != null)
        diet = def1.diet;
      else if (def2 != null)
        diet = def2.diet;
      if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, prefab.GetComponent<CreatureBrain>().species) >= 0))
      {
        dictionary[prefab.PrefabTag] = new Diet(diet);
        dictionary[prefab.PrefabTag].FilterDLC();
      }
    }
    return dictionary;
  }

  public Diet GetPrefabDiet(GameObject owner)
  {
    Diet diet;
    return this.diets.TryGetValue(owner.GetComponent<KPrefabID>().PrefabTag, out diet) ? diet : (Diet) null;
  }
}
