// Decompiled with JetBrains decompiler
// Type: MathfExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public static class MathfExtensions
{
  public static long Max(this long a, long b) => a < b ? b : a;

  public static long Min(this long a, long b) => a > b ? b : a;
}
