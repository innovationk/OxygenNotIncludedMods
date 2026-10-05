// Decompiled with JetBrains decompiler
// Type: UpdatePercentCompleteParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
  private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

  public UpdatePercentCompleteParameter()
    : base((HashedString) "percentComplete")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdatePercentCompleteParameter.Entry()
    {
      worker = sound.transform.GetComponent<WorkerBase>(),
      ev = sound.ev,
      parameterId = sound.description.GetParameterId(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
    {
      if (!((Object) entry.worker == (Object) null))
      {
        Workable workable = entry.worker.GetWorkable();
        if (!((Object) workable == (Object) null))
        {
          float percentComplete = workable.GetPercentComplete();
          int num = (int) entry.ev.setParameterByID(entry.parameterId, percentComplete);
        }
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
    public WorkerBase worker;
    public EventInstance ev;
    public PARAMETER_ID parameterId;
  }
}
