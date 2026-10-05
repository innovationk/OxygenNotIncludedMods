// Decompiled with JetBrains decompiler
// Type: AssignmentGroupControllerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AssignmentGroupControllerSideScreen : KScreen
{
  [SerializeField]
  private GameObject header;
  [SerializeField]
  private GameObject minionRowPrefab;
  [SerializeField]
  private GameObject footer;
  [SerializeField]
  private GameObject minionRowContainer;
  private AssignmentGroupController target;
  private List<GameObject> identityRowMap = new List<GameObject>();

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.RefreshRows();
  }

  protected override void OnCmpDisable()
  {
    for (int index = 0; index < this.identityRowMap.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.identityRowMap[index]);
    this.identityRowMap.Clear();
    base.OnCmpDisable();
  }

  public void SetTarget(GameObject target)
  {
    this.target = target.GetComponent<AssignmentGroupController>();
    this.RefreshRows();
  }

  private void RefreshRows()
  {
    int index1 = 0;
    WorldContainer myWorld1 = this.target.GetMyWorld();
    ClustercraftExteriorDoor component1 = this.target.GetComponent<ClustercraftExteriorDoor>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      myWorld1 = component1.GetInteriorDoor().GetMyWorld();
    List<AssignmentGroupControllerSideScreen.RowSortHelper> rowSortHelperList = new List<AssignmentGroupControllerSideScreen.RowSortHelper>();
    for (int idx = 0; idx < Components.MinionAssignablesProxy.Count; ++idx)
    {
      MinionAssignablesProxy assignablesProxy = Components.MinionAssignablesProxy[idx];
      GameObject targetGameObject = assignablesProxy.GetTargetGameObject();
      WorldContainer myWorld2 = targetGameObject.GetMyWorld();
      if (!((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null) && !targetGameObject.HasTag(GameTags.Dead))
      {
        MinionResume component2 = assignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
        bool flag1 = false;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
          flag1 = true;
        bool flag2 = myWorld2.ParentWorldId == myWorld1.ParentWorldId;
        rowSortHelperList.Add(new AssignmentGroupControllerSideScreen.RowSortHelper()
        {
          minion = assignablesProxy,
          isPilot = flag1,
          isSameWorld = flag2
        });
      }
    }
    rowSortHelperList.Sort((Comparison<AssignmentGroupControllerSideScreen.RowSortHelper>) ((a, b) =>
    {
      int num = b.isSameWorld.CompareTo(a.isSameWorld);
      return num != 0 ? num : b.isPilot.CompareTo(a.isPilot);
    }));
    foreach (AssignmentGroupControllerSideScreen.RowSortHelper rowSortHelper in rowSortHelperList)
    {
      MinionAssignablesProxy minion = rowSortHelper.minion;
      GameObject gameObject;
      if (index1 >= this.identityRowMap.Count)
      {
        gameObject = Util.KInstantiateUI(this.minionRowPrefab, this.minionRowContainer, true);
        this.identityRowMap.Add(gameObject);
      }
      else
      {
        gameObject = this.identityRowMap[index1];
        gameObject.SetActive(true);
      }
      ++index1;
      HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
      MultiToggle toggle = component3.GetReference<MultiToggle>("Toggle");
      toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
      component3.GetReference<CrewPortrait>("Portrait").SetIdentityObject((IAssignableIdentity) minion, false);
      LocText reference1 = component3.GetReference<LocText>("Label");
      LocText reference2 = component3.GetReference<LocText>("Designation");
      if (rowSortHelper.isSameWorld)
      {
        if (rowSortHelper.isPilot)
          reference2.text = (string) UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.PILOT;
        else
          reference2.text = "";
        reference1.color = Color.black;
        reference2.color = Color.black;
      }
      else
      {
        reference1.color = Color.grey;
        reference2.color = Color.grey;
        reference2.text = (string) UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.OFFWORLD;
        gameObject.transform.SetAsLastSibling();
      }
      toggle.onClick = (System.Action) (() =>
      {
        this.target.SetMember(minion, !this.target.CheckMinionIsMember(minion));
        toggle.ChangeState(this.target.CheckMinionIsMember(minion) ? 1 : 0);
        this.RefreshRows();
      });
      string message = this.UpdateToolTip(minion, !rowSortHelper.isSameWorld);
      toggle.GetComponent<ToolTip>().SetSimpleTooltip(message);
    }
    for (int index2 = index1; index2 < this.identityRowMap.Count; ++index2)
      this.identityRowMap[index2].SetActive(false);
    this.minionRowContainer.GetComponent<QuickLayout>().ForceUpdate();
  }

  private string UpdateToolTip(MinionAssignablesProxy minion, bool offworld)
  {
    string str = (string) (this.target.CheckMinionIsMember(minion) ? UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.UNASSIGN : UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.ASSIGN);
    if (offworld)
      str = $"{str}\n\n{UIConstants.ColorPrefixYellow}{(string) UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TOOLTIPS.DIFFERENT_WORLD}{UIConstants.ColorSuffix}";
    return str;
  }

  private struct RowSortHelper
  {
    public MinionAssignablesProxy minion;
    public bool isPilot;
    public bool isSameWorld;
  }
}
