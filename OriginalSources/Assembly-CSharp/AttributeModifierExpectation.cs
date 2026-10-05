// Decompiled with JetBrains decompiler
// Type: AttributeModifierExpectation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class AttributeModifierExpectation : Expectation
{
  public AttributeModifier modifier;
  public Sprite icon;

  public AttributeModifierExpectation(
    string id,
    string name,
    string description,
    AttributeModifier modifier,
    Sprite icon)
    : base(id, name, description, (Action<MinionResume>) (resume => resume.GetAttributes().Get(modifier.AttributeId).Add(modifier)), (Action<MinionResume>) (resume => resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier)))
  {
    this.modifier = modifier;
    this.icon = icon;
  }
}
