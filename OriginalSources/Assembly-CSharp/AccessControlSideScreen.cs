// Decompiled with JetBrains decompiler
// Type: AccessControlSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AccessControlSideScreen : SideScreenContent
{
  [SerializeField]
  private GameObject entityCategoryPrefab;
  [SerializeField]
  private GameObject rowPrefab;
  [SerializeField]
  private GameObject disabledOverlay;
  [SerializeField]
  private KImage headerBG;
  [SerializeField]
  private GameObject scrollContents;
  private GameObject standardMinionSectionHeader;
  private GameObject standardMinionSectionContent;
  private GameObject bionicMinionSectionHeader;
  private GameObject bionicMinionSectionContent;
  private GameObject robotSectionHeader;
  private GameObject robotSectionContent;
  private AccessControl target;
  private Door doorTarget;
  private bool containersSpawned;
  private List<GameObject> inactiveRowPool = new List<GameObject>();
  private Dictionary<MinionAssignablesProxy, GameObject> minionIdentityRows = new Dictionary<MinionAssignablesProxy, GameObject>();
  private Dictionary<Tag, GameObject> robotRows = new Dictionary<Tag, GameObject>();
  private static Dictionary<Tag, string> categoryNames = new Dictionary<Tag, string>()
  {
    {
      GameTags.Minions.Models.Standard,
      (string) DUPLICANTS.MODEL.STANDARD.NAME_ADJECTIVE
    },
    {
      GameTags.Minions.Models.Bionic,
      (string) DUPLICANTS.MODEL.BIONIC.NAME_ADJECTIVE
    },
    {
      GameTags.Robot,
      (string) ROBOTS.CATEGORY_NAME
    }
  };
  private List<GameObject> setInactiveQueue = new List<GameObject>();
  private bool robotsHasEverBeenOpened;

  public override string GetTitle()
  {
    return (UnityEngine.Object) this.target != (UnityEngine.Object) null ? string.Format(base.GetTitle(), (object) this.target.GetProperName()) : base.GetTitle();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SpawnContainers();
    Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionsChanged));
    Components.LiveMinionIdentities.OnAdd += new Action<MinionIdentity>(this.OnMinionsChanged);
    Components.LiveMinionIdentities.OnRemove += new Action<MinionIdentity>(this.OnMinionsChanged);
  }

  private void OnMinionsChanged(object data)
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.Refresh();
  }

  private void SpawnContainers()
  {
    if (this.containersSpawned)
      return;
    this.standardMinionSectionHeader = Util.KInstantiateUI(this.entityCategoryPrefab, this.scrollContents, true);
    this.standardMinionSectionContent = this.standardMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject;
    this.bionicMinionSectionHeader = Util.KInstantiateUI(this.entityCategoryPrefab, this.scrollContents, true);
    this.bionicMinionSectionContent = this.bionicMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject;
    this.robotSectionHeader = Util.KInstantiateUI(this.entityCategoryPrefab, this.scrollContents, true);
    this.robotSectionContent = this.robotSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject;
    this.containersSpawned = true;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<AccessControl>() != (UnityEngine.Object) null && target.GetComponent<AccessControl>().controlEnabled;
  }

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      this.ClearTarget();
    this.SpawnContainers();
    this.target = target.GetComponent<AccessControl>();
    this.doorTarget = target.GetComponent<Door>();
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
    target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
    this.gameObject.SetActive(true);
    this.RefreshContainerObjects();
    this.Refresh();
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
    this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
  }

  private void Refresh()
  {
    Rotatable component = this.target.GetComponent<Rotatable>();
    int num = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : (component.IsRotated ? 1 : 0);
    this.ClearOldRows();
    this.PopulateRows();
    this.standardMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EmptyRow").gameObject.SetActive(this.standardMinionSectionContent.transform.childCount <= 1);
    if (this.standardMinionSectionContent.transform.childCount <= 1)
      this.ToggleCategoryCollapsed(false, this.standardMinionSectionContent.rectTransform(), this.standardMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("CollapseToggle"));
    this.bionicMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EmptyRow").gameObject.SetActive(this.bionicMinionSectionContent.transform.childCount <= 1);
    if (this.bionicMinionSectionContent.transform.childCount <= 1)
      this.ToggleCategoryCollapsed(false, this.bionicMinionSectionContent.rectTransform(), this.bionicMinionSectionHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("CollapseToggle"));
    this.robotSectionHeader.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EmptyRow").gameObject.SetActive(this.robotSectionContent.transform.childCount <= 1);
    if (!this.robotsHasEverBeenOpened)
      this.ToggleCategoryCollapsed(false, this.robotSectionContent.rectTransform(), this.robotSectionHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("CollapseToggle"));
    foreach (GameObject setInactive in this.setInactiveQueue)
      setInactive.SetActive(false);
    this.disabledOverlay.SetActive(this.target.GetComponent<AccessControl>().overrideAccess == Door.ControlState.Locked);
  }

  private void ClearOldRows()
  {
    foreach (KeyValuePair<MinionAssignablesProxy, GameObject> minionIdentityRow in this.minionIdentityRows)
    {
      this.inactiveRowPool.Add(minionIdentityRow.Value);
      this.setInactiveQueue.Add(minionIdentityRow.Value);
    }
    this.minionIdentityRows.Clear();
    foreach (KeyValuePair<Tag, GameObject> robotRow in this.robotRows)
    {
      this.inactiveRowPool.Add(robotRow.Value);
      this.setInactiveQueue.Add(robotRow.Value);
    }
    this.robotRows.Clear();
  }

  private void RefreshContainerObjects()
  {
    RefreshContainer(this.standardMinionSectionHeader, GameTags.Minions.Models.Standard, true);
    RefreshContainer(this.bionicMinionSectionHeader, GameTags.Minions.Models.Bionic, Game.IsDlcActiveForCurrentSave("DLC3_ID"));
    RefreshContainer(this.robotSectionHeader, GameTags.Robot, true);

    void RefreshContainer(GameObject container, Tag containerTag, bool enabled)
    {
      if (!enabled)
      {
        container.SetActive(false);
      }
      else
      {
        container.SetActive(true);
        HierarchyReferences component = container.GetComponent<HierarchyReferences>();
        MultiToggle reference1 = component.GetReference<MultiToggle>("ToggleLeft");
        MultiToggle reference2 = component.GetReference<MultiToggle>("ToggleRight");
        component.GetReference<LocText>("CategoryLabel");
        MultiToggle collapseToggle = component.GetReference<MultiToggle>("CollapseToggle");
        RectTransform content = component.GetReference<RectTransform>("Content");
        component.GetReference<LocText>("CategoryLabel").SetText(AccessControlSideScreen.categoryNames[containerTag]);
        component.GetReference<ToolTip>("HeaderTooltip").SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.CATEGORY_HEADER_TOOLTIP);
        AccessControl.Permission defaultPermission = this.target.GetDefaultPermission(containerTag);
        bool flag1 = defaultPermission == AccessControl.Permission.Both || defaultPermission == AccessControl.Permission.GoLeft;
        bool flag2 = defaultPermission == AccessControl.Permission.Both || defaultPermission == AccessControl.Permission.GoRight;
        reference1.ChangeState(flag1 ? 0 : 1);
        reference2.ChangeState(flag2 ? 0 : 1);
        reference1.onClick = (System.Action) (() =>
        {
          switch (this.target.GetDefaultPermission(containerTag))
          {
            case AccessControl.Permission.Both:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.GoRight);
              break;
            case AccessControl.Permission.GoLeft:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.Neither);
              break;
            case AccessControl.Permission.GoRight:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.Both);
              break;
            case AccessControl.Permission.Neither:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.GoLeft);
              break;
          }
          this.RefreshContainerObjects();
        });
        reference2.onClick = (System.Action) (() =>
        {
          switch (this.target.GetDefaultPermission(containerTag))
          {
            case AccessControl.Permission.Both:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.GoLeft);
              break;
            case AccessControl.Permission.GoLeft:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.Both);
              break;
            case AccessControl.Permission.GoRight:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.Neither);
              break;
            case AccessControl.Permission.Neither:
              this.target.SetDefaultPermission(containerTag, AccessControl.Permission.GoRight);
              break;
          }
          this.RefreshContainerObjects();
        });
        collapseToggle.onClick = (System.Action) (() =>
        {
          if (containerTag == GameTags.Robot)
            this.robotsHasEverBeenOpened = true;
          this.ToggleCategoryCollapsed(!content.gameObject.activeSelf, content, collapseToggle);
        });
      }
    }
  }

  private void ToggleCategoryCollapsed(
    bool targetState,
    RectTransform content,
    MultiToggle collapseToggle)
  {
    content.gameObject.SetActive(targetState);
    collapseToggle.ChangeState(content.gameObject.activeSelf ? 1 : 0);
  }

  private GameObject InstantiateIndentityRow(GameObject parent)
  {
    if (this.inactiveRowPool.Count <= 0)
      return Util.KInstantiateUI(this.rowPrefab, parent, true);
    GameObject gameObject = this.inactiveRowPool[0];
    this.inactiveRowPool.Remove(gameObject);
    if ((UnityEngine.Object) gameObject.transform.parent != (UnityEngine.Object) parent.transform)
      gameObject.transform.SetParent(parent.transform);
    gameObject.transform.SetAsLastSibling();
    gameObject.SetActive(true);
    if (this.setInactiveQueue.Contains(gameObject))
      this.setInactiveQueue.Remove(gameObject);
    return gameObject;
  }

  private void PopulateRows()
  {
    for (int idx = 0; idx < Components.MinionAssignablesProxy.Count; ++idx)
    {
      MinionAssignablesProxy assignablesProxy = Components.MinionAssignablesProxy[idx];
      if (!assignablesProxy.HasTag(GameTags.Dead))
        this.ConfigureRow((object) assignablesProxy);
    }
    if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
      this.ConfigureRow((object) GameTags.Robots.Models.FetchDrone);
    if (Game.IsDlcActiveForCurrentSave("EXPANSION1_ID"))
      this.ConfigureRow((object) GameTags.Robots.Models.ScoutRover);
    this.ConfigureRow((object) GameTags.Robots.Models.MorbRover);
  }

  private void ConfigureRow(object entity)
  {
    GameObject parent = (GameObject) null;
    MinionAssignablesProxy minion = entity as MinionAssignablesProxy;
    Tag robotTag = GameTags.Robot;
    if (entity is Tag tag)
      robotTag = tag;
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      GameObject targetGameObject = minion.GetTargetGameObject();
      StoredMinionIdentity component = targetGameObject.GetComponent<StoredMinionIdentity>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (component.model == GameTags.Minions.Models.Standard)
          parent = this.standardMinionSectionContent;
        else if (component.model == GameTags.Minions.Models.Bionic)
          parent = this.bionicMinionSectionContent;
      }
      else if (targetGameObject.HasTag(GameTags.Minions.Models.Standard))
        parent = this.standardMinionSectionContent;
      else if (targetGameObject.HasTag(GameTags.Minions.Models.Bionic))
        parent = this.bionicMinionSectionContent;
    }
    else
      parent = this.robotSectionContent;
    GameObject gameObject1 = this.InstantiateIndentityRow(parent);
    HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
    CrewPortrait reference1 = component1.GetReference<CrewPortrait>("Portrait");
    RectTransform reference2 = component1.GetReference<RectTransform>("Icon");
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) reference1.identityObject != (UnityEngine.Object) minion)
        reference1.SetIdentityObject((IAssignableIdentity) minion, false);
      reference1.transform.parent.gameObject.SetActive(true);
      reference2.gameObject.SetActive(false);
    }
    else
    {
      reference1.transform.parent.gameObject.SetActive(false);
      reference2.gameObject.SetActive(true);
      reference2.GetComponent<Image>().sprite = Def.GetUISprite((object) robotTag).first;
      component1.GetReference<LocText>("NameLabel").SetText(robotTag.ProperName());
    }
    MultiToggle reference3 = component1.GetReference<MultiToggle>("UseDefaultButton");
    reference3.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.MINION_SELECT_TOOLTIP);
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      reference3.ChangeState(this.target.IsDefaultPermission(minion) ? 1 : 0);
      component1.GetReference<LocText>("AccessSettingLabel").SetText((string) (this.target.IsDefaultPermission(minion) ? STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM));
    }
    else
    {
      reference3.ChangeState(this.target.IsDefaultPermission(robotTag) ? 1 : 0);
      component1.GetReference<LocText>("AccessSettingLabel").SetText((string) (this.target.IsDefaultPermission(robotTag) ? STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : STRINGS.UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM));
    }
    reference3.onClick = (System.Action) (() =>
    {
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      {
        if (this.target.IsDefaultPermission(minion))
          this.target.SetPermission(minion, this.target.GetDefaultPermission(minion.GetMinionModel()));
        else
          this.target.ClearPermission(minion);
      }
      else if (this.target.IsDefaultPermission(robotTag))
      {
        this.target.ClearPermission(robotTag, GameTags.Robot);
        this.target.SetPermission(robotTag, this.target.GetDefaultPermission(robotTag));
      }
      else
        this.target.ClearPermission(robotTag, GameTags.Robot);
      this.Refresh();
    });
    MultiToggle reference4 = component1.GetReference<MultiToggle>("ToggleLeft");
    MultiToggle reference5 = component1.GetReference<MultiToggle>("ToggleRight");
    AccessControl.Permission permission = !((UnityEngine.Object) minion != (UnityEngine.Object) null) ? this.target.GetSetPermission(robotTag) : this.target.GetSetPermission(minion);
    bool flag1 = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft;
    bool flag2 = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight;
    reference4.ChangeState(flag1 ? 0 : 1);
    reference5.ChangeState(flag2 ? 0 : 1);
    reference4.onClick = (System.Action) (() =>
    {
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      {
        switch (this.target.GetSetPermission(minion))
        {
          case AccessControl.Permission.Both:
            this.target.SetPermission(minion, AccessControl.Permission.GoRight);
            break;
          case AccessControl.Permission.GoLeft:
            this.target.SetPermission(minion, AccessControl.Permission.Neither);
            break;
          case AccessControl.Permission.GoRight:
            this.target.SetPermission(minion, AccessControl.Permission.Both);
            break;
          case AccessControl.Permission.Neither:
            this.target.SetPermission(minion, AccessControl.Permission.GoLeft);
            break;
        }
      }
      else
      {
        switch (this.target.GetSetPermission(robotTag))
        {
          case AccessControl.Permission.Both:
            this.target.SetPermission(robotTag, AccessControl.Permission.GoRight);
            break;
          case AccessControl.Permission.GoLeft:
            this.target.SetPermission(robotTag, AccessControl.Permission.Neither);
            break;
          case AccessControl.Permission.GoRight:
            this.target.SetPermission(robotTag, AccessControl.Permission.Both);
            break;
          case AccessControl.Permission.Neither:
            this.target.SetPermission(robotTag, AccessControl.Permission.GoLeft);
            break;
        }
      }
      this.Refresh();
    });
    reference5.onClick = (System.Action) (() =>
    {
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      {
        switch (this.target.GetSetPermission(minion))
        {
          case AccessControl.Permission.Both:
            this.target.SetPermission(minion, AccessControl.Permission.GoLeft);
            break;
          case AccessControl.Permission.GoLeft:
            this.target.SetPermission(minion, AccessControl.Permission.Both);
            break;
          case AccessControl.Permission.GoRight:
            this.target.SetPermission(minion, AccessControl.Permission.Neither);
            break;
          case AccessControl.Permission.Neither:
            this.target.SetPermission(minion, AccessControl.Permission.GoRight);
            break;
        }
      }
      else
      {
        switch (this.target.GetSetPermission(robotTag))
        {
          case AccessControl.Permission.Both:
            this.target.SetPermission(robotTag, AccessControl.Permission.GoLeft);
            break;
          case AccessControl.Permission.GoLeft:
            this.target.SetPermission(robotTag, AccessControl.Permission.Both);
            break;
          case AccessControl.Permission.GoRight:
            this.target.SetPermission(robotTag, AccessControl.Permission.Neither);
            break;
          case AccessControl.Permission.Neither:
            this.target.SetPermission(robotTag, AccessControl.Permission.GoRight);
            break;
        }
      }
      this.Refresh();
    });
    GameObject gameObject2 = component1.GetReference<RectTransform>("DirectionToggles").gameObject;
    RectTransform reference6 = component1.GetReference<RectTransform>("DittoMark");
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      gameObject2.SetActive(!this.target.IsDefaultPermission(minion));
      reference6.gameObject.SetActive(this.target.IsDefaultPermission(minion));
    }
    else
    {
      gameObject2.SetActive(!this.target.IsDefaultPermission(robotTag));
      reference6.gameObject.SetActive(this.target.IsDefaultPermission(robotTag));
    }
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      this.minionIdentityRows.Add(minion, gameObject1);
    else
      this.robotRows.Add(robotTag, gameObject1);
  }

  private void OnDoorStateChanged(object data) => this.Refresh();

  private void OnAccessControlChanged(object data) => this.Refresh();
}
