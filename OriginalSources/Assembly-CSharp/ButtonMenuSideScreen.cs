// Decompiled with JetBrains decompiler
// Type: ButtonMenuSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ButtonMenuSideScreen : SideScreenContent
{
  public const int DefaultButtonMenuSideScreenSortOrder = 20;
  public LayoutElement buttonPrefab;
  public LayoutElement horizontalButtonPrefab;
  public GameObject horizontalGroupPrefab;
  public RectTransform buttonContainer;
  private List<GameObject> liveButtons = new List<GameObject>();
  private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();
  private List<ISidescreenButtonControl> targets;

  public override bool IsValidForTarget(GameObject target)
  {
    ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>() ?? target.GetSMI<ISidescreenButtonControl>();
    return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
  }

  public override int GetSideScreenSortOrder()
  {
    return this.targets == null ? 20 : this.targets[0].ButtonSideScreenSortOrder();
  }

  public override string GetTitle()
  {
    if (this.targets != null)
    {
      foreach (ISidescreenButtonControl target in this.targets)
      {
        string sidescreenTitle = target.SidescreenTitle;
        if (sidescreenTitle != null)
          return sidescreenTitle;
      }
    }
    return base.GetTitle();
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
      this.targets.AddRange((IEnumerable<ISidescreenButtonControl>) new_target.GetComponents<ISidescreenButtonControl>());
      this.Refresh();
    }
  }

  public GameObject GetHorizontalGroup(int id)
  {
    if (!this.horizontalGroups.ContainsKey(id))
      this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
    return this.horizontalGroups[id];
  }

  public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
  {
    to.ignoreLayout = from.ignoreLayout;
    to.minWidth = from.minWidth;
    to.minHeight = from.minHeight;
    to.preferredWidth = from.preferredWidth;
    to.preferredHeight = from.preferredHeight;
    to.flexibleWidth = from.flexibleWidth;
    to.flexibleHeight = from.flexibleHeight;
    to.layoutPriority = from.layoutPriority;
  }

  private void Refresh()
  {
    while (this.liveButtons.Count < this.targets.Count)
      this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
    foreach (int key in this.horizontalGroups.Keys)
      this.horizontalGroups[key].SetActive(false);
    for (int index = 0; index < this.liveButtons.Count; ++index)
    {
      if (index >= this.targets.Count)
      {
        this.liveButtons[index].SetActive(false);
      }
      else
      {
        if (!this.liveButtons[index].activeSelf)
          this.liveButtons[index].SetActive(true);
        int id = this.targets[index].HorizontalGroupID();
        LayoutElement component = this.liveButtons[index].GetComponent<LayoutElement>();
        KButton componentInChildren1 = this.liveButtons[index].GetComponentInChildren<KButton>();
        ToolTip componentInChildren2 = this.liveButtons[index].GetComponentInChildren<ToolTip>();
        LocText componentInChildren3 = this.liveButtons[index].GetComponentInChildren<LocText>();
        if (id >= 0)
        {
          GameObject horizontalGroup = this.GetHorizontalGroup(id);
          horizontalGroup.SetActive(true);
          this.liveButtons[index].transform.SetParent(horizontalGroup.transform, false);
          this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
        }
        else
        {
          this.liveButtons[index].transform.SetParent((Transform) this.buttonContainer, false);
          this.CopyLayoutSettings(component, this.buttonPrefab);
        }
        componentInChildren1.isInteractable = this.targets[index].SidescreenButtonInteractable();
        componentInChildren1.ClearOnClick();
        componentInChildren1.onClick += new System.Action(this.targets[index].OnSidescreenButtonPressed);
        componentInChildren1.onClick += new System.Action(this.Refresh);
        componentInChildren3.SetText(this.targets[index].SidescreenButtonText);
        componentInChildren2.SetSimpleTooltip(this.targets[index].SidescreenButtonTooltip);
      }
    }
  }
}
