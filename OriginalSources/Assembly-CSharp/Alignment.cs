// Decompiled with JetBrains decompiler
// Type: Alignment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public readonly struct Alignment(float x, float y)
{
  public readonly float x = x;
  public readonly float y = y;

  public static Alignment Custom(float x, float y) => new Alignment(x, y);

  public static Alignment TopLeft() => new Alignment(0.0f, 1f);

  public static Alignment Top() => new Alignment(0.5f, 1f);

  public static Alignment TopRight() => new Alignment(1f, 1f);

  public static Alignment Left() => new Alignment(0.0f, 0.5f);

  public static Alignment Center() => new Alignment(0.5f, 0.5f);

  public static Alignment Right() => new Alignment(1f, 0.5f);

  public static Alignment BottomLeft() => new Alignment(0.0f, 0.0f);

  public static Alignment Bottom() => new Alignment(0.5f, 0.0f);

  public static Alignment BottomRight() => new Alignment(1f, 0.0f);

  public static implicit operator Vector2(Alignment a) => new Vector2(a.x, a.y);

  public static implicit operator Alignment(Vector2 v) => new Alignment(v.x, v.y);
}
