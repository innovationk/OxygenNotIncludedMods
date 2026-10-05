// Decompiled with JetBrains decompiler
// Type: Klei.AI.TraitUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class TraitUtil
{
  public static System.Action CreateDisabledTaskTrait(
    string id,
    string name,
    string desc,
    string disabled_chore_group,
    bool is_valid_starter_trait)
  {
    return (System.Action) (() =>
    {
      ChoreGroup[] disabled_chore_groups = new ChoreGroup[1]
      {
        Db.Get().ChoreGroups.Get(disabled_chore_group)
      };
      Db.Get().CreateTrait(id, name, desc, (string) null, true, disabled_chore_groups, false, is_valid_starter_trait);
    });
  }

  public static System.Action CreateTrait(
    string id,
    string name,
    string desc,
    string attributeId,
    float delta,
    string[] chore_groups,
    bool positiveTrait = false)
  {
    return (System.Action) (() =>
    {
      List<ChoreGroup> choreGroupList = new List<ChoreGroup>();
      foreach (string choreGroup in chore_groups)
        choreGroupList.Add(Db.Get().ChoreGroups.Get(choreGroup));
      Db.Get().CreateTrait(id, name, desc, (string) null, true, choreGroupList.ToArray(), positiveTrait, true).Add(new AttributeModifier(attributeId, delta, name));
    });
  }

  public static System.Action CreateAttributeEffectTrait(
    string id,
    string name,
    string desc,
    string attributeId,
    float delta,
    string attributeId2,
    float delta2,
    bool positiveTrait = false)
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
      trait.Add(new AttributeModifier(attributeId, delta, name));
      trait.Add(new AttributeModifier(attributeId2, delta2, name));
    });
  }

  public static System.Action CreateAttributeEffectTrait(
    string id,
    string name,
    string desc,
    string[] attributeIds,
    float[] deltas,
    bool positiveTrait = false)
  {
    return TraitUtil.CreateAttributeEffectTrait(id, name, desc, attributeIds, deltas, positiveTrait, (Action<GameObject>) null);
  }

  public static System.Action CreateAttributeEffectTrait(
    string id,
    string name,
    string desc,
    string[] attributeIds,
    float[] deltas,
    bool positiveTrait = false,
    Action<GameObject> on_add = null)
  {
    return (System.Action) (() =>
    {
      Debug.Assert(attributeIds.Length == deltas.Length, (object) "CreateAttributeEffectTrait must have an equal number of attributeIds and deltas");
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
      for (int index = 0; index < attributeIds.Length; ++index)
        trait.Add(new AttributeModifier(attributeIds[index], deltas[index], name));
      trait.OnAddTrait = on_add;
    });
  }

  public static System.Action CreateAttributeEffectTrait(
    string id,
    string name,
    string desc,
    string attributeId,
    float delta,
    bool positiveTrait = false,
    Action<GameObject> on_add = null,
    bool is_valid_starter_trait = true)
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, is_valid_starter_trait);
      trait.Add(new AttributeModifier(attributeId, delta, name));
      trait.OnAddTrait = on_add;
    });
  }

  public static System.Action CreateEffectModifierTrait(
    string id,
    string name,
    string desc,
    string[] ignoredEffects,
    bool positiveTrait = false)
  {
    return (System.Action) (() => Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true).AddIgnoredEffects(ignoredEffects));
  }

  public static System.Action CreateNamedTrait(
    string id,
    string name,
    string desc,
    bool positiveTrait = false)
  {
    return (System.Action) (() => Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true));
  }

  public static System.Action CreateTrait(
    string id,
    string name,
    string desc,
    Action<GameObject> on_add,
    ChoreGroup[] disabled_chore_groups = null,
    bool positiveTrait = false,
    Func<string> extendedDescFn = null)
  {
    return TraitUtil.CreateTrait(id, name, desc, on_add, (string[]) null, disabled_chore_groups: disabled_chore_groups, positiveTrait: positiveTrait, extendedDescFn: extendedDescFn);
  }

  public static System.Action CreateTrait(
    string id,
    string name,
    string desc,
    Action<GameObject> on_add,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds = null,
    ChoreGroup[] disabled_chore_groups = null,
    bool positiveTrait = false,
    Func<string> extendedDescFn = null)
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, disabled_chore_groups, positiveTrait, true, requiredDlcIds, forbiddenDlcIds);
      trait.OnAddTrait = on_add;
      if (extendedDescFn == null)
        return;
      trait.ExtendedTooltip += extendedDescFn;
    });
  }

  public static System.Action CreateComponentTrait<T>(
    string id,
    string name,
    string desc,
    bool positiveTrait = false,
    Func<string> extendedDescFn = null)
    where T : KMonoBehaviour
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
      trait.OnAddTrait = (Action<GameObject>) (go => go.FindOrAddUnityComponent<T>());
      if (extendedDescFn == null)
        return;
      trait.ExtendedTooltip += extendedDescFn;
    });
  }

  public static System.Action CreateSkillGrantingTrait(
    string id,
    string name,
    string desc,
    string skillId)
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, true, true);
      trait.TooltipCB = (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_DESC, (object) desc, (object) SkillWidget.SkillPerksString(Db.Get().Skills.Get(skillId))));
      trait.OnAddTrait = (Action<GameObject>) (go =>
      {
        MinionResume component = go.GetComponent<MinionResume>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.GrantSkill(skillId);
      });
    });
  }

  public static System.Action CreateSkillGrantingTrait(
    string id,
    string name,
    string desc,
    string[] skillIds)
  {
    return (System.Action) (() =>
    {
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, true, true);
      trait.TooltipCB = (Func<string>) (() =>
      {
        string skillGrantingTrait = "" + (string) DUPLICANTS.TRAITS.GRANTED_SKILL_NO_MORALE_COST;
        for (int index = 0; index < skillIds.Length; ++index)
        {
          string skillId = skillIds[index];
          skillGrantingTrait = $"{skillGrantingTrait + "\n\n"}{GameUtil.ApplyBoldString(Db.Get().Skills.Get(skillId).Name)}:\n" + SkillWidget.SkillPerksString(Db.Get().Skills.Get(skillId));
        }
        return skillGrantingTrait;
      });
      trait.OnAddTrait = (Action<GameObject>) (go =>
      {
        MinionResume component = go.GetComponent<MinionResume>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        foreach (string skillId in skillIds)
          component.GrantSkill(skillId);
      });
    });
  }

  public static string GetSkillGrantingTraitNameById(string id)
  {
    string grantingTraitNameById = "";
    StringEntry result;
    if (Strings.TryGet($"STRINGS.DUPLICANTS.TRAITS.GRANTSKILL_{id.ToUpper()}.NAME", out result))
      grantingTraitNameById = result.String;
    return grantingTraitNameById;
  }

  public static System.Action CreateBionicUpgradeTrait(string id, string effectsDescription)
  {
    return (System.Action) (() =>
    {
      string name = (string) Strings.Get($"STRINGS.DUPLICANTS.TRAITS.{id.ToUpper()}.NAME");
      string desc = (string) Strings.Get($"STRINGS.DUPLICANTS.TRAITS.{id.ToUpper()}.DESC");
      Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, true, true);
      trait.TooltipCB = (Func<string>) (() => $"{desc}\n\n{effectsDescription}");
      trait.NameCB = (Func<string>) (() => name);
      string shortDescTooltip = (string) Strings.Get($"STRINGS.DUPLICANTS.TRAITS.{trait.Id.ToUpper()}.SHORT_DESC_TOOLTIP");
      trait.ShortDescTooltipCB = (Func<string>) (() => $"{shortDescTooltip}\n\n{effectsDescription}");
    });
  }
}
