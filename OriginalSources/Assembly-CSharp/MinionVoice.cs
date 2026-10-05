// Decompiled with JetBrains decompiler
// Type: MinionVoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public readonly struct MinionVoice
{
  public readonly int voiceIndex;
  public readonly string voiceId;
  public readonly bool isValid;
  private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();

  public MinionVoice(int voiceIndex)
  {
    this.voiceIndex = voiceIndex;
    this.voiceId = (voiceIndex + 1).ToString("D2");
    this.isValid = true;
  }

  public static MinionVoice ByPersonality(Personality personality)
  {
    return MinionVoice.ByPersonality(personality.Id);
  }

  public static MinionVoice ByPersonality(string personalityId)
  {
    switch (personalityId)
    {
      case "JORGE":
        return new MinionVoice(-2);
      case "MEEP":
        return new MinionVoice(2);
      default:
        MinionVoice minionVoice;
        if (!MinionVoice.personalityVoiceMap.TryGetValue(personalityId, out minionVoice))
        {
          minionVoice = MinionVoice.Random();
          MinionVoice.personalityVoiceMap.Add(personalityId, minionVoice);
        }
        return minionVoice;
    }
  }

  public static MinionVoice Random() => new MinionVoice(UnityEngine.Random.Range(0, 4));

  public static Option<MinionVoice> ByObject(UnityEngine.Object unityObject)
  {
    GameObject gameObject1;
    switch (unityObject)
    {
      case GameObject gameObject2:
        gameObject1 = gameObject2;
        break;
      case Component component:
        gameObject1 = component.gameObject;
        break;
      default:
        gameObject1 = (GameObject) null;
        break;
    }
    if (gameObject1.IsNullOrDestroyed())
      return (Option<MinionVoice>) Option.None;
    MinionVoiceProviderMB componentInParent = gameObject1.GetComponentInParent<MinionVoiceProviderMB>();
    return componentInParent.IsNullOrDestroyed() ? (Option<MinionVoice>) Option.None : componentInParent.voice;
  }

  public string GetSoundAssetName(string localName)
  {
    Debug.Assert(this.isValid);
    string d = localName;
    if (localName.Contains(":"))
      d = localName.Split(':', StringSplitOptions.None)[0];
    return StringFormatter.Combine("DupVoc_", this.voiceId, "_", d);
  }

  public string GetSoundPath(string localName)
  {
    return GlobalAssets.GetSound(this.GetSoundAssetName(localName), true);
  }

  public void PlaySoundUI(string localName)
  {
    Debug.Assert(this.isValid);
    string soundPath = this.GetSoundPath(localName);
    try
    {
      if ((UnityEngine.Object) SoundListenerController.Instance == (UnityEngine.Object) null)
        KFMOD.PlayUISound(soundPath);
      else
        KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition());
    }
    catch
    {
      DebugUtil.LogWarningArgs((object) $"AUDIOERROR: Missing [{soundPath}]");
    }
  }
}
