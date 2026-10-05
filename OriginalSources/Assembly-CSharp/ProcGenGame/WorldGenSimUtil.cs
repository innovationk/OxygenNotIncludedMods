// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldGenSimUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace ProcGenGame;

public static class WorldGenSimUtil
{
  private const int STEPS = 500;

  public static unsafe bool DoSettleSim(
    WorldGenSettings settings,
    BinaryWriter writer,
    uint simSeed,
    ref WorldgenSimData simData,
    WorldGen.OfflineCallbackFunction updateProgressFn,
    Klei.Data data,
    List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    Action<OfflineWorldGen.ErrorInfo> error_cb,
    int baseId)
  {
    Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
    SimMessages.CreateSimElementsTable(ElementLoader.elements);
    SimMessages.CreateDiseaseTable(WorldGen.diseaseStats);
    SimMessages.SimDataInitializeFromCells(Grid.WidthInCells, Grid.HeightInCells, simSeed, ref simData, true);
    int num1 = updateProgressFn(UI.WORLDGEN.SETTLESIM.key, 0.0f, WorldGenProgressStages.Stages.SettleSim) ? 1 : 0;
    Sim.Start();
    byte[] msg = new byte[Grid.CellCount];
    for (int index = 0; index < Grid.CellCount; ++index)
      msg[index] = byte.MaxValue;
    Vector2I a = new Vector2I(0, 0);
    Vector2I size = data.world.size;
    List<Game.SimActiveRegion> activeRegions = new List<Game.SimActiveRegion>();
    activeRegions.Add(new Game.SimActiveRegion()
    {
      region = new Pair<Vector2I, Vector2I>(a, size)
    });
    for (int index1 = 0; index1 < 500; ++index1)
    {
      if (index1 == 498)
      {
        HashSet<int> intSet = new HashSet<int>();
        if (templateSpawnTargets != null)
        {
          foreach (TemplateSpawning.TemplateSpawner templateSpawnTarget in templateSpawnTargets)
          {
            if (templateSpawnTarget.container.cells != null)
            {
              for (int index2 = 0; index2 < templateSpawnTarget.container.cells.Count; ++index2)
              {
                TemplateClasses.Cell cell = templateSpawnTarget.container.cells[index2];
                int num2 = Grid.OffsetCell(Grid.XYToCell(templateSpawnTarget.position.x, templateSpawnTarget.position.y), cell.location_x, cell.location_y);
                if (Grid.IsValidCell(num2) && !intSet.Contains(num2))
                {
                  intSet.Add(num2);
                  ushort elementIndex1 = ElementLoader.GetElementIndex(cell.element);
                  float temperature = cell.temperature;
                  float mass = cell.mass;
                  byte index3 = WorldGen.diseaseStats.GetIndex((HashedString) cell.diseaseName);
                  int diseaseCount = cell.diseaseCount;
                  ushort elementIndex2 = ElementLoader.GetElementIndex(cell.backwallElement);
                  float backwallMass = cell.backwallMass;
                  float backwallTemperature = cell.backwallTemperature;
                  SimMessages.ModifyCell(num2, elementIndex1, temperature, mass, index3, diseaseCount, SimMessages.ReplaceType.Replace);
                  SimMessages.SetBackwallData(num2, elementIndex2, backwallMass, backwallTemperature);
                }
              }
            }
          }
        }
      }
      SimMessages.NewGameFrame(0.2f, activeRegions);
      IntPtr num3 = Sim.HandleMessage(SimMessageHashes.PrepareGameData, msg.Length, msg);
      int num4 = updateProgressFn(UI.WORLDGEN.SETTLESIM.key, (float) index1 / 500f, WorldGenProgressStages.Stages.SettleSim) ? 1 : 0;
      if (num3 == IntPtr.Zero)
      {
        DebugUtil.LogWarningArgs((object) "Unexpected");
      }
      else
      {
        Sim.GameDataUpdate* data1 = (Sim.GameDataUpdate*) (void*) num3;
        Grid.elementIdx = data1->elementIdx;
        Grid.temperature = data1->temperature;
        Grid.mass = data1->mass;
        Grid.radiation = data1->radiation;
        Grid.properties = data1->properties;
        Grid.strengthInfo = data1->strengthInfo;
        Grid.insulation = data1->insulation;
        Grid.diseaseIdx = data1->diseaseIdx;
        Grid.diseaseCount = data1->diseaseCount;
        Grid.AccumulatedFlowValues = data1->accumulatedFlow;
        Grid.exposedToSunlight = (byte*) (void*) data1->propertyTextureExposedToSunlight;
        BackwallManager.UpdateFromSim(data1);
        for (int index4 = 0; index4 < data1->numSubstanceChangeInfo; ++index4)
        {
          Sim.SubstanceChangeInfo substanceChangeInfo = data1->substanceChangeInfo[index4];
          int cellIdx = substanceChangeInfo.cellIdx;
          simData.cells[cellIdx].elementIdx = data1->elementIdx[cellIdx];
          simData.cells[cellIdx].insulation = data1->insulation[cellIdx];
          simData.cells[cellIdx].properties = data1->properties[cellIdx];
          simData.cells[cellIdx].temperature = data1->temperature[cellIdx];
          simData.cells[cellIdx].mass = data1->mass[cellIdx];
          simData.cells[cellIdx].strengthInfo = data1->strengthInfo[cellIdx];
          simData.diseaseCells[cellIdx].diseaseIdx = data1->diseaseIdx[cellIdx];
          simData.diseaseCells[cellIdx].elementCount = data1->diseaseCount[cellIdx];
          Grid.Element[cellIdx] = ElementLoader.elements[(int) substanceChangeInfo.newElemIdx];
        }
        for (int index5 = 0; index5 < data1->numSolidInfo; ++index5)
        {
          Sim.SolidInfo solidInfo = data1->solidInfo[index5];
          Grid.SetSolid(solidInfo.cellIdx, solidInfo.isSolid != 0, (CellSolidEvent) null);
        }
      }
    }
    int num5 = WorldGenSimUtil.SaveSim(writer, data, baseId, error_cb) ? 1 : 0;
    Sim.Shutdown();
    return num5 != 0;
  }

