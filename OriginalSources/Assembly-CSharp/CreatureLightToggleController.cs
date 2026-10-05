// Decompiled with JetBrains decompiler
// Type: CreatureLightToggleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CreatureLightToggleController : 
  GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.light_off;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.light_off.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(false))).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
    this.turning_off.BatchUpdate((UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.BatchUpdateDelegate) ((instances, time_delta) => CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta))).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight)).Transition(this.light_off, (StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.Transition.ConditionCallback) (smi => smi.IsOff()));
    this.light_on.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(true))).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (Func<CreatureLightToggleController.Instance, object, bool>) ((smi, obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj)));
    this.turning_on.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(true))).BatchUpdate((UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.BatchUpdateDelegate) ((instances, time_delta) => CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta))).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (Func<CreatureLightToggleController.Instance, object, bool>) ((smi, obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj))).Transition(this.light_on, (StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.Transition.ConditionCallback) (smi => smi.IsOn()));
  }

  public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
  {
    return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay) && !smi.prefabID.HasTag(GameTags.Creatures.Unhappy);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
  {
    private const float DIM_TIME = 5f;
    private const float GLOW_TIME = 2f;
    private int originalLux;
    private float originalRange;
    private Light2D light;
    public KPrefabID prefabID;
    private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();
    public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = (CreatureLightToggleController.Instance.ModifyLuxDelegate) ((instance, time_delta) =>
    {
      float num = (float) instance.originalLux / 5f;
      instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0.0f, (float) instance.light.Lux - num * time_delta));
    });
    public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = (CreatureLightToggleController.Instance.ModifyLuxDelegate) ((instance, time_delta) =>
    {
      float num = (float) instance.originalLux / 2f;
      instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float) instance.originalLux, (float) instance.light.Lux + num * time_delta));
    });

    public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def)
      : base(master, def)
    {
      this.prefabID = this.gameObject.GetComponent<KPrefabID>();
      this.light = master.GetComponent<Light2D>();
      this.originalLux = this.light.Lux;
      this.originalRange = this.light.Range;
    }

    public void SwitchLight(bool on) => this.light.enabled = on;

    public static void ModifyBrightness(
      List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances,
      CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux,
      float time_delta)
    {
      CreatureLightToggleController.Instance.modify_brightness_job.Reset((object) null);
      for (int index = 0; index != instances.Count; ++index)
      {
        UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry instance = instances[index] with
        {
          lastUpdateTime = 0.0f
        };
        instances[index] = instance;
        CreatureLightToggleController.Instance data = instance.data;
        modify_lux(data, time_delta);
        data.light.Range = data.originalRange * (float) data.light.Lux / (float) data.originalLux;
        int num = (int) data.light.RefreshShapeAndPosition();
        if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
          CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
      }
      GlobalJobManager.Run((IWorkItemCollection) CreatureLightToggleController.Instance.modify_brightness_job);
      for (int idx = 0; idx != CreatureLightToggleController.Instance.modify_brightness_job.Count; ++idx)
        CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(idx).Finish();
      CreatureLightToggleController.Instance.modify_brightness_job.Reset((object) null);
    }

    public bool IsOff() => this.light.Lux == 0;

    public bool IsOn() => this.light.Lux >= this.originalLux;

    private struct ModifyBrightnessTask : IWorkItem<object>
    {
      private LightGridManager.LightGridEmitter emitter;

      public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
      {
        this.emitter = emitter;
        emitter.RemoveFromGrid();
      }

      public void Run(object context, int threadIndex) => this.emitter.UpdateLitCells();

      public void Finish() => this.emitter.AddToGrid(false);
    }

    public delegate void ModifyLuxDelegate(
      CreatureLightToggleController.Instance instance,
      float time_delta);
  }
}
