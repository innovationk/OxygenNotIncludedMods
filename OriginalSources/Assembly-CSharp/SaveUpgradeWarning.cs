// Decompiled with JetBrains decompiler
// Type: SaveUpgradeWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SaveUpgradeWarning")]
public class SaveUpgradeWarning : KMonoBehaviour
{
  [MyCmpReq]
  private Game game;
  private static string[] buildingIDsWithNewPorts = new string[6]
  {
    "LiquidVent",
    "GasVent",
    "GasVentHighPressure",
    "SolidVent",
    "LiquidReservoir",
    "GasReservoir"
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.game.OnLoad += new Action<Game.GameSaveData>(this.OnLoad);
  }

  protected override void OnCleanUp()
  {
    this.game.OnLoad -= new Action<Game.GameSaveData>(this.OnLoad);
    base.OnCleanUp();
  }

  private void OnLoad(Game.GameSaveData data)
  {
    List<SaveUpgradeWarning.Upgrade> upgradeList = new List<SaveUpgradeWarning.Upgrade>()
    {
      new SaveUpgradeWarning.Upgrade(7, 5, new System.Action(this.SuddenMoraleHelper)),
      new SaveUpgradeWarning.Upgrade(7, 13, new System.Action(this.BedAndBathHelper)),
      new SaveUpgradeWarning.Upgrade(7, 16 /*0x10*/, new System.Action(this.NewAutomationWarning)),
      new SaveUpgradeWarning.Upgrade(7, 32 /*0x20*/, new System.Action(this.SpaceScannersAndTelescopeUpdateWarning)),
      new SaveUpgradeWarning.Upgrade(7, 33, new System.Action(this.U50CritterWarning))
    };
    if (DlcManager.IsPureVanilla())
      upgradeList.Add(new SaveUpgradeWarning.Upgrade(7, 25, new System.Action(this.MergedownWarning)));
    foreach (SaveUpgradeWarning.Upgrade upgrade in upgradeList)
    {
      if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(upgrade.major, upgrade.minor))
        upgrade.action();
    }
  }

  private void SuddenMoraleHelper()
  {
    Effect morale_effect = Db.Get().effects.Get(nameof (SuddenMoraleHelper));
    CustomizableDialogScreen screen = Util.KInstantiateUI<CustomizableDialogScreen>(ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_BUFF, (System.Action) (() =>
    {
      foreach (Component component in Components.LiveMinionIdentities.Items)
        component.GetComponent<Effects>().Add(morale_effect, true);
      screen.Deactivate();
    }));
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_DISABLE, (System.Action) (() =>
    {
      SettingConfig morale = CustomGameSettingConfigs.Morale;
      CustomGameSettings.Instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
      CustomGameSettings.Instance.SetQualitySetting(morale, morale.GetLevel("Disabled").id);
      screen.Deactivate();
    }));
    screen.PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER, (object) Mathf.RoundToInt(morale_effect.duration / 600f)), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_TITLE);
  }

  private void BedAndBathHelper()
  {
    if ((UnityEngine.Object) SaveGame.Instance == (UnityEngine.Object) null)
      return;
    ColonyAchievementTracker achievementTracker = SaveGame.Instance.ColonyAchievementTracker;
    if ((UnityEngine.Object) achievementTracker == (UnityEngine.Object) null)
      return;
    ColonyAchievement basicComforts = Db.Get().ColonyAchievements.BasicComforts;
    ColonyAchievementStatus achievementStatus = (ColonyAchievementStatus) null;
    if (!achievementTracker.achievements.TryGetValue(basicComforts.Id, out achievementStatus))
      return;
    achievementStatus.failed = false;
  }

  private void NewAutomationWarning()
  {
    SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.CONFIRMDIALOG.OK, (System.Action) (() => screen.Deactivate()));
    foreach (string buildingIdsWithNewPort in SaveUpgradeWarning.buildingIDsWithNewPorts)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(buildingIdsWithNewPort);
      screen.AddListRow(buildingDef.GetUISprite(), buildingDef.Name, 150f, 50f);
    }
    screen.PopupConfirmDialog((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING, (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING_TITLE);
    this.StartCoroutine(this.SendAutomationWarningNotifications());
  }

  private IEnumerator SendAutomationWarningNotifications()
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    if (Components.BuildingCompletes.Count == 0)
      Debug.LogWarning((object) "Could not send automation warnings because buildings have not yet loaded");
    foreach (BuildingComplete buildingComplete in Components.BuildingCompletes)
    {
      foreach (string buildingIdsWithNewPort in SaveUpgradeWarning.buildingIDsWithNewPorts)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(buildingIdsWithNewPort);
        if ((UnityEngine.Object) buildingComplete.Def == (UnityEngine.Object) buildingDef)
        {
          List<ILogicUIElement> logicUiElementList = new List<ILogicUIElement>();
          LogicPorts component = buildingComplete.GetComponent<LogicPorts>();
          if (component.outputPorts != null)
            logicUiElementList.AddRange((IEnumerable<ILogicUIElement>) component.outputPorts);
          if (component.inputPorts != null)
            logicUiElementList.AddRange((IEnumerable<ILogicUIElement>) component.inputPorts);
          foreach (ILogicUIElement logicUiElement in logicUiElementList)
          {
            if ((UnityEngine.Object) Grid.Objects[logicUiElement.GetLogicUICell(), 31 /*0x1F*/] != (UnityEngine.Object) null)
            {
              Debug.Log((object) ("Triggering automation warning for building of type " + buildingIdsWithNewPort));
              GenericMessage genericMessage = new GenericMessage((string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.NAME, (string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, (string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, (KMonoBehaviour) buildingComplete);
              Messenger.Instance.QueueMessage((Message) genericMessage);
            }
          }
        }
      }
    }
  }

  private IEnumerator TemporaryDisableMeteorShowers(float timeOffDurationInCycles)
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    float timeToSleepUntil = GameUtil.GetCurrentTimeInCycles() + timeOffDurationInCycles;
    foreach (GameplayEvent resource in Db.Get().GameplayEvents.resources)
    {
      if (resource is MeteorShowerEvent && !(resource.Id == Db.Get().GameplayEvents.GassyMooteorEvent.Id))
        resource.SetSleepTimer(timeToSleepUntil);
    }
    if (DlcManager.IsPureVanilla())
    {
      List<GameplayEventInstance> results = new List<GameplayEventInstance>();
      GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(ref results);
      foreach (GameplayEventInstance eventInstance in results)
        GameplayEventManager.Instance.RemoveActiveEvent(eventInstance, "Cancelled by SaveUpgradeWarning for player's convenience by providing a window of time without meteors to allow players to adapt to new updates made to relevant buildings");
    }
  }

  private void SpaceScannersAndTelescopeUpdateWarning()
  {
    SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.CONFIRMDIALOG.OK, (System.Action) (() => screen.Deactivate()));
    screen.AddListRow(Assets.GetSprite((HashedString) "space_scanner_range"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "telescope_range"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TELESCOPES, 150f, 120f);
    screen.PopupConfirmDialog($"{(string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SUMMARY}\n\n{(string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_WARNING}", (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TITLE);
    this.StartCoroutine(this.TemporaryDisableMeteorShowers(20f));
  }

  private void U50CritterWarning()
  {
    SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.CONFIRMDIALOG.OK, (System.Action) (() => screen.Deactivate()));
    screen.AddListRow(Assets.GetSprite((HashedString) "u50_critter_moods"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_MOOD, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "u50_pacu"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_PACU, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "u50_suit_checkpoints"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_SUITCHECKPOINTS, 150f, 120f);
    screen.PopupConfirmDialog((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_SUMMARY, (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_TITLE);
  }

  private void MergedownWarning()
  {
    SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.DEVELOPMENTBUILDS.FULL_PATCH_NOTES, (System.Action) (() => App.OpenWebURL("https://forums.kleientertainment.com/game-updates/oni-alpha/")));
    screen.AddOption((string) UI.CONFIRMDIALOG.OK, (System.Action) (() => screen.Deactivate()));
    screen.AddListRow(Assets.GetSprite((HashedString) "upgrade_mergedown_fridge"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_FOOD, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "upgrade_mergedown_deodorizer"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_AIRFILTER, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "upgrade_mergedown_steamturbine"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_SIMULATION, 150f, 120f);
    screen.AddListRow(Assets.GetSprite((HashedString) "upgrade_mergedown_oxygen_meter"), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_BUILDINGS, 150f, 120f);
    screen.PopupConfirmDialog((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES, (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_TITLE);
    this.StartCoroutine(this.SendAutomationWarningNotifications());
  }

  private struct Upgrade(int major, int minor, System.Action action)
  {
    public int major = major;
    public int minor = minor;
    public System.Action action = action;
  }
}
