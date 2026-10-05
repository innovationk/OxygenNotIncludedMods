// Decompiled with JetBrains decompiler
// Type: ConditionListSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ConditionListSideScreen : SideScreenContent
{
  public GameObject rowPrefab;
  public GameObject rowContainer;
  [Tooltip("This list is indexed by the ProcessCondition.Status enum")]
  public static Color readyColor = Color.black;
  public static Color failedColor = Color.red;
  public static Color warningColor = new Color(1f, 0.3529412f, 0.0f, 1f);
  private IProcessConditionSet targetConditionSet;
  private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();

  public override bool IsValidForTarget(GameObject target) => false;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    if (!((Object) target != (Object) null))
      return;
    this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.Refresh();
  }

  private void Refresh()
  {
    bool flag = false;
    List<ProcessCondition> v;
    using (ProcessCondition.ListPool.Get(out v))
    {
      this.targetConditionSet.PopulateConditionSet(ProcessCondition.ProcessConditionType.All, v);
      foreach (ProcessCondition key in v)
      {
        if (!this.rows.ContainsKey(key))
        {
          flag = true;
          break;
        }
      }
      foreach (KeyValuePair<ProcessCondition, GameObject> row in this.rows)
      {
        if (!v.Contains(row.Key))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        this.Rebuild();
      foreach (KeyValuePair<ProcessCondition, GameObject> row in this.rows)
        ConditionListSideScreen.SetRowState(row.Value, row.Key);
    }
  }

  public static void SetRowState(GameObject row, ProcessCondition condition)
  {
    HierarchyReferences component = row.GetComponent<HierarchyReferences>();
    ProcessCondition.Status condition1 = condition.EvaluateCondition();
    component.GetReference<LocText>("Label").text = condition.GetStatusMessage(condition1);
    switch (condition1)
    {
      case ProcessCondition.Status.Failure:
        component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
        component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
        break;
      case ProcessCondition.Status.Warning:
        component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
        component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
        break;
      case ProcessCondition.Status.Ready:
        component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
        component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
        break;
    }
    component.GetReference<Image>("Check").gameObject.SetActive(condition1 == ProcessCondition.Status.Ready);
    component.GetReference<Image>("Dash").gameObject.SetActive(false);
    row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(condition1));
  }

  private void Rebuild()
  {
    this.ClearRows();
    this.BuildRows();
  }

  private void ClearRows()
  {
    foreach (KeyValuePair<ProcessCondition, GameObject> row in this.rows)
      Util.KDestroyGameObject(row.Value);
    this.rows.Clear();
  }

  private void BuildRows()
  {
    List<ProcessCondition> v;
    using (ProcessCondition.ListPool.Get(out v))
    {
      this.targetConditionSet.PopulateConditionSet(ProcessCondition.ProcessConditionType.All, v);
      foreach (ProcessCondition key in v)
      {
        if (key.ShowInUI())
        {
          GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
          this.rows.Add(key, gameObject);
        }
      }
    }
  }
}
