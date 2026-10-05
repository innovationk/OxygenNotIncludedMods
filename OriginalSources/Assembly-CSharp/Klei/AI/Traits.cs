// Decompiled with JetBrains decompiler
// Type: Klei.AI.Traits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
public class Traits : KMonoBehaviour, ISaveLoadable
{
  public List<Trait> TraitList = new List<Trait>();
  [Serialize]
  private List<string> TraitIds = new List<string>();

  public List<string> GetTraitIds() => this.TraitIds;

  public void SetTraitIds(List<string> traits) => this.TraitIds = traits;

  protected override void OnSpawn()
  {
    foreach (string traitId in this.TraitIds)
    {
      if (Db.Get().traits.Exists(traitId))
      {
        Trait trait = Db.Get().traits.Get(traitId);
        if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) trait))
          this.AddInternal(trait);
      }
    }
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
      return;
    List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
    if (!(bool) (UnityEngine.Object) this.GetComponent<MinionIdentity>())
      return;
    bool flag = true;
    foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
    {
      if (this.HasTrait(traitVal.id))
        flag = false;
    }
    if (!flag)
      return;
    DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
    this.Add(Db.Get().traits.Get(random.id));
  }

  private void AddInternal(Trait trait)
  {
    if (this.HasTrait(trait))
      return;
    this.TraitList.Add(trait);
    trait.AddTo(this.GetAttributes());
    if (trait.OnAddTrait == null)
      return;
    trait.OnAddTrait(this.gameObject);
  }

  public void Add(Trait trait)
  {
    DebugUtil.Assert(this.IsInitialized() || this.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, this.gameObject.name);
    if (trait.ShouldSave)
      this.TraitIds.Add(trait.Id);
    this.AddInternal(trait);
  }

  public bool HasTrait(string trait_id)
  {
    bool flag = false;
    foreach (Resource trait in this.TraitList)
    {
      if (trait.Id == trait_id)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public bool HasTrait(Trait trait)
  {
    foreach (Trait trait1 in this.TraitList)
    {
      if (trait1 == trait)
        return true;
    }
    return false;
  }

  public void Clear()
  {
    while (this.TraitList.Count > 0)
      this.Remove(this.TraitList[0]);
  }

  public void Remove(Trait trait)
  {
    for (int index = 0; index < this.TraitList.Count; ++index)
    {
      if (this.TraitList[index] == trait)
      {
        this.TraitList.RemoveAt(index);
        this.TraitIds.Remove(trait.Id);
        trait.RemoveFrom(this.GetAttributes());
        break;
      }
    }
  }

  public bool IsEffectIgnored(Effect effect)
  {
    foreach (Trait trait in this.TraitList)
    {
      if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
        return true;
    }
    return false;
  }

  public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
  {
    return this.IsChoreGroupDisabled(choreGroup, out Trait _);
  }

  public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
  {
    return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
  }

  public bool IsChoreGroupDisabled(HashedString choreGroupId)
  {
    return this.IsChoreGroupDisabled(choreGroupId, out Trait _);
  }

  public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
  {
    foreach (Trait trait in this.TraitList)
    {
      if (trait.disabledChoreGroups != null)
      {
        foreach (Resource disabledChoreGroup in trait.disabledChoreGroups)
        {
          if (disabledChoreGroup.IdHash == choreGroupId)
          {
            disablingTrait = trait;
            return true;
          }
        }
      }
    }
    disablingTrait = (Trait) null;
    return false;
  }
}
