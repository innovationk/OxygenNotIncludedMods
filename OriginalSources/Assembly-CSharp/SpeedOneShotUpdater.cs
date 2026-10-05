// Decompiled with JetBrains decompiler
// Type: SpeedOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
  public SpeedOneShotUpdater()
    : base((HashedString) "Speed")
  {
  }

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    int num = (int) sound.ev.setParameterByID(sound.description.GetParameterId(this.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue());
  }
}
