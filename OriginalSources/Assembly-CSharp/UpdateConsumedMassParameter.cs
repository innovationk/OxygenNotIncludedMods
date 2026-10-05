// Decompiled with JetBrains decompiler
// Type: UpdateConsumedMassParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;

#nullable disable
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
  private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

  public UpdateConsumedMassParameter()
    : base((HashedString) "consumedMass")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdateConsumedMassParameter.Entry()
    {
      creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
      ev = sound.ev,
      parameterId = sound.description.GetParameterId(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
    {
      if (!entry.creatureCalorieMonitor.IsNullOrStopped())
      {
        float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
        int num = (int) entry.ev.setParameterByID(entry.parameterId, fullness);
      }
    }
  }

  public override void Remove(LoopingSoundParameterUpdater.Sound sound)
  {
    for (int index = 0; index < this.entries.Count; ++index)
    {
      if (this.entries[index].ev.handle == sound.ev.handle)
      {
        this.entries.RemoveAt(index);
        break;
      }
    }
  }

  private struct Entry
  {
    public CreatureCalorieMonitor.Instance creatureCalorieMonitor;
    public EventInstance ev;
    public PARAMETER_ID parameterId;
  }
}
