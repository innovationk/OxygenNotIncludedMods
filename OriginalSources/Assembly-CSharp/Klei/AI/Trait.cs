// Decompiled with JetBrains decompiler
// Type: Klei.AI.Trait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class Trait : Modifier, IHasDlcRestrictions
{
  public float Rating;
  public bool ShouldSave;
  public bool PositiveTrait;
  public bool ValidStarterTrait;
  public Action<GameObject> OnAddTrait;
  public Func<string> TooltipCB;
  public Func<string> ExtendedTooltip;
  public Func<string> ShortDescCB;
  public Func<string> ShortDescTooltipCB;
  public Func<string> NameCB;
  public ChoreGroup[] disabledChoreGroups;
  public bool isTaskBeingRefused;
  public string[] ignoredEffects;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public Trait(
    string id,
    string name,
    string description,
    float rating,
    bool should_save,
    ChoreGroup[] disallowed_chore_groups,
    bool positive_trait,
    bool is_valid_starter_trait)
    : base(id, name, description)
  {
    this.Rating = rating;
    this.ShouldSave = should_save;
    this.disabledChoreGroups = disallowed_chore_groups;
    this.PositiveTrait = positive_trait;
    this.ValidStarterTrait = is_valid_starter_trait;
    this.ignoredEffects = new string[0];
    this.requiredDlcIds = (string[]) null;
    this.forbiddenDlcIds = (string[]) null;
  }

  public Trait(
    string id,
    string name,
    string description,
    float rating,
    bool should_save,
    ChoreGroup[] disallowed_chore_groups,
    bool positive_trait,
    bool is_valid_starter_trait,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : base(id, name, description)
  {
    this.Rating = rating;
    this.ShouldSave = should_save;
    this.disabledChoreGroups = disallowed_chore_groups;
    this.PositiveTrait = positive_trait;
    this.ValidStarterTrait = is_valid_starter_trait;
    this.ignoredEffects = new string[0];
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public void AddIgnoredEffects(string[] effects)
  {
    List<string> stringList = new List<string>((IEnumerable<string>) this.ignoredEffects);
    stringList.AddRange((IEnumerable<string>) effects);
    this.ignoredEffects = stringList.ToArray();
  }

  public string GetName() => this.NameCB != null ? this.NameCB() : this.Name;

  public string GetTooltip()
  {
    return this.TooltipCB == null ? this.description + this.GetAttributeModifiersString(true) + this.GetDisabledChoresString(true) + this.GetIgnoredEffectsString(true) + this.GetExtendedTooltipStr() : this.TooltipCB();
  }

  public string GetAttributeModifiersString(bool list_entry)
  {
    string attributeModifiersString = "";
    foreach (AttributeModifier selfModifier in this.SelfModifiers)
    {
      Attribute attribute = Db.Get().Attributes.Get(selfModifier.AttributeId);
      if (list_entry)
        attributeModifiersString += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
      attributeModifiersString += string.Format((string) DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, (object) attribute.Name, (object) selfModifier.GetFormattedString());
    }
    return attributeModifiersString;
  }

  public string GetDisabledChoresString(bool list_entry)
  {
    string disabledChoresString = "";
    if (this.disabledChoreGroups != null)
    {
      string format = (string) DUPLICANTS.TRAITS.CANNOT_DO_TASK;
      if (this.isTaskBeingRefused)
        format = (string) DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
      foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
      {
        if (list_entry)
          disabledChoresString += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
        disabledChoresString += string.Format(format, (object) disabledChoreGroup.Name);
      }
    }
    return disabledChoresString;
  }

  public string GetIgnoredEffectsString(bool list_entry)
  {
    string ignoredEffectsString = "";
    if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
    {
      for (int index = 0; index < this.ignoredEffects.Length; ++index)
      {
        string ignoredEffect = this.ignoredEffects[index];
        if (list_entry)
          ignoredEffectsString += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
        string str = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{ignoredEffect.ToUpper()}.NAME");
        ignoredEffectsString += string.Format((string) DUPLICANTS.TRAITS.IGNORED_EFFECTS, (object) str);
        if (!list_entry && index < this.ignoredEffects.Length - 1)
          ignoredEffectsString += "\n";
      }
    }
    return ignoredEffectsString;
  }

  public string GetExtendedTooltipStr()
  {
    string extendedTooltipStr = "";
    if (this.ExtendedTooltip != null)
    {
      foreach (Func<string> invocation in this.ExtendedTooltip.GetInvocationList())
        extendedTooltipStr = $"{extendedTooltipStr}\n{invocation()}";
    }
    return extendedTooltipStr;
  }

  public override void AddTo(Attributes attributes)
  {
    base.AddTo(attributes);
    ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.disabledChoreGroups == null)
      return;
    foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
      component.SetPermittedByTraits(disabledChoreGroup, false);
  }

  public override void RemoveFrom(Attributes attributes)
  {
    base.RemoveFrom(attributes);
    ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.disabledChoreGroups == null)
      return;
    foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
      component.SetPermittedByTraits(disabledChoreGroup, true);
  }
}
