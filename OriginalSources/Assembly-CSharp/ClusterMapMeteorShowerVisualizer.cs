// Decompiled with JetBrains decompiler
// Type: ClusterMapMeteorShowerVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
  private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig()
  {
    animFile = Assets.GetAnim((HashedString) "shower_question_mark_kanim"),
    initialAnim = "idle",
    playMode = KAnim.PlayMode.Once
  };
  public string p_name;
  public string clusterAnimName;
  public bool revealed;
  public bool forceRevealed;

  public override string Name => this.p_name;

  public override EntityLayer Layer => EntityLayer.Meteor;

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) this.clusterAnimName),
          initialAnim = this.AnimName,
          animPlaySpeedModifier = 0.5f
        },
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "shower_identify_kanim"),
          initialAnim = "identify_off",
          playMode = KAnim.PlayMode.Once
        },
        this.questionMarkAnimConfig
      };
    }
  }

  public ClusterRevealLevel clusterCellRevealLevel
  {
    get
    {
      ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(this.Location);
      return cellRevealLevel == ClusterRevealLevel.Visible || !this.forceRevealed ? cellRevealLevel : ClusterRevealLevel.Peeked;
    }
  }

  public string UI_ANIM_NAME
  {
    get
    {
      return !this.forceRevealed && (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible) ? "unknown" : "ui";
    }
  }

  public string AnimName
  {
    get
    {
      return !this.forceRevealed && (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible) ? "unknown" : "idle_loop";
    }
  }

  public string QuestionMarkAnimName
  {
    get
    {
      return !this.forceRevealed && (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible) ? this.questionMarkAnimConfig.initialAnim : "off";
    }
  }

  public KBatchedAnimController CreateQuestionMarkInstance(
    KBatchedAnimController origin,
    Transform parent)
  {
    KBatchedAnimController questionMarkInstance = Object.Instantiate<KBatchedAnimController>(origin, parent);
    questionMarkInstance.gameObject.SetActive(true);
    questionMarkInstance.SwapAnims(new KAnimFile[1]
    {
      this.questionMarkAnimConfig.animFile
    });
    questionMarkInstance.Play((HashedString) this.QuestionMarkAnimName);
    questionMarkInstance.gameObject.AddOrGet<ClusterMapIconFixRotation>();
    return questionMarkInstance;
  }

  public override Sprite GetUISprite()
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
      if (animConfigs.Count > 0)
        return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, this.UI_ANIM_NAME);
    }
    else
    {
      WorldContainer component = this.GetComponent<WorldContainer>();
      if ((Object) component != (Object) null)
      {
        ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
        return worldData == null ? (Sprite) null : Assets.GetSprite((HashedString) worldData.asteroidIcon);
      }
    }
    return (Sprite) null;
  }

  protected override void OnCleanUp()
  {
    if ((Object) ClusterMapScreen.Instance != (Object) null)
    {
      ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
      if ((Object) entityVisAnim != (Object) null)
        entityVisAnim.gameObject.SetActive(false);
    }
    base.OnCleanUp();
  }

  public void SetInitialLocation(AxialI startLocation)
  {
    this.m_location = startLocation;
    this.RefreshVisuals();
  }

  public override bool SpaceOutInSameHex() => true;

  public override bool KeepRotationWhenSpacingOutInHex() => true;

  public override bool ShowPath() => this.m_selectable.IsSelected;

  public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
  {
    ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
    switch (levelUsed)
    {
      case ClusterRevealLevel.Hidden:
        this.Deselect();
        break;
      case ClusterRevealLevel.Peeked:
        KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
        if ((Object) firstAnimController != (Object) null)
        {
          firstAnimController.SwapAnims(new KAnimFile[1]
          {
            this.AnimConfigs[0].animFile
          });
          KBatchedAnimController questionMarkInstance = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
          entityVisAnim.ManualAddAnimController(questionMarkInstance);
        }
        this.RefreshVisuals();
        this.Deselect();
        break;
      case ClusterRevealLevel.Visible:
        this.RefreshVisuals();
        break;
    }
    KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
    if (!((Object) animController != (Object) null) || this.revealed)
      return;
    animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
  }

  public void Deselect()
  {
    if (!this.m_selectable.IsSelected)
      return;
    this.m_selectable.Unselect();
  }

  public void RefreshVisuals()
  {
    ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
    if (!((Object) entityVisAnim != (Object) null))
      return;
    KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
    if ((Object) firstAnimController != (Object) null)
      firstAnimController.Play((HashedString) this.AnimName, KAnim.PlayMode.Loop);
    KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
    if (!((Object) animController != (Object) null))
      return;
    animController.Play((HashedString) this.QuestionMarkAnimName);
  }

  public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
  {
    this.revealed = true;
    this.RefreshVisuals();
    if (!playIdentifyAnimationIfVisible)
      return;
    ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
    KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
    entityVisAnim.GetAnimController(2);
    if (!((Object) animController != (Object) null))
      return;
    animController.Play((HashedString) "identify");
  }

  public void PlayHideAnimation()
  {
    this.revealed = false;
    if (!((Object) ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this) != (Object) null))
      return;
    this.RefreshVisuals();
  }
}
