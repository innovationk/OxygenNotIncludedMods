// Decompiled with JetBrains decompiler
// Type: MySmi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
public class MySmi(Dictionary<System.Type, MethodInfo> attributeMap) : 
  MyAttributeManager<StateMachine.Instance>(attributeMap)
{
  public static void Init()
  {
    MyAttributes.Register((IAttributeManager) new MySmi(new Dictionary<System.Type, MethodInfo>()
    {
      {
        typeof (MySmiGet),
        typeof (MySmi).GetMethod("FindSmi")
      },
      {
        typeof (MySmiReq),
        typeof (MySmi).GetMethod("RequireSmi")
      }
    }));
  }

  public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
  {
    StateMachineController component = c.GetComponent<StateMachineController>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? (StateMachine.Instance) component.GetSMI<T>() : (StateMachine.Instance) null;
  }

  public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
  {
    if (!isStart)
      return MySmi.FindSmi<T>(c, isStart);
    StateMachine.Instance smi = MySmi.FindSmi<T>(c, isStart);
    Debug.Assert(smi != null, (object) $"{c.GetType().ToString()} '{c.name}' requires a StateMachineInstance of type {typeof (T)}!");
    return smi;
  }
}
