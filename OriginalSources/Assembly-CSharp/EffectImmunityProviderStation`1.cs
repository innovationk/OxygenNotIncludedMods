// Decompiled with JetBrains decompiler
// Type: EffectImmunityProviderStation`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class EffectImmunityProviderStation<StateMachineInstanceType> : 
  GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>
  where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
  public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State inactive;
  public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State active;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.inactive;
    this.inactive.EventTransition(GameHashes.ActiveChanged, this.active, (StateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
    this.active.EventTransition(GameHashes.ActiveChanged, this.inactive, (StateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
  }

  public class Def : StateMachine.BaseDef
  {
    public System.Action<GameObject, StateMachineInstanceType> onEffectApplied;
    public Func<GameObject, bool> specialRequirements;
    public Func<GameObject, string> overrideFileName;
    public string[] overrideAnims;
    public CellOffset[][] range;

    public virtual string[] DefaultAnims()
    {
      return new string[3]{ "", "", "" };
    }

    public virtual string DefaultAnimFileName() => "anim_warmup_kanim";

    public string[] GetAnimNames()
    {
      return this.overrideAnims != null ? this.overrideAnims : this.DefaultAnims();
    }

    public string GetAnimFileName(GameObject entity)
    {
      return this.overrideFileName != null ? this.overrideFileName(entity) : this.DefaultAnimFileName();
    }
  }

  public abstract class BaseInstance : 
    GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.GameInstance
  {
    public string GetAnimFileName(GameObject entity) => this.def.GetAnimFileName(entity);

    public string PreAnimName => this.def.GetAnimNames()[0];

    public string LoopAnimName => this.def.GetAnimNames()[1];

    public string PstAnimName => this.def.GetAnimNames()[2];

    public bool CanBeUsed
    {
      get
      {
        if (!this.IsActive)
          return false;
        return this.def.specialRequirements == null || this.def.specialRequirements(this.gameObject);
      }
    }

    protected bool IsActive => this.IsInsideState((StateMachine.BaseState) this.sm.active);

    public BaseInstance(
      IStateMachineTarget master,
      EffectImmunityProviderStation<StateMachineInstanceType>.Def def)
      : base(master, def)
    {
    }

    public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
    {
      _cost = int.MaxValue;
      if (!this.CanBeUsed)
        return Grid.InvalidCell;
      int cell1 = Grid.PosToCell((StateMachine.Instance) this);
      int bestAvailableCell = Grid.InvalidCell;
      if (this.def.range == null)
      {
        if (!dupeLooking.CanReach(cell1))
          return Grid.InvalidCell;
        _cost = dupeLooking.GetNavigationCost(cell1);
        return cell1;
      }
      for (int index1 = 0; index1 < this.def.range.GetLength(0); ++index1)
      {
        int num = int.MaxValue;
        for (int index2 = 0; index2 < this.def.range[index1].Length; ++index2)
        {
          int cell2 = Grid.OffsetCell(cell1, this.def.range[index1][index2]);
          if (dupeLooking.CanReach(cell2))
          {
            int navigationCost = dupeLooking.GetNavigationCost(cell2);
            if (navigationCost < num)
            {
              num = navigationCost;
              bestAvailableCell = cell2;
            }
          }
        }
        if (bestAvailableCell != Grid.InvalidCell)
        {
          _cost = num;
          break;
        }
      }
      return bestAvailableCell;
    }

    public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
    {
      Effects component = target.GetComponent<Effects>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      this.ApplyImmunityEffect(component);
      if (!triggerEvents)
        return;
      System.Action<GameObject, StateMachineInstanceType> onEffectApplied = this.def.onEffectApplied;
      if (onEffectApplied == null)
        return;
      onEffectApplied(component.gameObject, (StateMachineInstanceType) this);
    }

    protected abstract void ApplyImmunityEffect(Effects target);

    public override void StartSM()
    {
      Components.EffectImmunityProviderStations.Add((StateMachine.Instance) this);
      base.StartSM();
    }

    protected override void OnCleanUp()
    {
      Components.EffectImmunityProviderStations.Remove((StateMachine.Instance) this);
      base.OnCleanUp();
    }
  }
}
