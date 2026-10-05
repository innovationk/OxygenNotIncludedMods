// Decompiled with JetBrains decompiler
// Type: BuildingFacadeCustomData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BuildingFacadeCustomData
{
  public const string DATA_KEY_LIGHT_COLOR = "LightColor";
  public const string DATA_KEY_LIGHT_OVERLAY_COLOR = "LightOverlayColor";

  public static void ApplyCustomData(Building building, Dictionary<string, string> newData)
  {
    BuildingFacadeCustomData.UpdateLightColor(newData, building);
  }

  private static void UpdateLightColor(Dictionary<string, string> newData, Building building)
  {
    Light2D component1;
    if (!Assets.GetPrefab(building.PrefabID()).TryGetComponent<Light2D>(out component1))
      return;
    Color color1 = component1.Color;
    Color overlayColour = component1.overlayColour;
    Light2D component2;
    if (!building.TryGetComponent<Light2D>(out component2))
      return;
    if (newData == null)
    {
      component2.Color = color1;
      component2.overlayColour = overlayColour;
    }
    else
    {
      string hex1;
      if (newData.TryGetValue("LightColor", out hex1))
      {
        Color color2 = Util.ColorFromHex(hex1);
        component2.Color = color2;
      }
      else
        component2.Color = color1;
      string hex2;
      if (newData.TryGetValue("LightOverlayColor", out hex2))
      {
        Color color3 = Util.ColorFromHex(hex2);
        component2.overlayColour = color3;
      }
      else
        component2.overlayColour = overlayColour;
    }
  }
}
