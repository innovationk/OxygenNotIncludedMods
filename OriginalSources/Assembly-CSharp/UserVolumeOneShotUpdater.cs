// Decompiled with JetBrains decompiler
// Type: UserVolumeOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
  private string playerPref;

  public UserVolumeOneShotUpdater(string parameter, string player_pref)
    : base((HashedString) parameter)
  {
    this.playerPref = player_pref;
  }

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    if (string.IsNullOrEmpty(this.playerPref))
      return;
    float num1 = KPlayerPrefs.GetFloat(this.playerPref);
    int num2 = (int) sound.ev.setParameterByID(sound.description.GetParameterId(this.parameter), num1);
  }
}
