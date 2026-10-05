// Decompiled with JetBrains decompiler
// Type: SapTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SapTree : 
  GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>
{
  public SapTree.AliveStates alive;
  public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead;
  private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.TargetParameter foodItem;
  private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.BoolParameter hasNearbyEnemy;
  private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.FloatParameter storedSap;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead = this.dead;
    string name1 = (string) CREATURES.STATUSITEMS.DEAD.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    dead.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State.Callback) (smi =>
    {
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.master.Trigger(1623392196);
      smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
    }));
    this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState((GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State) this.alive.normal);
    this.alive.normal.DefaultState(this.alive.normal.idle).EventTransition(GameHashes.Wilt, (GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State) this.alive.wilting, (StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Transition.ConditionCallback) (smi => smi.wiltCondition.IsWilting())).Update((System.Action<SapTree.StatesInstance, float>) ((smi, dt) => smi.CheckForFood()), UpdateRate.SIM_1000ms);
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state = this.alive.normal.idle.PlayAnim("idle", KAnim.PlayMode.Loop);
    string name2 = (string) CREATURES.STATUSITEMS.IDLE.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).ParamTransition<bool>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<bool>) this.hasNearbyEnemy, this.alive.normal.attacking_pre, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue).ParamTransition<float>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>) this.storedSap, this.alive.normal.oozing, (StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>.Callback) ((smi, p) => (double) p >= (double) smi.def.stomachSize)).ParamTransition<GameObject>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<GameObject>) this.foodItem, this.alive.normal.eating, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNotNull);
    this.alive.normal.eating.PlayAnim("eat_pre", KAnim.PlayMode.Once).QueueAnim("eat_loop", true).Update((System.Action<SapTree.StatesInstance, float>) ((smi, dt) => smi.EatFoodItem(dt)), UpdateRate.SIM_1000ms).ParamTransition<GameObject>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<GameObject>) this.foodItem, this.alive.normal.eating_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNull).ParamTransition<float>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>) this.storedSap, this.alive.normal.eating_pst, (StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>.Callback) ((smi, p) => (double) p >= (double) smi.def.stomachSize));
    this.alive.normal.eating_pst.PlayAnim("eat_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
    this.alive.normal.oozing.PlayAnim("ooze_pre", KAnim.PlayMode.Once).QueueAnim("ooze_loop", true).Update((System.Action<SapTree.StatesInstance, float>) ((smi, dt) => smi.Ooze(dt))).ParamTransition<float>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>) this.storedSap, this.alive.normal.oozing_pst, (StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0)).ParamTransition<bool>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<bool>) this.hasNearbyEnemy, this.alive.normal.oozing_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue);
    this.alive.normal.oozing_pst.PlayAnim("ooze_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
    this.alive.normal.attacking_pre.PlayAnim("attacking_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.attacking);
    this.alive.normal.attacking.PlayAnim("attacking_loop", KAnim.PlayMode.Once).Enter((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State.Callback) (smi => smi.DoAttack())).OnAnimQueueComplete(this.alive.normal.attacking_cooldown);
    this.alive.normal.attacking_cooldown.PlayAnim("attacking_pst", KAnim.PlayMode.Once).QueueAnim("attack_cooldown", true).ParamTransition<bool>((StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.Parameter<bool>) this.hasNearbyEnemy, this.alive.normal.attacking_done, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsFalse).ScheduleGoTo((Func<SapTree.StatesInstance, float>) (smi => smi.def.attackCooldown), (StateMachine.BaseState) this.alive.normal.attacking);
    this.alive.normal.attacking_done.PlayAnim("attack_to_idle", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
    this.alive.wilting.PlayAnim("withered", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, (GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State) this.alive.normal).ToggleTag(GameTags.PreventEmittingDisease);
  }

  public class Def : StateMachine.BaseDef
  {
    public Vector2I foodSenseArea;
    public float massEatRate;
    public float kcalorieToKGConversionRatio;
    public float stomachSize;
    public float oozeRate;
    public List<Vector3> oozeOffsets;
    public Vector2I attackSenseArea;
    public float attackCooldown;
  }

  public class AliveStates : 
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.PlantAliveSubState
  {
    public SapTree.NormalStates normal;
    public SapTree.WiltingState wilting;
  }

  public class NormalStates : 
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
  {
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State idle;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating_pst;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing_pst;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_pre;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_cooldown;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_done;
  }

  public class WiltingState : 
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
  {
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pre;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting;
    public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pst;
  }

  public class StatesInstance : 
    GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.GameInstance
  {
    [MyCmpReq]
    public WiltCondition wiltCondition;
    [MyCmpReq]
    public EntombVulnerable entombVulnerable;
    [MyCmpReq]
    private Storage storage;
    [MyCmpReq]
    private Weapon weapon;
    private HandleVector<int>.Handle partitionerEntry;
    private Extents feedExtents;
    private Extents attackExtents;

    public StatesInstance(IStateMachineTarget master, SapTree.Def def)
      : base(master, def)
    {
      Vector2I xy = Grid.PosToXY(this.gameObject.transform.GetPosition());
      Vector2I vector2I1 = new Vector2I(xy.x - def.attackSenseArea.x / 2, xy.y);
      this.attackExtents = new Extents(vector2I1.x, vector2I1.y, def.attackSenseArea.x, def.attackSenseArea.y);
      this.partitionerEntry = GameScenePartitioner.Instance.Add("SapTreeAttacker", (object) this, this.attackExtents, GameScenePartitioner.Instance.objectLayers[0], new System.Action<object>(this.OnMinionChanged));
      Vector2I vector2I2 = new Vector2I(xy.x - def.foodSenseArea.x / 2, xy.y);
      this.feedExtents = new Extents(vector2I2.x, vector2I2.y, def.foodSenseArea.x, def.foodSenseArea.y);
    }

    protected override void OnCleanUp()
    {
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    }

    public void EatFoodItem(float dt)
    {
      Pickupable pickupable = this.sm.foodItem.Get(this).GetComponent<Pickupable>().Take(this.def.massEatRate * dt);
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      float mass = pickupable.GetComponent<Edible>().Calories * (1f / 1000f) * this.def.kcalorieToKGConversionRatio;
      Util.KDestroyGameObject(pickupable.gameObject);
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      this.storage.AddLiquid(SimHashes.Resin, mass, component.Temperature, byte.MaxValue, 0, true, false);
      double num = (double) this.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this);
    }

    public void Ooze(float dt)
    {
      float amount = Mathf.Min(this.sm.storedSap.Get(this), dt * this.def.oozeRate);
      if ((double) amount <= 0.0)
        return;
      int index = Mathf.FloorToInt(GameClock.Instance.GetTime() % (float) this.def.oozeOffsets.Count);
      this.storage.DropSome(SimHashes.Resin.CreateTag(), amount, dumpLiquid: true, offset: this.def.oozeOffsets[index]);
      double num = (double) this.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this);
    }

    private static Util.IterationInstruction checkForFoodVisitor(
      object obj,
      ref (SapTree.StatesInstance, bool) context)
    {
      Pickupable pickupable = Unsafe.As<Pickupable>(obj);
      if ((UnityEngine.Object) pickupable.GetComponent<Edible>() != (UnityEngine.Object) null)
      {
        context.Item1.sm.foodItem.Set(pickupable.gameObject, context.Item1, false);
        context.Item2 = true;
      }
      return !context.Item2 ? Util.IterationInstruction.Continue : Util.IterationInstruction.Halt;
    }

    public void CheckForFood()
    {
      (SapTree.StatesInstance, bool) context = (this, false);
      GameScenePartitioner.Instance.VisitEntries<(SapTree.StatesInstance, bool)>(this.feedExtents.x, this.feedExtents.y, this.feedExtents.width, this.feedExtents.height, GameScenePartitioner.Instance.pickupablesLayer, new GameScenePartitioner.VisitorRef<(SapTree.StatesInstance, bool)>(SapTree.StatesInstance.checkForFoodVisitor), ref context);
      if (context.Item2)
        return;
      this.sm.foodItem.Set((KMonoBehaviour) null, this);
    }

    public bool DoAttack()
    {
      this.sm.hasNearbyEnemy.Set(this.weapon.AttackArea(this.transform.GetPosition()) > 0, this);
      return true;
    }

    private void OnMinionChanged(object obj)
    {
      if (!((UnityEngine.Object) (obj as GameObject) != (UnityEngine.Object) null))
        return;
      this.sm.hasNearbyEnemy.Set(true, this);
    }
  }
}
