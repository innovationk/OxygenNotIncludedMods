// Decompiled with JetBrains decompiler
// Type: HighEnergyParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticle : StateMachineComponent<HighEnergyParticle.StatesInstance>
{
  [Serialize]
  private EightDirection direction;
  [Serialize]
  public float speed;
  [Serialize]
  public float payload;
  [MyCmpReq]
  private RadiationEmitter emitter;
  [Serialize]
  public float perCellFalloff;
  [Serialize]
  public HighEnergyParticle.CollisionType collision;
  [Serialize]
  public HighEnergyParticlePort capturedBy;
  public short emitRadius;
  public float emitRate;
  public float emitSpeed;
  private LoopingSounds loopingSounds;
  public string flyingSound;
  public bool isCollideable;

  protected override void OnPrefabInit()
  {
    this.loopingSounds = this.gameObject.GetComponent<LoopingSounds>();
    this.flyingSound = GlobalAssets.GetSound("Radbolt_travel_LP");
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.HighEnergyParticles.Add(this);
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.HighEnergyParticleCount, (object) this.gameObject);
    this.emitter.SetEmitting(false);
    this.emitter.Refresh();
    this.SetDirection(this.direction);
    this.gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
    this.StartLoopingSound();
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.StopLoopingSound();
    Components.HighEnergyParticles.Remove(this);
    if (!((UnityEngine.Object) this.capturedBy != (UnityEngine.Object) null) || !((UnityEngine.Object) this.capturedBy.currentParticle == (UnityEngine.Object) this))
      return;
    this.capturedBy.currentParticle = (HighEnergyParticle) null;
  }

  public void SetDirection(EightDirection direction)
  {
    this.direction = direction;
    this.smi.master.transform.rotation = Quaternion.Euler(0.0f, 0.0f, EightDirectionUtil.GetAngle(direction));
  }

  public void Collide(HighEnergyParticle.CollisionType collisionType)
  {
    this.collision = collisionType;
    GameObject gameObject = new GameObject("HEPcollideFX");
    gameObject.SetActive(false);
    gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.PosToCell(this.smi.master.transform.position), Grid.SceneLayer.FXFront));
    KBatchedAnimController fxAnim = gameObject.AddComponent<KBatchedAnimController>();
    fxAnim.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "hep_impact_kanim")
    };
    fxAnim.initialAnim = "graze";
    gameObject.SetActive(true);
    switch (collisionType)
    {
      case HighEnergyParticle.CollisionType.Captured:
        fxAnim.Play((HashedString) "full");
        break;
      case HighEnergyParticle.CollisionType.CaptureAndRelease:
        fxAnim.Play((HashedString) "partial");
        break;
      case HighEnergyParticle.CollisionType.PassThrough:
        fxAnim.Play((HashedString) "graze");
        break;
    }
    fxAnim.onAnimComplete += (KAnimControllerBase.KAnimEvent) (arg => Util.KDestroyGameObject((Component) fxAnim));
    if (collisionType == HighEnergyParticle.CollisionType.PassThrough)
      this.collision = HighEnergyParticle.CollisionType.None;
    else
      this.smi.sm.destroySignal.Trigger(this.smi);
  }

  public void DestroyNow() => this.smi.sm.destroySimpleSignal.Trigger(this.smi);

  private void Capture(HighEnergyParticlePort input)
  {
    if ((UnityEngine.Object) input.currentParticle != (UnityEngine.Object) null)
    {
      DebugUtil.LogArgs((object) "Particle was backed up and caused an explosion!");
      this.smi.sm.destroySignal.Trigger(this.smi);
    }
    else
    {
      this.capturedBy = input;
      input.currentParticle = this;
      input.Capture(this);
      if ((UnityEngine.Object) input.currentParticle == (UnityEngine.Object) this)
      {
        input.currentParticle = (HighEnergyParticle) null;
        this.capturedBy = (HighEnergyParticlePort) null;
        this.Collide(HighEnergyParticle.CollisionType.Captured);
      }
      else
      {
        this.capturedBy = (HighEnergyParticlePort) null;
        this.Collide(HighEnergyParticle.CollisionType.CaptureAndRelease);
      }
    }
  }

  public void Uncapture()
  {
    if ((UnityEngine.Object) this.capturedBy != (UnityEngine.Object) null)
      this.capturedBy.currentParticle = (HighEnergyParticle) null;
    this.capturedBy = (HighEnergyParticlePort) null;
  }

  public void CheckCollision()
  {
    if (this.collision != HighEnergyParticle.CollisionType.None)
      return;
    int cell = Grid.PosToCell(this.smi.master.transform.GetPosition());
    GameObject gameObject1 = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
    {
      gameObject1.GetComponent<Operational>();
      HighEnergyParticlePort component = gameObject1.GetComponent<HighEnergyParticlePort>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Vector2 posCcc = (Vector2) Grid.CellToPosCCC(component.GetHighEnergyParticleInputPortPosition(), Grid.SceneLayer.NoLayer);
        if (this.GetComponent<KCircleCollider2D>().Intersects(posCcc))
        {
          if (component.InputActive() && component.AllowCapture(this))
          {
            this.Capture(component);
            return;
          }
          this.Collide(HighEnergyParticle.CollisionType.PassThrough);
        }
      }
    }
    KCircleCollider2D component1 = this.GetComponent<KCircleCollider2D>();
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    ListPool<ScenePartitionerEntry, HighEnergyParticle>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, HighEnergyParticle>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x - 1, y - 1, 3, 3, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      KCollider2D kcollider2D = partitionerEntry.obj as KCollider2D;
      HighEnergyParticle component2 = kcollider2D.gameObject.GetComponent<HighEnergyParticle>();
      if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null) && !((UnityEngine.Object) component2 == (UnityEngine.Object) this) && component2.isCollideable && component1.Intersects((Vector2) component2.transform.position) & kcollider2D.Intersects((Vector2) this.transform.position))
      {
        this.payload += component2.payload;
        component2.DestroyNow();
        this.Collide(HighEnergyParticle.CollisionType.HighEnergyParticle);
        return;
      }
    }
    gathered_entries.Recycle();
    GameObject gameObject2 = Grid.Objects[cell, 3];
    if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
    {
      ObjectLayerListItem objectLayerListItem = gameObject2.GetComponent<Pickupable>().objectLayerListItem;
      while (objectLayerListItem != null)
      {
        Pickupable pickupable = objectLayerListItem.pickupable;
        objectLayerListItem = objectLayerListItem.nextItem;
        if (!((UnityEngine.Object) pickupable == (UnityEngine.Object) null) && pickupable.KPrefabID.HasTag(GameTags.Creature))
        {
          Health component3 = pickupable.GetComponent<Health>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && !component3.IsDefeated())
          {
            component3.Damage(20f);
            this.Collide(HighEnergyParticle.CollisionType.Creature);
            return;
          }
        }
      }
    }
    GameObject go1 = Grid.Objects[cell, 0];
    if ((UnityEngine.Object) go1 != (UnityEngine.Object) null)
    {
      Health component4 = go1.GetComponent<Health>();
      if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && !component4.IsDefeated() && !go1.HasTag(GameTags.Dead) && !go1.HasTag(GameTags.Dying))
      {
        component4.Damage(20f);
        WoundMonitor.Instance smi = go1.GetSMI<WoundMonitor.Instance>();
        if (smi != null && !component4.IsDefeated())
          smi.PlayKnockedOverImpactAnimation();
        go1.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt((float) ((double) this.payload * 0.5 / 0.0099999997764825821)), "HEPImpact");
        this.Collide(HighEnergyParticle.CollisionType.Minion);
        return;
      }
    }
    if (!Grid.IsSolidCell(cell))
      return;
    GameObject go2 = Grid.Objects[cell, 9];
    if (!((UnityEngine.Object) go2 == (UnityEngine.Object) null) && go2.HasTag(GameTags.HEPPassThrough) && !((UnityEngine.Object) this.capturedBy == (UnityEngine.Object) null) && !((UnityEngine.Object) this.capturedBy.gameObject != (UnityEngine.Object) go2))
      return;
    this.Collide(HighEnergyParticle.CollisionType.Solid);
  }

  public void MovingUpdate(float dt)
  {
    if (this.collision != HighEnergyParticle.CollisionType.None)
      return;
    Vector3 position = this.transform.GetPosition();
    int cell1 = Grid.PosToCell(position);
    Vector3 vector3 = position + EightDirectionUtil.GetNormal(this.direction) * this.speed * dt;
    int cell2 = Grid.PosToCell(vector3);
    SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance += this.speed * dt;
    this.loopingSounds.UpdateVelocity(this.flyingSound, (Vector2) (vector3 - position));
    if (!Grid.IsValidCell(cell2))
    {
      this.smi.sm.destroySimpleSignal.Trigger(this.smi);
    }
    else
    {
      if (cell1 != cell2)
      {
        this.payload -= 0.1f;
        byte index = Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id);
        int disease_delta = Mathf.FloorToInt(5f);
        if (!Grid.Element[cell2].IsVacuum)
          SimMessages.ModifyDiseaseOnCell(cell2, index, disease_delta);
      }
      if ((double) this.payload <= 0.0)
        this.smi.sm.destroySimpleSignal.Trigger(this.smi);
      this.transform.SetPosition(vector3);
    }
  }

  private void StartLoopingSound() => this.loopingSounds.StartSound(this.flyingSound);

  private void StopLoopingSound() => this.loopingSounds.StopSound(this.flyingSound);

  public enum CollisionType
  {
    None,
    Solid,
    Creature,
    Minion,
    Captured,
    HighEnergyParticle,
    CaptureAndRelease,
    PassThrough,
  }

  public class StatesInstance(HighEnergyParticle smi) : 
    GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.GameInstance(smi)
  {
  }

  public class States : 
    GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle>
  {
    public HighEnergyParticle.States.ReadyStates ready;
    public HighEnergyParticle.States.DestructionStates destroying;
    public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State catchAndRelease;
    public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySignal;
    public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySimpleSignal;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.ready.pre;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.ready.OnSignal(this.destroySimpleSignal, this.destroying.instant).OnSignal(this.destroySignal, this.destroying.explode, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.Creature)).OnSignal(this.destroySignal, this.destroying.explode, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.Minion)).OnSignal(this.destroySignal, this.destroying.explode, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.Solid)).OnSignal(this.destroySignal, this.destroying.blackhole, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.HighEnergyParticle)).OnSignal(this.destroySignal, this.destroying.captured, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.Captured)).OnSignal(this.destroySignal, this.catchAndRelease, (StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Parameter<StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.SignalParameter>.Callback) ((smi, param) => smi.master.collision == HighEnergyParticle.CollisionType.CaptureAndRelease)).Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi =>
      {
        smi.master.emitter.SetEmitting(true);
        smi.master.isCollideable = true;
      })).Update((Action<HighEnergyParticle.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.MovingUpdate(dt);
        smi.master.CheckCollision();
      }), UpdateRate.SIM_EVERY_TICK);
      this.ready.pre.PlayAnim("travel_pre").OnAnimQueueComplete(this.ready.moving);
      this.ready.moving.PlayAnim("travel_loop", KAnim.PlayMode.Loop);
      this.catchAndRelease.Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi => smi.master.collision = HighEnergyParticle.CollisionType.None)).PlayAnim("explode", KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.pre);
      this.destroying.Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi =>
      {
        smi.master.isCollideable = false;
        smi.master.StopLoopingSound();
      }));
      this.destroying.instant.Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi => UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.gameObject)));
      this.destroying.explode.PlayAnim("explode").Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi => this.EmitRemainingPayload(smi)));
      this.destroying.blackhole.PlayAnim("collision").Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi => this.EmitRemainingPayload(smi)));
      this.destroying.captured.PlayAnim("travel_pst").OnAnimQueueComplete(this.destroying.instant).Enter((StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State.Callback) (smi => smi.master.emitter.SetEmitting(false)));
    }

    private void EmitRemainingPayload(HighEnergyParticle.StatesInstance smi)
    {
      smi.master.GetComponent<KBatchedAnimController>().GetCurrentAnim();
      smi.master.emitter.emitRadiusX = (short) 6;
      smi.master.emitter.emitRadiusY = (short) 6;
      smi.master.emitter.emitRads = (float) ((double) smi.master.payload * 0.5 * 600.0 / 9.0);
      smi.master.emitter.Refresh();
      SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.gameObject), SimHashes.Fallout, CellEventLogger.Instance.ElementEmitted, smi.master.payload * (1f / 1000f), 5000f, Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt((float) ((double) smi.master.payload * 0.5 / 0.0099999997764825821)));
      smi.Schedule(1f, (Action<object>) (obj => UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.gameObject)), (object) null);
    }

    public class ReadyStates : 
      GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
    {
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State pre;
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State moving;
    }

    public class DestructionStates : 
      GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
    {
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State instant;
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State explode;
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State captured;
      public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State blackhole;
    }
  }
}
