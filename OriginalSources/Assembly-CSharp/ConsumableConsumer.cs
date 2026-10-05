// Decompiled with JetBrains decompiler
// Type: ConsumableConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
  [Obsolete("Deprecated, use forbiddenTagSet")]
  [Serialize]
  [HideInInspector]
  public Tag[] forbiddenTags;
  [Serialize]
  public HashSet<Tag> forbiddenTagSet;
  public HashSet<Tag> dietaryRestrictionTagSet;
  public System.Action consumableRulesChanged;

  [System.Runtime.Serialization.OnDeserialized]
  [Obsolete]
  private void OnDeserialized()
  {
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
      return;
    this.forbiddenTagSet = new HashSet<Tag>((IEnumerable<Tag>) this.forbiddenTags);
    this.forbiddenTags = (Tag[]) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if ((UnityEngine.Object) ConsumerManager.instance != (UnityEngine.Object) null)
    {
      this.forbiddenTagSet = new HashSet<Tag>((IEnumerable<Tag>) ConsumerManager.instance.DefaultForbiddenTagsList);
      this.SetModelDietaryRestrictions();
    }
    else
    {
      this.forbiddenTagSet = new HashSet<Tag>();
      this.dietaryRestrictionTagSet = new HashSet<Tag>();
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetModelDietaryRestrictions();
  }

  private void SetModelDietaryRestrictions()
  {
    if (this.HasTag(GameTags.Minions.Models.Standard))
    {
      this.dietaryRestrictionTagSet = new HashSet<Tag>((IEnumerable<Tag>) ConsumerManager.instance.StandardDuplicantDietaryRestrictions);
    }
    else
    {
      if (!this.HasTag(GameTags.Minions.Models.Bionic))
        return;
      this.dietaryRestrictionTagSet = new HashSet<Tag>((IEnumerable<Tag>) ConsumerManager.instance.BionicDuplicantDietaryRestrictions);
    }
  }

  public bool IsPermitted(string consumable_id)
  {
    Tag tag = new Tag(consumable_id);
    return !this.forbiddenTagSet.Contains(tag) && !this.dietaryRestrictionTagSet.Contains(tag);
  }

  public bool IsDietRestricted(string consumable_id)
  {
    return this.dietaryRestrictionTagSet.Contains(new Tag(consumable_id));
  }

  public void SetPermitted(string consumable_id, bool is_allowed)
  {
    Tag tag = new Tag(consumable_id);
    is_allowed = is_allowed && !this.dietaryRestrictionTagSet.Contains((Tag) consumable_id);
    if (is_allowed)
      this.forbiddenTagSet.Remove(tag);
    else
      this.forbiddenTagSet.Add(tag);
    this.consumableRulesChanged.Signal();
  }
}
