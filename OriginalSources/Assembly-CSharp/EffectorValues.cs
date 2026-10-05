// Decompiled with JetBrains decompiler
// Type: EffectorValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public struct EffectorValues(int amt, int rad)
{
  public int amount = amt;
  public int radius = rad;

  public override bool Equals(object obj) => obj is EffectorValues p && this.Equals(p);

  public bool Equals(EffectorValues p)
  {
    if ((ValueType) p == null)
      return false;
    if ((ValueType) this == (ValueType) p)
      return true;
    return !(this.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius;
  }

  public override int GetHashCode() => this.amount ^ this.radius;

  public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
  {
    if ((ValueType) lhs != null)
      return lhs.Equals(rhs);
    return (ValueType) rhs == null;
  }

  public static bool operator !=(EffectorValues lhs, EffectorValues rhs) => !(lhs == rhs);
}
