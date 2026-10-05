// Decompiled with JetBrains decompiler
// Type: Database.SkillAttributePerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
namespace Database;

public class SkillAttributePerk : SkillPerk
{
  public AttributeModifier modifier;

  public SkillAttributePerk(
    string id,
    string attributeId,
    float modifierBonus,
    string modifierDesc,
    bool modifierCanStack = false)
    : base(id, "", (Action<MinionResume>) null, (Action<MinionResume>) null, (Action<MinionResume>) (identity => { }), (string[]) null, false)
  {
    SkillAttributePerk skillAttributePerk = this;
    Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
    this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc);
    this.Name = string.Format((string) UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, (object) this.modifier.GetFormattedString(), (object) attribute.Name);
    this.OnApply = (Action<MinionResume>) (identity =>
    {
      if (!modifierCanStack && identity.GetAttributes().Get(skillAttributePerk.modifier.AttributeId).Modifiers.FindIndex((Predicate<AttributeModifier>) (mod => mod == skillAttributePerk.modifier)) != -1)
        return;
      identity.GetAttributes().Add(skillAttributePerk.modifier);
    });
    this.OnRemove = (Action<MinionResume>) (identity => identity.GetAttributes().Remove(skillAttributePerk.modifier));
  }
}
