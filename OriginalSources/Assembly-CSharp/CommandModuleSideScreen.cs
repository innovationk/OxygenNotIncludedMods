// Decompiled with JetBrains decompiler
// Type: CommandModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CommandModuleSideScreen : SideScreenContent
{
  private LaunchConditionManager target;
  public GameObject conditionListContainer;
  public GameObject prefabConditionLineItem;
  public MultiToggle destinationButton;
  public MultiToggle debugVictoryButton;
  [Tooltip("This list is indexed by the ProcessCondition.Status enum")]
  public List<Color> statusColors;
  private Dictionary<ProcessCondition, GameObject> conditionTable = new Dictionary<ProcessCondition, GameObject>();
  private SchedulerHandle updateHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ScheduleUpdate();
    this.debugVictoryButton.onClick += (System.Action) (() =>
    {
      SpaceDestination destination = SpacecraftManager.instance.destinations.Find((Predicate<SpaceDestination>) (match => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole));
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
      this.target.Launch(destination);
    });
    this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
  }

  private bool CheckHydrogenRocket()
  {
    RocketModule rocketModule = this.target.rocketModules.Find((Predicate<RocketModule>) (match => (bool) (UnityEngine.Object) match.GetComponent<RocketEngine>()));
    return (UnityEngine.Object) rocketModule != (UnityEngine.Object) null && rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
  }

  private void ScheduleUpdate()
  {
    this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, (Action<object>) (o =>
    {
      this.RefreshConditions();
      this.ScheduleUpdate();
    }), (object) null, (SchedulerGroup) null);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LaunchConditionManager>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<LaunchConditionManager>();
      if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a LaunchConditionManager component");
      }
      else
      {
        this.ClearConditions();
        this.ConfigureConditions();
        this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
      }
    }
  }

  private void ClearConditions()
  {
    foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
      Util.KDestroyGameObject(keyValuePair.Value);
    this.conditionTable.Clear();
  }

  private void ConfigureConditions()
  {
    foreach (ProcessCondition launchCondition in this.target.GetLaunchConditionList())
      this.conditionTable.Add(launchCondition, Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true));
    this.RefreshConditions();
  }

  public void RefreshConditions()
  {
    bool flag1 = false;
    List<ProcessCondition> launchConditionList = this.target.GetLaunchConditionList();
    foreach (ProcessCondition key in launchConditionList)
    {
      if (this.conditionTable.ContainsKey(key))
      {
        GameObject gameObject = this.conditionTable[key];
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        if (key.GetParentCondition() != null && key.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
          gameObject.SetActive(false);
        else if (!gameObject.activeSelf)
          gameObject.SetActive(true);
        ProcessCondition.Status condition = key.EvaluateCondition();
        bool flag2 = condition == ProcessCondition.Status.Ready;
        component.GetReference<LocText>("Label").text = key.GetStatusMessage(condition);
        component.GetReference<LocText>("Label").color = flag2 ? Color.black : Color.red;
        component.GetReference<Image>("Box").color = flag2 ? Color.black : Color.red;
        component.GetReference<Image>("Check").gameObject.SetActive(flag2);
        gameObject.GetComponent<ToolTip>().SetSimpleTooltip(key.GetStatusTooltip(condition));
      }
      else
      {
        flag1 = true;
        break;
      }
    }
    foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
    {
      if (!launchConditionList.Contains(keyValuePair.Key))
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
    {
      this.ClearConditions();
      this.ConfigureConditions();
    }
    this.destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
    this.destinationButton.onClick = (System.Action) (() => ManagementMenu.Instance.ToggleStarmap());
  }

  protected override void OnCleanUp()
  {
    this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }
}
