// Decompiled with JetBrains decompiler
// Type: MoleTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class MoleTuning
{
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "MoleEgg".ToTag(),
      weight = 0.98f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "MoleDelicacyEgg".ToTag(),
      weight = 0.02f
    }
  };
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DELICACY = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "MoleEgg".ToTag(),
      weight = 0.32f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "MoleDelicacyEgg".ToTag(),
      weight = 0.65f
    }
  };
  public static float STANDARD_CALORIES_PER_CYCLE = 4800000f;
  public static float STANDARD_STARVE_CYCLES = 10f;
  public static float STANDARD_STOMACH_SIZE = MoleTuning.STANDARD_CALORIES_PER_CYCLE * MoleTuning.STANDARD_STARVE_CYCLES;
  public static float DELICACY_STOMACH_SIZE = MoleTuning.STANDARD_STOMACH_SIZE / 2f;
  public static int PEN_SIZE_PER_CREATURE = 0;
  public static float EGG_MASS = 2f;
  public static int DEPTH_TO_HIDE = 2;
  public static HashedString[] GINGER_SYMBOL_NAMES = new HashedString[6]
  {
    (HashedString) "del_ginger",
    (HashedString) "del_ginger1",
    (HashedString) "del_ginger2",
    (HashedString) "del_ginger3",
    (HashedString) "del_ginger4",
    (HashedString) "del_ginger5"
  };
}
