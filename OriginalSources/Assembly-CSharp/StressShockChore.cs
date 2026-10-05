// Decompiled with JetBrains decompiler
// Type: StressShockChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StressShockChore : Chore<StressShockChore.StatesInstance>
{
  public const float FaceBeamZOffset = 0.01f;

  private static bool CheckBlocked(int sourceCell, int destinationCell)
  {
    HashSet<int> outputCells = new HashSet<int>();
    Grid.CollectCellsInLine(sourceCell, destinationCell, outputCells);
    bool flag = false;
    foreach (int i in outputCells)
    {
      if (Grid.Solid[i])
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public static void AddBatteryDrainModifier(StressShockChore.StatesInstance smi)
  {
    smi.SetDrainModifierActiveState(true);
  }

  public static void RemoveBatteryDrainModifier(StressShockChore.StatesInstance smi)
  {
    smi.SetDrainModifierActiveState(false);
  }

  public static void ForceStressMonitorToTimeOut(StressShockChore.StatesInstance smi)
  {
    smi.GetSMI<StressBehaviourMonitor.Instance>()?.ManualSetStressTier2TimeCounter(150f);
  }

  public StressShockChore(
    ChoreType chore_type,
    IStateMachineTarget target,
    Notification notification,
    Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.StressShock, target, target.GetComponent<ChoreProvider>(), false, on_complete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new StressShockChore.StatesInstance(this, target.gameObject, notification);
  }

  public class StatesInstance : 
    GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.GameInstance
  {
    public Notification notification;
    [MySmiReq]
    public BionicBatteryMonitor.Instance batteryMonitor;
    public BionicBatteryMonitor.WattageModifier powerDrainModifier = new BionicBatteryMonitor.WattageModifier(nameof (StressShockChore), string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, (object) DUPLICANTS.TRAITS.STRESSSHOCKER.DRAIN_ATTRIBUTE, (object) ("<b>+</b>" + GameUtil.GetFormattedWattage(TUNING.STRESS.SHOCKER.POWER_CONSUMPTION_RATE))), TUNING.STRESS.SHOCKER.POWER_CONSUMPTION_RATE, TUNING.STRESS.SHOCKER.POWER_CONSUMPTION_RATE);

    public StatesInstance(StressShockChore master, GameObject shocker, Notification notification)
      : base(master)
    {
      this.sm.shocker.Set(shocker, this.smi, false);
      this.notification = notification;
    }

    public void SetDrainModifierActiveState(bool draining)
    {
      if (draining)
        this.batteryMonitor.AddOrUpdateModifier(this.powerDrainModifier);
      else
        this.batteryMonitor.RemoveModifier(this.powerDrainModifier.id);
    }

    public void FindDestination()
    {
      int idleCell = this.FindIdleCell();
      if (idleCell != -1 && idleCell != Grid.PosToCell(this.gameObject))
      {
        this.sm.targetMoveLocation.Set(idleCell, this.smi);
        this.GoTo((StateMachine.BaseState) this.sm.shocking.runAroundShockingStuff);
      }
      else
      {
        int minionTarget = this.FindMinionTarget();
        if (minionTarget != -1 && minionTarget != Grid.PosToCell(this.gameObject))
        {
          this.sm.targetMoveLocation.Set(minionTarget, this.smi);
          this.GoTo((StateMachine.BaseState) this.sm.shocking.runAroundShockingStuff);
        }
        else
        {
          this.sm.targetMoveLocation.Set(Grid.PosToCell(this.gameObject), this.smi);
          this.GoTo((StateMachine.BaseState) this.sm.shocking.standStillShockingStuff);
        }
      }
    }

    private int FindMinionTarget()
    {
      Navigator component = this.smi.gameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return Grid.InvalidCell;
      int num = int.MaxValue;
      int minionTarget = Grid.InvalidCell;
      List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.smi.gameObject.GetMyWorldId());
      for (int index = 0; index < worldItems.Count; ++index)
      {
        if (!worldItems[index].IsNullOrDestroyed() && !((UnityEngine.Object) worldItems[index].gameObject == (UnityEngine.Object) this.gameObject))
        {
          int cell = Grid.PosToCell((KMonoBehaviour) worldItems[index]);
          if (component.CanReach(cell))
          {
            int navigationCost = component.GetNavigationCost(cell);
            if (navigationCost < num)
            {
              num = navigationCost;
              minionTarget = cell;
            }
          }
        }
      }
      return minionTarget;
    }

    private int FindIdleCell()
    {
      Navigator component = this.smi.master.GetComponent<Navigator>();
      MinionPathFinderAbilities currentAbilities = (MinionPathFinderAbilities) component.GetCurrentAbilities();
      currentAbilities.SetIdleNavMaskEnabled(true);
      IdleCellQuery query = PathFinderQueries.idleCellQuery.Reset(this.GetComponent<MinionBrain>(), UnityEngine.Random.Range(90, 180));
      component.RunQuery((PathFinderQuery) query);
      if (query.GetResultCell() == Grid.PosToCell(this.gameObject))
      {
        query = PathFinderQueries.idleCellQuery.Reset(this.GetComponent<MinionBrain>(), UnityEngine.Random.Range(0, 90));
        component.RunQuery((PathFinderQuery) query);
      }
      currentAbilities.SetIdleNavMaskEnabled(false);
      return query.GetResultCell();
    }

    public void ShockUpdateRender(StressShockChore.StatesInstance smi, float dt)
    {
      if ((UnityEngine.Object) smi.sm.faceLightningFX.Get(smi) != (UnityEngine.Object) null)
        smi.sm.faceLightningFX.Get(smi).transform.SetPosition(smi.FaceOriginLocation());
      if (!((UnityEngine.Object) smi.sm.beamTarget.Get(smi) != (UnityEngine.Object) null))
        return;
      Vector3 vector3 = smi.sm.beamTarget.Get(smi).transform.position + Vector3.up / 2f;
      if ((UnityEngine.Object) smi.sm.beamFX.Get(smi) == (UnityEngine.Object) null)
        smi.MakeBeam();
      if (StressShockChore.CheckBlocked(Grid.PosToCell(smi.sm.beamFX.Get(smi).transform.position), Grid.PosToCell(vector3)))
        return;
      smi.AimBeam(vector3, 0);
    }

    public void ShockUpdate200(StressShockChore.StatesInstance smi, float dt)
    {
      float num1 = dt * TUNING.STRESS.SHOCKER.POWER_CONSUMPTION_RATE;
      double num2 = (double) smi.sm.powerConsumed.Delta(num1, smi);
      smi.batteryMonitor.ConsumePower(num1);
      if (!((UnityEngine.Object) smi.sm.beamTarget.Get(smi) != (UnityEngine.Object) null))
        return;
      Health component1 = smi.sm.beamTarget.Get(smi).GetComponent<Health>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.Damage(dt * TUNING.STRESS.SHOCKER.DAMAGE_RATE);
      }
      else
      {
        Electrobank component2 = smi.sm.beamTarget.Get(smi).GetComponent<Electrobank>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          component2.Damage(dt * TUNING.STRESS.SHOCKER.DAMAGE_RATE);
        }
        else
        {
          if (!smi.sm.beamTarget.Get(smi).HasTag(GameTags.Wires))
            return;
          BuildingHP component3 = smi.sm.beamTarget.Get(smi).GetComponent<BuildingHP>();
          if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
            return;
          component3.DoDamage(Mathf.RoundToInt(dt * TUNING.STRESS.SHOCKER.DAMAGE_RATE));
        }
      }
    }

    public void PickShockTarget(StressShockChore.StatesInstance smi)
    {
      int cell1 = Grid.PosToCell(smi.master.gameObject);
      int worldId = (int) Grid.WorldIdx[cell1];
      List<GameObject> tList = new List<GameObject>();
      float num1 = UnityEngine.Random.Range(0.0f, 2f);
      foreach (Health worldItem in Components.Health.GetWorldItems(worldId))
      {
        if (!worldItem.IsNullOrDestroyed() && !((UnityEngine.Object) worldItem.gameObject == (UnityEngine.Object) smi.master.gameObject))
        {
          int cell2 = Grid.PosToCell((KMonoBehaviour) worldItem);
          float num2 = Vector2.Distance((Vector2) Grid.CellToPos2D(cell1), (Vector2) Grid.CellToPos2D(cell2));
          if ((double) num2 <= (double) TUNING.STRESS.SHOCKER.SHOCK_RADIUS && (double) num2 > (double) num1 && !StressShockChore.CheckBlocked(cell1, cell2))
            tList.Add(worldItem.gameObject);
        }
      }
      if (tList.Count == 0)
      {
        Vector2I xy = Grid.CellToXY(cell1);
        List<ScenePartitionerEntry> gathered_entries = new List<ScenePartitionerEntry>();
        GameScenePartitioner.Instance.GatherEntries(xy.x - TUNING.STRESS.SHOCKER.SHOCK_RADIUS, xy.y - TUNING.STRESS.SHOCKER.SHOCK_RADIUS, TUNING.STRESS.SHOCKER.SHOCK_RADIUS * 2, TUNING.STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.completeBuildings, gathered_entries);
        foreach (ScenePartitionerEntry partitionerEntry in gathered_entries)
        {
          if (!StressShockChore.CheckBlocked(cell1, Grid.PosToCell(new Vector2((float) partitionerEntry.x, (float) partitionerEntry.y))))
          {
            BuildingComplete buildingComplete = partitionerEntry.obj as BuildingComplete;
            if ((UnityEngine.Object) buildingComplete != (UnityEngine.Object) null)
              tList.Add(buildingComplete.gameObject);
          }
        }
      }
      if (tList.Count == 0)
      {
        this.ClearBeam(false);
      }
      else
      {
        GameObject random = tList.GetRandom<GameObject>();
        GameObject gameObject1 = random;
        float num3 = float.MaxValue;
        foreach (GameObject gameObject2 in tList)
        {
          if (tList.Count <= 1 || !((UnityEngine.Object) gameObject2 == (UnityEngine.Object) this.sm.previousTarget.Get(smi)))
          {
            float num4 = Vector2.Distance((Vector2) this.transform.position, (Vector2) gameObject2.transform.position);
            if ((double) num4 < (double) num3)
            {
              num3 = num4;
              gameObject1 = gameObject2;
            }
          }
        }
        if ((UnityEngine.Object) random != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && UnityEngine.Random.Range(0, 100) > 50)
          this.sm.beamTarget.Set(gameObject1, smi);
        else
          this.sm.beamTarget.Set(gameObject1, smi);
      }
    }

    public void MakeBeam()
    {
      GameObject gameObject1 = new GameObject("shockFX");
      gameObject1.SetActive(false);
      KBatchedAnimController kbatchedAnimController1 = gameObject1.AddComponent<KBatchedAnimController>();
      this.sm.beamFX.Set(kbatchedAnimController1, this.smi);
      kbatchedAnimController1.SwapAnims(new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "bionic_dupe_stress_beam_fx_kanim")
      });
      gameObject1.SetActive(true);
      Vector3 vector3 = ((Vector3) this.GetComponent<KBatchedAnimController>().GetSymbolTransform((HashedString) "snapTo_hat", out bool _).GetColumn(3) - Vector3.up / 4f) with
      {
        z = this.transform.position.z + 0.01f
      };
      gameObject1.transform.position = vector3;
      kbatchedAnimController1.Play((HashedString) "beam1", KAnim.PlayMode.Loop);
      if ((UnityEngine.Object) this.sm.faceLightningFX.Get(this.smi) != (UnityEngine.Object) null)
      {
        Util.KDestroyGameObject(this.sm.faceLightningFX.Get(this.smi).gameObject);
        this.sm.faceLightningFX.Set((KBatchedAnimController) null, this.smi);
      }
      GameObject gameObject2 = new GameObject("faceLightningFX");
      gameObject2.SetActive(false);
      KBatchedAnimController kbatchedAnimController2 = gameObject2.AddComponent<KBatchedAnimController>();
      this.sm.faceLightningFX.Set(kbatchedAnimController2, this.smi);
      kbatchedAnimController2.SwapAnims(new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "bionic_dupe_stress_lightning_fx_kanim")
      });
      gameObject2.SetActive(true);
      gameObject2.transform.position = this.FaceOriginLocation();
      kbatchedAnimController2.Play((HashedString) "lightning", KAnim.PlayMode.Loop);
      GameObject gameObject3 = new GameObject("impactFX");
      gameObject3.SetActive(false);
      KBatchedAnimController kbatchedAnimController3 = gameObject3.AddComponent<KBatchedAnimController>();
      this.sm.impactFX.Set(kbatchedAnimController3, this.smi);
      kbatchedAnimController3.SwapAnims(new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "bionic_dupe_stress_beam_impact_fx_kanim")
      });
      gameObject3.SetActive(true);
      kbatchedAnimController3.Play((HashedString) "stress_beam_impact_fx", KAnim.PlayMode.Loop);
    }

    public Vector3 FaceOriginLocation()
    {
      return ((Vector3) this.GetComponent<KBatchedAnimController>().GetSymbolTransform((HashedString) "snapTo_hat", out bool _).GetColumn(3) - Vector3.up / 4f) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.FXFront)
      };
    }

    public void ClearBeam(bool clearFaceFX = true)
    {
      this.sm.previousTarget.Set(this.sm.beamTarget.Get(this.smi), this.smi);
      this.sm.beamTarget.Set((GameObject) null, this.smi);
      if ((UnityEngine.Object) this.sm.beamFX.Get(this.smi) != (UnityEngine.Object) null)
      {
        Util.KDestroyGameObject(this.sm.beamFX.Get(this.smi).gameObject);
        this.sm.beamFX.Set((KBatchedAnimController) null, this.smi);
      }
      if ((UnityEngine.Object) this.sm.impactFX.Get(this.smi) != (UnityEngine.Object) null)
      {
        Util.KDestroyGameObject(this.sm.impactFX.Get(this.smi).gameObject);
        this.sm.impactFX.Set((KBatchedAnimController) null, this.smi);
      }
      if (!clearFaceFX || !((UnityEngine.Object) this.sm.faceLightningFX.Get(this.smi) != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject(this.sm.faceLightningFX.Get(this.smi).gameObject);
      this.sm.faceLightningFX.Set((KBatchedAnimController) null, this.smi);
    }

    public void AimBeam(Vector3 targetPosition, int beamIdx)
    {
      Vector3 position = this.FaceOriginLocation() with
      {
        z = this.transform.position.z + 0.01f
      };
      this.smi.sm.beamFX.Get(this.smi).transform.SetPosition(position);
      float num1 = MathUtil.AngleSigned(Vector3.up, Vector3.Normalize(targetPosition - this.smi.sm.beamFX.Get(this.smi).transform.position), Vector3.forward) + 90f;
      this.smi.sm.beamFX.Get(this.smi).Rotation = num1;
      this.smi.sm.impactFX.Get(this.smi).transform.position = targetPosition;
      this.smi.sm.faceLightningFX.Get(this.smi).FlipX = (double) targetPosition.x < (double) this.smi.sm.faceLightningFX.Get(this.smi).transform.position.x;
      float num2 = Vector3.Distance(this.smi.sm.beamFX.Get(this.smi).transform.position with
      {
        z = 0.0f
      }, targetPosition with { z = 0.0f });
      if ((double) num2 > 3.0)
      {
        if (this.smi.sm.beamFX.Get(this.smi).CurrentAnim == null || this.smi.sm.beamFX.Get(this.smi).CurrentAnim.name != "beam3")
          this.smi.sm.beamFX.Get(this.smi).Play((HashedString) "beam3", KAnim.PlayMode.Loop);
        this.smi.sm.beamFX.Get(this.smi).animWidth = num2 / 3f;
      }
      else if ((double) num2 > 2.0)
      {
        if (this.smi.sm.beamFX.Get(this.smi).CurrentAnim == null || this.smi.sm.beamFX.Get(this.smi).CurrentAnim.name != "beam2")
          this.smi.sm.beamFX.Get(this.smi).Play((HashedString) "beam2", KAnim.PlayMode.Loop);
        this.smi.sm.beamFX.Get(this.smi).animWidth = num2 / 2f;
      }
      else
      {
        if (this.smi.sm.beamFX.Get(this.smi).CurrentAnim == null || this.smi.sm.beamFX.Get(this.smi).CurrentAnim.name != "beam1")
          this.smi.sm.beamFX.Get(this.smi).Play((HashedString) "beam1", KAnim.PlayMode.Loop);
        this.smi.sm.beamFX.Get(this.smi).animWidth = num2;
      }
    }

    public void ShowBeam(bool show)
    {
      if ((UnityEngine.Object) this.smi.sm.impactFX.Get(this.smi) != (UnityEngine.Object) null)
        this.smi.sm.impactFX.Get(this.smi).enabled = show;
      if (!((UnityEngine.Object) this.smi.sm.beamFX.Get(this.smi) != (UnityEngine.Object) null))
        return;
      this.smi.sm.beamFX.Get(this.smi).enabled = show;
    }
  }

  public class States : 
    GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore>
  {
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.TargetParameter shocker;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController[]> cosmeticBeamFXs;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> beamFX;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> impactFX;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> faceLightningFX;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> beamTarget;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> previousTarget;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.IntParameter targetMoveLocation;
    public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.FloatParameter powerConsumed;
    public StressShockChore.States.ShockStates shocking;
    public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State delay;
    public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State complete;
    public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State offline;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.shocking.findDestination;
      this.serializable = StateMachine.SerializeType.Never;
      this.Target(this.shocker);
      this.shocking.EventTransition(GameHashes.BionicOffline, this.offline).DefaultState(this.shocking.findDestination).ToggleAnims("anim_loco_stressshocker_kanim").ParamTransition<float>((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.Parameter<float>) this.powerConsumed, this.complete, (StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.Parameter<float>.Callback) ((smi, p) => (double) p >= (double) TUNING.STRESS.SHOCKER.MAX_POWER_USE)).Enter((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi => smi.MakeBeam())).Exit((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi => smi.ClearBeam()));
      this.shocking.findDestination.Enter("FindDestination", (StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi =>
      {
        smi.ShowBeam(false);
        smi.FindDestination();
      })).Update((Action<StressShockChore.StatesInstance, float>) ((smi, dt) =>
      {
        float delta_value = dt * TUNING.STRESS.SHOCKER.FAKE_POWER_CONSUMPTION_RATE;
        double num = (double) smi.sm.powerConsumed.Delta(delta_value, smi);
        smi.FindDestination();
      }), UpdateRate.SIM_1000ms);
      this.shocking.runAroundShockingStuff.MoveTo((Func<StressShockChore.StatesInstance, int>) (smi => smi.sm.targetMoveLocation.Get(smi)), this.shocking.findDestination, this.delay).Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi => smi.ShowBeam(true))).Update((Action<StressShockChore.StatesInstance, float>) ((smi, dt) =>
      {
        smi.PickShockTarget(smi);
        smi.ShockUpdate200(smi, dt);
      })).Update((Action<StressShockChore.StatesInstance, float>) ((smi, dt) => smi.ShockUpdateRender(smi, dt)), UpdateRate.RENDER_EVERY_TICK);
      this.shocking.standStillShockingStuff.Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi => smi.ShowBeam(true))).PlayAnim("interrupt_shocker", KAnim.PlayMode.Loop).ScheduleGoTo(2f, (StateMachine.BaseState) this.delay).Update((Action<StressShockChore.StatesInstance, float>) ((smi, dt) =>
      {
        smi.PickShockTarget(smi);
        smi.ShockUpdate200(smi, dt);
      })).Update((Action<StressShockChore.StatesInstance, float>) ((smi, dt) => smi.ShockUpdateRender(smi, dt)), UpdateRate.RENDER_EVERY_TICK);
      this.delay.ScheduleGoTo(0.5f, (StateMachine.BaseState) this.shocking);
      this.complete.Enter((StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback) (smi => smi.StopSM("complete")));
      this.offline.Enter(new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.ForceStressMonitorToTimeOut)).ReturnSuccess();
    }

    public class ShockStates : 
      GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State
    {
      public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State findDestination;
      public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State runAroundShockingStuff;
      public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State standStillShockingStuff;
    }
  }
}
