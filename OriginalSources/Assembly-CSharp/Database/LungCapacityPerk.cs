// Decompiled with JetBrains decompiler
// Type: Database.LungCapacityPerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class LungCapacityPerk : SkillPerk
{
  private HashSet<MinionResume> breathBoostConsumed = new HashSet<MinionResume>();
  private AttributeModifier modifier;
  private float bonus;

  public LungCapacityPerk(string id, float modifierBonus, string modifierDesc)
    : base(id, "", (Action<MinionResume>) null, (Action<MinionResume>) null, (Action<MinionResume>) null, (string[]) null, false)
  {
    this.bonus = modifierBonus;
    string id1 = Db.Get().Amounts.Breath.maxAttribute.Id;
    Klei.AI.Attribute attribute = Db.Get().Attributes.Get(id1);
    this.modifier = new AttributeModifier(id1, this.bonus, modifierDesc);
    this.Name = string.Format((string) UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, (object) this.modifier.GetFormattedString(), (object) attribute.Name);
    this.OnApply = new Action<MinionResume>(this.ApplyPerk);
    this.OnRemove = new Action<MinionResume>(this.RemovePerk);
  }

  private void ApplyPerk(MinionResume identity)
  {
    ArrayRef<AttributeModifier> modifiers = identity.GetAttributes().Get(this.modifier.AttributeId).Modifiers;
    bool flag = false;
    for (int i = 0; i != modifiers.Count; ++i)
    {
      if (modifiers[i] == this.modifier)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      identity.GetAttributes().Add(this.modifier);
    if (!this.breathBoostConsumed.Add(identity))
      return;
    AmountInstance amountInstance = Db.Get().Amounts.Breath.Lookup((Component) identity);
    if (amountInstance == null)
      return;
    double num = (double) amountInstance.SetValue(amountInstance.value + this.bonus);
  }

  private void RemovePerk(MinionResume identity)
  {
    identity.GetAttributes().Remove(this.modifier);
    this.breathBoostConsumed.Add(identity);
  }

  public void ResetConsumedBreathBoosts() => this.breathBoostConsumed.Clear();
}
