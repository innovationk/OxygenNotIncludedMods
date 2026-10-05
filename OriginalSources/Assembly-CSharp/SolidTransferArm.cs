// Decompiled with JetBrains decompiler
// Type: SolidTransferArm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : 
  StateMachineComponent<SolidTransferArm.SMInstance>,
  ISim1000ms,
  IRenderEveryTick
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private KPrefabID kPrefabID;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private Rotatable rotatable;
  [MyCmpAdd]
  private StandardWorker worker;
  [MyCmpAdd]
  private ChoreConsumer choreConsumer;
  [MyCmpAdd]
  private ChoreDriver choreDriver;
  public int pickupRange = 4;
  private float max_carry_weight = 1000f;
  private List<Pickupable> pickupables = new List<Pickupable>();
  private KBatchedAnimController arm_anim_ctrl;
  private GameObject arm_go;
  private LoopingSounds looping_sounds;
  private bool rotateSoundPlaying;
  private string rotateSoundName = "TransferArm_rotate";
  private EventReference rotateSound;
  private KAnimLink link;
  private float arm_rot = 45f;
  private float turn_rate = 360f;
  private bool rotation_complete;
  private int gameCell;
  private SolidTransferArm.ArmAnim arm_anim;
  private HashSet<int> reachableCells = new HashSet<int>();
  private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>((Action<SolidTransferArm, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>((Action<SolidTransferArm, object>) ((component, data) => component.OnEndChore(data)));
  private static Func<object, SolidTransferArm, Util.IterationInstruction> AsyncUpdateVisitor = (Func<object, SolidTransferArm, Util.IterationInstruction>) ((obj, arm) =>
  {
    Pickupable pickupable = obj as Pickupable;
    if (Grid.GetCellRange(arm.gameCell, pickupable.cachedCell) <= arm.pickupRange && arm.IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) && pickupable.CouldBePickedUpByTransferArm(arm.kPrefabID.InstanceID))
      arm.pickupables.Add(pickupable);
    return Util.IterationInstruction.Continue;
  });
  private static HashedString HASH_ROTATION = (HashedString) "rotation";

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreConsumer.AddProvider((ChoreProvider) GlobalChoreProvider.Instance);
    this.choreConsumer.SetReach(this.pickupRange);
    Klei.AI.Attributes attributes = this.GetAttributes();
    if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
      attributes.Add(Db.Get().Attributes.CarryAmount);
    AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, this.gameObject.GetProperName());
    this.GetAttributes().Add(modifier);
    this.worker.usesMultiTool = false;
    this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
    this.simRenderLoadBalance = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    string name = component.name + ".arm";
    this.arm_go = new GameObject(name);
    this.arm_go.SetActive(false);
    this.arm_go.transform.parent = component.transform;
    this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
    this.rotateSound = RuntimeManager.PathToEventReference(GlobalAssets.GetSound(this.rotateSoundName));
    this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
    this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
    this.arm_anim_ctrl.AnimFiles = new KAnimFile[1]
    {
      component.AnimFiles[0]
    };
    this.arm_anim_ctrl.initialAnim = "arm";
    this.arm_anim_ctrl.isMovable = true;
    this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
    component.SetSymbolVisiblity((KAnimHashedString) "arm_target", false);
    this.arm_go.transform.SetPosition((Vector3) component.GetSymbolTransform(new HashedString("arm_target"), out bool _).GetColumn(3) with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm)
    });
    this.arm_go.SetActive(true);
    this.gameCell = Grid.PosToCell(this.arm_go);
    this.link = new KAnimLink((KAnimControllerBase) component, (KAnimControllerBase) this.arm_anim_ctrl);
    ChoreGroups choreGroups = Db.Get().ChoreGroups;
    for (int idx = 0; idx < choreGroups.Count; ++idx)
      this.choreConsumer.SetPermittedByUser(choreGroups[idx], true);
    this.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
    this.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
    this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0.0f);
    this.DropLeftovers();
    component.enabled = false;
    component.enabled = true;
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    MinionGroupProber.Get().Vacate(this.reachableCells.ToList<int>());
    base.OnCleanUp();
  }

  public static void BatchUpdate(
    List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms,
    float time_delta)
  {
    SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Reset(solid_transfer_arms);
    GlobalJobManager.Run((IWorkItemCollection) SolidTransferArm.SolidTransferArmBatchUpdater.Instance);
    SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Finish();
  }

  private void Sim()
  {
    Chore.Precondition.Context out_context = new Chore.Precondition.Context();
    if (this.choreConsumer.FindNextChore(ref out_context))
    {
      if (out_context.chore is FetchChore)
      {
        this.choreDriver.SetChore(out_context);
        this.storage.DropUnlessMatching(out_context.chore as FetchChore);
        this.arm_anim_ctrl.enabled = false;
        this.arm_anim_ctrl.enabled = true;
      }
      else
        Debug.Assert(false, (object) ("I am but a lowly transfer arm. I should only acquire FetchChores: " + out_context.chore?.ToString()));
    }
    this.operational.SetActive(this.choreDriver.HasChore());
  }

  public void Sim1000ms(float dt)
  {
  }

  private void UpdateArmAnim()
  {
    FetchAreaChore currentChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
    if ((bool) (UnityEngine.Object) this.worker.GetWorkable() && currentChore != null && this.rotation_complete)
    {
      this.StopRotateSound();
      this.SetArmAnim(currentChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
    }
    else
      this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
  }

  private bool AsyncUpdate()
  {
    int x;
    int y;
    Grid.CellToXY(this.gameCell, out x, out y);
    bool flag = false;
    for (int index1 = y - this.pickupRange; index1 < y + this.pickupRange + 1; ++index1)
    {
      for (int index2 = x - this.pickupRange; index2 < x + this.pickupRange + 1; ++index2)
      {
        int cell = Grid.XYToCell(index2, index1);
        if (Grid.IsValidCell(cell) && Grid.IsPhysicallyAccessible(x, y, index2, index1, true) != this.reachableCells.Contains(cell))
          flag = true;
      }
    }
    if (flag)
    {
      ListPool<int, SolidTransferArm>.PooledList cells1 = ListPool<int, SolidTransferArm>.Allocate();
      ListPool<int, SolidTransferArm>.PooledList cells2 = ListPool<int, SolidTransferArm>.Allocate();
      cells1.AddRange((IEnumerable<int>) this.reachableCells);
      this.reachableCells.Clear();
      for (int index3 = y - this.pickupRange; index3 < y + this.pickupRange + 1; ++index3)
      {
        for (int index4 = x - this.pickupRange; index4 < x + this.pickupRange + 1; ++index4)
        {
          int cell = Grid.XYToCell(index4, index3);
          if (Grid.IsValidCell(cell) && Grid.IsPhysicallyAccessible(x, y, index4, index3, true))
          {
            this.reachableCells.Add(cell);
            cells2.Add(cell);
          }
        }
      }
      MinionGroupProber.Get().Occupy((List<int>) cells2);
      MinionGroupProber.Get().Vacate((List<int>) cells1);
      cells2.Recycle();
      cells1.Recycle();
    }
    this.pickupables.Clear();
    GameScenePartitioner.Instance.ReadonlyVisitEntries<SolidTransferArm>(x - this.pickupRange, y - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.pickupablesLayer, SolidTransferArm.AsyncUpdateVisitor, this);
    GameScenePartitioner.Instance.ReadonlyVisitEntries<SolidTransferArm>(x - this.pickupRange, y - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.storedPickupablesLayer, SolidTransferArm.AsyncUpdateVisitor, this);
    return flag;
  }

  public bool IsCellReachable(int cell) => this.reachableCells.Contains(cell);

  private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
  {
    return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
  }

  public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
  {
    return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
  }

  public void RenderEveryTick(float dt)
  {
    if ((bool) (UnityEngine.Object) this.worker.GetWorkable())
      this.RotateArm(Vector3.Normalize(this.worker.GetWorkable().GetTargetPoint() with
      {
        z = 0.0f
      } - this.transform.GetPosition() with { z = 0.0f }), false, dt);
    this.UpdateArmAnim();
  }

  private void OnEndChore(object data) => this.DropLeftovers();

  private void DropLeftovers()
  {
    if (this.storage.IsEmpty() || this.choreDriver.HasChore())
      return;
    this.storage.DropAll();
  }

  private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
  {
    if (new_anim == this.arm_anim)
      return;
    this.arm_anim = new_anim;
    switch (this.arm_anim)
    {
      case SolidTransferArm.ArmAnim.Idle:
        this.arm_anim_ctrl.Play((HashedString) "arm", KAnim.PlayMode.Loop);
        break;
      case SolidTransferArm.ArmAnim.Pickup:
        this.arm_anim_ctrl.Play((HashedString) "arm_pickup", KAnim.PlayMode.Loop);
        break;
      case SolidTransferArm.ArmAnim.Drop:
        this.arm_anim_ctrl.Play((HashedString) "arm_drop", KAnim.PlayMode.Loop);
        break;
    }
  }

  private void OnOperationalChanged(object data)
  {
    if (((Boxed<bool>) data).value)
      return;
    if (this.choreDriver.HasChore())
      this.choreDriver.StopChore();
    this.UpdateArmAnim();
  }

  private void SetArmRotation(float rot)
  {
    this.arm_rot = rot;
    this.arm_go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.arm_rot);
  }

  private void RotateArm(Vector3 target_dir, bool warp, float dt)
  {
    float a = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
    if ((double) a < -180.0)
      a += 360f;
    if ((double) a > 180.0)
      a -= 360f;
    if (!warp)
      a = Mathf.Clamp(a, -this.turn_rate * dt, this.turn_rate * dt);
    this.arm_rot += a;
    this.SetArmRotation(this.arm_rot);
    this.rotation_complete = Mathf.Approximately(a, 0.0f);
    if (!warp && !this.rotation_complete)
    {
      if (!this.rotateSoundPlaying)
        this.StartRotateSound();
      this.SetRotateSoundParameter(this.arm_rot);
    }
    else
      this.StopRotateSound();
  }

  private void StartRotateSound()
  {
    if (this.rotateSoundPlaying)
      return;
    this.looping_sounds.StartSound(this.rotateSound);
    this.rotateSoundPlaying = true;
  }

  private void SetRotateSoundParameter(float arm_rot)
  {
    if (!this.rotateSoundPlaying)
      return;
    this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
  }

  private void StopRotateSound()
  {
    if (!this.rotateSoundPlaying)
      return;
    this.looping_sounds.StopSound(this.rotateSound);
    this.rotateSoundPlaying = false;
  }

  private enum ArmAnim
  {
    Idle,
    Pickup,
    Drop,
  }

  public class SMInstance(SolidTransferArm master) : 
    GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
  {
    public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;
    public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;
    public SolidTransferArm.States.ReadyStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.DoNothing();
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State) this.on, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).Enter((StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State.Callback) (smi => smi.master.StopRotateSound()));
      this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
    }

    public class ReadyStates : 
      GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
    {
      public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;
      public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
    }
  }

  private class SolidTransferArmBatchUpdater : 
    WorkItemCollection<List<UpdateBucketWithUpdater<ISim1000ms>.Entry>>
  {
    private static readonly List<UpdateBucketWithUpdater<ISim1000ms>.Entry> EmptyList = new List<UpdateBucketWithUpdater<ISim1000ms>.Entry>();
    private const int kBatchSize = 8;
    private static SolidTransferArm.SolidTransferArmBatchUpdater instance;

    public static SolidTransferArm.SolidTransferArmBatchUpdater Instance
    {
      get
      {
        if (SolidTransferArm.SolidTransferArmBatchUpdater.instance == null)
          SolidTransferArm.SolidTransferArmBatchUpdater.instance = new SolidTransferArm.SolidTransferArmBatchUpdater();
        return SolidTransferArm.SolidTransferArmBatchUpdater.instance;
      }
    }

    public void Reset(
      List<UpdateBucketWithUpdater<ISim1000ms>.Entry> entries)
    {
      this.sharedData = entries;
      this.count = (entries.Count + 8 - 1) / 8;
    }

    public override void RunItem(
      int item,
      ref List<UpdateBucketWithUpdater<ISim1000ms>.Entry> shared_data,
      int threadIndex)
    {
      int num1 = item * 8;
      int num2 = Math.Min(shared_data.Count, num1 + 8);
      for (int index = num1; index < num2; ++index)
      {
        SolidTransferArm data = (SolidTransferArm) shared_data[index].data;
        if (data.operational.IsOperational)
          data.AsyncUpdate();
      }
    }

    public void Finish()
    {
      foreach (UpdateBucketWithUpdater<ISim1000ms>.Entry entry in this.sharedData)
      {
        SolidTransferArm data = (SolidTransferArm) entry.data;
        if (data.operational.IsOperational)
          data.Sim();
      }
      this.Reset(SolidTransferArm.SolidTransferArmBatchUpdater.EmptyList);
    }
  }

  public struct CachedPickupable
  {
    public Pickupable pickupable;
    public int storage_cell;
  }
}
