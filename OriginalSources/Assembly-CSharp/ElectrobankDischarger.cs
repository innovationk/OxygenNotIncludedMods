// Decompiled with JetBrains decompiler
// Type: ElectrobankDischarger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ElectrobankDischarger : Generator
{
  public float wattageRating;
  [MyCmpReq]
  private Storage storage;
  private ElectrobankDischarger.StatesInstance smi;
  private List<Electrobank> storedCells = new List<Electrobank>();
  private MeterController meterController;
  protected FilteredStorage filteredStorage;

  public float ElectrobankJoulesStored
  {
    get
    {
      float electrobankJoulesStored = 0.0f;
      foreach (Electrobank storedCell in this.storedCells)
        electrobankJoulesStored += storedCell.Charge;
      return electrobankJoulesStored;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new ElectrobankDischarger.StatesInstance(this);
    this.smi.StartSM();
    this.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
    this.RefreshCells();
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.PowerFetch);
    this.filteredStorage.SetHasMeter(false);
    this.filteredStorage.FilterChanged();
    this.storage.onDestroyItemsDropped += new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeconstruction);
    this.UpdateSymbolSwap();
  }

  private void OnBatteriesDroppedFromDeconstruction(List<GameObject> items)
  {
    if (items == null)
      return;
    for (int index = 0; index < items.Count; ++index)
    {
      Electrobank component = items[index].GetComponent<Electrobank>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
      {
        double num = (double) component.RemovePower(component.Charge, true);
      }
    }
  }

  protected override void OnCleanUp()
  {
    this.filteredStorage.CleanUp();
    base.OnCleanUp();
  }

  private void OnStorageChange(object data = null)
  {
    this.RefreshCells();
    this.UpdateSymbolSwap();
  }

  public void UpdateMeter()
  {
    if (this.meterController == null)
      this.meterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
    this.meterController.SetPositionPercent(this.smi.master.ElectrobankJoulesStored / 120000f);
  }

  public void UpdateSymbolSwap()
  {
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    SymbolOverrideController component2 = component1.GetComponent<SymbolOverrideController>();
    component1.SetSymbolVisiblity((KAnimHashedString) "electrobank_l", false);
    if (this.storage.items.Count > 0)
    {
      KAnim.Build.Symbol symbol = this.storage.items[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.symbols[0];
      component2.AddSymbolOverride((HashedString) "electrobank_s", symbol);
    }
    else
      component2.RemoveSymbolOverride((HashedString) "electrobank_s");
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    bool flag = false;
    ushort circuitId = this.CircuitID;
    this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
    if (!this.operational.IsOperational)
    {
      if (!this.operational.IsActive)
        return;
      this.operational.SetActive(false);
    }
    else
    {
      float joulesAvailable = 0.0f;
      float num = Mathf.Min(this.wattageRating * dt, this.Capacity - this.JoulesAvailable);
      for (int index = this.storedCells.Count - 1; index >= 0; --index)
      {
        joulesAvailable += this.storedCells[index].RemovePower(num - joulesAvailable, true);
        if ((double) joulesAvailable >= (double) num)
          break;
      }
      if ((double) joulesAvailable > 0.0)
      {
        flag = true;
        this.GenerateJoules(joulesAvailable);
      }
      this.operational.SetActive(flag);
    }
  }

  private void RefreshCells(object data = null)
  {
    this.storedCells.Clear();
    foreach (GameObject gameObject in this.storage.GetItems())
    {
      Electrobank component = gameObject.GetComponent<Electrobank>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.storedCells.Add(component);
    }
  }

  public class StatesInstance(ElectrobankDischarger master) : 
    GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger>
  {
    public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State noBattery;
    public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State inoperational;
    public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging;
    public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging_pst;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.noBattery;
      this.root.EventTransition(GameHashes.ActiveChanged, this.discharging, (StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.noBattery.PlayAnim("off").EnterTransition(this.inoperational, (StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.Transition.ConditionCallback) (smi => smi.master.storage.items.Count != 0)).Enter((StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State.Callback) (smi => smi.master.UpdateMeter()));
      this.inoperational.PlayAnim("on").Enter((StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State.Callback) (smi => smi.master.UpdateMeter())).EnterTransition(this.noBattery, (StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.Transition.ConditionCallback) (smi => smi.master.storage.items.Count == 0));
      this.discharging.Enter((StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State.Callback) (smi => smi.master.UpdateMeter())).EventTransition(GameHashes.ActiveChanged, this.inoperational, (StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).QueueAnim("working_pre").QueueAnim("working_loop", true).Update((Action<ElectrobankDischarger.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ElectrobankJoulesAvailable, (object) smi.master);
        smi.master.UpdateMeter();
      }));
      this.discharging_pst.Enter((StateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State.Callback) (smi => smi.master.UpdateMeter())).PlayAnim("working_pst");
    }
  }
}
