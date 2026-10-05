// Decompiled with JetBrains decompiler
// Type: RadiationGridManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class RadiationGridManager
{
  public const float STANDARD_MASS_FALLOFF = 1000000f;
  public const int RADIATION_LINGER_RATE = 4;
  public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();
  public static List<Tuple<int, int>> previewLightCells = new List<Tuple<int, int>>();
  public static int[] previewLux;

  public static int CalculateFalloff(float falloffRate, int cell, int origin)
  {
    return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float) Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
  }

  public static void Initialise()
  {
    RadiationGridManager.emitters = new List<RadiationGridEmitter>();
  }

  public static void Shutdown() => RadiationGridManager.emitters.Clear();

  public static void Refresh()
  {
    for (int index = 0; index < RadiationGridManager.emitters.Count; ++index)
    {
      if (RadiationGridManager.emitters[index].enabled)
        RadiationGridManager.emitters[index].Emit();
    }
  }
}
