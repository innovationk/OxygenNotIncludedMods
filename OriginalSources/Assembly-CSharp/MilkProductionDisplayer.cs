// Decompiled with JetBrains decompiler
// Type: MilkProductionDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MilkProductionDisplayer : 
  AsPercentAmountDisplayer,
  IVariableImageAmountDisplayer,
  IAmountDisplayer
{
  public Dictionary<Tag, string> IconPerElement = new Dictionary<Tag, string>();

  public MilkProductionDisplayer(GameUtil.TimeSlice deltaTimeSlice)
    : base(deltaTimeSlice)
  {
  }

  public MilkProductionDisplayer(
    GameUtil.TimeSlice deltaTimeSlice,
    Dictionary<Tag, string> customIconsPerElement)
    : base(deltaTimeSlice)
  {
    this.IconPerElement = customIconsPerElement;
  }

  public override string GetDescription(Amount master, AmountInstance instance)
  {
    Element elementByHash = ElementLoader.FindElementByHash(instance.gameObject.GetSMI<MilkProductionMonitor.Instance>().def.element);
    return $"{GameUtil.SafeStringFormat((string) CREATURES.STATS.MILKPRODUCTION.DISPLAYED_NAME, (object) elementByHash.name)}: {this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance))}";
  }

  public override string GetTooltipDescription(Amount master, AmountInstance instance)
  {
    Element elementByHash = ElementLoader.FindElementByHash(instance.gameObject.GetSMI<MilkProductionMonitor.Instance>().def.element);
    return string.Format(GameUtil.SafeStringFormat(master.description, (object) elementByHash.name), (object) this.formatter.GetFormattedValue(instance.value));
  }

  public Sprite GetIcon(Amount master, AmountInstance instance)
  {
    string name;
    return this.IconPerElement.TryGetValue(ElementLoader.FindElementByHash(instance.gameObject.GetSMI<MilkProductionMonitor.Instance>().def.element).tag, out name) ? Assets.GetSprite((HashedString) name) : Assets.GetSprite((HashedString) master.uiSprite);
  }
}
