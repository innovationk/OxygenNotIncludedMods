// Decompiled with JetBrains decompiler
// Type: AsteroidDescriptor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public struct AsteroidDescriptor(
  string text,
  string tooltip,
  Color associatedColor,
  List<Tuple<string, Color, float>> bands = null,
  string associatedIcon = null)
{
  public string text = text;
  public string tooltip = tooltip;
  public List<Tuple<string, Color, float>> bands = bands;
  public Color associatedColor = associatedColor;
  public string associatedIcon = associatedIcon;
}
