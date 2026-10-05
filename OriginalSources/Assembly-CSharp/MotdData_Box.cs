// Decompiled with JetBrains decompiler
// Type: MotdData_Box
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MotdData_Box
{
  public string category;
  public string guid;
  public long startTime;
  public long finishTime;
  public string title;
  public string text;
  public string image;
  public string href;
  public Texture2D resolvedImage;
  public bool resolvedImageIsFromDisk;

  public bool ShouldDisplay()
  {
    long unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    return unixTimeSeconds >= this.startTime && this.finishTime >= unixTimeSeconds;
  }
}
