// Decompiled with JetBrains decompiler
// Type: DebugOverlays
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DebugOverlays : KScreen
{
  public static DebugOverlays instance { get; private set; }

  protected override void OnPrefabInit()
  {
    DebugOverlays.instance = this;
    KPopupMenu componentInChildren = this.GetComponentInChildren<KPopupMenu>();
    componentInChildren.SetOptions((IList<string>) new string[5]
    {
      "None",
      "Rooms",
      "Lighting",
      "Style",
      "Flow"
    });
    componentInChildren.OnSelect += new Action<string, int>(this.OnSelect);
    this.gameObject.SetActive(false);
  }

  private void OnSelect(string str, int index)
  {
    switch (str)
    {
      case "None":
        SimDebugView.Instance.SetMode(OverlayModes.None.ID);
        break;
      case "Flow":
        SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
        break;
      case "Lighting":
        SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
        break;
      case "Rooms":
        SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
        break;
      default:
        Debug.LogError((object) ("Unknown debug view: " + str));
        break;
    }
  }
}
