// Decompiled with JetBrains decompiler
// Type: BionicMicrochipMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BionicMicrochipMonitor : 
  GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>
{
  public const float MICROCHIP_PRODUCTION_TIME = 150f;
  public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State idle;
  public BionicMicrochipMonitor.ProductionStates production;
  public StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.FloatParameter Progress;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.TagTransition(GameTags.BionicBedTime, (GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State) this.production);
    this.production.TagTransition(GameTags.BionicBedTime, this.idle, true).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateProgresesBar)).Exit(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ClearProgressBar)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicMicrochipGeneration).DefaultState(this.production.charging);
    this.production.charging.ParamTransition<float>((StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.Parameter<float>) this.Progress, this.production.produceOne, GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.IsGTEOne).Update(new System.Action<BionicMicrochipMonitor.Instance, float>(BionicMicrochipMonitor.ProgressUpdate));
    this.production.produceOne.Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateMicrochip)).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ResetProgress)).GoTo(this.production.charging);
  }

  public static void ClearProgressBar(BionicMicrochipMonitor.Instance smi)
  {
    smi.ClearProgressBar();
  }

  public static void CreateProgresesBar(BionicMicrochipMonitor.Instance smi)
  {
    smi.CreateProgressBar();
  }

  public static void ResetProgress(BionicMicrochipMonitor.Instance smi)
  {
    double num = (double) smi.sm.Progress.Set(0.0f, smi);
  }

  public static void CreateMicrochip(BionicMicrochipMonitor.Instance smi) => smi.CreateMicrochip();

  public static void ProgressUpdate(BionicMicrochipMonitor.Instance smi, float dt)
  {
    float num1 = dt / 150f;
    float progress = smi.Progress;
    double num2 = (double) smi.sm.Progress.Set(progress + num1, smi);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class ProductionStates : 
    GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State
  {
    public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State charging;
    public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State produceOne;
  }

  public new class Instance(IStateMachineTarget master, BionicMicrochipMonitor.Def def) : 
    GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.GameInstance(master, def)
  {
    public ProgressBar progressBar;

    public float Progress => this.sm.Progress.Get(this);

    public void CreateMicrochip()
    {
      Util.KInstantiate(Assets.GetPrefab(PowerStationToolsConfig.tag), Grid.CellToPos(Grid.PosToCell(this.smi.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore)).SetActive(true);
    }

    public void CreateProgressBar()
    {
      this.progressBar = ProgressBar.CreateProgressBar(this.gameObject, (Func<float>) (() => this.Progress));
      this.smi.progressBar.SetVisibility(true);
      this.smi.progressBar.barColor = Color.green;
    }

    public void ClearProgressBar()
    {
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject(this.smi.progressBar.gameObject);
      this.progressBar = (ProgressBar) null;
    }
  }
}
