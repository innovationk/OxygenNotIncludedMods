// Decompiled with JetBrains decompiler
// Type: Klei.AI.FertilityModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Klei.AI;

public class FertilityModifier : Resource
{
  public string Description;
  public Tag TargetTag;
  public Func<string, string> TooltipCB;
  public FertilityModifier.FertilityModFn ApplyFunction;

  public FertilityModifier(
    string id,
    Tag targetTag,
    string name,
    string description,
    Func<string, string> tooltipCB,
    FertilityModifier.FertilityModFn applyFunction)
    : base(id, name)
  {
    this.Description = description;
    this.TargetTag = targetTag;
    this.TooltipCB = tooltipCB;
    this.ApplyFunction = applyFunction;
  }

  public string GetTooltip()
  {
    return this.TooltipCB != null ? this.TooltipCB(this.Description) : this.Description;
  }

  public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
}
