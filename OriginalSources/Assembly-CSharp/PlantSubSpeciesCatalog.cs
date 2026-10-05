// Decompiled with JetBrains decompiler
// Type: PlantSubSpeciesCatalog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantSubSpeciesCatalog : KMonoBehaviour
{
  public static PlantSubSpeciesCatalog Instance;
  [Serialize]
  private Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> discoveredSubspeciesBySpecies = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();
  [Serialize]
  private HashSet<Tag> identifiedSubSpecies = new HashSet<Tag>();

  public static void DestroyInstance()
  {
    PlantSubSpeciesCatalog.Instance = (PlantSubSpeciesCatalog) null;
  }

  public bool AnyNonOriginalDiscovered
  {
    get
    {
      foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> subspeciesBySpecy in this.discoveredSubspeciesBySpecies)
      {
        if (subspeciesBySpecy.Value.Find((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (ss => !ss.IsOriginal)).IsValid)
          return true;
      }
      return false;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PlantSubSpeciesCatalog.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.EnsureOriginalSubSpecies();
    this.RemoveInvalidMutantPlants();
  }

  public List<Tag> GetAllDiscoveredSpecies()
  {
    return this.discoveredSubspeciesBySpecies.Keys.ToList<Tag>();
  }

  public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllSubSpeciesForSpecies(Tag speciesID)
  {
    List<PlantSubSpeciesCatalog.SubSpeciesInfo> subSpeciesInfoList;
    return this.discoveredSubspeciesBySpecies.TryGetValue(speciesID, out subSpeciesInfoList) ? subSpeciesInfoList : (List<PlantSubSpeciesCatalog.SubSpeciesInfo>) null;
  }

  public bool GetOriginalSubSpecies(
    Tag speciesID,
    out PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo)
  {
    if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
    {
      subSpeciesInfo = new PlantSubSpeciesCatalog.SubSpeciesInfo();
      return false;
    }
    subSpeciesInfo = this.discoveredSubspeciesBySpecies[speciesID].Find((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (i => i.IsOriginal));
    return true;
  }

  public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpecies(Tag speciesID, Tag subSpeciesID)
  {
    return this.discoveredSubspeciesBySpecies[speciesID].Find((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (i => i.ID == subSpeciesID));
  }

  public PlantSubSpeciesCatalog.SubSpeciesInfo FindSubSpecies(Tag subSpeciesID)
  {
    foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> subspeciesBySpecy in this.discoveredSubspeciesBySpecies)
    {
      PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = subspeciesBySpecy.Value.Find((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (i => i.ID == subSpeciesID));
      if (subSpecies.ID.IsValid)
        return subSpecies;
    }
    return new PlantSubSpeciesCatalog.SubSpeciesInfo();
  }

  public void DiscoverSubSpecies(
    PlantSubSpeciesCatalog.SubSpeciesInfo newSubSpeciesInfo,
    MutantPlant source)
  {
    if (this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Contains(newSubSpeciesInfo))
      return;
    this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Add(newSubSpeciesInfo);
    Notification notification = new Notification((string) MISC.NOTIFICATIONS.NEWMUTANTSEED.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(this.NewSubspeciesTooltipCB), (object) newSubSpeciesInfo, click_focus: source.transform);
    this.gameObject.AddOrGet<Notifier>().Add(notification);
  }

  private string NewSubspeciesTooltipCB(List<Notification> notifications, object data)
  {
    PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = (PlantSubSpeciesCatalog.SubSpeciesInfo) data;
    return MISC.NOTIFICATIONS.NEWMUTANTSEED.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
  }

  public void IdentifySubSpecies(Tag subSpeciesID)
  {
    if (!this.identifiedSubSpecies.Add(subSpeciesID))
      return;
    this.FindSubSpecies(subSpeciesID);
    foreach (MutantPlant mutantPlant in Components.MutantPlants)
    {
      if (mutantPlant.HasTag(subSpeciesID))
        mutantPlant.UpdateNameAndTags();
    }
    GeneticAnalysisCompleteMessage analysisCompleteMessage = new GeneticAnalysisCompleteMessage(subSpeciesID);
    Messenger.Instance.QueueMessage((Message) analysisCompleteMessage);
  }

  public bool IsSubSpeciesIdentified(Tag subSpeciesID)
  {
    return this.identifiedSubSpecies.Contains(subSpeciesID);
  }

  public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllUnidentifiedSubSpecies(Tag speciesID)
  {
    return this.discoveredSubspeciesBySpecies[speciesID].FindAll((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (ss => !this.IsSubSpeciesIdentified(ss.ID)));
  }

  public bool IsValidPlantableSeed(Tag seedID, Tag subspeciesID)
  {
    if (!seedID.IsValid)
      return false;
    MutantPlant component = Assets.GetPrefab(seedID).GetComponent<MutantPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return !subspeciesID.IsValid;
    List<PlantSubSpeciesCatalog.SubSpeciesInfo> speciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(component.SpeciesID);
    return speciesForSpecies != null && speciesForSpecies.FindIndex((Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo>) (s => s.ID == subspeciesID)) != -1 && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesID);
  }

  private void EnsureOriginalSubSpecies()
  {
    foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<MutantPlant>())
    {
      MutantPlant component = gameObject.GetComponent<MutantPlant>();
      Tag speciesId = component.SpeciesID;
      if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesId))
      {
        this.discoveredSubspeciesBySpecies[speciesId] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>();
        this.discoveredSubspeciesBySpecies[speciesId].Add(component.GetSubSpeciesInfo());
      }
      this.identifiedSubSpecies.Add(component.SubSpeciesID);
    }
  }

  private void RemoveInvalidMutantPlants()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> subspeciesBySpecy in this.discoveredSubspeciesBySpecies)
    {
      GameObject prefab = Assets.GetPrefab(subspeciesBySpecy.Key);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && (UnityEngine.Object) prefab.GetComponent<MutantPlant>() == (UnityEngine.Object) null)
        tagList.Add(subspeciesBySpecy.Key);
    }
    foreach (Tag key in tagList)
    {
      foreach (PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo in this.discoveredSubspeciesBySpecies[key])
        this.identifiedSubSpecies.Remove(subSpeciesInfo.ID);
      this.discoveredSubspeciesBySpecies.Remove(key);
    }
  }

  [Serializable]
  public struct SubSpeciesInfo(Tag speciesID, List<string> mutationIDs) : 
    IEquatable<PlantSubSpeciesCatalog.SubSpeciesInfo>
  {
    public Tag speciesID = speciesID;
    public Tag ID = PlantSubSpeciesCatalog.SubSpeciesInfo.SubSpeciesIDFromMutations(speciesID, mutationIDs);
    public List<string> mutationIDs = mutationIDs != null ? new List<string>((IEnumerable<string>) mutationIDs) : new List<string>();
    private const string ORIGINAL_SUFFIX = "_Original";

    public bool IsValid => this.ID.IsValid;

    public bool IsOriginal => this.mutationIDs == null || this.mutationIDs.Count == 0;

    public static Tag SubSpeciesIDFromMutations(Tag speciesID, List<string> mutationIDs)
    {
      if (mutationIDs == null || mutationIDs.Count == 0)
        return (Tag) (speciesID.ToString() + "_Original");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append((object) speciesID);
      foreach (string mutationId in mutationIDs)
      {
        stringBuilder.Append("_");
        stringBuilder.Append(mutationId);
      }
      return stringBuilder.ToString().ToTag();
    }

    public string GetMutationsNames()
    {
      return this.mutationIDs == null || this.mutationIDs.Count == 0 ? (string) CREATURES.PLANT_MUTATIONS.NONE.NAME : string.Join(", ", (IEnumerable<string>) Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs));
    }

    public string GetNameWithMutations(string properName, bool identified, bool cleanOriginal)
    {
      return this.mutationIDs == null || this.mutationIDs.Count == 0 ? (!cleanOriginal ? CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", (string) CREATURES.PLANT_MUTATIONS.NONE.NAME) : properName) : (identified ? CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", string.Join(", ", (IEnumerable<string>) Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs))) : CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", (string) CREATURES.PLANT_MUTATIONS.UNIDENTIFIED));
    }

    public static bool operator ==(
      PlantSubSpeciesCatalog.SubSpeciesInfo obj1,
      PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
    {
      return obj1.Equals(obj2);
    }

    public static bool operator !=(
      PlantSubSpeciesCatalog.SubSpeciesInfo obj1,
      PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
    {
      return !(obj1 == obj2);
    }

    public override bool Equals(object other)
    {
      return other is PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo && this == subSpeciesInfo;
    }

    public bool Equals(PlantSubSpeciesCatalog.SubSpeciesInfo other) => this.ID == other.ID;

    public override int GetHashCode() => this.ID.GetHashCode();

    public string GetMutationsTooltip()
    {
      if (this.mutationIDs == null || this.mutationIDs.Count == 0)
        return (string) CREATURES.STATUSITEMS.ORIGINALPLANTMUTATION.TOOLTIP;
      if (!PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.ID))
        return (string) CREATURES.STATUSITEMS.UNKNOWNMUTATION.TOOLTIP;
      string mutationId = this.mutationIDs[0];
      PlantMutation plantMutation = Db.Get().PlantMutations.Get(mutationId);
      return $"{CREATURES.STATUSITEMS.SPECIFICPLANTMUTATION.TOOLTIP.Replace("{MutationName}", plantMutation.Name)}\n{plantMutation.GetTooltip()}";
    }
  }
}
