// Decompiled with JetBrains decompiler
// Type: CritterCondoStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class CritterCondoStates : 
  GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>
{
  public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State goingToCondo;
  public CritterCondoStates.InteractState interact;
  public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State behaviourComplete;
  public StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.TargetParameter targetCondo;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingToCondo;
    this.root.Enter(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ReserveCondo)).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.UnreserveCondo));
    this.goingToCondo.MoveTo(new Func<CritterCondoStates.Instance, int>(CritterCondoStates.GetCondoInteractCell), (GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State) this.interact).ToggleMainStatusItem((Func<CritterCondoStates.Instance, StatusItem>) (smi => CritterCondoStates.GetTargetCondo(smi).def.moveToStatusItem)).OnTargetLost(this.targetCondo, (GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State) null);
    this.interact.DefaultState(this.interact.pre).OnTargetLost(this.targetCondo, (GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State) null).Enter((StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback) (smi =>
    {
      this.SetFacing(smi);
      smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    })).Exit((StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback) (smi => smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures))).ToggleMainStatusItem((Func<CritterCondoStates.Instance, StatusItem>) (smi => CritterCondoStates.GetTargetCondo(smi).def.interactStatusItem));
    this.interact.pre.PlayAnim("cc_working_pre").Enter((StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback) (smi => CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pre"))).OnAnimQueueComplete(this.interact.loop);
    this.interact.loop.PlayAnim("cc_working").Enter((StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback) (smi => CritterCondoStates.PlayCondoBuildingAnim(smi, smi.def.working_anim))).OnAnimQueueComplete(this.interact.pst);
    this.interact.pst.PlayAnim("cc_working_pst").Enter((StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback) (smi => CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pst"))).OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviour_InteractWithCritterCondo).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ApplyEffects));
  }

  private void SetFacing(CritterCondoStates.Instance smi)
  {
    bool isRotated = CritterCondoStates.GetTargetCondo(smi).Get<Rotatable>().IsRotated;
    smi.Get<Facing>().SetFacing(isRotated);
  }

  private static CritterCondo.Instance GetTargetCondo(CritterCondoStates.Instance smi)
  {
    GameObject go = smi.sm.targetCondo.Get(smi);
    CritterCondo.Instance smi1 = (UnityEngine.Object) go != (UnityEngine.Object) null ? go.GetSMI<CritterCondo.Instance>() : (CritterCondo.Instance) null;
    return smi1.IsNullOrStopped() ? (CritterCondo.Instance) null : smi1;
  }

  private static void ReserveCondo(CritterCondoStates.Instance smi)
  {
    CritterCondo.Instance targetCondo = smi.GetSMI<CritterCondoInteractMontior.Instance>().targetCondo;
    if (targetCondo == null)
      return;
    smi.sm.targetCondo.Set(targetCondo.gameObject, smi, false);
    targetCondo.SetReserved(true);
  }

  private static void UnreserveCondo(CritterCondoStates.Instance smi)
  {
    CritterCondo.Instance targetCondo = CritterCondoStates.GetTargetCondo(smi);
    if (targetCondo == null)
      return;
    targetCondo.GetComponent<KBatchedAnimController>().Play((HashedString) "on", KAnim.PlayMode.Loop);
    smi.sm.targetCondo.Set((KMonoBehaviour) null, smi);
    targetCondo.SetReserved(false);
  }

  private static int GetCondoInteractCell(CritterCondoStates.Instance smi)
  {
    CritterCondo.Instance targetCondo = CritterCondoStates.GetTargetCondo(smi);
    if (targetCondo == null)
      return Grid.InvalidCell;
    int cell = targetCondo.GetInteractStartCell();
    if (smi.isLargeCritter)
    {
      bool isRotated = targetCondo.Get<Rotatable>().IsRotated;
      if (targetCondo.def.condoTag == (Tag) "UnderwaterCritterCondo")
      {
        if (!isRotated)
          cell = Grid.CellLeft(cell);
      }
      else
      {
        Vector2I xy1 = Grid.PosToXY(smi.gameObject.transform.position);
        Vector2I xy2 = Grid.CellToXY(cell);
        if (xy1.x > xy2.x && !isRotated)
          cell = Grid.CellLeft(cell);
        else if (xy1.x < xy2.x & isRotated)
          cell = Grid.CellRight(cell);
      }
    }
    return cell;
  }

  private static void ApplyEffects(CritterCondoStates.Instance smi)
  {
    smi.Get<Effects>().Add(CritterCondoStates.GetTargetCondo(smi).def.effectId, true);
  }

  private static void PlayCondoBuildingAnim(CritterCondoStates.Instance smi, string anim_name)
  {
    smi.sm.targetCondo.GetSMI<CritterCondo.Instance>(smi)?.UpdateCritterAnims(anim_name, smi.def.entersBuilding, smi.def.fgLayer);
  }

  public class Def : StateMachine.BaseDef
  {
    public bool entersBuilding = true;
    public string working_anim = "cc_working";
    public CritterCondo.CreatureFGLayerType fgLayer;
  }

  public new class Instance : 
    GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.GameInstance
  {
    public bool isLargeCritter;

    public Instance(Chore<CritterCondoStates.Instance> chore, CritterCondoStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviour_InteractWithCritterCondo);
      this.isLargeCritter = this.GetComponent<KPrefabID>().HasTag(GameTags.LargeCreature);
    }
  }

  public class InteractState : 
    GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State
  {
    public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pre;
    public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State loop;
    public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pst;
  }
}
