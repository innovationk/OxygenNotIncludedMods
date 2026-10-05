// Decompiled with JetBrains decompiler
// Type: VentBubbleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class VentBubbleStates : 
  GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>
{
  private const float INFLATED_DURATION = 30f;
  private const int INFLATE_POSITION_MIN_TILES_ABOVE_FLOOR = 3;
  public static StatusItem InflatedStatus = new StatusItem(nameof (InflatedStatus), (string) CREATURES.STATUSITEMS.PUFFER_INFLATED.NAME, (string) CREATURES.STATUSITEMS.PUFFER_INFLATED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  public static StatusItem VentingStatus = new StatusItem(nameof (VentingStatus), (string) CREATURES.STATUSITEMS.PUFFER_VENTING.NAME, (string) CREATURES.STATUSITEMS.PUFFER_VENTING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  public static StatusItem SharingAirStatus = new StatusItem(nameof (SharingAirStatus), (string) CREATURES.STATUSITEMS.PUFFER_SHARING_AIR.NAME, (string) CREATURES.STATUSITEMS.PUFFER_SHARING_AIR.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  public static StatusItem DupeConsumingAirStatus = new StatusItem(nameof (SharingAirStatus), (string) DUPLICANTS.STATUSITEMS.PUFFER_SHARING_AIR.NAME, (string) DUPLICANTS.STATUSITEMS.PUFFER_SHARING_AIR.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State position_above_floor;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State full;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State venting;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State venting_pst;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State empty;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State dupe_venting;
  public GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State dupe_venting_pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.position_above_floor;
    this.position_above_floor.MoveTo(new Func<VentBubbleStates.Instance, int>(VentBubbleStates.GetCellAboveFloor), this.full, this.full);
    this.full.Enter(new StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State.Callback(VentBubbleStates.DrainStomachToStorage)).Enter(new StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State.Callback(VentBubbleStates.EnableBreathingLocation)).PlayAnim("full_pre", KAnim.PlayMode.Once).QueueAnim("full_loop", true).ToggleMainStatusItem(VentBubbleStates.InflatedStatus).ScheduleGoTo(30f, (StateMachine.BaseState) this.venting).WorkableStartTransition((Func<VentBubbleStates.Instance, Workable>) (smi => this.GetWorkable(smi)), this.dupe_venting);
    this.dupe_venting.ToggleMainStatusItem(VentBubbleStates.SharingAirStatus).Update((System.Action<VentBubbleStates.Instance, float>) ((smi, dt) => smi.EmitBubble(smi.def.emitMass * 0.25f, 0.75f)), UpdateRate.SIM_1000ms).WorkableStopTransition((Func<VentBubbleStates.Instance, Workable>) (smi => this.GetWorkable(smi)), this.dupe_venting_pst).Exit(new StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State.Callback(VentBubbleStates.DisableBreathingLocation));
    this.dupe_venting_pst.Enter((StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State.Callback) (smi =>
    {
      if (smi.GetComponent<Storage>().IsEmpty())
        smi.GoTo((StateMachine.BaseState) this.empty);
      else
        smi.GoTo((StateMachine.BaseState) this.full);
    }));
    this.venting.PlayAnim("deflate_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.State.Callback(VentBubbleStates.DisableBreathingLocation)).ToggleMainStatusItem(VentBubbleStates.VentingStatus).Update(new System.Action<VentBubbleStates.Instance, float>(VentBubbleStates.EmitBubble), UpdateRate.SIM_1000ms).Transition(this.venting_pst, GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.Not(new StateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.Transition.ConditionCallback(VentBubbleStates.HasStoredElement)), UpdateRate.SIM_1000ms);
    this.venting_pst.PlayAnim("full_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.empty);
    this.empty.BehaviourComplete(GameTags.Creatures.Poop);
  }

  private static int GetCellAboveFloor(VentBubbleStates.Instance smi)
  {
    int cell1 = Grid.PosToCell(smi.transform.GetPosition());
    if (!Grid.IsValidCell(cell1))
      return Grid.InvalidCell;
    int x;
    int y;
    Grid.CellToXY(cell1, out x, out y);
    for (int index1 = 0; index1 <= 8; ++index1)
    {
      for (int index2 = -1; index2 <= 1; index2 += 2)
      {
        if (index1 != 0 || index2 != -1)
        {
          int cell2 = Grid.XYToCell(x + index1 * index2, y);
          if (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
          {
            int num1 = cell2;
            int num2;
            for (num2 = 0; Grid.IsValidCell(num1) && !Grid.Solid[num1] && num2 <= 32 /*0x20*/; ++num2)
              num1 = Grid.CellBelow(num1);
            if (Grid.IsValidCell(num1) && Grid.Solid[num1])
            {
              if (num2 >= 3)
                return VentBubbleStates.AvoidLiquidSurface(cell2);
              int num3 = num1;
              bool flag = false;
              for (int index3 = 0; index3 < 3; ++index3)
              {
                num3 = Grid.CellAbove(num3);
                if (!Grid.IsValidCell(num3) || Grid.Solid[num3])
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
                return VentBubbleStates.AvoidLiquidSurface(num3);
            }
          }
        }
      }
    }
    return cell1;
  }

  private static int AvoidLiquidSurface(int cell)
  {
    int cell1 = Grid.CellAbove(cell);
    if (Grid.IsValidCell(cell1) && Grid.Element[cell].IsLiquid && !Grid.Element[cell1].IsLiquid)
    {
      int num = Grid.CellBelow(cell);
      if (Grid.IsValidCell(num) && !Grid.Solid[num])
        return num;
    }
    return cell;
  }

  private static bool HasStoredElement(VentBubbleStates.Instance smi) => smi.HasStoredElement();

  private static void EmitBubble(VentBubbleStates.Instance smi, float dt)
  {
    smi.EmitBubble(smi.def.emitMass);
  }

  private static void DrainStomachToStorage(VentBubbleStates.Instance smi)
  {
    smi.DrainStomachToStorage();
  }

  private Workable GetWorkable(VentBubbleStates.Instance smi)
  {
    UnderwaterBreathingLocationWorkable locationWorkable = smi.Get<UnderwaterBreathingLocationWorkable>();
    locationWorkable.overrideAnims = smi.def.dupebreathingAnimFiles;
    locationWorkable.workAnims = smi.def.dupebreathingAnims;
    locationWorkable.workingPstComplete = smi.def.dupebreathingPst;
    locationWorkable.workingPstFailed = smi.def.dupebreathingPst;
    locationWorkable.synchronizeAnims = true;
    locationWorkable.workLayer = Grid.SceneLayer.Move;
    locationWorkable.SetWorkerStatusItem(VentBubbleStates.DupeConsumingAirStatus);
    return (Workable) smi.Get<UnderwaterBreathingLocationWorkable>();
  }

  private static void EnableBreathingLocation(VentBubbleStates.Instance smi)
  {
    UnderwaterBreathingLocation component = smi.GetComponent<UnderwaterBreathingLocation>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.MarkCells();
  }

  private static void DisableBreathingLocation(VentBubbleStates.Instance smi)
  {
    UnderwaterBreathingLocation component = smi.GetComponent<UnderwaterBreathingLocation>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UnmarkCells();
  }

  public class Def : StateMachine.BaseDef
  {
    public SimHashes element;
    public float emitMass = 1f;
    public KAnimFile[] dupebreathingAnimFiles;
    public HashedString[] dupebreathingAnims;
    public HashedString[] dupebreathingPst;
  }

  public new class Instance : 
    GameStateMachine<VentBubbleStates, VentBubbleStates.Instance, IStateMachineTarget, VentBubbleStates.Def>.GameInstance
  {
    [MyCmpGet]
    private Storage storage;
    private Tag elementTag;
    private static Chore.Precondition ShouldInflate = new Chore.Precondition()
    {
      id = nameof (ShouldInflate),
      description = "__ Blowter has no oxygen",
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && ((double) context.consumerState.gameObject.GetComponent<Storage>().MassStored() > 0.0 || context.consumerState.consumer.RunBehaviourPrecondition(GameTags.Creatures.Poop)))
    };

    public Instance(Chore<VentBubbleStates.Instance> chore, VentBubbleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(VentBubbleStates.Instance.ShouldInflate, (object) null);
      this.elementTag = ElementLoader.FindElementByHash(def.element).tag;
    }

    protected override void OnCleanUp()
    {
      VentBubbleStates.DisableBreathingLocation(this);
      base.OnCleanUp();
    }

    public void DrainStomachToStorage()
    {
      if (this.gameObject.GetSMI<CreatureCalorieMonitor.Instance>() == null)
        return;
      this.gameObject.Trigger(-667597687, (object) new PoopData(false, this.storage));
    }

    public bool HasStoredElement()
    {
      GameObject first = this.storage.FindFirst(this.elementTag);
      return !((UnityEngine.Object) first == (UnityEngine.Object) null) && (double) first.GetComponent<PrimaryElement>().Mass > 0.0;
    }

    public void EmitBubble(float min_mass, float y_offset = 0.0f)
    {
      GameObject first = this.storage.FindFirst(this.elementTag);
      if ((UnityEngine.Object) first == (UnityEngine.Object) null)
        return;
      PrimaryElement component1 = first.GetComponent<PrimaryElement>();
      float mass1 = component1.Mass;
      if ((double) mass1 <= 0.0)
        return;
      float mass2 = Mathf.Min(mass1, min_mass);
      component1.Mass -= mass2;
      Vector3 position = this.master.transform.GetPosition();
      position.y += 0.75f;
      Facing component2 = this.GetComponent<Facing>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        position.x += component2.GetFacing() ? -0.45f : 0.45f;
      position.y += y_offset;
      BubbleManager.instance.SpawnBubble(this.def.element, (Vector2) position, mass2, component1.Temperature, BubbleManager.Disease.None);
    }
  }
}