  private static bool SaveSim(
    BinaryWriter writer,
    Klei.Data data,
    int baseId,
    Action<OfflineWorldGen.ErrorInfo> error_cb)
  {
    try
    {
      KSerialization.Manager.Clear();
      SimSaveFileStructure saveFileStructure = new SimSaveFileStructure();
      for (int index = 0; index < data.overworldCells.Count; ++index)
        saveFileStructure.worldDetail.overworldCells.Add(new WorldDetailSave.OverworldCell(SettingsCache.GetCachedSubWorld(data.overworldCells[index].node.type).zoneType, data.overworldCells[index]));
      saveFileStructure.worldDetail.globalWorldSeed = data.globalWorldSeed;
      saveFileStructure.worldDetail.globalWorldLayoutSeed = data.globalWorldLayoutSeed;
      saveFileStructure.worldDetail.globalTerrainSeed = data.globalTerrainSeed;
      saveFileStructure.worldDetail.globalNoiseSeed = data.globalNoiseSeed;
      saveFileStructure.WidthInCells = Grid.WidthInCells;
      saveFileStructure.HeightInCells = Grid.HeightInCells;
      saveFileStructure.x = data.world.offset.x;
      saveFileStructure.y = data.world.offset.y;
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter writer1 = new BinaryWriter((Stream) output))
          Sim.Save(writer1, saveFileStructure.x, saveFileStructure.y);
        saveFileStructure.Sim = output.ToArray();
      }
      try
      {
        using (MemoryStream output = new MemoryStream())
        {
          using (BinaryWriter writer2 = new BinaryWriter((Stream) output))
            Serializer.Serialize((object) saveFileStructure, writer2);
          KSerialization.Manager.SerializeDirectory(writer);
          writer.Write(output.ToArray());
        }
      }
      catch (Exception ex)
      {
        DebugUtil.LogErrorArgs((object) "Couldn't serialize", (object) ex.Message, (object) ex.StackTrace);
      }
      return true;
    }
    catch (Exception ex)
    {
      error_cb(new OfflineWorldGen.ErrorInfo()
      {
        errorDesc = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, (object) WorldGen.WORLDGEN_SAVE_FILENAME),
        exception = ex
      });
      DebugUtil.LogErrorArgs((object) "Couldn't write", (object) ex.Message, (object) ex.StackTrace);
      return false;
    }
  }

  public static void LoadSim(
    IReader reader,
    int baseCount,
    List<SimSaveFileStructure> loadedWorlds)
  {
    try
    {
      for (int index = 0; index != baseCount; ++index)
      {
        SimSaveFileStructure saveFileStructure = new SimSaveFileStructure();
        KSerialization.Manager.DeserializeDirectory(reader);
        Deserializer.Deserialize((object) saveFileStructure, reader);
        if (saveFileStructure.worldDetail == null)
          Debug.LogError((object) ("Detail is null for world " + index.ToString()));
        else
          loadedWorlds.Add(saveFileStructure);
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((object) "LoadSim Error!\n", (object) ex.Message, (object) ex.StackTrace);
    }
  }
}
