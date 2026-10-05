// Decompiled with JetBrains decompiler
// Type: GravitasLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GravitasLocker : 
  GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
  public const float CLOSE_WORKTIME = 1f;
  public const float OPEN_WORKTIME = 1.5f;
  public const string CLOSED_ANIM_NAME = "on";
  public const string OPENING_ANIM_NAME = "working";
  public const string OPENED = "empty";
  private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter IsOpen;
  private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WasEmptied;
  private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WorkOrderGiven;
  public GravitasLocker.CloseStates close;
  public GravitasLocker.OpenStates open;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.close;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.close.ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.IsOpen, (GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State) this.open, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue).DefaultState(this.close.idle);
    this.close.idle.PlayAnim("on").ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.WorkOrderGiven, (GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State) this.close.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
    this.close.work.DefaultState(this.close.work.waitingForDupe);
    this.close.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartlWorkChore_OpenLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((Func<GravitasLocker.Instance, Workable>) (smi => smi.GetWorkable()), this.close.work.complete).ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.WorkOrderGiven, (GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State) this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
    this.close.work.complete.Enter((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback) (smi => this.WorkOrderGiven.Set(false, smi))).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Open)).TriggerOnEnter(GameHashes.UIRefresh);
    this.open.ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.IsOpen, (GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State) this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse).DefaultState(this.open.opening);
    this.open.opening.PlayAnim("working").OnAnimQueueComplete(this.open.idle);
    this.open.idle.PlayAnim("empty").Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.SpawnLoot)).ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.WorkOrderGiven, (GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State) this.open.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
    this.open.work.DefaultState(this.open.work.waitingForDupe);
    this.open.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartWorkChore_CloseLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((Func<GravitasLocker.Instance, Workable>) (smi => smi.GetWorkable()), this.open.work.complete).ParamTransition<bool>((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.Parameter<bool>) this.WorkOrderGiven, this.open.idle, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
    this.open.work.complete.Enter((StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback) (smi => this.WorkOrderGiven.Set(false, smi))).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Close)).TriggerOnEnter(GameHashes.UIRefresh);
  }

  public static void Open(GravitasLocker.Instance smi) => smi.Open();

  public static void Close(GravitasLocker.Instance smi) => smi.Close();

  public static void SpawnLoot(GravitasLocker.Instance smi) => smi.SpawnLoot();

  public static void StartWorkChore_CloseLocker(GravitasLocker.Instance smi)
  {
    smi.CreateWorkChore_CloseLocker();
  }

  public static void StartlWorkChore_OpenLocker(GravitasLocker.Instance smi)
  {
    smi.CreateWorkChore_OpenLocker();
  }

  public static void StopWorkChore(GravitasLocker.Instance smi) => smi.StopWorkChore();

  public class Def : StateMachine.BaseDef
  {
    public bool CanBeClosed;
    public string SideScreen_OpenButtonText;
    public string SideScreen_OpenButtonTooltip;
    public string SideScreen_CancelOpenButtonText;
    public string SideScreen_CancelOpenButtonTooltip;
    public string SideScreen_CloseButtonText;
    public string SideScreen_CloseButtonTooltip;
    public string SideScreen_CancelCloseButtonText;
    public string SideScreen_CancelCloseButtonTooltip;
    public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";
    public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";
    public string[] ObjectsToSpawn = new string[0];
    public string[] LootSymbols = new string[0];
  }

  public class WorkStates : 
    GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
  {
    public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State waitingForDupe;
    public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State complete;
  }

  public class CloseStates : 
    GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
  {
    public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;
    public GravitasLocker.WorkStates work;
  }

  public class OpenStates : 
    GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
  {
    public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State opening;
    public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;
    public GravitasLocker.WorkStates work;
  }

  public new class Instance(IStateMachineTarget master, GravitasLocker.Def def) : 
    GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.GameInstance(master, def),
    ISidescreenButtonControl
  {
    [MyCmpGet]
    private Workable workable;
    [MyCmpGet]
    private KBatchedAnimController animController;
    private Chore chore;
    private Vector3[] dropSpawnPositions;

    public bool WorkOrderGiven => this.smi.sm.WorkOrderGiven.Get(this.smi);

    public bool IsOpen => this.smi.sm.IsOpen.Get(this.smi);

    public bool HasContents
    {
      get => !this.smi.sm.WasEmptied.Get(this.smi) && this.def.ObjectsToSpawn.Length != 0;
    }

    public Workable GetWorkable() => this.workable;

    public void Open() => this.smi.sm.IsOpen.Set(true, this.smi);

    public void Close() => this.smi.sm.IsOpen.Set(false, this.smi);

    public override void StartSM()
    {
      this.DefineDropSpawnPositions();
      base.StartSM();
      this.UpdateContentPreviewSymbols();
    }

    public void DefineDropSpawnPositions()
    {
      if (this.dropSpawnPositions != null || this.def.LootSymbols.Length == 0)
        return;
      this.dropSpawnPositions = new Vector3[this.def.LootSymbols.Length];
      for (int index = 0; index < this.dropSpawnPositions.Length; ++index)
      {
        bool symbolVisible;
        Vector3 column = (Vector3) this.animController.GetSymbolTransform((HashedString) this.def.LootSymbols[index], out symbolVisible).GetColumn(3) with
        {
          z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
        };
        this.dropSpawnPositions[index] = symbolVisible ? column : this.gameObject.transform.GetPosition();
      }
    }

    public void CreateWorkChore_CloseLocker()
    {
      if (this.chore != null)
        return;
      this.workable.SetWorkTime(1f);
      this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, (IStateMachineTarget) this.workable, override_anims: Assets.GetAnim((HashedString) this.def.CLOSE_INTERACT_ANIM_NAME), priority_class: PriorityScreen.PriorityClass.high);
    }

    public void CreateWorkChore_OpenLocker()
    {
      if (this.chore != null)
        return;
      this.workable.SetWorkTime(1.5f);
      this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this.workable, override_anims: Assets.GetAnim((HashedString) this.def.OPEN_INTERACT_ANIM_NAME), priority_class: PriorityScreen.PriorityClass.high);
    }

    public void StopWorkChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Canceled by user");
      this.chore = (Chore) null;
    }

    public void SpawnLoot()
    {
      if (!this.HasContents)
        return;
      for (int index = 0; index < this.def.ObjectsToSpawn.Length; ++index)
      {
        string name = this.def.ObjectsToSpawn[index];
        GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject), 0, 0, name);
        gameObject.SetActive(true);
        if (this.dropSpawnPositions != null && index < this.dropSpawnPositions.Length)
          gameObject.transform.position = this.dropSpawnPositions[index];
      }
      this.smi.sm.WasEmptied.Set(true, this.smi);
      this.UpdateContentPreviewSymbols();
    }

    public void UpdateContentPreviewSymbols()
    {
      for (int index = 0; index < this.def.LootSymbols.Length; ++index)
        this.animController.SetSymbolVisiblity((KAnimHashedString) this.def.LootSymbols[index], false);
      if (!this.HasContents)
        return;
      for (int index = 0; index < Mathf.Min(this.def.LootSymbols.Length, this.def.ObjectsToSpawn.Length); ++index)
      {
        KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab((Tag) this.def.ObjectsToSpawn[index]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
        SymbolOverrideController component = this.gameObject.GetComponent<SymbolOverrideController>();
        string lootSymbol = this.def.LootSymbols[index];
        HashedString target_symbol = (HashedString) lootSymbol;
        KAnim.Build.Symbol source_symbol = symbolByIndex;
        component.AddSymbolOverride(target_symbol, source_symbol);
        this.animController.SetSymbolVisiblity((KAnimHashedString) lootSymbol, true);
      }
    }

    public string SidescreenButtonText
    {
      get
      {
        return !this.IsOpen ? (!this.WorkOrderGiven ? this.def.SideScreen_OpenButtonText : this.def.SideScreen_CancelOpenButtonText) : (!this.WorkOrderGiven ? this.def.SideScreen_CloseButtonText : this.def.SideScreen_CancelCloseButtonText);
      }
    }

    public string SidescreenButtonTooltip
    {
      get
      {
        return !this.IsOpen ? (!this.WorkOrderGiven ? this.def.SideScreen_OpenButtonTooltip : this.def.SideScreen_CancelOpenButtonTooltip) : (!this.WorkOrderGiven ? this.def.SideScreen_CloseButtonTooltip : this.def.SideScreen_CancelCloseButtonTooltip);
      }
    }

    public bool SidescreenEnabled() => !this.IsOpen || this.def.CanBeClosed;

    public bool SidescreenButtonInteractable() => !this.IsOpen || this.def.CanBeClosed;

    public int HorizontalGroupID() => 0;

    public int ButtonSideScreenSortOrder() => 20;

    public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
      throw new NotImplementedException();
    }

    public void OnSidescreenButtonPressed()
    {
      this.smi.sm.WorkOrderGiven.Set(!this.smi.sm.WorkOrderGiven.Get(this.smi), this.smi);
    }
  }
}
