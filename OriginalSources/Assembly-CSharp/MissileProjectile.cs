// Decompiled with JetBrains decompiler
// Type: MissileProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MissileProjectile : 
  GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>
{
  public GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State launch;
  public GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State explode;
  public StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.BoolParameter triggerexplode = new StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.BoolParameter(false);
  public StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.ObjectParameter<Comet> meteorTarget = new StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.ObjectParameter<Comet>();

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ParamTransition<Comet>((StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.Parameter<Comet>) this.meteorTarget, this.launch, (StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.Parameter<Comet>.Callback) ((smi, comet) => (UnityEngine.Object) comet != (UnityEngine.Object) null));
    this.launch.Update("Launch", (System.Action<MissileProjectile.StatesInstance, float>) ((smi, dt) => smi.UpdateLaunch(dt)), UpdateRate.SIM_EVERY_TICK).ParamTransition<bool>((StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.Parameter<bool>) this.triggerexplode, this.explode, GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.IsTrue).Enter((StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State.Callback) (smi =>
    {
      Vector3 position = smi.master.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)
      };
      smi.smokeTrailFX = Util.KInstantiate(EffectPrefabs.Instance.MissileSmokeTrailFX, position);
      smi.smokeTrailFX.transform.SetParent(smi.master.transform);
      smi.smokeTrailFX.SetActive(true);
      smi.StartTakeoff();
      KFMOD.PlayOneShot(GlobalAssets.GetSound("MissileLauncher_Missile_ignite"), CameraController.Instance.GetVerticallyScaledPosition(position));
    }));
    this.explode.Enter((StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State.Callback) (smi =>
    {
      smi.TriggerExplosion();
      foreach (ParticleSystem componentsInChild in smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>())
        componentsInChild.emission.enabled = false;
    }));
  }

  public class Def : StateMachine.BaseDef
  {
    public float MeteorDebrisMassModifier = 0.25f;
    public float ExplosionRange = 2f;
    public float debrisSpeed = 6f;
    public float debrisMaxAngle = 40f;
    public string explosionEffectAnim = "missile_explosion_kanim";
  }

  public class StatesInstance : 
    GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.GameInstance
  {
    public KBatchedAnimController animController;
    private float launchSpeed;
    public GameObject smokeTrailFX;

    private Vector3 Position => this.transform.position + this.animController.Offset;

    public StatesInstance(IStateMachineTarget master, MissileProjectile.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
    }

    public void StartTakeoff()
    {
      if (!GameComps.Fallers.Has((object) this.gameObject))
        return;
      GameComps.Fallers.Remove(this.gameObject);
    }

    public void UpdateLaunch(float dt)
    {
      int myWorldId = this.gameObject.GetMyWorldId();
      Comet comet = this.sm.meteorTarget.Get(this.smi);
      if (!comet.IsNullOrDestroyed())
      {
        Vector3 targetPosition = comet.TargetPosition;
        this.sm.triggerexplode.Set(this.InExplosionRange(targetPosition, this.Position), this.smi);
        Vector3 v2 = Vector3.Normalize(targetPosition - this.Position);
        Vector3 normalized = (targetPosition - this.Position).normalized;
        this.animController.Rotation = MathUtil.AngleSigned(Vector3.up, v2, Vector3.forward);
        if (Grid.IsValidCellInWorld(Grid.PosToCell(this.Position), myWorldId))
        {
          this.transform.SetPosition(this.transform.position + normalized * (this.launchSpeed * dt));
        }
        else
        {
          KBatchedAnimController animController = this.animController;
          animController.Offset = animController.Offset + normalized * (this.launchSpeed * dt);
        }
        foreach (Component componentsInChild in this.smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>())
          componentsInChild.gameObject.transform.SetPositionAndRotation(this.Position, Quaternion.identity);
      }
      else
      {
        if (this.sm.triggerexplode.Get(this.smi))
          return;
        if (!this.smi.smokeTrailFX.IsNullOrDestroyed())
          Util.KDestroyGameObject(this.smi.smokeTrailFX);
        if (!GameComps.Fallers.Has((object) this.gameObject))
          GameComps.Fallers.Add(this.gameObject, Vector2.down);
        this.gameObject.GetComponent<KSelectable>().enabled = true;
        this.smi.GoTo("root");
      }
    }

    public void PrepareLaunch(
      Comet meteor_target,
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
      this.sm.triggerexplode.Set(false, this.smi);
      this.sm.meteorTarget.Set(meteor_target, this.smi);
      this.launchSpeed = speed;
    }

    public void TriggerExplosion()
    {
      if (!this.smi.sm.meteorTarget.IsNullOrDestroyed())
      {
        this.SpawnMeteorResources(this.smi.sm.meteorTarget.Get(this.smi));
        Util.KDestroyGameObject((Component) this.smi.sm.meteorTarget.Get(this.smi));
      }
      this.Explode();
    }

    private void SpawnMeteorResources(Comet meteor)
    {
      PrimaryElement meteorPE = meteor.GetComponent<PrimaryElement>();
      Element element = meteorPE.Element;
      int world = meteor.GetMyWorldId();
      switch (world)
      {
        case -1:
        case (int) byte.MaxValue:
          WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(meteor.transform.GetPosition() - Vector3.down * Grid.CellSizeInMeters);
          world = (UnityEngine.Object) worldFromPosition == (UnityEngine.Object) null ? world : worldFromPosition.id;
          break;
      }
      int num1 = Grid.IsValidCellInWorld(Grid.PosToCell(meteor.TargetPosition), world) ? 1 : 0;
      float num2 = meteor.ExplosionMass * this.def.MeteorDebrisMassModifier;
      float num3 = meteor.AddTileMass * this.def.MeteorDebrisMassModifier;
      int num_nonTiles_ores = meteor.GetRandomNumOres();
      float num4 = num_nonTiles_ores > 0 ? num2 / (float) num_nonTiles_ores : 1f;
      float temperature = meteor.GetRandomTemperatureForOres();
      int num_tile_ores = meteor.addTiles;
      float num5 = num_tile_ores > 0 ? num3 / (float) num_tile_ores : 1f;
      Vector3 normalized = (meteor.TargetPosition - this.Position).normalized;
      Vector2 vector2_1 = new Vector2(normalized.x, normalized.y);
      Vector2 vector2_2 = new Vector2(vector2_1.y, -vector2_1.x);
      Func<int, int, float, Vector3> func = (Func<int, int, float, Vector3>) ((objectIndex, objectCount, maxAngleAllowed) =>
      {
        int num6 = objectCount % 2 == 0 ? objectCount : objectCount - 1;
        double num7 = (double) maxAngleAllowed * 2.0 / (double) num6;
        bool flag = objectIndex % 2 == 0;
        double num8 = (double) Mathf.CeilToInt((float) objectIndex / 2f);
        float num9 = (float) (num7 * num8 * (Math.PI / 180.0) * (flag ? 1.0 : -1.0));
        return new Vector3(Mathf.Cos(4.712389f + num9), Mathf.Sin(4.712389f + num9), 0.0f).normalized * this.def.debrisSpeed;
      });
      System.Action<Substance, float, Vector3> action1 = (System.Action<Substance, float, Vector3>) ((substance, mass, velocity) =>
      {
        Vector3 position = velocity.normalized * 0.75f + new Vector3(0.0f, 0.55f, 0.0f) + this.Position;
        GameObject go = substance.SpawnResource(position, mass, temperature, meteorPE.DiseaseIdx, meteorPE.DiseaseCount / (num_nonTiles_ores + num_tile_ores));
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, (Vector2) velocity);
      });
      System.Action<string, Vector3> action2 = (System.Action<string, Vector3>) ((prefabName, velocity) =>
      {
        Vector3 pos = velocity.normalized * 0.75f + new Vector3(0.0f, 0.55f, 0.0f) + this.Position;
        GameObject go = Scenario.SpawnPrefab(Grid.PosToCell(pos), 0, 0, prefabName);
        go.SetActive(true);
        pos.z = go.transform.position.z;
        go.transform.position = pos;
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, (Vector2) velocity);
      });
      Substance substance1 = element.substance;
      if (num1 != 0)
      {
        int num10 = num_nonTiles_ores + num_tile_ores + (meteor.lootOnDestroyedByMissile == null ? 0 : meteor.lootOnDestroyedByMissile.Length);
        for (int index = 0; index < num_nonTiles_ores; ++index)
        {
          Vector3 vector3 = func(index, num10, this.def.debrisMaxAngle);
          action1(substance1, num4, vector3);
        }
        for (int index = 0; index < num_tile_ores; ++index)
        {
          Vector3 vector3 = func(num_nonTiles_ores + index, num10, this.def.debrisMaxAngle);
          action1(substance1, num5, vector3);
        }
        if (meteor.lootOnDestroyedByMissile == null)
          return;
        for (int index = 0; index < meteor.lootOnDestroyedByMissile.Length; ++index)
        {
          Vector3 vector3 = func(num_nonTiles_ores + num_tile_ores + index, num10, this.def.debrisMaxAngle);
          string str = meteor.lootOnDestroyedByMissile[index];
          action2(str, vector3);
        }
      }
      else
      {
        if (world == -1 || world == (int) byte.MaxValue)
          return;
        int num11 = Grid.PosToCell(meteor.TargetPosition);
        Vector3 position = meteor.TargetPosition;
        Vector2 worldOffset;
        for (worldOffset = (Vector2) meteor.GetMyWorld().WorldOffset; !Grid.IsValidCellInWorld(num11, world) && (double) position.y > (double) worldOffset.y; position = Grid.CellToPos(num11))
          num11 = Grid.CellBelow(num11);
        if ((double) position.y <= (double) worldOffset.y)
          return;
        substance1.SpawnResource(position, num2 + num3, temperature, meteorPE.DiseaseIdx, meteorPE.DiseaseCount);
        if (meteor.lootOnDestroyedByMissile == null)
          return;
        for (int index = 0; index < meteor.lootOnDestroyedByMissile.Length; ++index)
        {
          string name = meteor.lootOnDestroyedByMissile[index];
          Scenario.SpawnPrefab(num11, 0, 0, name).SetActive(true);
        }
      }
    }

    private void Explode()
    {
      if (GameComps.Fallers.Has((object) this.gameObject))
        GameComps.Fallers.Remove(this.gameObject);
      this.SpawnExplosionFX(this.def.explosionEffectAnim, this.gameObject.transform.position with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2)
      }, this.animController.Offset);
      this.animController.SetSymbolVisiblity((KAnimHashedString) "missile_body", false);
      this.animController.SetSymbolVisiblity((KAnimHashedString) "missile_head", false);
    }

    private bool InExplosionRange(Vector3 target_pos, Vector3 current_pos)
    {
      return (double) Vector2.Distance((Vector2) target_pos, (Vector2) current_pos) <= (double) this.def.ExplosionRange;
    }

    private void SpawnExplosionFX(string anim, Vector3 pos, Vector3 offset)
    {
      KBatchedAnimController effect = FXHelpers.CreateEffect(anim, pos, this.gameObject.transform, layer: Grid.SceneLayer.FXFront2);
      effect.Offset = offset;
      effect.Play((HashedString) "idle");
      effect.onAnimComplete += (KAnimControllerBase.KAnimEvent) (obj => Util.KDestroyGameObject(this.gameObject));
    }
  }
}
