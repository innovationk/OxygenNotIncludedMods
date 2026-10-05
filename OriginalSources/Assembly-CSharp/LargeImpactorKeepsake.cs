// Decompiled with JetBrains decompiler
// Type: LargeImpactorKeepsake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LargeImpactorKeepsake : 
  GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>
{
  private GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.State notification;
  private GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.State idle;
  private StateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.BoolParameter HasNotificationBeenAknowledged;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.notification;
    this.notification.ParamTransition<bool>((StateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.Parameter<bool>) this.HasNotificationBeenAknowledged, this.idle, GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.IsTrue).ToggleNotification(new Func<LargeImpactorKeepsake.Instance, Notification>(LargeImpactorKeepsake.GetNotification));
    this.idle.DoNothing();
  }

  public static Notification GetNotification(LargeImpactorKeepsake.Instance smi)
  {
    return smi.notification;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.GameInstance
  {
    public Notification notification;

    public Instance(IStateMachineTarget master, LargeImpactorKeepsake.Def def)
      : base(master, def)
    {
      this.notification = this.CreateDeathNotification();
    }

    private Notification CreateDeathNotification()
    {
      string name = (string) MISC.NOTIFICATIONS.LARGE_IMPACTOR_KEEPSAKE.NAME;
      Transform transform = this.gameObject.transform;
      object obj = (object) this;
      Notification.ClickCallback custom_click_callback = new Notification.ClickCallback(this.MarkAsAknowledgedAndFocusCamera);
      object custom_click_data = obj;
      Transform click_focus = transform;
      return new Notification(name, NotificationType.Event, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.LARGE_IMPACTOR_KEEPSAKE.TOOLTIP), expires: false, custom_click_callback: custom_click_callback, custom_click_data: custom_click_data, click_focus: click_focus, clear_on_click: true);
    }

    private void MarkAsAknowledgedAndFocusCamera(object data)
    {
      if (data == null)
        return;
      LargeImpactorKeepsake.Instance smi = (LargeImpactorKeepsake.Instance) data;
      smi.sm.HasNotificationBeenAknowledged.Set(true, smi);
      GameUtil.FocusCamera(this.gameObject.transform);
    }
  }
}
