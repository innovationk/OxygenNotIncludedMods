// Decompiled with JetBrains decompiler
// Type: ElementDropperMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ElementDropperMonitor : 
  GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
  public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;
  public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;
  public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventHandler(GameHashes.DeathAnimComplete, (StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State.Callback) (smi => smi.DropDeathElement()));
    this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Parameter<StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.SignalParameter>.Callback) ((smi, param) => smi.ShouldDropElement()));
    this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<ElementDropperMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.satisfied))).EventHandler(GameHashes.ObjectMovementStateChanged, (GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameEvent.Callback) ((smi, d) =>
    {
      if (Boxed<GameHashes>.Unbox(d) != GameHashes.ObjectMovementWakeUp)
        return;
      smi.GoTo((StateMachine.BaseState) this.satisfied);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
    public SimHashes dirtyEmitElement;
    public float dirtyProbabilityPercent;
    public float dirtyCellToTargetMass;
    public float dirtyMassPerDirty;
    public float dirtyMassReleaseOnDeath;
    public byte emitDiseaseIdx = byte.MaxValue;
    public float emitDiseasePerKg;
  }

  public new class Instance : 
    GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
  {
    private ulong cellChangeHandlerID;
    private static readonly System.Action<object> OnCellChangeDispatcher = (System.Action<object>) (obj => Unsafe.As<ElementDropperMonitor.Instance>(obj).OnCellChange());

    public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def)
      : base(master, def)
    {
      this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, ElementDropperMonitor.Instance.OnCellChangeDispatcher, (object) this, "ElementDropperMonitor.Instance");
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
    }

    private void OnCellChange() => this.sm.cellChangedSignal.Trigger(this);

    public bool ShouldDropElement()
    {
      return this.IsValidDropCell() && (double) UnityEngine.Random.Range(0.0f, 100f) < (double) this.def.dirtyProbabilityPercent;
    }

    public void DropDeathElement()
    {
      this.DropElement(this.def.dirtyMassReleaseOnDeath, this.def.dirtyEmitElement, this.def.emitDiseaseIdx, Mathf.RoundToInt(this.def.dirtyMassReleaseOnDeath * this.def.dirtyMassPerDirty));
    }

    public void DropPeriodicElement()
    {
      this.DropElement(this.def.dirtyMassPerDirty, this.def.dirtyEmitElement, this.def.emitDiseaseIdx, Mathf.RoundToInt(this.def.emitDiseasePerKg * this.def.dirtyMassPerDirty));
    }

    public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
    {
      if ((double) mass <= 0.0)
        return;
      Element elementByHash = ElementLoader.FindElementByHash(element_id);
      float temperature = this.GetComponent<PrimaryElement>().Temperature;
      if (elementByHash.IsGas || elementByHash.IsLiquid)
        SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count);
      else if (elementByHash.IsSolid)
        elementByHash.substance.SpawnResource(this.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f), mass, temperature, disease_idx, disease_count, forceTemperature: true);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, elementByHash.name, this.gameObject.transform);
    }

    public bool IsValidDropCell()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return Grid.IsValidCell(cell) && Grid.IsGas(cell) && (double) Grid.Mass[cell] <= 1.0;
    }
  }
}
