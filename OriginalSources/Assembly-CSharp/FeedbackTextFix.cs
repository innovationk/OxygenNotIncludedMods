// Decompiled with JetBrains decompiler
// Type: FeedbackTextFix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

#nullable disable
public class FeedbackTextFix : MonoBehaviour
{
  public string newKey;
  public LocText locText;

  private void Awake()
  {
    if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
      Object.DestroyImmediate((Object) this);
    else
      this.locText.key = this.newKey;
  }
}
