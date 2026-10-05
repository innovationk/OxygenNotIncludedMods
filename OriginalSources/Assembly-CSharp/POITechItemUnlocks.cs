// Decompiled with JetBrains decompiler
// Type: POITechItemUnlocks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class POITechItemUnlocks : 
  GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
  public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State locked;
  public POITechItemUnlocks.UnlockedStates unlocked;
  public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter isUnlocked;
  public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter pendingChore;
  public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter seenNotification;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.locked;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.Parameter<bool>) this.isUnlocked, (GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State) this.unlocked, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
    this.unlocked.ParamTransition<bool>((StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.Parameter<bool>) this.seenNotification, this.unlocked.notify, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsFalse).ParamTransition<bool>((StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.Parameter<bool>) this.seenNotification, this.unlocked.done, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
    this.unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired).ToggleNotification((Func<POITechItemUnlocks.Instance, Notification>) (smi =>
    {
      smi.notificationReference = EventInfoScreen.CreateNotification(POITechItemUnlocks.GenerateEventPopupData(smi));
      smi.notificationReference.Type = NotificationType.MessageImportant;
      return smi.notificationReference;
    }));
    this.unlocked.done.PlayAnim("off");
  }

  private static void OnNotificationAknowledged(object o)
  {
    Game.Instance.Trigger(1633134300, (object) (GameObject) o);
  }

  private static string GetMessageBody(POITechItemUnlocks.Instance smi)
  {
    string str = "";
    foreach (TechItem unlockTechItem in smi.unlockTechItems)
      str = $"{str}\n    • {unlockTechItem.Name}";
    return string.Format((string) (smi.def.loreUnlockId != null ? MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY : MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.MESSAGEBODY), (object) str);
  }

  private static EventInfoData GenerateEventPopupData(POITechItemUnlocks.Instance smi)
  {
    EventInfoData eventPopupData = new EventInfoData((string) MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, POITechItemUnlocks.GetMessageBody(smi), (HashedString) smi.def.animName);
    int length = Mathf.Max(2, Components.LiveMinionIdentities.Count);
    GameObject[] gameObjectArray = new GameObject[length];
    using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle<MinionIdentity>().GetEnumerator())
    {
      for (int index = 0; index < length; ++index)
      {
        if (!enumerator.MoveNext())
        {
          gameObjectArray = new GameObject[0];
          break;
        }
        gameObjectArray[index] = enumerator.Current.gameObject;
      }
    }
    eventPopupData.minions = gameObjectArray;
    if (smi.def.loreUnlockId != null)
      eventPopupData.AddOption((string) MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE).callback = (System.Action) (() =>
      {
        smi.sm.seenNotification.Set(true, smi);
        smi.notificationReference = (Notification) null;
        Game.Instance.unlocks.Unlock(smi.def.loreUnlockId);
        ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId);
        POITechItemUnlocks.OnNotificationAknowledged((object) smi.gameObject);
      });
    eventPopupData.AddDefaultOption((System.Action) (() =>
    {
      smi.sm.seenNotification.Set(true, smi);
      smi.notificationReference = (Notification) null;
      POITechItemUnlocks.OnNotificationAknowledged((object) smi.gameObject);
    }));
    eventPopupData.clickFocus = smi.gameObject.transform;
    return eventPopupData;
  }

  public class Def : StateMachine.BaseDef
  {
    public List<string> POITechUnlockIDs;
    public LocString PopUpName;
    public string animName;
    public string loreUnlockId;
  }

  public new class Instance : 
    GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.GameInstance,
    ISidescreenButtonControl
  {
    public List<TechItem> unlockTechItems;
    public Notification notificationReference;
    private int onBuildingSelectHandle = -1;
    private Chore unlockChore;

    public Instance(IStateMachineTarget master, POITechItemUnlocks.Def def)
      : base(master, def)
    {
      this.unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
      foreach (string poiTechUnlockId in def.POITechUnlockIDs)
      {
        TechItem techItem = Db.Get().TechItems.TryGet(poiTechUnlockId);
        if (techItem != null)
          this.unlockTechItems.Add(techItem);
        else
          DebugUtil.DevAssert(false, $"Invalid tech item {poiTechUnlockId} for POI Tech Unlock");
      }
    }

    public override void StartSM()
    {
      this.onBuildingSelectHandle = this.Subscribe(-1503271301, new System.Action<object>(this.OnBuildingSelect));
      this.UpdateUnlocked();
      base.StartSM();
      if (!this.sm.pendingChore.Get(this) || this.unlockChore != null)
        return;
      this.CreateChore();
    }

    public override void StopSM(string reason)
    {
      this.Unsubscribe(ref this.onBuildingSelectHandle);
      base.StopSM(reason);
    }

    public void OnBuildingSelect(object obj)
    {
      if (!((Boxed<bool>) obj).value || this.sm.seenNotification.Get(this) || this.notificationReference == null)
        return;
      this.notificationReference.customClickCallback(this.notificationReference.customClickData);
    }

    private void ShowPopup()
    {
    }

    public void UnlockTechItems()
    {
      foreach (TechItem unlockTechItem in this.unlockTechItems)
        unlockTechItem?.POIUnlocked();
      MusicManager.instance.PlaySong("Stinger_ResearchComplete");
      this.UpdateUnlocked();
    }

    private void UpdateUnlocked()
    {
      bool flag = true;
      foreach (TechItem unlockTechItem in this.unlockTechItems)
      {
        if (!unlockTechItem.IsComplete())
        {
          flag = false;
          break;
        }
      }
      this.sm.isUnlocked.Set(flag, this.smi);
    }

    public string SidescreenButtonText
    {
      get
      {
        if (this.sm.isUnlocked.Get(this.smi))
          return (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
        return this.unlockChore != null ? (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF : (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
      }
    }

    public string SidescreenButtonTooltip
    {
      get
      {
        if (this.sm.isUnlocked.Get(this.smi))
          return (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
        return this.unlockChore != null ? (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF : (string) UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
      }
    }

    public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
      throw new NotImplementedException();
    }

    public bool SidescreenEnabled()
    {
      return this.smi.IsInsideState((StateMachine.BaseState) this.sm.locked);
    }

    public bool SidescreenButtonInteractable()
    {
      return this.smi.IsInsideState((StateMachine.BaseState) this.sm.locked);
    }

    public void OnSidescreenButtonPressed()
    {
      if (this.unlockChore == null)
      {
        this.smi.sm.pendingChore.Set(true, this.smi);
        this.smi.CreateChore();
      }
      else
      {
        this.smi.sm.pendingChore.Set(false, this.smi);
        this.smi.CancelChore();
      }
    }

    private void CreateChore()
    {
      Workable component = (Workable) this.smi.master.GetComponent<POITechItemUnlockWorkable>();
      Prioritizable.AddRef(this.gameObject);
      this.Trigger(1980521255);
      this.unlockChore = (Chore) new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) component);
    }

    private void CancelChore()
    {
      this.unlockChore.Cancel("UserCancel");
      this.unlockChore = (Chore) null;
      Prioritizable.RemoveRef(this.gameObject);
      this.Trigger(1980521255);
    }

    public int HorizontalGroupID() => -1;

    public int ButtonSideScreenSortOrder() => 20;
  }

  public class UnlockedStates : 
    GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State
  {
    public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State notify;
    public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State done;
  }
}
