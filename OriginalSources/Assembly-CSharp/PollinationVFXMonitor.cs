// Decompiled with JetBrains decompiler
// Type: PollinationVFXMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class PollinationVFXMonitor : 
  GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>
{
  private GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State idle;
  private GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State pollinated;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventTransition(GameHashes.EffectAdded, this.pollinated, new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Transition.ConditionCallback(PollinationVFXMonitor.IsPollinated));
    this.pollinated.EventTransition(GameHashes.EffectRemoved, this.idle, GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Not(new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Transition.ConditionCallback(PollinationVFXMonitor.IsPollinated))).Toggle("Toggle Pollination VFX", new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State.Callback(PollinationVFXMonitor.CreatePollinationEffect), new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State.Callback(PollinationVFXMonitor.DestroyPollinationEffect));
  }

  private static bool IsPollinated(PollinationVFXMonitor.Instance smi) => smi.IsPollinated();

  private static void DestroyPollinationEffect(PollinationVFXMonitor.Instance smi)
  {
    smi.DestroyPollinationEffect();
  }

  private static void CreatePollinationEffect(PollinationVFXMonitor.Instance smi)
  {
    smi.CreatePollinationEffect();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.GameInstance
  {
    private Effects effects;
    private ParticleSystem pollinationEffect;
    private OccupyArea occupyArea;
    private bool isHangingPlant;

    public Instance(IStateMachineTarget master, PollinationVFXMonitor.Def def)
      : base(master, def)
    {
      this.effects = this.GetComponent<Effects>();
      this.occupyArea = this.GetComponent<OccupyArea>();
    }

    public override void StartSM()
    {
      this.isHangingPlant = this.gameObject.HasTag(GameTags.Hanging);
      base.StartSM();
    }

    public bool IsPollinated()
    {
      if ((Object) this.effects == (Object) null)
        return false;
      foreach (HashedString pollinationEffect in PollinationMonitor.PollinationEffects)
      {
        if (this.effects.HasEffect(pollinationEffect))
          return true;
      }
      return false;
    }

    public void CreatePollinationEffect()
    {
      this.DestroyPollinationEffect();
      Vector4 vector4 = new Vector4(float.MaxValue, float.MinValue, float.MaxValue, float.MinValue);
      foreach (CellOffset occupiedCellsOffset in this.occupyArea.OccupiedCellsOffsets)
      {
        if ((double) occupiedCellsOffset.x < (double) vector4.x)
          vector4.x = (float) occupiedCellsOffset.x;
        if ((double) occupiedCellsOffset.x > (double) vector4.y)
          vector4.y = (float) occupiedCellsOffset.x;
        if ((double) occupiedCellsOffset.y < (double) vector4.z)
          vector4.z = (float) occupiedCellsOffset.y;
        if ((double) occupiedCellsOffset.y > (double) vector4.w)
          vector4.w = (float) occupiedCellsOffset.y;
      }
      int num1 = 1 + (int) Mathf.Clamp(vector4.y - vector4.x, 0.0f, (float) int.MaxValue);
      int num2 = 1 + (int) Mathf.Clamp(vector4.w - vector4.z, 0.0f, (float) int.MaxValue);
      this.pollinationEffect = Util.KInstantiate(EffectPrefabs.Instance.PlantPollinated, Grid.CellToPosCBC(this.occupyArea.GetOffsetCellWithRotation(new CellOffset(0, this.isHangingPlant ? -num2 + 1 : 0)), Grid.SceneLayer.BuildingFront), Quaternion.identity, this.gameObject, "PollinationVFX").GetComponent<ParticleSystem>();
      ParticleSystem.ShapeModule shape = this.pollinationEffect.shape;
      Vector3 scale = shape.scale;
      Vector3 position = shape.position;
      scale.x = (float) num1;
      scale.y = (float) num2;
      position.y = (float) num2 * 0.5f;
      shape.scale = scale;
      shape.position = position;
    }

    public void DestroyPollinationEffect()
    {
      if (!((Object) this.pollinationEffect != (Object) null))
        return;
      this.pollinationEffect.DeleteObject();
      this.pollinationEffect = (ParticleSystem) null;
    }
  }
}
