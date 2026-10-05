// Decompiled with JetBrains decompiler
// Type: InfraredVisualizerComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InfraredVisualizerComponents : KGameObjectComponentManager<InfraredVisualizerData>
{
  public HandleVector<int>.Handle Add(GameObject go)
  {
    return this.Add(go, new InfraredVisualizerData(go));
  }

  public void UpdateTemperature()
  {
  }

  public void ClearOverlayColour()
  {
  }

  public static void ClearOverlayColour(KBatchedAnimController controller)
  {
  }
}
