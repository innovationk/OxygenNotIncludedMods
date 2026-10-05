// Decompiled with JetBrains decompiler
// Type: MinionSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class MinionSelectScreen : CharacterSelectionController
{
  [SerializeField]
  private NewBaseScreen newBasePrefab;
  [SerializeField]
  private WattsonMessage wattsonMessagePrefab;
  public const string WattsonGameObjName = "WattsonMessage";
  public KButton backButton;

  protected override void OnPrefabInit()
  {
    this.IsStarterMinion = true;
    base.OnPrefabInit();
    if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
      MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f);
    GameObject gameObject = Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, GameObject.Find("ScreenSpaceOverlayCanvas"));
    gameObject.name = "WattsonMessage";
    gameObject.SetActive(false);
    Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
    this.backButton.onClick += (System.Action) (() =>
    {
      LoadScreen.ForceStopGame();
      App.LoadScene("frontend");
    });
    this.InitializeContainers();
    this.StartCoroutine(this.SetDefaultMinionsRoutine());
  }

  private IEnumerator SetDefaultMinionsRoutine()
  {
    yield return (object) SequenceUtil.WaitForNextFrame;
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
    bool aquaticStart = MinionSelectScreen.IsAquaticStartWorld(clusterData);
    if (clusterData.startingMinions != null)
    {
      DebugUtil.Assert(clusterData.startingMinions.Length <= 3, "Cannot have more than 3 Minion presets");
      SetupMinion((CharacterContainer) this.containers[2], clusterData.startingMinions.Length != 0 ? clusterData.startingMinions[0] : (string) null);
      SetupMinion((CharacterContainer) this.containers[1], clusterData.startingMinions.Length > 1 ? clusterData.startingMinions[1] : (string) null);
      SetupMinion((CharacterContainer) this.containers[0], clusterData.startingMinions.Length > 2 ? clusterData.startingMinions[2] : (string) null);
    }

    void SetupMinion(CharacterContainer container, string specificMinion)
    {
      if (specificMinion != null)
        container.SetMinion(new MinionStartingStats(Db.Get().Personalities.Get(specificMinion.ToUpper())));
      else
        container.GenerateCharacter(true);
      if (!aquaticStart)
        return;
      MinionSelectScreen.EnsureSwimmingSkill(container);
      container.OnReshuffled -= new Action<CharacterContainer>(MinionSelectScreen.EnsureSwimmingSkill);
      container.OnReshuffled += new Action<CharacterContainer>(MinionSelectScreen.EnsureSwimmingSkill);
    }
  }

  private static bool IsAquaticStartWorld(ClusterLayout cluster)
  {
    if (cluster == null)
      return false;
    string startWorld = cluster.GetStartWorld();
    if (string.IsNullOrEmpty(startWorld))
      return false;
    ProcGen.World worldData = SettingsCache.worlds.GetWorldData(startWorld);
    return worldData != null && worldData.worldTags != null && worldData.worldTags.Contains("Aquatic");
  }

  private static void EnsureSwimmingSkill(CharacterContainer container)
  {
    if ((UnityEngine.Object) container == (UnityEngine.Object) null)
      return;
    MinionStartingStats stats = container.Stats;
    if (stats == null || stats.Traits == null || stats.personality != null && stats.personality.model == GameTags.Minions.Models.Bionic)
      return;
    foreach (Trait trait in stats.Traits)
    {
      if (trait != null && trait.Id == "GrantSkill_Swimming")
        return;
    }
    Trait trait1 = Db.Get().traits.TryGet("GrantSkill_Swimming");
    if (trait1 == null)
      return;
    int index = stats.Traits.Count > 0 ? 1 : 0;
    stats.Traits.Insert(index, trait1);
    container.SetMinion(stats);
  }

  public void SetProceedButtonActive(bool state, string tooltip = null)
  {
    if (state)
      this.EnableProceedButton();
    else
      this.DisableProceedButton();
    ToolTip component = this.proceedButton.GetComponent<ToolTip>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    if (tooltip != null)
      component.toolTip = tooltip;
    else
      component.ClearMultiStringTooltip();
  }

  protected override void OnSpawn()
  {
    this.OnDeliverableAdded();
    this.EnableProceedButton();
    this.proceedButton.GetComponentInChildren<LocText>().text = (string) UI.IMMIGRANTSCREEN.EMBARK;
    this.containers.ForEach((Action<ITelepadDeliverableContainer>) (container =>
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if (!((UnityEngine.Object) characterContainer != (UnityEngine.Object) null))
        return;
      characterContainer.DisableSelectButton();
    }));
  }

  protected override void OnProceed()
  {
    Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas);
    MusicManager.instance.StopSong("Music_FrontEnd");
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
    int num = 0;
    this.selectedDeliverables.Clear();
    foreach (CharacterContainer container in this.containers)
    {
      this.selectedDeliverables.Add((ITelepadDeliverable) container.Stats);
      if (container.Stats.personality.model == BionicMinionConfig.MODEL)
        ++num;
    }
    NewBaseScreen.Instance.Init(SaveLoader.Instance.Cluster, this.selectedDeliverables.ToArray());
    if (this.OnProceedEvent != null)
      this.OnProceedEvent();
    if (Game.IsDlcActiveForCurrentSave("DLC3_ID") && Components.RoleStations.Count > 0)
    {
      BuildingFacade component = Components.RoleStations[0].GetComponent<BuildingFacade>();
      bool flag = !component.IsOriginal;
      if (num == 3 || !flag && num > 0)
        component.ApplyBuildingFacade(Db.GetBuildingFacades().Get("permit_hqbase_cyberpunk"));
    }
    Game.Instance.Trigger(-838649377, (object) null);
    BuildWatermark.Instance.gameObject.SetActive(false);
    this.Deactivate();
  }

  private void OnBaseAlreadyCreated(object data)
  {
    Game.Instance.StopFE();
    Game.Instance.StartBE();
    Game.Instance.SetGameStarted();
    this.Deactivate();
  }

  private void ReshuffleAll()
  {
    if (this.OnReshuffleEvent == null)
      return;
    this.OnReshuffleEvent(this.IsStarterMinion);
  }

  public override void OnPressBack()
  {
    foreach (ITelepadDeliverableContainer container in this.containers)
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if ((UnityEngine.Object) characterContainer != (UnityEngine.Object) null)
        characterContainer.ForceStopEditingTitle();
    }
  }
}
