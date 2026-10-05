// Decompiled with JetBrains decompiler
// Type: UpdateRocketLandingParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
  private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

  public UpdateRocketLandingParameter()
    : base((HashedString) "rocketLanding")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdateRocketLandingParameter.Entry()
    {
      rocketModule = sound.transform.GetComponent<RocketModule>(),
      ev = sound.ev,
      parameterId = sound.description.GetParameterId(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
    {
      if (!((Object) entry.rocketModule == (Object) null))
      {
        LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
        if (!((Object) conditionManager == (Object) null))
        {
          ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
          EventInstance ev;
          if (component != null)
          {
            if (component.isLanding)
            {
              ev = entry.ev;
              int num = (int) ev.setParameterByID(entry.parameterId, 1f);
            }
            else
            {
              ev = entry.ev;
              int num = (int) ev.setParameterByID(entry.parameterId, 0.0f);
            }
          }
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
    public RocketModule rocketModule;
    public EventInstance ev;
    public PARAMETER_ID parameterId;
  }
}
