// Decompiled with JetBrains decompiler
// Type: TUNING.PLANTS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace TUNING;

public class PLANTS
{
  public const float MAX_MUTATION_CHANCE = 0.8f;

  public class SAFE_ELEMENTS
  {
    [Tooltip("Populated in Assets.SubstanceListHookup with tag GameTags.AnyWater")]
    public static SimHashes[] AllWaters;
    [Tooltip("AllWaters + Ink")]
    public static SimHashes[] MurkyWaters;
  }

  public class MASS_KG
  {
    public const float TIER0 = 0.25f;
    public const float TIER1 = 1f;
    public const float TIER2 = 2f;
    public const float TIER3 = 4f;
    public const float TIER4 = 8f;
  }

  public static class RADIATION_THRESHOLDS
  {
    public const float NONE = 0.0f;
    public const float TIER_0 = 250f;
    public const float TIER_1 = 900f;
    public const float TIER_2 = 2200f;
    public const float TIER_3 = 4600f;
    public const float TIER_4 = 7400f;
    public const float TIER_5 = 9800f;
    public const float TIER_6 = 12200f;
    public const float MUTANT_BASELINE = 250f;
  }

  public class HUSK_PER_CYCLE
  {
    public const float TIER0 = 0.0f;
    public const float TIER1 = 2f;
    public const float TIER2 = 4f;
    public const float TIER3 = 8f;
  }
}
