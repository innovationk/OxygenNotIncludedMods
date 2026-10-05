// Decompiled with JetBrains decompiler
// Type: ClusterMapLongRangeMissile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterMapLongRangeMissile : 
  GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>
{
  public StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.TargetParameter targetObject;
  public StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.AxialIParameter destinationHex;
  public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State initialization;
  public ClusterMapLongRangeMissile.TravellingStates travelling;
  public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State contact;
  public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State exploding_with_visual;
  public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State cleanup;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.initialization;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.ToggleTag(GameTags.EntityInSpace);
    this.initialization.Enter((StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback) (smi =>
    {
      if (smi.exploded)
        smi.GoTo((StateMachine.BaseState) smi.sm.cleanup);
      else if ((Object) this.targetObject.Get(smi) != (Object) null)
        smi.GoTo((StateMachine.BaseState) smi.sm.travelling.moving);
      else
        smi.GoTo((StateMachine.BaseState) smi.sm.contact);
    }));
    this.travelling.ToggleStatusItem(Db.Get().MiscStatusItems.LongRangeMissileTTI).OnTargetLost(this.targetObject, this.contact).Target(this.targetObject).EventHandler(GameHashes.ClusterLocationChanged, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback(ClusterMapLongRangeMissile.UpdatePath)).Target(this.masterTarget);
    this.travelling.moving.ToggleTag(GameTags.LongRangeMissileMoving).EnterTransition(this.travelling.idle, (StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback) (smi => !smi.IsTraveling())).EventTransition(GameHashes.ClusterDestinationReached, this.travelling.idle);
    this.travelling.idle.ToggleTag(GameTags.LongRangeMissileIdle).Transition(this.contact, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HitTarget), UpdateRate.SIM_1000ms).Transition(this.contact, GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Not(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.CanHitTarget)), UpdateRate.SIM_1000ms);
    this.contact.Enter(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback(ClusterMapLongRangeMissile.TriggerDamage)).EnterTransition(this.exploding_with_visual, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HasVisualizer)).EnterTransition(this.cleanup, GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Not(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HasVisualizer)));
    this.exploding_with_visual.ToggleTag(GameTags.LongRangeMissileExploding).EventTransition(GameHashes.RocketExploded, this.cleanup);
    this.cleanup.Enter((StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback) (smi => smi.gameObject.DeleteObject())).GoTo((GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State) null);
  }

  private static bool HasVisualizer(ClusterMapLongRangeMissile.StatesInstance smi)
  {
    return smi != null && (Object) ClusterMapScreen.Instance.GetEntityVisAnim(smi.GetComponent<ClusterGridEntity>()) != (Object) null;
  }

  public static void TriggerDamage(ClusterMapLongRangeMissile.StatesInstance smi)
  {
    GameObject go = smi.sm.targetObject.Get(smi);
    if ((Object) go != (Object) null && ClusterMapLongRangeMissile.CanHitTarget(smi))
      go.Trigger(-2056344675, (object) MissileLongRangeConfig.DamageEventPayload.sharedInstance);
    smi.exploded = true;
  }

  public static bool HitTarget(ClusterMapLongRangeMissile.StatesInstance smi)
  {
    ClusterGridEntity clusterGridEntity = smi.sm.targetObject.Get<ClusterGridEntity>(smi);
    return !((Object) clusterGridEntity == (Object) null) && clusterGridEntity.Location == smi.sm.destinationHex.Get(smi);
  }

  public static bool CanHitTarget(ClusterMapLongRangeMissile.StatesInstance smi)
  {
    return (Object) smi.sm.targetObject.Get(smi) != (Object) null;
  }

  private static void UpdatePath(ClusterMapLongRangeMissile.StatesInstance smi)
  {
    ClusterDestinationSelector component1 = smi.GetComponent<ClusterDestinationSelector>();
    if ((Object) component1 == (Object) null)
      return;
    ClusterGridEntity target = smi.sm.targetObject.Get<ClusterGridEntity>(smi);
    if ((Object) target == (Object) null)
      return;
    ClusterGridEntity component2 = smi.GetComponent<ClusterGridEntity>();
    AxialI interceptPoint = ClusterMapLongRangeMissile.StatesInstance.FindInterceptPoint(component2.Location, target, component1);
    if (!(interceptPoint != smi.sm.destinationHex.Get(smi)))
      return;
    smi.Travel(component2.Location, interceptPoint);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class TravellingStates : 
    GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State
  {
    public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State moving;
    public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State idle;
  }

  public class StatesInstance : 
    GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.GameInstance
  {
    [Serialize]
    public bool exploded;
    public KBatchedAnimController animController;

    public StatesInstance(IStateMachineTarget master, ClusterMapLongRangeMissile.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
    }

    public void Setup(
      AxialI source,
      ClusterGridEntity target,
      MissileLongRangeProjectile.Def projectile_def)
    {
      BallisticClusterGridEntity component = this.GetComponent<BallisticClusterGridEntity>();
      component.nameKey = new StringKey(projectile_def.missileName);
      this.GetComponent<InfoDescription>().description = (string) Strings.Get(projectile_def.missileDesc);
      component.SwapSymbolFromSameAnim("payload", projectile_def.starmapOverrideSymbol);
      KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) projectile_def.starmapOverrideSymbol);
      this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "payload", symbol);
      this.sm.targetObject.Set(target.gameObject, this, false);
      this.Travel(source, ClusterMapLongRangeMissile.StatesInstance.FindInterceptPoint(source, target, this.GetComponent<ClusterDestinationSelector>()));
    }

    public static AxialI FindInterceptPoint(
      AxialI source,
      ClusterGridEntity target,
      ClusterDestinationSelector selector,
      int maxGridRange = 99999)
    {
      ClusterTraveler component = target.GetComponent<ClusterTraveler>();
      if ((Object) component != (Object) null)
      {
        List<AxialI> currentPath = component.CurrentPath;
        AxialI interceptPoint = target.Location;
        foreach (AxialI axialI in currentPath)
        {
          float num = component.TravelETA(axialI);
          List<AxialI> path = ClusterGrid.Instance.GetPath(source, axialI, selector);
          if (path != null && path.Count != 0 && path.Count <= maxGridRange && (double) path.Count * 600.0 / 10.0 < (double) num)
            return interceptPoint;
          interceptPoint = axialI;
        }
      }
      return target.Location;
    }

    public float InterceptETA()
    {
      ClusterTraveler component1 = this.GetComponent<ClusterTraveler>();
      float a = 0.0f;
      float b = component1.TravelETA();
      GameObject gameObject = this.sm.targetObject.Get(this);
      if ((Object) gameObject != (Object) null)
      {
        ClusterTraveler component2 = gameObject.GetComponent<ClusterTraveler>();
        if ((Object) component2 != (Object) null && component1.CurrentPath != null)
          a = component2.TravelETA(component1.Destination);
      }
      return Mathf.Max(a, b);
    }

    public void Travel(AxialI source, AxialI destination)
    {
      this.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
      this.sm.destinationHex.Set(destination, this);
      this.GoTo((StateMachine.BaseState) this.sm.travelling.moving);
    }

    public bool IsTraveling()
    {
      ClusterTraveler component = this.GetComponent<ClusterTraveler>();
      return component.CurrentPath != null && component.CurrentPath.Count != 0;
    }
  }
}
