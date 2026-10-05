// Decompiled with JetBrains decompiler
// Type: MutantPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
  [Serialize]
  private bool analyzed;
  [Serialize]
  private List<string> mutationIDs;
  private List<Guid> statusItemHandles = new List<Guid>();
  private const int MAX_MUTATIONS = 1;
  [SerializeField]
  private Tag speciesID;
  private Tag cachedSubspeciesID;
  private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>((Action<MutantPlant, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>((Action<MutantPlant, object>) ((component, data) => component.OnSplitFromChunk(data)));

  public List<string> MutationIDs => this.mutationIDs;

  public bool IsOriginal => this.mutationIDs == null || this.mutationIDs.Count == 0;

  public bool IsIdentified
  {
    get
    {
      return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
    }
  }

  public Tag SpeciesID
  {
    get
    {
      Debug.Assert(this.speciesID != (Tag) (string) null, (object) "Ack, forgot to configure the species ID for this mutantPlant!");
      return this.speciesID;
    }
    set => this.speciesID = value;
  }

  public Tag SubSpeciesID
  {
    get
    {
      if (this.cachedSubspeciesID == (Tag) (string) null)
        this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
      return this.cachedSubspeciesID;
    }
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
    this.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
  }

  protected override void OnSpawn()
  {
    if (this.IsOriginal || this.HasTag(GameTags.Plant))
      this.analyzed = true;
    if (!this.IsOriginal)
      this.AddTag(GameTags.MutatedSeed);
    this.AddTag(this.SubSpeciesID);
    Components.MutantPlants.Add(this);
    base.OnSpawn();
    this.ApplyMutations();
    this.UpdateNameAndTags();
    PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
  }

  protected override void OnCleanUp()
  {
    Components.MutantPlants.Remove(this);
    base.OnCleanUp();
  }

  private void OnAbsorb(object data)
  {
    MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
    Debug.Assert((UnityEngine.Object) component != (UnityEngine.Object) null && this.SubSpeciesID == component.SubSpeciesID, (object) "Two seeds of different subspecies just absorbed!");
  }

  private void OnSplitFromChunk(object data)
  {
    MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.CopyMutationsTo(this);
  }

  public void Mutate()
  {
    List<string> mutations = this.mutationIDs != null ? new List<string>((IEnumerable<string>) this.mutationIDs) : new List<string>();
    while (mutations.Count >= 1 && mutations.Count > 0)
      mutations.RemoveAt(UnityEngine.Random.Range(0, mutations.Count));
    mutations.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
    this.SetSubSpecies(mutations);
  }

  public void Analyze()
  {
    this.analyzed = true;
    this.UpdateNameAndTags();
  }

  public void ApplyMutations()
  {
    if (this.IsOriginal)
      return;
    foreach (string mutationId in this.mutationIDs)
      Db.Get().PlantMutations.Get(mutationId).ApplyTo(this);
  }

  public void DummySetSubspecies(List<string> mutations) => this.mutationIDs = mutations;

  public void SetSubSpecies(List<string> mutations)
  {
    if (this.gameObject.HasTag(this.SubSpeciesID))
      this.gameObject.RemoveTag(this.SubSpeciesID);
    this.cachedSubspeciesID = Tag.Invalid;
    this.mutationIDs = mutations;
    this.UpdateNameAndTags();
  }

  public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
  {
    return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
  }

  public void CopyMutationsTo(MutantPlant target)
  {
    target.SetSubSpecies(this.mutationIDs);
    target.analyzed = this.analyzed;
  }

  public void UpdateNameAndTags()
  {
    bool identified = !this.IsInitialized() || this.IsIdentified;
    bool cleanOriginal = (UnityEngine.Object) PlantSubSpeciesCatalog.Instance == (UnityEngine.Object) null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
    KPrefabID component1 = this.GetComponent<KPrefabID>();
    component1.AddTag(this.SubSpeciesID);
    component1.SetTag(GameTags.UnidentifiedSeed, !identified);
    this.gameObject.name = $"{component1.PrefabTag.ToString()} ({this.SubSpeciesID.ToString()})";
    this.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component1.PrefabTag.ProperName(), identified, cleanOriginal));
    KSelectable component2 = this.GetComponent<KSelectable>();
    foreach (Guid statusItemHandle in this.statusItemHandles)
      component2.RemoveStatusItem(statusItemHandle);
    this.statusItemHandles.Clear();
    if (cleanOriginal)
      return;
    if (this.IsOriginal)
      this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation));
    else if (!identified)
    {
      this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation));
    }
    else
    {
      foreach (string mutationId in this.mutationIDs)
        this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, (object) Db.Get().PlantMutations.Get(mutationId)));
    }
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    if (this.IsOriginal)
      return (List<Descriptor>) null;
    List<Descriptor> descriptors = new List<Descriptor>();
    foreach (string mutationId in this.mutationIDs)
      Db.Get().PlantMutations.Get(mutationId).GetDescriptors(ref descriptors, go);
    return descriptors;
  }

  public List<string> GetSoundEvents()
  {
    List<string> soundEvents = new List<string>();
    if (this.mutationIDs != null)
    {
      foreach (string mutationId in this.mutationIDs)
      {
        PlantMutation plantMutation = Db.Get().PlantMutations.Get(mutationId);
        soundEvents.AddRange((IEnumerable<string>) plantMutation.AdditionalSoundEvents);
      }
    }
    return soundEvents;
  }
}
