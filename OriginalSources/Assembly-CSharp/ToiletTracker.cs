// Decompiled with JetBrains decompiler
// Type: ToiletTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class ToiletTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData() => throw new NotImplementedException();

  public override string FormatValueString(float value) => value.ToString();
}
