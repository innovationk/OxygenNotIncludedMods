// Decompiled with JetBrains decompiler
// Type: GeothermalPlantComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
  public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";
  public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";
  public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";

  string ICheckboxListGroupControl.Title
  {
    get => (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;
  }

  string ICheckboxListGroupControl.Description
  {
    get => (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;
  }

  public ICheckboxListGroupControl.ListGroup[] GetData()
  {
    ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
    ICheckboxListGroupControl.CheckboxItem[] checkboxItems = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
    for (int index = 0; index < checkboxItems.Length; ++index)
    {
      ICheckboxListGroupControl.CheckboxItem checkboxItem = new ICheckboxListGroupControl.CheckboxItem();
      bool complete = activateGeothermalPlant.requirementChecklist[index].Success();
      checkboxItem.isOn = complete;
      checkboxItem.text = (activateGeothermalPlant.requirementChecklist[index] as VictoryColonyAchievementRequirement).Name();
      checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[index].GetProgress(complete);
      checkboxItems[index] = checkboxItem;
    }
    return new ICheckboxListGroupControl.ListGroup[1]
    {
      new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, checkboxItems)
    };
  }

  public bool SidescreenEnabled() => true;

  public int CheckboxSideScreenSortOrder() => 100;

  public static bool GeothermalControllerRepaired()
  {
    return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
  }

  public static bool GeothermalFacilityDiscovered()
  {
    return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
  }

  protected override void OnSpawn()
  {
    this.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public static void DisplayPopup(
    string title,
    string desc,
    HashedString anim,
    System.Action onDismissCallback,
    Transform clickFocus = null)
  {
    EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
    if (Components.LiveMinionIdentities.Count >= 2)
    {
      int idx = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
      int num = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
      eventInfoData.minions = new GameObject[2]
      {
        Components.LiveMinionIdentities[idx].gameObject,
        Components.LiveMinionIdentities[(idx + num) % Components.LiveMinionIdentities.Count].gameObject
      };
    }
    else if (Components.LiveMinionIdentities.Count == 1)
      eventInfoData.minions = new GameObject[1]
      {
        Components.LiveMinionIdentities[0].gameObject
      };
    eventInfoData.AddDefaultOption(onDismissCallback);
    eventInfoData.clickFocus = clickFocus;
    EventInfoScreen.ShowPopup(eventInfoData);
  }

  protected void RevealAllVentsAndController()
  {
    WorldGenSpawner worldGenSpawner1 = SaveGame.Instance.worldGenSpawner;
    Tag[] tagArray1 = new Tag[1]
    {
      (Tag) "GeothermalVentEntity"
    };
    foreach (WorldGenSpawner.Spawnable spawnable in worldGenSpawner1.GetSpawnablesWithTag(true, tagArray1))
    {
      int x;
      int y;
      Grid.CellToXY(spawnable.cell, out x, out y);
      GridVisibility.Reveal(x, y + 2, 5, 5f);
    }
    WorldGenSpawner worldGenSpawner2 = SaveGame.Instance.worldGenSpawner;
    Tag[] tagArray2 = new Tag[1]
    {
      (Tag) "GeothermalControllerEntity"
    };
    foreach (WorldGenSpawner.Spawnable spawnable in worldGenSpawner2.GetSpawnablesWithTag(true, tagArray2))
    {
      int x;
      int y;
      Grid.CellToXY(spawnable.cell, out x, out y);
      GridVisibility.Reveal(x, y + 3, 7, 7f);
    }
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  protected void OnObjectSelect(object _)
  {
    this.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
    if (SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
      return;
    SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
    GeothermalPlantComponent.DisplayPopup((string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, (HashedString) "geothermalplantintro_kanim", new System.Action(this.RevealAllVentsAndController));
  }

  public static void OnVentingHotMaterial(int worldid)
  {
    foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(worldid))
    {
      if (geothermalVent.IsQuestEntombed())
      {
        geothermalVent.SetQuestComplete();
        if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
        {
          GeothermalVictorySequence.VictoryVent = geothermalVent;
          GeothermalPlantComponent.DisplayPopup((string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, (HashedString) "geothermalplantachievement_kanim", (System.Action) (() => SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true));
          break;
        }
      }
    }
  }

  public List<KSelectable> GetRelatedEntities()
  {
    List<KSelectable> relatedEntities = new List<KSelectable>();
    int myWorldId = this.GetMyWorldId();
    foreach (GeothermalController geothermalController in Components.GeothermalControllers.GetItems(myWorldId))
      relatedEntities.Add(geothermalController.GetComponent<KSelectable>());
    foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(myWorldId))
      relatedEntities.Add(geothermalVent.GetComponent<KSelectable>());
    return relatedEntities;
  }
}
