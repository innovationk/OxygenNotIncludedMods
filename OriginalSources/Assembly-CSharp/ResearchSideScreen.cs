// Decompiled with JetBrains decompiler
// Type: ResearchSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResearchSideScreen : SideScreenContent
{
  public KButton selectResearchButton;
  public Image researchButtonIcon;
  public GameObject content;
  private GameObject target;
  private Action<object> refreshDisplayStateDelegate;
  public LocText DescriptionText;

  public ResearchSideScreen()
  {
    this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.selectResearchButton.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleResearch());
    Research.Instance.Subscribe(-1914338957, this.refreshDisplayStateDelegate);
    Research.Instance.Subscribe(-125623018, this.refreshDisplayStateDelegate);
    this.RefreshDisplayState();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshDisplayState();
    this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
    this.target.gameObject.Subscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Subscribe(-592767678, this.refreshDisplayStateDelegate);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!(bool) (UnityEngine.Object) this.target)
      return;
    this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
    this.target = (GameObject) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Research.Instance.Unsubscribe(-1914338957, this.refreshDisplayStateDelegate);
    Research.Instance.Unsubscribe(-125623018, this.refreshDisplayStateDelegate);
    if (!(bool) (UnityEngine.Object) this.target)
      return;
    this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
    this.target = (GameObject) null;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<ResearchCenter>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<NuclearResearchCenter>() != (UnityEngine.Object) null;
  }

  private void RefreshDisplayState(object _ = null)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
      return;
    string str1 = "";
    ResearchCenter component1 = SelectTool.Instance.selected.GetComponent<ResearchCenter>();
    NuclearResearchCenter component2 = SelectTool.Instance.selected.GetComponent<NuclearResearchCenter>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      str1 = component1.research_point_type_id;
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      str1 = component2.researchTypeID;
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    this.researchButtonIcon.sprite = Research.Instance.researchTypes.GetResearchType(str1).sprite;
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch == null)
    {
      this.DescriptionText.text = $"<b>{(string) STRINGS.UI.UISIDESCREENS.RESEARCHSIDESCREEN.NOSELECTEDRESEARCH}</b>";
    }
    else
    {
      string str2 = "";
      if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(str1) || (double) activeResearch.tech.costsByResearchTypeID[str1] <= 0.0)
        str2 += "<color=#7f7f7f>";
      string str3 = $"{str2}<b>{activeResearch.tech.Name}</b>";
      if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(str1) || (double) activeResearch.tech.costsByResearchTypeID[str1] <= 0.0)
        str3 += "</color>";
      foreach (KeyValuePair<string, float> keyValuePair in activeResearch.tech.costsByResearchTypeID)
      {
        if ((double) keyValuePair.Value != 0.0)
        {
          bool flag = keyValuePair.Key == str1;
          str3 += "\n   ";
          str3 += "<b>";
          if (!flag)
            str3 += "<color=#7f7f7f>";
          str3 = $"{str3}- {Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name}: {activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key].ToString()}/{activeResearch.tech.costsByResearchTypeID[keyValuePair.Key].ToString()}";
          if (!flag)
            str3 += "</color>";
          str3 += "</b>";
        }
      }
      this.DescriptionText.text = str3;
    }
  }
}
