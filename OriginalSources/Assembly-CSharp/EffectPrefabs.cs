// Decompiled with JetBrains decompiler
// Type: EffectPrefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectPrefabs : MonoBehaviour
{
  public GameObject DreamBubble;
  public GameObject ThoughtBubble;
  public GameObject ThoughtBubbleConvo;
  public GameObject MeteorBackground;
  public GameObject SparkleStreakFX;
  public GameObject HappySingerFX;
  public GameObject HugFrenzyFX;
  public GameObject GameplayEventDisplay;
  public GameObject OpenTemporalTearBeam;
  public GameObject MissileSmokeTrailFX;
  public GameObject LongRangeMissileSmokeTrailFX;
  public GameObject PlantPollinated;

  public static EffectPrefabs Instance { get; private set; }

  private void Awake() => EffectPrefabs.Instance = this;
}
