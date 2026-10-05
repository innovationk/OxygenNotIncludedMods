// Decompiled with JetBrains decompiler
// Type: NToggleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NToggleSideScreen : SideScreenContent
{
  [SerializeField]
  private KToggle buttonPrefab;
  [SerializeField]
  private LocText description;
  private INToggleSideScreenControl target;
  private List<KToggle> buttonList = new List<KToggle>();

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<INToggleSideScreenControl>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.target = target.GetComponent<INToggleSideScreenControl>();
    if (this.target == null)
      return;
    this.titleKey = this.target.SidescreenTitleKey;
    this.gameObject.SetActive(true);
    this.Refresh();
  }

  private void Refresh()
  {
    for (int index = 0; index < Mathf.Max(this.target.Options.Count, this.buttonList.Count); ++index)
    {
      if (index >= this.target.Options.Count)
      {
        this.buttonList[index].gameObject.SetActive(false);
      }
      else
      {
        if (index >= this.buttonList.Count)
        {
          KToggle ktoggle = Util.KInstantiateUI<KToggle>(this.buttonPrefab.gameObject, this.ContentContainer);
          int idx = index;
          ktoggle.onClick += (System.Action) (() =>
          {
            this.target.QueueSelectedOption(idx);
            this.Refresh();
          });
          this.buttonList.Add(ktoggle);
        }
        this.buttonList[index].GetComponentInChildren<LocText>().text = (string) this.target.Options[index];
        this.buttonList[index].GetComponentInChildren<ToolTip>().toolTip = (string) this.target.Tooltips[index];
        if (this.target.SelectedOption == index && this.target.QueuedOption == index)
        {
          this.buttonList[index].isOn = true;
          foreach (ImageToggleState componentsInChild in this.buttonList[index].GetComponentsInChildren<ImageToggleState>())
            componentsInChild.SetActive();
          this.buttonList[index].GetComponent<ImageToggleStateThrobber>().enabled = false;
        }
        else if (this.target.QueuedOption == index)
        {
          this.buttonList[index].isOn = true;
          foreach (ImageToggleState componentsInChild in this.buttonList[index].GetComponentsInChildren<ImageToggleState>())
            componentsInChild.SetActive();
          this.buttonList[index].GetComponent<ImageToggleStateThrobber>().enabled = true;
        }
        else
        {
          this.buttonList[index].isOn = false;
          foreach (ImageToggleState componentsInChild in this.buttonList[index].GetComponentsInChildren<ImageToggleState>())
          {
            componentsInChild.SetInactive();
            componentsInChild.SetInactive();
          }
          this.buttonList[index].GetComponent<ImageToggleStateThrobber>().enabled = false;
        }
        this.buttonList[index].gameObject.SetActive(true);
      }
    }
    this.description.text = this.target.Description;
    this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.target.Description));
  }
}
