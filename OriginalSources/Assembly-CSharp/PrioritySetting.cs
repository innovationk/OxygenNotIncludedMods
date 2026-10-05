// Decompiled with JetBrains decompiler
// Type: PrioritySetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public struct PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value) : 
  IComparable<PrioritySetting>
{
  public PriorityScreen.PriorityClass priority_class = priority_class;
  public int priority_value = priority_value;

  public override int GetHashCode()
  {
    return ((int) this.priority_class << 28).GetHashCode() ^ this.priority_value.GetHashCode();
  }

  public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.Equals((object) rhs);
  }

  public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return !lhs.Equals((object) rhs);
  }

  public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) <= 0;
  }

  public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) >= 0;
  }

  public static bool operator <(PrioritySetting lhs, PrioritySetting rhs) => lhs.CompareTo(rhs) < 0;

  public static bool operator >(PrioritySetting lhs, PrioritySetting rhs) => lhs.CompareTo(rhs) > 0;

  public override bool Equals(object obj)
  {
    return obj is PrioritySetting prioritySetting && prioritySetting.priority_class == this.priority_class && ((PrioritySetting) obj).priority_value == this.priority_value;
  }

  public int CompareTo(PrioritySetting other)
  {
    if (this.priority_class > other.priority_class)
      return 1;
    if (this.priority_class < other.priority_class)
      return -1;
    if (this.priority_value > other.priority_value)
      return 1;
    return this.priority_value < other.priority_value ? -1 : 0;
  }
}
