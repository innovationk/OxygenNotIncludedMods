// Decompiled with JetBrains decompiler
// Type: CritterCondo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CritterCondo : 
  GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
  public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State inoperational;
  public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational));
    this.operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational))).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)));
  }

  private static bool IsOperational(CritterCondo.Instance smi)
  {
    return smi.def.IsCritterCondoOperationalCb(smi);
  }

  public enum CreatureFGLayerType
  {
    SmallCreatureLayer,
    LargeCreatureLayer,
    SquidLayer,
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Func<CritterCondo.Instance, bool> IsCritterCondoOperationalCb;
    public System.Action<KBatchedAnimController, CritterCondo.CreatureFGLayerType> UpdateForegroundVisibilitySymbols;
    public StatusItem moveToStatusItem;
    public StatusItem interactStatusItem;
    public Tag condoTag = (Tag) nameof (CritterCondo);
    public string effectId;

    public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>();
  }

  public new class Instance : 
    GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.GameInstance
  {
    private KBatchedAnimController foregroundController;
    private KBatchedAnimController animController;

    public Instance(IStateMachineTarget master, CritterCondo.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.foregroundController = ((IEnumerable<KBatchedAnimController>) this.animController.GetComponentsInChildren<KBatchedAnimController>()).First<KBatchedAnimController>((Func<KBatchedAnimController, bool>) (kbac => (UnityEngine.Object) kbac != (UnityEngine.Object) this.animController));
    }

    public override void StartSM()
    {
      base.StartSM();
      Components.CritterCondos.Add(this.smi.GetMyWorldId(), this);
    }

    protected override void OnCleanUp()
    {
      Components.CritterCondos.Remove(this.smi.GetMyWorldId(), this);
    }

    public bool IsReserved() => this.HasTag(GameTags.Creatures.ReservedByCreature);

    public void SetReserved(bool isReserved)
    {
      if (isReserved)
        this.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
      else if (this.HasTag(GameTags.Creatures.ReservedByCreature))
        this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
      else
        Debug.LogWarningFormat((UnityEngine.Object) this.smi.gameObject, "Tried to unreserve a condo that wasn't reserved");
    }

    public int GetInteractStartCell() => Grid.PosToCell((StateMachine.Instance) this);

    public bool CanBeReserved() => !this.IsReserved() && CritterCondo.IsOperational(this);

    public void UpdateCritterAnims(
      string anim_name,
      bool enters,
      CritterCondo.CreatureFGLayerType fg_layer)
    {
      if (enters)
        this.animController.Play((HashedString) anim_name);
      if (this.def.UpdateForegroundVisibilitySymbols == null)
        return;
      this.def.UpdateForegroundVisibilitySymbols(this.foregroundController, fg_layer);
    }
  }
}
