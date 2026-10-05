// Decompiled with JetBrains decompiler
// Type: Padding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public readonly struct Padding(float left, float right, float top, float bottom)
{
  public readonly float top = top;
  public readonly float bottom = bottom;
  public readonly float left = left;
  public readonly float right = right;

  public float Width => this.left + this.right;

  public float Height => this.top + this.bottom;

  public static Padding All(float padding) => new Padding(padding, padding, padding, padding);

  public static Padding Symmetric(float horizontal, float vertical)
  {
    return new Padding(horizontal, horizontal, vertical, vertical);
  }

  public static Padding Only(float left = 0.0f, float right = 0.0f, float top = 0.0f, float bottom = 0.0f)
  {
    return new Padding(left, right, top, bottom);
  }

  public static Padding Vertical(float vertical) => new Padding(0.0f, 0.0f, vertical, vertical);

  public static Padding Horizontal(float horizontal)
  {
    return new Padding(horizontal, horizontal, 0.0f, 0.0f);
  }

  public static Padding Top(float amount) => new Padding(0.0f, 0.0f, amount, 0.0f);

  public static Padding Left(float amount) => new Padding(amount, 0.0f, 0.0f, 0.0f);

  public static Padding Bottom(float amount) => new Padding(0.0f, 0.0f, 0.0f, amount);

  public static Padding Right(float amount) => new Padding(0.0f, amount, 0.0f, 0.0f);

  public static Padding operator +(Padding a, Padding b)
  {
    return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
  }

  public static Padding operator -(Padding a, Padding b)
  {
    return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
  }

  public static Padding operator *(float f, Padding p) => p * f;

  public static Padding operator *(Padding p, float f)
  {
    return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
  }

  public static Padding operator /(Padding p, float f)
  {
    return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
  }
}
