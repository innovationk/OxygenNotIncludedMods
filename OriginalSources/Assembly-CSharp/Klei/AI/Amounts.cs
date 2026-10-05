// Decompiled with JetBrains decompiler
// Type: Klei.AI.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Klei.AI;

public class Amounts(GameObject go) : Modifications<Amount, AmountInstance>(go)
{
  public float GetValue(string amount_id) => this.Get(amount_id).value;

  public void SetValue(string amount_id, float value) => this.Get(amount_id).value = value;

  public override AmountInstance Add(AmountInstance instance)
  {
    instance.Activate();
    return base.Add(instance);
  }

  public override void Remove(AmountInstance instance)
  {
    instance.Deactivate();
    base.Remove(instance);
  }

  public void Cleanup()
  {
    for (int idx = 0; idx < this.Count; ++idx)
      this[idx].Deactivate();
  }
}
