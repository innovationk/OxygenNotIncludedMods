// Decompiled with JetBrains decompiler
// Type: DivergentTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;

#nullable disable
public static class DivergentTuning
{
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BEETLE = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "DivergentBeetleEgg".ToTag(),
      weight = 0.98f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "DivergentWormEgg".ToTag(),
      weight = 0.02f
    }
  };
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WORM = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "DivergentBeetleEgg".ToTag(),
      weight = 0.33f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "DivergentWormEgg".ToTag(),
      weight = 0.67f
    }
  };
  public static int TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION = 2;
  public static float STANDARD_CALORIES_PER_CYCLE = 700000f;
  public static float STANDARD_STARVE_CYCLES = 10f;
  public static float STANDARD_STOMACH_SIZE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE * DivergentTuning.STANDARD_STARVE_CYCLES;
  public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
  public static int PEN_SIZE_PER_CREATURE_WORM = CREATURES.SPACE_REQUIREMENTS.TIER4;
  public static float EGG_MASS = 2f;
}
