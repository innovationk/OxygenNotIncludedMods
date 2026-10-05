// Decompiled with JetBrains decompiler
// Type: BreathabilityTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BreathabilityTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData()
  {
    float num1 = 0.0f;
    if (Components.LiveMinionIdentities.GetWorldItems(this.WorldID).Count == 0)
    {
      this.AddPoint(0.0f);
    }
    else
    {
      int num2 = 0;
      foreach (Component worldItem in Components.LiveMinionIdentities.GetWorldItems(this.WorldID))
      {
        OxygenBreather component = worldItem.GetComponent<OxygenBreather>();
        if (!((Object) component == (Object) null))
        {
          OxygenBreather.IGasProvider currentGasProvider = component.GetCurrentGasProvider();
          ++num2;
          if (!component.IsOutOfOxygen)
          {
            num1 += 100f;
            if (currentGasProvider.IsLowOxygen())
              num1 -= 50f;
          }
        }
      }
      this.AddPoint((float) Mathf.RoundToInt(num1 / (float) num2));
    }
  }

  public override string FormatValueString(float value) => value.ToString() + "%";
}
