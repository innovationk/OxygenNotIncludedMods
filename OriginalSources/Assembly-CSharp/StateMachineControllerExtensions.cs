// Decompiled with JetBrains decompiler
// Type: StateMachineControllerExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class StateMachineControllerExtensions
{
  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(
    this StateMachine.Instance smi)
    where StateMachineInstanceType : StateMachine.Instance
  {
    return smi.gameObject.GetSMI<StateMachineInstanceType>();
  }

  public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
  {
    return cmp.gameObject.GetDef<DefType>();
  }

  public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component == (Object) null ? default (DefType) : component.GetDef<DefType>();
  }

  public static InterfaceType GetDefImplementingInterface<InterfaceType>(this GameObject go) where InterfaceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component == (Object) null ? default (InterfaceType) : component.GetDefImplementingInterfaceOfType<InterfaceType>();
  }

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
  {
    return cmp.gameObject.GetSMI<StateMachineInstanceType>();
  }

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component != (Object) null ? component.GetSMI<StateMachineInstanceType>() : default (StateMachineInstanceType);
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this Component cmp)
    where StateMachineInstanceType : class
  {
    return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this GameObject go)
    where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component != (Object) null ? component.GetAllSMI<StateMachineInstanceType>() : new List<StateMachineInstanceType>();
  }
}
