// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifiersExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Klei.AI;

public static class ModifiersExtensions
{
  public static Attributes GetAttributes(this KMonoBehaviour cmp) => cmp.gameObject.GetAttributes();

  public static Attributes GetAttributes(this GameObject go)
  {
    Modifiers component = go.GetComponent<Modifiers>();
    return (Object) component != (Object) null ? component.attributes : (Attributes) null;
  }

  public static Amounts GetAmounts(this KMonoBehaviour cmp)
  {
    return cmp is Modifiers ? ((Modifiers) cmp).amounts : cmp.gameObject.GetAmounts();
  }

  public static Amounts GetAmounts(this GameObject go)
  {
    Modifiers component = go.GetComponent<Modifiers>();
    return (Object) component != (Object) null ? component.amounts : (Amounts) null;
  }

  public static Sicknesses GetSicknesses(this KMonoBehaviour cmp) => cmp.gameObject.GetSicknesses();

  public static Sicknesses GetSicknesses(this GameObject go)
  {
    Modifiers component = go.GetComponent<Modifiers>();
    return (Object) component != (Object) null ? component.sicknesses : (Sicknesses) null;
  }
}
