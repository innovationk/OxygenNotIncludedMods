// Decompiled with JetBrains decompiler
// Type: GasAndLiquidConsumerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class GasAndLiquidConsumerMonitor : 
  GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.cooldown.Enter("ClearTargetCell", (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State.Callback) (smi => smi.ClearTargetCell())).ScheduleGoTo((Func<GasAndLiquidConsumerMonitor.Instance, float>) (smi => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown)), (StateMachine.BaseState) this.satisfied);
    this.satisfied.Enter("ClearTargetCell", (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State.Callback) (smi => smi.ClearTargetCell())).TagTransition((Func<GasAndLiquidConsumerMonitor.Instance, Tag[]>) (smi => smi.def.transitionTag), this.looking);
    this.looking.ToggleBehaviour((Func<GasAndLiquidConsumerMonitor.Instance, Tag>) (smi => smi.def.behaviourTag), (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.Transition.ConditionCallback) (smi => smi.targetCell != -1), (System.Action<GasAndLiquidConsumerMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown))).TagTransition((Func<GasAndLiquidConsumerMonitor.Instance, Tag[]>) (smi => smi.def.transitionTag), this.satisfied, true).PreBrainUpdate((System.Action<GasAndLiquidConsumerMonitor.Instance>) (smi => smi.FindElement()));
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag[] transitionTag = new Tag[1]
    {
      GameTags.Creatures.Hungry
    };
    public Tag behaviourTag = GameTags.Creatures.WantsToEat;
    public float minCooldown = 5f;
    public float maxCooldown = 5f;
    public Diet diet;
    public float consumptionRate = 0.5f;
    public Tag consumableElementTag = Tag.Invalid;
  }

  public new class Instance : 
    GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
  {
    public int targetCell = -1;
    private Element targetElement;
    private Navigator navigator;
    private int massUnavailableFrameCount;
    [MyCmpGet]
    private Storage storage;
    private static System.Action<Sim.MassConsumedCallback, object> OnMassConsumedAction = new System.Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback);

    public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def)
      : base(master, def)
    {
      this.navigator = this.smi.GetComponent<Navigator>();
      DebugUtil.Assert(this.smi.def.diet != null || (UnityEngine.Object) this.storage != (UnityEngine.Object) null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
    }

    public void ClearTargetCell()
    {
      this.targetCell = -1;
      this.massUnavailableFrameCount = 0;
    }

    public void FindElement()
    {
      this.targetCell = -1;
      this.FindTargetCell();
    }

    public Element GetTargetElement() => this.targetElement;

    public bool IsConsumableCell(int cell, out Element element)
    {
      element = Grid.Element[cell];
      bool flag1 = true;
      bool flag2 = true;
      if (this.smi.def.consumableElementTag != Tag.Invalid)
        flag1 = element.HasTag(this.smi.def.consumableElementTag);
      if (this.smi.def.diet != null)
      {
        flag2 = false;
        foreach (Diet.Info info in this.smi.def.diet.infos)
        {
          if (info.IsMatch(element.tag))
          {
            flag2 = true;
            break;
          }
        }
      }
      return flag1 & flag2;
    }

    public void FindTargetCell()
    {
      GasAndLiquidConsumerMonitor.ConsumableCellQuery query = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(this.smi, 25);
      this.navigator.RunQuery((PathFinderQuery) query);
      if (!query.success)
        return;
      this.targetCell = query.GetResultCell();
      this.targetElement = query.targetElement;
    }

    public void Consume(float dt)
    {
      int index = Game.Instance.massConsumedCallbackManager.Add(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedAction, (object) this, nameof (GasAndLiquidConsumerMonitor)).index;
      SimMessages.ConsumeMass(Grid.PosToCell((StateMachine.Instance) this), this.targetElement.id, this.def.consumptionRate * dt, (byte) 3, index);
    }

    private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
    {
      ((GasAndLiquidConsumerMonitor.Instance) data).OnMassConsumed(mcd);
    }

    private void OnMassConsumed(Sim.MassConsumedCallback mcd)
    {
      if (!this.IsRunning())
        return;
      if ((double) mcd.mass > 0.0)
      {
        if (this.def.diet != null)
        {
          this.massUnavailableFrameCount = 0;
          Diet.Info dietInfo = this.def.diet.GetDietInfo(this.targetElement.tag);
          if (dietInfo == null)
            return;
          float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
          this.BoxingTrigger<CreatureCalorieMonitor.CaloriesConsumedEvent>(-2038961714, new CreatureCalorieMonitor.CaloriesConsumedEvent()
          {
            tag = this.targetElement.tag,
            calories = calories
          });
        }
        else
        {
          if (!((UnityEngine.Object) this.storage != (UnityEngine.Object) null))
            return;
          this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount);
        }
      }
      else
      {
        ++this.massUnavailableFrameCount;
        if (this.massUnavailableFrameCount < 2)
          return;
        this.Trigger(801383139);
      }
    }
  }

  public class ConsumableCellQuery : PathFinderQuery
  {
    public bool success;
    public Element targetElement;
    private GasAndLiquidConsumerMonitor.Instance smi;
    private int maxIterations;

    public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
    {
      this.smi = smi;
      this.maxIterations = maxIterations;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      int cell1 = Grid.CellAbove(cell);
      this.success = this.smi.IsConsumableCell(cell, out this.targetElement) || Grid.IsValidCell(cell1) && this.smi.IsConsumableCell(cell1, out this.targetElement);
      return this.success || --this.maxIterations <= 0;
    }
  }
}
