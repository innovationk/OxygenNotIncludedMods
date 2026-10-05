// Decompiled with JetBrains decompiler
// Type: ColonyDiagnosticUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColonyDiagnosticUtility : KMonoBehaviour, ISim1000ms
{
  public static ColonyDiagnosticUtility Instance;
  private Dictionary<int, List<ColonyDiagnostic>> worldDiagnostics = new Dictionary<int, List<ColonyDiagnostic>>();
  [Serialize]
  public Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySettings = new Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>>();
  [Serialize]
  public Dictionary<int, Dictionary<string, List<string>>> diagnosticCriteriaDisabled = new Dictionary<int, Dictionary<string, List<string>>>();
  [Serialize]
  private Dictionary<string, float> diagnosticTutorialStatus = new Dictionary<string, float>()
  {
    {
      "ToiletDiagnostic",
      450f
    },
    {
      "BedDiagnostic",
      900f
    },
    {
      "BreathabilityDiagnostic",
      1800f
    },
    {
      "FoodDiagnostic",
      3000f
    },
    {
      "FarmDiagnostic",
      6000f
    },
    {
      "StressDiagnostic",
      9000f
    },
    {
      "PowerUseDiagnostic",
      12000f
    },
    {
      "BatteryDiagnostic",
      12000f
    },
    {
      "IdleDiagnostic",
      600f
    }
  };
  public static bool IgnoreFirstUpdate = true;
  public static ColonyDiagnostic.DiagnosticResult NoDataResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.NO_DATA);

  public static void DestroyInstance()
  {
    ColonyDiagnosticUtility.Instance = (ColonyDiagnosticUtility) null;
  }

  public ColonyDiagnostic.DiagnosticResult.Opinion GetWorldDiagnosticResult(int worldID)
  {
    ColonyDiagnostic.DiagnosticResult.Opinion val1 = ColonyDiagnostic.DiagnosticResult.Opinion.Good;
    foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
    {
      if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
      {
        switch (this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id])
        {
          case ColonyDiagnosticUtility.DisplaySetting.Always:
          case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
            val1 = (ColonyDiagnostic.DiagnosticResult.Opinion) Math.Min((int) val1, (int) colonyDiagnostic.LatestResult.opinion);
            continue;
          default:
            continue;
        }
      }
    }
    return val1;
  }

  public string GetWorldDiagnosticResultStatus(int worldID)
  {
    ColonyDiagnostic colonyDiagnostic1 = (ColonyDiagnostic) null;
    foreach (ColonyDiagnostic colonyDiagnostic2 in this.worldDiagnostics[worldID])
    {
      if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic2.id))
      {
        switch (this.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id])
        {
          case ColonyDiagnosticUtility.DisplaySetting.Always:
          case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
            if (colonyDiagnostic1 == null || colonyDiagnostic2.LatestResult.opinion < colonyDiagnostic1.LatestResult.opinion)
            {
              colonyDiagnostic1 = colonyDiagnostic2;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    return colonyDiagnostic1 == null || colonyDiagnostic1.LatestResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal ? "" : colonyDiagnostic1.name;
  }

  public string GetWorldDiagnosticResultTooltip(int worldID)
  {
    string diagnosticResultTooltip = "";
    foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
    {
      if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
      {
        switch (this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id])
        {
          case ColonyDiagnosticUtility.DisplaySetting.Always:
          case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
            if (colonyDiagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
            {
              diagnosticResultTooltip = $"{diagnosticResultTooltip}\n{colonyDiagnostic.LatestResult.GetFormattedMessage()}";
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    return diagnosticResultTooltip;
  }

  public bool IsDiagnosticTutorialDisabled(string id)
  {
    return ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id) && (double) GameClock.Instance.GetTime() < (double) ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id];
  }

  public void ClearDiagnosticTutorialSetting(string id)
  {
    if (!ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id))
      return;
    ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id] = -1f;
  }

  public bool IsCriteriaEnabled(int worldID, string diagnosticID, string criteriaID)
  {
    Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
    return dictionary.ContainsKey(diagnosticID) && !dictionary[diagnosticID].Contains(criteriaID);
  }

  public void SetCriteriaEnabled(
    int worldID,
    string diagnosticID,
    string criteriaID,
    bool enabled)
  {
    Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
    Debug.Assert(dictionary.ContainsKey(diagnosticID), (object) $"Trying to set criteria on World {worldID} lacks diagnostic {diagnosticID} that criteria {criteriaID} relates to");
    List<string> stringList = dictionary[diagnosticID];
    if (enabled && stringList.Contains(criteriaID))
      stringList.Remove(criteriaID);
    if (enabled || stringList.Contains(criteriaID))
      return;
    stringList.Add(criteriaID);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ColonyDiagnosticUtility.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 33))
    {
      string key1 = "IdleDiagnostic";
      foreach (int key2 in this.diagnosticDisplaySettings.Keys)
      {
        WorldContainer world = ClusterManager.Instance.GetWorld(key2);
        if (this.diagnosticDisplaySettings[key2].ContainsKey(key1) && this.diagnosticDisplaySettings[key2][key1] != ColonyDiagnosticUtility.DisplaySetting.Always)
          this.diagnosticDisplaySettings[key2][key1] = world.IsModuleInterior ? ColonyDiagnosticUtility.DisplaySetting.Never : ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
      }
    }
    foreach (int worldID in ClusterManager.Instance.GetWorldIDsSorted())
      this.AddWorld(worldID);
    ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
    ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
  }

  private void Refresh(object data) => this.AddWorld(((Boxed<int>) data).value);

  private void RemoveWorld(object data)
  {
    int key = ((Boxed<int>) data).value;
    if (!this.diagnosticDisplaySettings.Remove(key))
      return;
    List<ColonyDiagnostic> colonyDiagnosticList;
    if (this.worldDiagnostics.TryGetValue(key, out colonyDiagnosticList))
    {
      foreach (ColonyDiagnostic colonyDiagnostic in colonyDiagnosticList)
        colonyDiagnostic.OnCleanUp();
    }
    this.worldDiagnostics.Remove(key);
  }

  public ColonyDiagnostic GetDiagnostic(string id, int worldID)
  {
    return this.worldDiagnostics[worldID].Find((Predicate<ColonyDiagnostic>) (match => match.id == id));
  }

  public T GetDiagnostic<T>(int worldID) where T : ColonyDiagnostic
  {
    return (T) this.worldDiagnostics[worldID].Find((Predicate<ColonyDiagnostic>) (match => match is T));
  }

  public string GetDiagnosticName(string id)
  {
    foreach (KeyValuePair<int, List<ColonyDiagnostic>> worldDiagnostic in this.worldDiagnostics)
    {
      foreach (ColonyDiagnostic colonyDiagnostic in worldDiagnostic.Value)
      {
        if (colonyDiagnostic.id == id)
          return colonyDiagnostic.name;
      }
    }
    Debug.LogWarning((object) $"Cannot locate name of diagnostic {id} because no worlds have a diagnostic with that id ");
    return "";
  }

  public ChoreGroupDiagnostic GetChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup)
  {
    return (ChoreGroupDiagnostic) this.worldDiagnostics[worldID].Find((Predicate<ColonyDiagnostic>) (match => match is ChoreGroupDiagnostic && ((ChoreGroupDiagnostic) match).choreGroup == choreGroup));
  }

  public WorkTimeDiagnostic GetWorkTimeDiagnostic(int worldID, ChoreGroup choreGroup)
  {
    return (WorkTimeDiagnostic) this.worldDiagnostics[worldID].Find((Predicate<ColonyDiagnostic>) (match => match is WorkTimeDiagnostic && ((WorkTimeDiagnostic) match).choreGroup == choreGroup));
  }

  private void TryAddDiagnosticToWorldCollection(
    ref List<ColonyDiagnostic> newWorldDiagnostics,
    ColonyDiagnostic newDiagnostic)
  {
    if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) newDiagnostic))
      newDiagnostic.OnCleanUp();
    else
      newWorldDiagnostics.Add(newDiagnostic);
  }

  public void AddWorld(int worldID)
  {
    bool flag = false;
    if (!this.diagnosticDisplaySettings.ContainsKey(worldID))
    {
      this.diagnosticDisplaySettings.Add(worldID, new Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>());
      flag = true;
    }
    if (!this.diagnosticCriteriaDisabled.ContainsKey(worldID))
      this.diagnosticCriteriaDisabled.Add(worldID, new Dictionary<string, List<string>>());
    List<ColonyDiagnostic> newWorldDiagnostics = new List<ColonyDiagnostic>();
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new IdleDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new BreathabilityDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new FoodDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new StressDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new RadiationDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new ReactorDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new SelfChargingElectrobankDiagnostic(worldID));
    this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new BionicBatteryDiagnostic(worldID));
    if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
    {
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new FloatingRocketDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new RocketFuelDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new RocketOxidizerDiagnostic(worldID));
    }
    else
    {
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new BedDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new ToiletDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new PowerUseDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new BatteryDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new TrappedDuplicantDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new FarmDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new EntombedDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new RocketsInOrbitDiagnostic(worldID));
      this.TryAddDiagnosticToWorldCollection(ref newWorldDiagnostics, (ColonyDiagnostic) new MeteorDiagnostic(worldID));
    }
    this.worldDiagnostics.Add(worldID, newWorldDiagnostics);
    foreach (ColonyDiagnostic colonyDiagnostic in newWorldDiagnostics)
    {
      if (!this.diagnosticDisplaySettings[worldID].ContainsKey(colonyDiagnostic.id))
        this.diagnosticDisplaySettings[worldID].Add(colonyDiagnostic.id, ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
      if (!this.diagnosticCriteriaDisabled[worldID].ContainsKey(colonyDiagnostic.id))
        this.diagnosticCriteriaDisabled[worldID].Add(colonyDiagnostic.id, new List<string>());
    }
    if (!flag)
      return;
    this.diagnosticDisplaySettings[worldID]["BreathabilityDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
    this.diagnosticDisplaySettings[worldID]["FoodDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
    this.diagnosticDisplaySettings[worldID]["StressDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
    if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
    {
      this.diagnosticDisplaySettings[worldID]["FloatingRocketDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
      this.diagnosticDisplaySettings[worldID]["RocketFuelDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
      this.diagnosticDisplaySettings[worldID]["RocketOxidizerDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
      this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Never;
    }
    else
      this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
  }

  public void Sim1000ms(float dt)
  {
    if (!ColonyDiagnosticUtility.IgnoreFirstUpdate)
      return;
    ColonyDiagnosticUtility.IgnoreFirstUpdate = false;
  }

  public static bool PastNewBuildingGracePeriod(Transform building)
  {
    BuildingComplete component = building.GetComponent<BuildingComplete>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || (double) GameClock.Instance.GetTime() - (double) component.creationTime >= 600.0;
  }

  public static bool IgnoreRocketsWithNoCrewRequested(
    int worldID,
    out ColonyDiagnostic.DiagnosticResult result)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
    string message = (string) (world.IsModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID);
    result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, message);
    if (world.IsModuleInterior)
    {
      for (int idx = 0; idx < Components.Clustercrafts.Count; ++idx)
      {
        WorldContainer interiorWorld = Components.Clustercrafts[idx].ModuleInterface.GetInteriorWorld();
        if (!((UnityEngine.Object) interiorWorld == (UnityEngine.Object) null) && interiorWorld.id == worldID)
        {
          PassengerRocketModule passengerModule = Components.Clustercrafts[idx].ModuleInterface.GetPassengerModule();
          if ((UnityEngine.Object) passengerModule != (UnityEngine.Object) null && !passengerModule.ShouldCrewGetIn())
          {
            result = new ColonyDiagnostic.DiagnosticResult();
            result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
            result.Message = (string) UI.COLONY_DIAGNOSTICS.NO_MINIONS_REQUESTED;
            return true;
          }
        }
      }
    }
    return false;
  }

  public enum DisplaySetting
  {
    Always,
    AlertOnly,
    Never,
    LENGTH,
  }
}
