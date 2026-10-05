// Decompiled with JetBrains decompiler
// Type: UserVolumeLoopingUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;

#nullable disable
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
  private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();
  private string playerPref;

  public UserVolumeLoopingUpdater(string parameter, string player_pref)
    : base((HashedString) parameter)
  {
    this.playerPref = player_pref;
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UserVolumeLoopingUpdater.Entry()
    {
      ev = sound.ev,
      parameterId = sound.description.GetParameterId(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    if (string.IsNullOrEmpty(this.playerPref))
      return;
    float num1 = KPlayerPrefs.GetFloat(this.playerPref);
    foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
    {
      int num2 = (int) entry.ev.setParameterByID(entry.parameterId, num1);
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
    public EventInstance ev;
    public PARAMETER_ID parameterId;
  }
}
