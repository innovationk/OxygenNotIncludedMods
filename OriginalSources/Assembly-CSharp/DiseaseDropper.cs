// Decompiled with JetBrains decompiler
// Type: DiseaseDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DiseaseDropper : 
  GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>
{
  public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State working;
  public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State stopped;
  public StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.Signal cellChangedSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.stopped;
    this.root.EventHandler(GameHashes.BurstEmitDisease, (StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State.Callback) (smi => smi.DropSingleEmit()));
    this.working.TagTransition(GameTags.PreventEmittingDisease, this.stopped).Update((System.Action<DiseaseDropper.Instance, float>) ((smi, dt) => smi.DropPeriodic(dt)));
    this.stopped.TagTransition(GameTags.PreventEmittingDisease, this.working, true);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public byte diseaseIdx = byte.MaxValue;
    public int singleEmitQuantity;
    public int averageEmitPerSecond;
    public float emitFrequency = 1f;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      if (this.singleEmitQuantity > 0)
        descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity))));
      if (this.averageEmitPerSecond > 0)
        descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond))));
      return descriptors;
    }
  }

  public new class Instance(IStateMachineTarget master, DiseaseDropper.Def def) : 
    GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.GameInstance(master, def)
  {
    private float timeSinceLastDrop;

    public bool ShouldDropDisease() => true;

    public void DropSingleEmit()
    {
      this.DropDisease(this.def.diseaseIdx, this.def.singleEmitQuantity);
    }

    public void DropPeriodic(float dt)
    {
      this.timeSinceLastDrop += dt;
      if (this.def.averageEmitPerSecond <= 0 || (double) this.def.emitFrequency <= 0.0)
        return;
      for (; (double) this.timeSinceLastDrop > (double) this.def.emitFrequency; this.timeSinceLastDrop -= this.def.emitFrequency)
        this.DropDisease(this.def.diseaseIdx, (int) ((double) this.def.averageEmitPerSecond * (double) this.def.emitFrequency));
    }

    public void DropDisease(byte disease_idx, int disease_count)
    {
      if (disease_count <= 0 || disease_idx == byte.MaxValue)
        return;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (!Grid.IsValidCell(cell))
        return;
      SimMessages.ModifyDiseaseOnCell(cell, disease_idx, disease_count);
    }

    public bool IsValidDropCell()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return Grid.IsValidCell(cell) && Grid.IsGas(cell) && (double) Grid.Mass[cell] <= 1.0;
    }
  }
}
