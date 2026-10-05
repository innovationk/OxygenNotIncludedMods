// Decompiled with JetBrains decompiler
// Type: VomitChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class VomitChore : Chore<VomitChore.StatesInstance>
{
  private static KAnimFile GetAnimFileName(VomitChore.StatesInstance smi)
  {
    string name = "anim_vomit_kanim";
    GameObject gameObject = smi.sm.vomiter.Get(smi);
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return Assets.GetAnim((HashedString) name);
    MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return Assets.GetAnim((HashedString) name);
    return component.model == BionicMinionConfig.MODEL ? Assets.GetAnim((HashedString) "anim_bionic_vomit_kanim") : Assets.GetAnim((HashedString) name);
  }

  public VomitChore(
    ChoreType chore_type,
    IStateMachineTarget target,
    StatusItem status_item,
    Notification notification,
    Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), on_complete: on_complete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new VomitChore.StatesInstance(this, target.gameObject, status_item, notification);
  }

  public class StatesInstance : 
    GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.GameInstance
  {
    public StatusItem statusItem;
    private AmountInstance bodyTemperature;
    public Notification notification;
    private SafetyQuery vomitCellQuery;

    public SimHashes elementToVomit { private set; get; } = SimHashes.DirtyWater;

    public StatesInstance(
      VomitChore master,
      GameObject vomiter,
      StatusItem status_item,
      Notification notification)
      : base(master)
    {
      this.sm.vomiter.Set(vomiter, this.smi, false);
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
      this.statusItem = status_item;
      this.notification = notification;
      this.vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, this.GetComponent<KMonoBehaviour>(), 10);
      MinionIdentity component = vomiter.GetComponent<MinionIdentity>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !(component.model == BionicMinionConfig.MODEL))
        return;
      this.elementToVomit = SimHashes.LiquidGunk;
    }

    private static bool CanEmitLiquid(int cell)
    {
      bool flag = true;
      if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || ((int) Grid.Properties[cell] & 2) != 0)
        flag = false;
      return flag;
    }

    public void SpawnDirtyWater(float dt) => this.SpawnVomitLiquid(dt, SimHashes.DirtyWater);

    public void SpawnVomitLiquid(float dt, SimHashes element)
    {
      if ((double) dt <= 0.0)
        return;
      float totalTime = this.GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
      float num1 = dt / totalTime;
      Sicknesses sicknesses = this.master.GetComponent<MinionModifiers>().sicknesses;
      SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
      int idx = 0;
      while (idx < sicknesses.Count && sicknesses[idx].modifier.sicknessType != Sickness.SicknessType.Pathogen)
        ++idx;
      Facing component = this.sm.vomiter.Get(this.smi).GetComponent<Facing>();
      int cell = Grid.PosToCell(component.transform.GetPosition());
      int num2 = component.GetFrontCell();
      if (!VomitChore.StatesInstance.CanEmitLiquid(num2))
        num2 = cell;
      Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
      if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        equippable.GetComponent<Storage>().AddLiquid(element, TUNING.STRESS.VOMIT_AMOUNT * num1, this.bodyTemperature.value, invalid.idx, invalid.count);
      else
        SimMessages.AddRemoveSubstance(num2, element, CellEventLogger.Instance.Vomit, TUNING.STRESS.VOMIT_AMOUNT * num1, this.bodyTemperature.value, invalid.idx, invalid.count);
    }

    public int GetVomitCell()
    {
      this.vomitCellQuery.Reset();
      Navigator component = this.GetComponent<Navigator>();
      component.RunQuery((PathFinderQuery) this.vomitCellQuery);
      int vomitCell = this.vomitCellQuery.GetResultCell();
      if (Grid.InvalidCell == vomitCell)
        vomitCell = Grid.PosToCell((KMonoBehaviour) component);
      return vomitCell;
    }
  }

  public class States : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore>
  {
    public StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.TargetParameter vomiter;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State moveto;
    public VomitChore.States.VomitState vomit;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover_pst;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State complete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.moveto;
      this.Target(this.vomiter);
      this.root.ToggleAnims("anim_emotes_default_kanim");
      this.moveto.TriggerOnEnter(GameHashes.BeginWalk).TriggerOnExit(GameHashes.EndWalk).ToggleAnims("anim_loco_vomiter_kanim").MoveTo((Func<VomitChore.StatesInstance, int>) (smi => smi.GetVomitCell()), (GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State) this.vomit, (GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State) this.vomit);
      this.vomit.DefaultState(this.vomit.buildup).ToggleAnims(new Func<VomitChore.StatesInstance, KAnimFile>(VomitChore.GetAnimFileName)).ToggleStatusItem((Func<VomitChore.StatesInstance, StatusItem>) (smi => smi.statusItem)).DoNotification((Func<VomitChore.StatesInstance, Notification>) (smi => smi.notification)).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Enter((StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State.Callback) (smi =>
      {
        if ((double) smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value <= 0.0)
          return;
        smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
      })).Exit((StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State.Callback) (smi =>
      {
        smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
        float f = Mathf.Min(smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value, 20f);
        double num = (double) smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-f);
        if ((double) f < 1.0)
          return;
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(f).ToString() + (string) UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform);
      }));
      this.vomit.buildup.PlayAnim("vomit_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.vomit.release);
      this.vomit.release.ToggleEffect("Vomiting").PlayAnim("vomit_loop", KAnim.PlayMode.Once).Update("SpawnVomitLiquid", (Action<VomitChore.StatesInstance, float>) ((smi, dt) => smi.SpawnVomitLiquid(dt, smi.elementToVomit))).OnAnimQueueComplete(this.vomit.release_pst);
      this.vomit.release_pst.PlayAnim("vomit_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover);
      this.recover.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true).ScheduleGoTo(8f, (StateMachine.BaseState) this.recover_pst);
      this.recover_pst.QueueAnim("breathe_pst").OnAnimQueueComplete(this.complete);
      this.complete.ReturnSuccess();
    }

    public class VomitState : 
      GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State
    {
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State buildup;
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release;
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release_pst;
    }
  }
}
