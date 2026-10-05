// Decompiled with JetBrains decompiler
// Type: LaunchPadConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
  private List<ProcessCondition> conditions;

  public List<ProcessCondition> GetConditionSet(
    ProcessCondition.ProcessConditionType conditionType)
  {
    return conditionType != ProcessCondition.ProcessConditionType.RocketStorage ? (List<ProcessCondition>) null : this.conditions;
  }

  public int PopulateConditionSet(
    ProcessCondition.ProcessConditionType conditionType,
    List<ProcessCondition> conditions)
  {
    int num = 0;
    if (conditionType == ProcessCondition.ProcessConditionType.RocketStorage)
    {
      conditions.AddRange((IEnumerable<ProcessCondition>) this.conditions);
      num += this.conditions.Count;
    }
    return num;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.conditions = new List<ProcessCondition>();
    this.conditions.Add((ProcessCondition) new TransferCargoCompleteCondition(this.gameObject));
  }
}
