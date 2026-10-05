// Decompiled with JetBrains decompiler
// Type: Klei.Actions.ActionTypeAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Klei.Actions;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ActionTypeAttribute : Attribute
{
  public readonly string TypeName;
  public readonly string GroupName;
  public readonly bool GenerateConfig;

  public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
  {
    this.TypeName = typeName;
    this.GroupName = groupName;
    this.GenerateConfig = generateConfig;
  }

  public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
  {
    bool flag1 = object.Equals((object) lhs, (object) null);
    bool flag2 = object.Equals((object) rhs, (object) null);
    if (flag1 | flag2)
      return flag1 == flag2;
    return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
  }

  public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs) => !(lhs == rhs);

  public override bool Equals(object obj) => base.Equals(obj);

  public override int GetHashCode() => base.GetHashCode();
}
