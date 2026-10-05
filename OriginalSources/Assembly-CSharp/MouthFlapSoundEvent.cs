// Decompiled with JetBrains decompiler
// Type: MouthFlapSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : 
  SoundEvent(file_name, sound_name, frame, false, is_looping, (float) SoundEvent.IGNORE_INTERVAL, true)
{
  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(this.name, (string) null);
  }
}
