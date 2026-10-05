// Decompiled with JetBrains decompiler
// Type: Klei.Input.InterfaceToolConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.Input;

[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
public class InterfaceToolConfig : ScriptableObject
{
  [SerializeField]
  private DigToolActionFactory.Actions digAction;
  public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();
  [SerializeField]
  [Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
  private int priority;
  [SerializeField]
  [Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
  private string inputAction = Action.Invalid.ToString();

  public DigAction DigAction
  {
    get
    {
      return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
    }
  }

  public int Priority => this.priority;

  public Action InputAction => (Action) Enum.Parse(typeof (Action), this.inputAction);

  public class Comparer : IComparer<InterfaceToolConfig>
  {
    public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
    {
      if (lhs.Priority == rhs.Priority)
        return 0;
      return lhs.Priority <= rhs.Priority ? -1 : 1;
    }
  }
}
