// Decompiled with JetBrains decompiler
// Type: DiscoveredResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
  public static DiscoveredResources Instance;
  [Serialize]
  private HashSet<Tag> Discovered = new HashSet<Tag>();
  [Serialize]
  private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();
  [Serialize]
  public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();

  public static void DestroyInstance() => DiscoveredResources.Instance = (DiscoveredResources) null;

  public event Action<Tag, Tag> OnDiscover;

  public void Discover(Tag tag, Tag categoryTag)
  {
    int num = this.Discovered.Add(tag) ? 1 : 0;
    this.DiscoverCategory(categoryTag, tag);
    if (num == 0)
      return;
    if (this.OnDiscover != null)
      this.OnDiscover(categoryTag, tag);
    if (this.newDiscoveries.ContainsKey(tag))
      return;
    this.newDiscoveries.Add(tag, (float) GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
  }

  public void Discover(Tag tag)
  {
    this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DiscoveredResources.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.FilterDisabledContent();
  }

  private void FilterDisabledContent()
  {
    HashSet<Tag> tagSet = new HashSet<Tag>();
    foreach (Tag tag in this.Discovered)
    {
      Element element = ElementLoader.GetElement(tag);
      if (element != null && element.disabled)
      {
        tagSet.Add(tag);
      }
      else
      {
        GameObject prefab = Assets.TryGetPrefab(tag);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && prefab.HasTag(GameTags.DeprecatedContent))
          tagSet.Add(tag);
        else if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
          tagSet.Add(tag);
      }
    }
    foreach (Tag tag in tagSet)
      this.Discovered.Remove(tag);
    foreach (KeyValuePair<Tag, HashSet<Tag>> discoveredCategory in this.DiscoveredCategories)
    {
      foreach (Tag tag in tagSet)
      {
        if (discoveredCategory.Value.Contains(tag))
          discoveredCategory.Value.Remove(tag);
      }
    }
    foreach (string key in new List<string>()
    {
      "Pacu",
      "PacuCleaner",
      "PacuTropical",
      "PacuBaby",
      "PacuCleanerBaby",
      "PacuTropicalBaby"
    })
    {
      if (this.DiscoveredCategories.ContainsKey((Tag) key))
      {
        List<Tag> list = this.DiscoveredCategories[(Tag) key].ToList<Tag>();
        SolidConsumerMonitor.Def def = Assets.GetPrefab((Tag) key).GetDef<SolidConsumerMonitor.Def>();
        foreach (Tag tag in list)
        {
          if (def.diet.GetDietInfo(tag) == null)
            this.DiscoveredCategories[(Tag) key].Remove(tag);
        }
      }
    }
    if (this.DiscoveredCategories.ContainsKey(GameTags.IndustrialIngredient))
    {
      foreach (string item_tag in new List<string>()
      {
        "CrabShell",
        "CrabWoodShell"
      })
      {
        if (this.DiscoveredCategories[GameTags.IndustrialIngredient].Contains((Tag) item_tag))
        {
          this.DiscoveredCategories[GameTags.IndustrialIngredient].Remove((Tag) item_tag);
          this.DiscoverCategory(GameTags.Organics, (Tag) item_tag);
        }
      }
    }
    if (!this.DiscoveredCategories.ContainsKey(GameTags.IndustrialIngredient))
      return;
    foreach (string item_tag in new List<string>()
    {
      "OrbitalResearchDatabank",
      "ResearchDatabank"
    })
    {
      if (this.DiscoveredCategories[GameTags.IndustrialIngredient].Contains((Tag) item_tag))
      {
        this.DiscoveredCategories[GameTags.IndustrialIngredient].Remove((Tag) item_tag);
        this.DiscoverCategory(GameTags.TechComponents, (Tag) item_tag);
      }
    }
  }

  public bool CheckAllDiscoveredAreNew()
  {
    foreach (Tag key in this.Discovered)
    {
      if (!this.newDiscoveries.ContainsKey(key))
        return false;
    }
    return true;
  }

  private void DiscoverCategory(Tag category_tag, Tag item_tag)
  {
    HashSet<Tag> tagSet;
    if (!this.DiscoveredCategories.TryGetValue(category_tag, out tagSet))
    {
      tagSet = new HashSet<Tag>();
      this.DiscoveredCategories[category_tag] = tagSet;
    }
    tagSet.Add(item_tag);
  }

  public HashSet<Tag> GetDiscovered() => this.Discovered;

  public bool IsDiscovered(Tag tag)
  {
    return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
  }

  public bool AnyDiscovered(ICollection<Tag> tags)
  {
    foreach (Tag tag in (IEnumerable<Tag>) tags)
    {
      if (this.IsDiscovered(tag))
        return true;
    }
    return false;
  }

  public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
  {
    return this.DiscoveredCategories.TryGetValue(tag, out resources);
  }

  public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
  {
    HashSet<Tag> tagSet;
    return this.DiscoveredCategories.TryGetValue(tag, out tagSet) ? tagSet : new HashSet<Tag>();
  }

  public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
  {
    Dictionary<Tag, HashSet<Tag>> resourcesFromTagSet = new Dictionary<Tag, HashSet<Tag>>();
    foreach (Tag tag in tagSet)
    {
      HashSet<Tag> tagSet1;
      if (this.DiscoveredCategories.TryGetValue(tag, out tagSet1))
        resourcesFromTagSet[tag] = tagSet1;
    }
    return resourcesFromTagSet;
  }

  public static Tag GetCategoryForTags(HashSet<Tag> tags)
  {
    Tag categoryForTags = Tag.Invalid;
    foreach (Tag tag in tags)
    {
      if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
      {
        categoryForTags = tag;
        break;
      }
    }
    return categoryForTags;
  }

  public static Tag GetCategoryForEntity(KPrefabID entity)
  {
    ElementChunk component = entity.GetComponent<ElementChunk>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetComponent<PrimaryElement>().Element.materialCategory : DiscoveredResources.GetCategoryForTags(entity.Tags);
  }

  public void Sim4000ms(float dt)
  {
    float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, float> newDiscovery in this.newDiscoveries)
    {
      if ((double) num - (double) newDiscovery.Value > 3.0)
        tagList.Add(newDiscovery.Key);
    }
    foreach (Tag key in tagList)
      this.newDiscoveries.Remove(key);
  }
}
