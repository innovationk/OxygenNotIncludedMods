// Decompiled with JetBrains decompiler
// Type: Klei.Actions.ActionFactory`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Klei.Actions;

public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
{
  private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();
  private static ActionFactoryType actionFactory = default (ActionFactoryType);

  public static ActionType GetOrCreateAction(EnumType actionType)
  {
    ActionType action;
    if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out action))
    {
      ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
      ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = action = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType);
    }
    return action;
  }

  private static void EnsureFactoryInstance()
  {
    if ((object) ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
      return;
    ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = Activator.CreateInstance(typeof (ActionFactoryType)) as ActionFactoryType;
  }

  protected virtual ActionType CreateAction(EnumType actionType)
  {
    throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
  }
}
