// Decompiled with JetBrains decompiler
// Type: SegmentedCreature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SegmentedCreature : 
  GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>
{
  public SegmentedCreature.RectractStates retracted;
  public SegmentedCreature.FreeMovementStates freeMovement;
  private StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.BoolParameter isRetracted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.freeMovement.idle;
    this.root.Enter(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
    this.retracted.DefaultState(this.retracted.pre).Enter((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback) (smi => this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop))).Exit(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
    this.retracted.pre.Update(new System.Action<SegmentedCreature.Instance, float>(this.UpdateRetractedPre), UpdateRate.SIM_EVERY_TICK);
    this.retracted.loop.ParamTransition<bool>((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Parameter<bool>) this.isRetracted, (GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State) this.freeMovement, (StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Parameter<bool>.Callback) ((smi, p) => !this.isRetracted.Get(smi))).Update(new System.Action<SegmentedCreature.Instance, float>(this.UpdateRetractedLoop), UpdateRate.SIM_EVERY_TICK);
    this.freeMovement.DefaultState(this.freeMovement.idle).ParamTransition<bool>((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Parameter<bool>) this.isRetracted, (GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State) this.retracted, (StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Parameter<bool>.Callback) ((smi, p) => this.isRetracted.Get(smi))).Update(new System.Action<SegmentedCreature.Instance, float>(this.UpdateFreeMovement), UpdateRate.SIM_EVERY_TICK);
    this.freeMovement.idle.Transition(this.freeMovement.moving, (StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Transition.ConditionCallback) (smi => smi.navigator.IsMoving())).Enter((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback) (smi => this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, true)));
    this.freeMovement.moving.Transition(this.freeMovement.idle, (StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.Transition.ConditionCallback) (smi => !smi.navigator.IsMoving())).Enter((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback) (smi =>
    {
      this.PlayBodySegmentsAnim(smi, "walking_pre", KAnim.PlayMode.Once);
      this.PlayBodySegmentsAnim(smi, "walking_loop", KAnim.PlayMode.Loop, frameOffset: smi.def.animFrameOffset);
    })).Exit((StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback) (smi => this.PlayBodySegmentsAnim(smi, "walking_pst", KAnim.PlayMode.Once, true)));
  }

  private void PlayBodySegmentsAnim(
    SegmentedCreature.Instance smi,
    string animName,
    KAnim.PlayMode playMode,
    bool queue = false,
    int frameOffset = 0)
  {
    LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
    int num1 = 0;
    for (; linkedListNode != null; linkedListNode = linkedListNode.Next)
    {
      if (queue)
        linkedListNode.Value.animController.Queue((HashedString) animName, playMode);
      else
        linkedListNode.Value.animController.Play((HashedString) animName, playMode);
      if (frameOffset > 0)
      {
        float currentNumFrames = (float) linkedListNode.Value.animController.GetCurrentNumFrames();
        float num2 = (float) num1 * ((float) frameOffset / currentNumFrames);
        linkedListNode.Value.animController.SetElapsedTime(num2);
      }
      ++num1;
    }
  }

  private void UpdateRetractedPre(SegmentedCreature.Instance smi, float dt)
  {
    if ((double) this.UpdateHeadPosition(smi) == 0.0)
      return;
    bool flag = true;
    for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
    {
      linkedListNode.Value.distanceToPreviousSegment = Mathf.Max(smi.def.minSegmentSpacing, linkedListNode.Value.distanceToPreviousSegment - dt * smi.def.retractionSegmentSpeed);
      if ((double) linkedListNode.Value.distanceToPreviousSegment > (double) smi.def.minSegmentSpacing)
        flag = false;
    }
    SegmentedCreature.CreatureSegment creatureSegment = smi.GetHeadSegmentNode().Value;
    LinkedListNode<SegmentedCreature.PathNode> linkedListNode1 = smi.path.First;
    Vector3 forward = creatureSegment.Forward;
    Quaternion rotation = creatureSegment.Rotation;
    int num = 0;
    for (; linkedListNode1 != null; linkedListNode1 = linkedListNode1.Next)
    {
      Vector3 b = creatureSegment.Position - smi.def.pathSpacing * (float) num * forward;
      linkedListNode1.Value.position = Vector3.Lerp(linkedListNode1.Value.position, b, dt * smi.def.retractionPathSpeed);
      linkedListNode1.Value.rotation = Quaternion.Slerp(linkedListNode1.Value.rotation, rotation, dt * smi.def.retractionPathSpeed);
      ++num;
    }
    this.UpdateBodyPosition(smi);
    if (!flag)
      return;
    smi.GoTo((StateMachine.BaseState) this.retracted.loop);
  }

  private void UpdateRetractedLoop(SegmentedCreature.Instance smi, float dt)
  {
    if ((double) this.UpdateHeadPosition(smi) == 0.0)
      return;
    this.SetRetractedPath(smi);
    this.UpdateBodyPosition(smi);
  }

  private void SetRetractedPath(SegmentedCreature.Instance smi)
  {
    SegmentedCreature.CreatureSegment creatureSegment = smi.GetHeadSegmentNode().Value;
    LinkedListNode<SegmentedCreature.PathNode> linkedListNode = smi.path.First;
    Vector3 position = creatureSegment.Position;
    Quaternion rotation = creatureSegment.Rotation;
    Vector3 forward = creatureSegment.Forward;
    int num = 0;
    for (; linkedListNode != null; linkedListNode = linkedListNode.Next)
    {
      linkedListNode.Value.position = position - smi.def.pathSpacing * (float) num * forward;
      linkedListNode.Value.rotation = rotation;
      ++num;
    }
  }

  private void UpdateFreeMovement(SegmentedCreature.Instance smi, float dt)
  {
    float spacing = this.UpdateHeadPosition(smi);
    if ((double) spacing == 0.0)
      return;
    this.AdjustBodySegmentsSpacing(smi, spacing);
    this.UpdateBodyPosition(smi);
  }

  private float UpdateHeadPosition(SegmentedCreature.Instance smi)
  {
    SegmentedCreature.CreatureSegment creatureSegment = smi.GetHeadSegmentNode().Value;
    if (creatureSegment.Position == smi.previousHeadPosition)
      return 0.0f;
    SegmentedCreature.PathNode pathNode1 = smi.path.First.Value;
    SegmentedCreature.PathNode pathNode2 = smi.path.First.Next.Value;
    float magnitude1 = (pathNode1.position - pathNode2.position).magnitude;
    float magnitude2 = (creatureSegment.Position - pathNode2.position).magnitude;
    float num1 = magnitude2 - magnitude1;
    pathNode1.position = creatureSegment.Position;
    pathNode1.rotation = creatureSegment.Rotation;
    smi.previousHeadPosition = pathNode1.position;
    Vector3 normalized = (pathNode1.position - pathNode2.position).normalized;
    int num2 = Mathf.FloorToInt(magnitude2 / smi.def.pathSpacing);
    for (int index = 0; index < num2; ++index)
    {
      Vector3 vector3 = pathNode2.position + normalized * smi.def.pathSpacing;
      LinkedListNode<SegmentedCreature.PathNode> last = smi.path.Last;
      last.Value.position = vector3;
      last.Value.rotation = pathNode1.rotation;
      float num3 = magnitude2 - (float) index * smi.def.pathSpacing;
      float t = num3 - smi.def.pathSpacing / num3;
      last.Value.rotation = Quaternion.Lerp(pathNode1.rotation, pathNode2.rotation, t);
      smi.path.RemoveLast();
      smi.path.AddAfter(smi.path.First, last);
      pathNode2 = last.Value;
    }
    return num1;
  }

  private void AdjustBodySegmentsSpacing(SegmentedCreature.Instance smi, float spacing)
  {
    for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
    {
      linkedListNode.Value.distanceToPreviousSegment += spacing;
      if ((double) linkedListNode.Value.distanceToPreviousSegment < (double) smi.def.minSegmentSpacing)
      {
        spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.minSegmentSpacing;
        linkedListNode.Value.distanceToPreviousSegment = smi.def.minSegmentSpacing;
      }
      else
      {
        if ((double) linkedListNode.Value.distanceToPreviousSegment <= (double) smi.def.maxSegmentSpacing)
          break;
        spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.maxSegmentSpacing;
        linkedListNode.Value.distanceToPreviousSegment = smi.def.maxSegmentSpacing;
      }
    }
  }

  private void UpdateBodyPosition(SegmentedCreature.Instance smi)
  {
    LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode1 = smi.GetFirstBodySegmentNode();
    LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
    float num1 = 0.0f;
    float num2 = smi.LengthPercentage();
    int num3 = 0;
    while (linkedListNode1 != null)
    {
      float toPreviousSegment = linkedListNode1.Value.distanceToPreviousSegment;
      float num4 = 0.0f;
      Vector3 vector3_1;
      for (; linkedListNode2.Next != null; linkedListNode2 = linkedListNode2.Next)
      {
        vector3_1 = linkedListNode2.Value.position - linkedListNode2.Next.Value.position;
        num4 = vector3_1.magnitude - num1;
        if ((double) toPreviousSegment >= (double) num4)
        {
          toPreviousSegment -= num4;
          num1 = 0.0f;
        }
        else
          break;
      }
      if (linkedListNode2.Next == null)
      {
        linkedListNode1.Value.SetPosition(linkedListNode2.Value.position);
        linkedListNode1.Value.SetRotation(smi.path.Last.Value.rotation);
      }
      else
      {
        SegmentedCreature.PathNode pathNode1 = linkedListNode2.Value;
        SegmentedCreature.PathNode pathNode2 = linkedListNode2.Next.Value;
        SegmentedCreature.CreatureSegment creatureSegment = linkedListNode1.Value;
        Vector3 position = linkedListNode2.Value.position;
        vector3_1 = linkedListNode2.Next.Value.position - linkedListNode2.Value.position;
        Vector3 vector3_2 = vector3_1.normalized * toPreviousSegment;
        Vector3 vector3_3 = position + vector3_2;
        creatureSegment.SetPosition(vector3_3);
        linkedListNode1.Value.SetRotation(Quaternion.Slerp(pathNode1.rotation, pathNode2.rotation, toPreviousSegment / num4));
        num1 = toPreviousSegment;
      }
      linkedListNode1.Value.animController.FlipX = (double) linkedListNode1.Previous.Value.Position.x < (double) linkedListNode1.Value.Position.x;
      linkedListNode1.Value.animController.animScale = smi.baseAnimScale + (float) ((double) smi.baseAnimScale * (double) smi.def.compressedMaxScale * ((double) (smi.def.numBodySegments - num3) / (double) smi.def.numBodySegments) * (1.0 - (double) num2));
      linkedListNode1 = linkedListNode1.Next;
      ++num3;
    }
  }

  private void DrawDebug(SegmentedCreature.Instance smi, float dt)
  {
    SegmentedCreature.CreatureSegment creatureSegment = smi.GetHeadSegmentNode().Value;
    DrawUtil.Arrow(creatureSegment.Position, creatureSegment.Position + creatureSegment.Up, 0.05f, Color.red);
    DrawUtil.Arrow(creatureSegment.Position, creatureSegment.Position + creatureSegment.Forward * 0.06f, 0.05f, Color.cyan);
    int num = 0;
    foreach (SegmentedCreature.PathNode pathNode in smi.path)
    {
      Color rgb = Color.HSVToRGB((float) num / (float) smi.def.numPathNodes, 1f, 1f);
      DrawUtil.Gnomon(pathNode.position, 0.05f, Color.cyan);
      DrawUtil.Arrow(pathNode.position, pathNode.position + pathNode.rotation * Vector3.up * 0.5f, 0.025f, rgb);
      ++num;
    }
    for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
    {
      DrawUtil.Circle(linkedListNode.Value.Position, 0.05f, Color.white, new Vector3?(Vector3.forward));
      DrawUtil.Gnomon(linkedListNode.Value.Position, 0.05f, Color.white);
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public HashedString segmentTrackerSymbol;
    public Vector3 headOffset = Vector3.zero;
    public Vector3 bodyPivot = Vector3.zero;
    public Vector3 tailPivot = Vector3.zero;
    public int numBodySegments;
    public float minSegmentSpacing;
    public float maxSegmentSpacing;
    public int numPathNodes;
    public float pathSpacing;
    public KAnimFile midAnim;
    public KAnimFile tailAnim;
    public string movingAnimName;
    public string idleAnimName;
    public float retractionSegmentSpeed = 1f;
    public float retractionPathSpeed = 1f;
    public float compressedMaxScale = 1.2f;
    public int animFrameOffset;
    public HashSet<HashedString> hideBoddyWhenStartingAnimNames = new HashSet<HashedString>()
    {
      (HashedString) "rocket_biological"
    };
    public HashSet<HashedString> retractWhenStartingAnimNames = new HashSet<HashedString>()
    {
      (HashedString) "trapped",
      (HashedString) "trussed",
      (HashedString) "escape",
      (HashedString) "drown_pre",
      (HashedString) "drown_loop",
      (HashedString) "drown_pst",
      (HashedString) "rocket_biological"
    };
    public HashSet<HashedString> retractWhenEndingAnimNames = new HashSet<HashedString>()
    {
      (HashedString) "floor_floor_2_0",
      (HashedString) "grooming_pst",
      (HashedString) "fall"
    };
  }

  public class RectractStates : 
    GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
  {
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State pre;
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State loop;
  }

  public class FreeMovementStates : 
    GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
  {
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State idle;
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State moving;
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State layEgg;
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State poop;
    public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State dead;
  }

  public new class Instance : 
    GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.GameInstance
  {
    private const int NUM_CREATURE_SLOTS = 10;
    private static int creatureBatchSlot;
    public float baseAnimScale;
    public Vector3 previousHeadPosition;
    public float previousDist;
    public LinkedList<SegmentedCreature.PathNode> path = new LinkedList<SegmentedCreature.PathNode>();
    public LinkedList<SegmentedCreature.CreatureSegment> segments = new LinkedList<SegmentedCreature.CreatureSegment>();
    public Navigator navigator;

    public Instance(IStateMachineTarget master, SegmentedCreature.Def def)
      : base(master, def)
    {
      Debug.Assert((double) def.numBodySegments * (double) def.maxSegmentSpacing < (double) def.numPathNodes * (double) def.pathSpacing);
      this.CreateSegments();
      this.navigator = master.GetComponent<Navigator>();
    }

    private void CreateSegments()
    {
      float zRelativeOffset = (float) SegmentedCreature.Instance.creatureBatchSlot * 0.01f;
      SegmentedCreature.Instance.creatureBatchSlot = (SegmentedCreature.Instance.creatureBatchSlot + 1) % 10;
      SegmentedCreature.CreatureSegment creatureSegment = this.segments.AddFirst(new SegmentedCreature.CreatureSegment(this.GetComponent<KBatchedAnimController>(), this.gameObject, zRelativeOffset, this.smi.def.headOffset, Vector3.zero)).Value;
      this.gameObject.SetActive(false);
      creatureSegment.animController = this.GetComponent<KBatchedAnimController>();
      creatureSegment.animController.SetSymbolVisiblity((KAnimHashedString) this.smi.def.segmentTrackerSymbol, false);
      creatureSegment.symbol = this.smi.def.segmentTrackerSymbol;
      creatureSegment.SetPosition(this.transform.position);
      this.gameObject.SetActive(true);
      this.baseAnimScale = creatureSegment.animController.animScale;
      creatureSegment.animController.onAnimEnter += new KAnimControllerBase.KAnimEvent(this.AnimEntered);
      creatureSegment.animController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.AnimComplete);
      for (int index = 0; index < this.def.numBodySegments; ++index)
      {
        GameObject go = new GameObject(this.gameObject.GetProperName() + $" Segment {index}");
        go.SetActive(false);
        go.transform.parent = this.transform;
        go.transform.position = creatureSegment.Position;
        KAnimFile kanimFile = this.def.midAnim;
        Vector3 pivot = this.def.bodyPivot;
        if (index == this.def.numBodySegments - 1)
        {
          kanimFile = this.def.tailAnim;
          pivot = this.def.tailPivot;
        }
        KBatchedAnimController slave = go.AddOrGet<KBatchedAnimController>();
        slave.AnimFiles = new KAnimFile[1]{ kanimFile };
        slave.isMovable = true;
        slave.SetSymbolVisiblity((KAnimHashedString) this.smi.def.segmentTrackerSymbol, false);
        slave.sceneLayer = creatureSegment.animController.sceneLayer;
        this.segments.AddLast(new SegmentedCreature.CreatureSegment(creatureSegment.animController, go, zRelativeOffset + (float) (index + 1) * 0.0001f, Vector3.zero, pivot)
        {
          animController = slave,
          symbol = this.smi.def.segmentTrackerSymbol,
          distanceToPreviousSegment = this.smi.def.minSegmentSpacing,
          animLink = new KAnimLink((KAnimControllerBase) creatureSegment.animController, (KAnimControllerBase) slave)
        });
        go.SetActive(true);
      }
      for (int index = 0; index < this.def.numPathNodes; ++index)
        this.path.AddLast(new SegmentedCreature.PathNode(creatureSegment.Position));
    }

    public void AnimEntered(HashedString name)
    {
      if (this.smi.def.retractWhenStartingAnimNames.Contains(name))
        this.smi.sm.isRetracted.Set(true, this.smi);
      else
        this.smi.sm.isRetracted.Set(false, this.smi);
      if (this.smi.def.hideBoddyWhenStartingAnimNames.Contains(name))
        this.SetBodySegmentsVisibility(false);
      else
        this.SetBodySegmentsVisibility(true);
    }

    public void SetBodySegmentsVisibility(bool visible)
    {
      for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
        linkedListNode.Value.animController.SetVisiblity(visible);
    }

    public void AnimComplete(HashedString name)
    {
      if (!this.smi.def.retractWhenEndingAnimNames.Contains(name))
        return;
      this.smi.sm.isRetracted.Set(true, this.smi);
    }

    public LinkedListNode<SegmentedCreature.CreatureSegment> GetHeadSegmentNode()
    {
      return this.smi.segments.First;
    }

    public LinkedListNode<SegmentedCreature.CreatureSegment> GetFirstBodySegmentNode()
    {
      return this.smi.segments.First.Next;
    }

    public float LengthPercentage()
    {
      float num1 = 0.0f;
      for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
        num1 += linkedListNode.Value.distanceToPreviousSegment;
      float num2 = this.MinLength();
      float max = this.MaxLength();
      return Mathf.Clamp(num1 - num2, 0.0f, max) / (max - num2);
    }

    public float MinLength()
    {
      return this.smi.def.minSegmentSpacing * (float) this.smi.def.numBodySegments;
    }

    public float MaxLength()
    {
      return this.smi.def.maxSegmentSpacing * (float) this.smi.def.numBodySegments;
    }

    protected override void OnCleanUp()
    {
      this.GetHeadSegmentNode().Value.animController.onAnimEnter -= new KAnimControllerBase.KAnimEvent(this.AnimEntered);
      this.GetHeadSegmentNode().Value.animController.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.AnimComplete);
      for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
        linkedListNode.Value.CleanUp();
    }
  }

  public class PathNode
  {
    public Vector3 position;
    public Quaternion rotation;

    public PathNode(Vector3 position)
    {
      this.position = position;
      this.rotation = Quaternion.identity;
    }
  }

  public class CreatureSegment
  {
    public KBatchedAnimController animController;
    public KAnimLink animLink;
    public float distanceToPreviousSegment;
    public HashedString symbol;
    public Vector3 offset;
    public Vector3 pivot;
    public KBatchedAnimController head;
    private float zRelativeOffset;
    private Transform m_transform;

    public float ZOffset => Grid.GetLayerZ(this.head.sceneLayer) + this.zRelativeOffset;

    public CreatureSegment(
      KBatchedAnimController head,
      GameObject go,
      float zRelativeOffset,
      Vector3 offset,
      Vector3 pivot)
    {
      this.head = head;
      this.m_transform = go.transform;
      this.zRelativeOffset = zRelativeOffset;
      this.offset = offset;
      this.pivot = pivot;
      this.SetPosition(go.transform.position);
    }

    public Vector3 Position
    {
      get
      {
        Vector3 vector3 = this.offset;
        vector3.x *= this.animController.FlipX ? -1f : 1f;
        if (vector3 != Vector3.zero)
          vector3 = this.Rotation * vector3;
        if (!this.symbol.IsValid)
          return this.m_transform.position + vector3;
        return (Vector3) this.animController.GetSymbolTransform(this.symbol, out bool _).GetColumn(3) with
        {
          z = this.ZOffset
        } + vector3;
      }
    }

    public void SetPosition(Vector3 value)
    {
      bool flag = false;
      if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null && this.animController.sceneLayer != this.head.sceneLayer)
      {
        this.animController.SetSceneLayer(this.head.sceneLayer);
        flag = true;
      }
      value.z = this.ZOffset;
      this.m_transform.position = value;
      if (!flag)
        return;
      this.animController.enabled = false;
      this.animController.enabled = true;
    }

    public void SetRotation(Quaternion rotation) => this.m_transform.rotation = rotation;

    public Quaternion Rotation
    {
      get
      {
        if (!this.symbol.IsValid)
          return this.m_transform.rotation;
        Vector3 toDirection = this.animController.GetSymbolLocalTransform(this.symbol, out bool _).MultiplyVector(Vector3.right);
        if (!this.animController.FlipX)
          toDirection.y *= -1f;
        return Quaternion.FromToRotation(Vector3.right, toDirection);
      }
    }

    public Vector3 Forward
    {
      get => this.Rotation * (this.animController.FlipX ? Vector3.left : Vector3.right);
    }

    public Vector3 Up => this.Rotation * Vector3.up;

    public void CleanUp() => UnityEngine.Object.Destroy((UnityEngine.Object) this.m_transform.gameObject);
  }
}
