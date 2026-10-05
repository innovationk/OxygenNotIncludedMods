// Decompiled with JetBrains decompiler
// Type: CrabTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;

#nullable disable
public static class CrabTuning
{
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabEgg".ToTag(),
      weight = 0.97f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabWoodEgg".ToTag(),
      weight = 0.02f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabFreshWaterEgg".ToTag(),
      weight = 0.01f
    }
  };
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WOOD = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabEgg".ToTag(),
      weight = 0.32f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabWoodEgg".ToTag(),
      weight = 0.65f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabFreshWaterEgg".ToTag(),
      weight = 0.02f
    }
  };
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_FRESH = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabEgg".ToTag(),
      weight = 0.32f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabWoodEgg".ToTag(),
      weight = 0.02f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "CrabFreshWaterEgg".ToTag(),
      weight = 0.65f
    }
  };
  public static float STANDARD_CALORIES_PER_CYCLE = 100000f;
  public static float STANDARD_STARVE_CYCLES = 10f;
  public static float STANDARD_STOMACH_SIZE = CrabTuning.STANDARD_CALORIES_PER_CYCLE * CrabTuning.STANDARD_STARVE_CYCLES;
  public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
  public static float EGG_MASS = 2f;
  public static CellOffset[] DEFEND_OFFSETS = new CellOffset[5]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 1),
    new CellOffset(-1, 1)
  };

  public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
  {
    return CrabTuning.IsValidTimeToDrop(smi) && CrabTuning.IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry) && smi.prefabID.HasTag(GameTags.Creatures.Happy);
  }

  public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
  {
    if (smi.spawnedThisCycle)
      return false;
    return (double) smi.timeOfLastDrop <= 0.0 || (double) GameClock.Instance.GetTime() - (double) smi.timeOfLastDrop > 600.0;
  }

  public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    return Grid.IsValidCell(cell) && Grid.Element[cell].id != SimHashes.Ethanol;
  }
}
