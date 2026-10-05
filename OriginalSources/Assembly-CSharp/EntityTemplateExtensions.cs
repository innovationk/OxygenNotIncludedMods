// Decompiled with JetBrains decompiler
// Type: EntityTemplateExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class EntityTemplateExtensions
{
  public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController machineController = go.AddOrGet<StateMachineController>();
    DefType def = machineController.GetDef<DefType>();
    if ((object) def == null)
    {
      def = Activator.CreateInstance<DefType>();
      machineController.AddDef((StateMachine.BaseDef) def);
      def.Configure(go);
    }
    return def;
  }

  public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
  {
    ComponentType componentType = go.GetComponent<ComponentType>();
    if ((UnityEngine.Object) componentType == (UnityEngine.Object) null)
      componentType = go.AddComponent<ComponentType>();
    return componentType;
  }

  public static void RemoveDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController component;
    if (!go.TryGetComponent<StateMachineController>(out component))
      return;
    DefType def = component.GetDef<DefType>();
    if ((object) def == null)
      return;
    component.cmpdef.defs.Remove((StateMachine.BaseDef) def);
  }
}
