// Decompiled with JetBrains decompiler
// Type: SetLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
  [MyCmpAdd]
  private Prioritizable prioritizable;
  public string[][] possible_contents_ids;
  public string machineSound;
  public string overrideAnim;
  public Vector2I dropOffset = Vector2I.zero;
  public int[] numDataBanks;
  [Serialize]
  private string[] contents;
  public bool dropOnDeconstruct;
  public bool skipAnim;
  [Serialize]
  private bool pendingRummage;
  [Serialize]
  private bool used;
  private Chore chore;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public void ChooseContents()
  {
    this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (this.contents == null)
    {
      this.ChooseContents();
    }
    else
    {
      foreach (string content in this.contents)
      {
        if ((UnityEngine.Object) Assets.GetPrefab((Tag) content) == (UnityEngine.Object) null)
        {
          this.ChooseContents();
          break;
        }
      }
    }
    if (!this.pendingRummage)
      return;
    this.ActivateChore();
  }

  public void DropContents()
  {
    if (this.contents == null)
      return;
    if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
    {
      int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
      for (int index = 0; index <= num; ++index)
      {
        Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), this.smi.master.transform);
      }
    }
    for (int index = 0; index < this.contents.Length; ++index)
    {
      GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[index], Grid.SceneLayer.Front);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        gameObject.SetActive(true);
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[index].ToTag()).GetProperName(), this.smi.master.transform);
      }
    }
    this.gameObject.Trigger(-372600542, (object) this);
  }

  private void OnClickOpen() => this.ActivateChore();

  private void OnClickCancel() => this.CancelChore();

  public void ActivateChore(object param = null)
  {
    if (this.chore != null)
      return;
    Prioritizable.AddRef(this.gameObject);
    this.Trigger(1980521255, (object) null);
    this.pendingRummage = true;
    this.GetComponent<Workable>().SetWorkTime(1.5f);
    this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, on_complete: (Action<Chore>) (o => this.CompleteChore()), on_begin: (Action<Chore>) (o => this.smi.GoTo((StateMachine.BaseState) this.smi.sm.being_worked)), on_end: (Action<Chore>) (o => this.OnChoreEnd()), override_anims: Assets.GetAnim((HashedString) this.overrideAnim), priority_class: PriorityScreen.PriorityClass.high);
  }

  public void CancelChore(object param = null)
  {
    if (this.chore == null)
      return;
    this.pendingRummage = false;
    Prioritizable.RemoveRef(this.gameObject);
    this.Trigger(1980521255, (object) null);
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void OnChoreEnd()
  {
    if (!this.skipAnim || this.chore == null)
      return;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.closed);
  }

  private void CompleteChore()
  {
    this.used = true;
    if (this.skipAnim)
    {
      this.DropContents();
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.off);
    }
    else
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.open);
    this.chore = (Chore) null;
    this.pendingRummage = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
    Prioritizable.RemoveRef(this.gameObject);
  }

  public string SidescreenButtonText
  {
    get
    {
      if (this.used)
        return (string) UI.USERMENUACTIONS.OPENPOI.ALREADY_RUMMAGED;
      return this.chore != null ? (string) UI.USERMENUACTIONS.OPENPOI.NAME_OFF : (string) UI.USERMENUACTIONS.OPENPOI.NAME;
    }
  }

  public string SidescreenButtonTooltip
  {
    get
    {
      if (this.used)
        return (string) UI.USERMENUACTIONS.OPENPOI.TOOLTIP_ALREADYRUMMAGED;
      return this.chore != null ? (string) UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF : (string) UI.USERMENUACTIONS.OPENPOI.TOOLTIP;
    }
  }

  public bool SidescreenEnabled() => true;

  public int HorizontalGroupID() => -1;

  public void OnSidescreenButtonPressed()
  {
    if (this.chore == null)
      this.OnClickOpen();
    else
      this.OnClickCancel();
  }

  public bool SidescreenButtonInteractable() => !this.used;

  public int ButtonSideScreenSortOrder() => 20;

  public void SetButtonTextOverride(ButtonMenuTextOverride text)
  {
    throw new NotImplementedException();
  }

  public class StatesInstance(SetLocker master) : 
    GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance(master)
  {
    public override void StartSM()
    {
      base.StartSM();
      this.smi.Subscribe(-702296337, (Action<object>) (o =>
      {
        if (!this.smi.master.dropOnDeconstruct || !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.closed))
          return;
        this.smi.master.DropContents();
      }));
    }
  }

  public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
  {
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State being_worked;
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;
    public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.closed;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.closed.PlayAnim("on").Enter((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StartSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
      this.being_worked.DoNothing();
      this.open.PlayAnim("working_pre").QueueAnim("working_loop").QueueAnim("working_pst").OnAnimQueueComplete(this.off).Exit((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi => smi.master.DropContents()));
      this.off.PlayAnim("off").Enter((StateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StopSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
    }
  }
}
