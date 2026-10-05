// Decompiled with JetBrains decompiler
// Type: AutoRocketUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class AutoRocketUtility
{
  public static void StartAutoRocket(LaunchPad selectedPad)
  {
    selectedPad.StartCoroutine(AutoRocketUtility.AutoRocketRoutine(selectedPad));
  }

  private static IEnumerator AutoRocketRoutine(LaunchPad selectedPad)
  {
    GameObject oxidizerTank = AutoRocketUtility.AddOxidizerTank(AutoRocketUtility.AddEngine(selectedPad));
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    AutoRocketUtility.AddOxidizer(oxidizerTank);
    GameObject baseModule = AutoRocketUtility.AddPassengerModule(oxidizerTank);
    AutoRocketUtility.AddDrillCone(AutoRocketUtility.AddSolidStorageModule(baseModule));
    PassengerRocketModule passengerModule = baseModule.GetComponent<PassengerRocketModule>();
    ClustercraftExteriorDoor exteriorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>();
    int max = 100;
    while ((Object) exteriorDoor.GetInteriorDoor() == (Object) null && max > 0)
    {
      --max;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    WorldContainer interiorWorld = passengerModule.GetComponent<RocketModuleCluster>().CraftInterface.GetInteriorWorld();
    RocketControlStation worldItem = Components.RocketControlStations.GetWorldItems(interiorWorld.id)[0];
    GameObject minion = AutoRocketUtility.AddPilot(worldItem);
    AutoRocketUtility.AddOxygen(worldItem);
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    AutoRocketUtility.AssignCrew(minion, passengerModule);
  }

  private static GameObject AddEngine(LaunchPad selectedPad)
  {
    BuildingDef buildingDef = Assets.GetBuildingDef("KeroseneEngineClusterSmall");
    List<Tag> elements = new List<Tag>()
    {
      SimHashes.Steel.CreateTag()
    };
    GameObject gameObject = selectedPad.AddBaseModule(buildingDef, (IList<Tag>) elements);
    Element element = ElementLoader.GetElement(gameObject.GetComponent<RocketEngineCluster>().fuelTag);
    Storage component = gameObject.GetComponent<Storage>();
    if (element.IsGas)
    {
      component.AddGasChunk(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false);
      return gameObject;
    }
    if (element.IsLiquid)
    {
      component.AddLiquid(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0);
      return gameObject;
    }
    if (!element.IsSolid)
      return gameObject;
    component.AddOre(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0);
    return gameObject;
  }

  private static GameObject AddPassengerModule(GameObject baseModule)
  {
    ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
    BuildingDef buildingDef = Assets.GetBuildingDef("HabitatModuleMedium");
    List<Tag> tagList = new List<Tag>()
    {
      SimHashes.Cuprite.CreateTag()
    };
    BuildingDef def = buildingDef;
    List<Tag> buildMaterials = tagList;
    return component.AddModule(def, (IList<Tag>) buildMaterials);
  }

  private static GameObject AddSolidStorageModule(GameObject baseModule)
  {
    ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
    BuildingDef buildingDef = Assets.GetBuildingDef("SolidCargoBaySmall");
    List<Tag> tagList = new List<Tag>()
    {
      SimHashes.Steel.CreateTag()
    };
    BuildingDef def = buildingDef;
    List<Tag> buildMaterials = tagList;
    return component.AddModule(def, (IList<Tag>) buildMaterials);
  }

  private static GameObject AddDrillCone(GameObject baseModule)
  {
    ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
    BuildingDef buildingDef = Assets.GetBuildingDef("NoseconeHarvest");
    List<Tag> tagList = new List<Tag>()
    {
      SimHashes.Steel.CreateTag(),
      SimHashes.Polypropylene.CreateTag()
    };
    BuildingDef def = buildingDef;
    List<Tag> buildMaterials = tagList;
    GameObject gameObject = component.AddModule(def, (IList<Tag>) buildMaterials);
    gameObject.GetComponent<Storage>().AddOre(SimHashes.Diamond, 1000f, 273f, byte.MaxValue, 0);
    return gameObject;
  }

  private static GameObject AddOxidizerTank(GameObject baseModule)
  {
    ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
    BuildingDef buildingDef = Assets.GetBuildingDef("SmallOxidizerTank");
    List<Tag> tagList = new List<Tag>()
    {
      SimHashes.Cuprite.CreateTag()
    };
    BuildingDef def = buildingDef;
    List<Tag> buildMaterials = tagList;
    return component.AddModule(def, (IList<Tag>) buildMaterials);
  }

  private static void AddOxidizer(GameObject oxidizerTank)
  {
    SimHashes simHashes = SimHashes.OxyRock;
    Element elementByHash = ElementLoader.FindElementByHash(simHashes);
    DiscoveredResources.Instance.Discover(elementByHash.tag, elementByHash.GetMaterialCategoryTag());
    oxidizerTank.GetComponent<OxidizerTank>().DEBUG_FillTank(simHashes);
  }

  private static GameObject AddPilot(RocketControlStation station)
  {
    MinionStartingStats minionStartingStats = new MinionStartingStats(false, isDebugMinion: true);
    Vector3 position = station.transform.position;
    GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
    GameObject gameObject = Util.KInstantiate(prefab);
    gameObject.name = prefab.name;
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Move);
    gameObject.transform.SetLocalPosition(posCbc);
    gameObject.SetActive(true);
    minionStartingStats.Apply(gameObject);
    MinionResume component = gameObject.GetComponent<MinionResume>();
    if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
      component.ForceAddSkillPoint();
    string id = Db.Get().Skills.RocketPiloting1.Id;
    MinionResume.SkillMasteryConditions[] masteryConditions = component.GetSkillMasteryConditions(id);
    bool flag = component.CanMasterSkill(masteryConditions);
    if (((!((Object) component != (Object) null) ? 0 : (!component.HasMasteredSkill(id) ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      component.MasterSkill(id);
    return gameObject;
  }

  private static void AddOxygen(RocketControlStation station)
  {
    SimMessages.ReplaceElement(Grid.PosToCell(station.transform.position + Vector3.up * 2f), SimHashes.OxyRock, CellEventLogger.Instance.DebugTool, 1000f, 273f);
  }

  private static void AssignCrew(GameObject minion, PassengerRocketModule passengerModule)
  {
    for (int idx = 0; idx < Components.MinionAssignablesProxy.Count; ++idx)
    {
      if ((Object) Components.MinionAssignablesProxy[idx].GetTargetGameObject() == (Object) minion)
      {
        passengerModule.GetComponent<AssignmentGroupController>().SetMember(Components.MinionAssignablesProxy[idx], true);
        break;
      }
    }
    passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
  }

  private static void SetDestination(
    CraftModuleInterface craftModuleInterface,
    PassengerRocketModule passengerModule)
  {
    craftModuleInterface.GetComponent<ClusterDestinationSelector>().SetDestination(passengerModule.GetMyWorldLocation() + AxialI.NORTHEAST);
  }
}
