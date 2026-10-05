// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using ProcGen;
using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class ColonyAchievement : Resource, IHasDlcRestrictions
{
  public string description;
  public bool isVictoryCondition;
  public string messageTitle;
  public string messageBody;
  public string shortVideoName;
  public string loopVideoName;
  public string platformAchievementId;
  public string icon;
  public string clusterTag;
  public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();
  public Action<KMonoBehaviour> victorySequence;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;
  public string dlcIdFrom;

  public EventReference victoryNISSnapshot { get; private set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public ColonyAchievement()
  {
    this.Id = "Disabled";
    this.platformAchievementId = "Disabled";
    this.Name = "Disabled";
    this.description = "Disabled";
    this.isVictoryCondition = false;
    this.requirementChecklist = new List<ColonyAchievementRequirement>();
    this.messageTitle = string.Empty;
    this.messageBody = string.Empty;
    this.shortVideoName = string.Empty;
    this.loopVideoName = string.Empty;
    this.platformAchievementId = string.Empty;
    this.icon = string.Empty;
    this.clusterTag = string.Empty;
    this.Disabled = true;
  }

  public ColonyAchievement(
    string Id,
    string platformAchievementId,
    string Name,
    string description,
    bool isVictoryCondition,
    List<ColonyAchievementRequirement> requirementChecklist,
    string messageTitle = "",
    string messageBody = "",
    string videoDataName = "",
    string victoryLoopVideo = "",
    Action<KMonoBehaviour> VictorySequence = null,
    EventReference victorySnapshot = default (EventReference),
    string icon = "",
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    string dlcIdFrom = null,
    string clusterTag = null)
    : base(Id, Name)
  {
    this.Id = Id;
    this.platformAchievementId = platformAchievementId;
    this.Name = Name;
    this.description = description;
    this.isVictoryCondition = isVictoryCondition;
    this.requirementChecklist = requirementChecklist;
    this.messageTitle = messageTitle;
    this.messageBody = messageBody;
    this.shortVideoName = videoDataName;
    this.loopVideoName = victoryLoopVideo;
    this.victorySequence = VictorySequence;
    this.victoryNISSnapshot = victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot;
    this.icon = icon;
    this.clusterTag = clusterTag;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
    this.dlcIdFrom = dlcIdFrom;
  }

  public bool IsValidForSave()
  {
    if (this.clusterTag.IsNullOrWhiteSpace())
      return true;
    DebugUtil.Assert((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null, "IsValidForSave called when CustomGamesSettings is not initialized.");
    ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
    return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(this.clusterTag);
  }
}
