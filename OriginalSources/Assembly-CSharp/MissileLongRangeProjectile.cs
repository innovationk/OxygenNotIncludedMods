// Decompiled with JetBrains decompiler
// Type: MissileLongRangeProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MissileLongRangeProjectile : 
  GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>
{
  public GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State launch;
  public GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State leaveworld;
  public StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.BoolParameter triggeroutofworld = new StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.BoolParameter(false);
  public StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.ObjectParameter<GameObject> asteroidTarget = new StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.ObjectParameter<GameObject>();

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ParamTransition<GameObject>((StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.Parameter<GameObject>) this.asteroidTarget, this.launch, (StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.Parameter<GameObject>.Callback) ((smi, target) => !target.IsNullOrDestroyed()));
    this.launch.Update("Launch", (System.Action<MissileLongRangeProjectile.StatesInstance, float>) ((smi, dt) => smi.UpdateLaunch(dt)), UpdateRate.SIM_EVERY_TICK).ParamTransition<bool>((StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.Parameter<bool>) this.triggeroutofworld, this.leaveworld, GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.IsTrue).Enter((StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State.Callback) (smi =>
    {
      Vector3 position = smi.master.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)
      };
      smi.smokeTrailFX = Util.KInstantiate(EffectPrefabs.Instance.LongRangeMissileSmokeTrailFX, position);
      smi.smokeTrailFX.transform.SetParent(smi.master.transform);
      smi.smokeTrailFX.SetActive(true);
      smi.StartTakeoff();
      KFMOD.PlayOneShot(GlobalAssets.GetSound("MissileLauncher_Missile_ignite"), CameraController.Instance.GetVerticallyScaledPosition(position));
    }));
    this.leaveworld.Enter((StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State.Callback) (smi => smi.ExitWorldEnterStarmap()));
  }

  public class Def : StateMachine.BaseDef
  {
    public string starmapOverrideSymbol = "payload";
    public string missileName = "STRINGS.ITEMS.MISSILE_LONGRANGE.NAME";
    public string missileDesc = "STRINGS.ITEMS.MISSILE_LONGRANGE.DESC";
  }

  public class StatesInstance : 
    GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.GameInstance
  {
    public KBatchedAnimController animController;
    [Serialize]
    private float launchSpeed;
    public GameObject smokeTrailFX;
    private WorldContainer myWorld;
    [Serialize]
    private AxialI myLocation;
    [Serialize]
    private int myWorldId = -1;
    [Serialize]
    private Ref<KPrefabID> launchedTarget = new Ref<KPrefabID>();

    private Vector3 Position => this.transform.position + this.animController.Offset;

    public StatesInstance(IStateMachineTarget master, MissileLongRangeProjectile.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
    }

    public override void StartSM()
    {
      base.StartSM();
      if (!((UnityEngine.Object) this.launchedTarget.Get() != (UnityEngine.Object) null))
        return;
      this.sm.asteroidTarget.Set(this.launchedTarget.Get().gameObject, this);
      this.myWorld = ClusterManager.Instance.GetWorld(this.myWorldId);
    }

    public void StartTakeoff()
    {
      if (GameComps.Fallers.Has((object) this.gameObject))
        GameComps.Fallers.Remove(this.gameObject);
      this.GetComponent<Pickupable>().handleFallerComponents = false;
    }

    public void UpdateLaunch(float dt)
    {
      this.animController.Rotation = MathUtil.AngleSigned(Vector3.up, Vector3.up, Vector3.forward);
      int cell = Grid.PosToCell(this.Position);
      Vector2I xy = Grid.CellToXY(cell);
      if (!Grid.IsValidCell(cell))
      {
        this.smi.sm.triggeroutofworld.Set(true, this.smi);
      }
      else
      {
        if (Grid.IsValidCellInWorld(Grid.PosToCell(this.Position), this.myWorldId) && (double) xy.y < (double) this.myWorld.maximumBounds.y)
        {
          this.transform.SetPosition(this.transform.position + Vector3.up * (this.launchSpeed * dt));
        }
        else
        {
          KBatchedAnimController animController = this.animController;
          animController.Offset = animController.Offset + Vector3.up * (this.launchSpeed * dt);
        }
        foreach (Component componentsInChild in this.smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>())
          componentsInChild.gameObject.transform.SetPositionAndRotation(this.Position, Quaternion.identity);
      }
    }

    public void PrepareLaunch(
      GameObject asteroid_target,
      float speed,
      Vector3 launchPos,
      float launchAngle)
    {
      this.gameObject.transform.SetParent((Transform) null);
      this.gameObject.layer = LayerMask.NameToLayer("Default");
      launchPos.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
      this.gameObject.transform.SetLocalPosition(launchPos);
      this.animController.Rotation = launchAngle;
      this.animController.Offset = Vector3.back;
      this.animController.SetVisiblity(true);
      this.gameObject.GetSMI<FetchableMonitor.Instance>()?.SetForceUnfetchable(true);
      this.sm.triggeroutofworld.Set(false, this.smi);
      this.sm.asteroidTarget.Set(asteroid_target, this.smi);
      this.launchedTarget = new Ref<KPrefabID>(asteroid_target.GetComponent<KPrefabID>());
      this.launchSpeed = speed;
      this.myWorld = this.gameObject.GetMyWorld();
      this.myWorldId = this.myWorld.id;
      ClusterGridEntity component = this.myWorld.GetComponent<ClusterGridEntity>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      this.myLocation = component.Location;
    }

    public void ExitWorldEnterStarmap()
    {
      GameObject go1 = this.sm.asteroidTarget.Get(this.smi);
      if ((UnityEngine.Object) go1 != (UnityEngine.Object) null)
      {
        ClusterGridEntity component = go1.GetComponent<ClusterGridEntity>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          GameObject go2 = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "ClusterMapLongRangeMissile"), Grid.SceneLayer.NoLayer);
          go2.SetActive(true);
          go2.GetSMI<ClusterMapLongRangeMissile.StatesInstance>().Setup(this.myLocation, component, this.def);
        }
        else
          go1.Trigger(-2056344675, (object) MissileLongRangeConfig.DamageEventPayload.sharedInstance);
      }
      Util.KDestroyGameObject(this.gameObject);
    }
  }
}
