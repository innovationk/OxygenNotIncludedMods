// Decompiled with JetBrains decompiler
// Type: SpeedLoopingSoundUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
  private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

  public SpeedLoopingSoundUpdater()
    : base((HashedString) "Speed")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new SpeedLoopingSoundUpdater.Entry()
    {
      ev = sound.ev,
      parameterId = sound.description.GetParameterId(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
    foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
    {
      int num = (int) entry.ev.setParameterByID(entry.parameterId, speedParameterValue);
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

  public static float GetSpeedParameterValue() => Time.timeScale * 1f;

  private struct Entry
  {
    public EventInstance ev;
    public PARAMETER_ID parameterId;
  }
}
