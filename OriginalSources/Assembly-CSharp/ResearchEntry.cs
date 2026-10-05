// Decompiled with JetBrains decompiler
// Type: ResearchEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
  [Header("Labels")]
  [SerializeField]
  private LocText researchName;
  [Header("Transforms")]
  [SerializeField]
  private Transform progressBarContainer;
  [SerializeField]
  private Transform lineContainer;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject iconPanel;
  [SerializeField]
  private GameObject iconPrefab;
  [SerializeField]
  private GameObject linePrefab;
  [SerializeField]
  private GameObject progressBarPrefab;
  [Header("Graphics")]
  [SerializeField]
  private Image BG;
  [SerializeField]
  private Image titleBG;
  [SerializeField]
  private Image borderHighlight;
  [SerializeField]
  private Image filterHighlight;
  [SerializeField]
  private Image filterLowlight;
  [SerializeField]
  private Sprite hoverBG;
  [SerializeField]
  private Sprite completedBG;
  [Header("Colors")]
  [SerializeField]
  private Color defaultColor = Color.blue;
  [SerializeField]
  private Color completedColor = Color.yellow;
  [SerializeField]
  private Color pendingColor = Color.magenta;
  [SerializeField]
  private Color completedHeaderColor = Color.grey;
  [SerializeField]
  private Color incompleteHeaderColor = Color.grey;
  [SerializeField]
  private Color pendingHeaderColor = Color.grey;
  private Sprite defaultBG;
  [MyCmpGet]
  private KToggle toggle;
  private ResearchScreen researchScreen;
  private Dictionary<Tech, UILineRenderer> techLineMap;
  private Tech targetTech;
  private bool isOn = true;
  private Coroutine fadeRoutine;
  public Color activeLineColor;
  public Color inactiveLineColor;
  public int lineThickness_active = 6;
  public int lineThickness_inactive = 2;
  public Material StandardUIMaterial;
  public ResearchScreen researchScreenReference;
  private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();
  public static readonly string UnlockedTechKey = "UnlockedTech";
  private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>()
  {
    {
      ResearchEntry.UnlockedTechKey,
      (object) null
    }
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.techLineMap = new Dictionary<Tech, UILineRenderer>();
    this.BG.color = this.defaultColor;
    this.QueueStateChanged(false);
    if (this.targetTech != null)
    {
      foreach (TechInstance research in Research.Instance.GetResearchQueue())
      {
        if (research.tech == this.targetTech)
          this.QueueStateChanged(true);
      }
    }
    this.StartCoroutine(this.DelayedSetupForLines());
  }

  private IEnumerator DelayedSetupForLines()
  {
    yield return (object) null;
    yield return (object) null;
    this.SetupLines();
  }

  public void SetupLines()
  {
    foreach (Tech tech in this.targetTech.requiredTech)
    {
      float num1 = this.researchScreenReference.GetEntry(this.targetTech).rectTransform().rect.width / 2f;
      float num2 = this.researchScreenReference.GetEntry(tech).rectTransform().rect.width / 2f;
      Vector2 vector2_1 = Vector2.zero;
      Vector2 vector2_2 = Vector2.zero;
      if ((double) tech.center.y > (double) this.targetTech.center.y + 2.0)
      {
        vector2_1 = new Vector2(0.0f, 20f);
        vector2_2 = new Vector2(0.0f, -20f);
      }
      else if ((double) tech.center.y < (double) this.targetTech.center.y - 2.0)
      {
        vector2_1 = new Vector2(0.0f, -20f);
        vector2_2 = new Vector2(0.0f, 20f);
      }
      UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
      float num3 = 32f;
      component.Points = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f) + vector2_1,
        new Vector2(-num3, 0.0f) + vector2_1,
        new Vector2(-num3, tech.center.y - this.targetTech.center.y) + vector2_2,
        new Vector2((float) (-((double) this.targetTech.center.x - (double) num1 - ((double) tech.center.x + (double) num2)) - 4.0), tech.center.y - this.targetTech.center.y) + vector2_2
      };
      component.LineThickness = (float) this.lineThickness_inactive;
      component.color = this.inactiveLineColor;
      this.techLineMap.Add(tech, component);
    }
  }

  public void SetTech(Tech newTech)
  {
    if (newTech == null)
    {
      Debug.LogError((object) "The research provided is null!");
    }
    else
    {
      if (this.targetTech == newTech)
        return;
      foreach (ResearchType type in Research.Instance.researchTypes.Types)
      {
        if (newTech.costsByResearchTypeID.ContainsKey(type.id) && (double) newTech.costsByResearchTypeID[type.id] > 0.0)
        {
          GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
          Image componentsInChild = gameObject.GetComponentsInChildren<Image>()[2];
          Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
          componentsInChild.color = type.color;
          Sprite sprite = type.sprite;
          component.sprite = sprite;
          this.progressBarsByResearchTypeID[type.id] = gameObject;
        }
      }
      if ((UnityEngine.Object) this.researchScreen == (UnityEngine.Object) null)
        this.researchScreen = this.transform.parent.GetComponentInParent<ResearchScreen>();
      if (newTech.IsComplete())
        this.ResearchCompleted(false);
      this.targetTech = newTech;
      this.researchName.text = this.targetTech.Name;
      string str1 = "";
      foreach (TechItem unlockedItem in this.targetTech.unlockedItems)
      {
        if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) unlockedItem))
        {
          HierarchyReferences component = this.GetFreeIcon().GetComponent<HierarchyReferences>();
          if (str1 != "")
            str1 += ", ";
          str1 += unlockedItem.Name;
          component.GetReference<KImage>("Icon").sprite = unlockedItem.UISprite();
          component.GetReference<KImage>("Background");
          KImage reference = component.GetReference<KImage>("DLCOverlay");
          bool flag = unlockedItem.requiredDlcIds != null;
          reference.gameObject.SetActive(flag);
          if (flag)
            reference.color = DlcManager.GetDlcBannerColor(unlockedItem.requiredDlcIds[unlockedItem.requiredDlcIds.Length - 1]);
          string str2 = $"{unlockedItem.Name}\n{unlockedItem.description}";
          if (flag)
          {
            str2 += "\n";
            foreach (string requiredDlcId in unlockedItem.requiredDlcIds)
              str2 += string.Format((string) RESEARCH.MESSAGING.DLC.DLC_CONTENT, (object) DlcManager.GetDlcTitle(requiredDlcId));
          }
          component.GetComponent<ToolTip>().toolTip = str2;
        }
      }
      string str3 = string.Format((string) STRINGS.UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, (object) str1);
      this.researchName.GetComponent<ToolTip>().toolTip = $"{this.targetTech.Name}\n{this.targetTech.desc}\n\n{str3}";
      this.toggle.ClearOnClick();
      this.toggle.onClick += new System.Action(this.OnResearchClicked);
      this.toggle.onPointerEnter += (KToggle.PointerEvent) (() =>
      {
        this.researchScreen.TurnEverythingOff();
        this.OnHover(true, this.targetTech);
      });
      this.toggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() => !this.targetTech.IsComplete());
      this.toggle.onPointerExit += (KToggle.PointerEvent) (() => this.researchScreen.TurnEverythingOff());
    }
  }

  public void SetEverythingOff()
  {
    if (!this.isOn)
      return;
    this.borderHighlight.gameObject.SetActive(false);
    foreach (KeyValuePair<Tech, UILineRenderer> techLine in this.techLineMap)
    {
      techLine.Value.LineThickness = (float) this.lineThickness_inactive;
      techLine.Value.Points[0].x = 0.0f;
      techLine.Value.color = this.inactiveLineColor;
    }
    this.isOn = false;
  }

  public void SetEverythingOn()
  {
    if (this.isOn)
      return;
    this.UpdateProgressBars();
    this.borderHighlight.gameObject.SetActive(true);
    foreach (KeyValuePair<Tech, UILineRenderer> techLine in this.techLineMap)
    {
      techLine.Value.LineThickness = (float) this.lineThickness_active;
      techLine.Value.Points[0].x = (float) -this.lineThickness_inactive;
      techLine.Value.color = this.activeLineColor;
    }
    this.transform.SetAsLastSibling();
    this.isOn = true;
  }

  public void OnHover(bool entered, Tech hoverSource)
  {
    this.SetEverythingOn();
    foreach (Tech tech in this.targetTech.requiredTech)
    {
      ResearchEntry entry = this.researchScreen.GetEntry(tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.OnHover(entered, this.targetTech);
    }
  }

  private void OnResearchClicked()
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch != null && activeResearch.tech != this.targetTech)
      this.researchScreen.CancelResearch();
    Research.Instance.SetActiveResearch(this.targetTech, true);
    if (DebugHandler.InstantBuildMode)
      Research.Instance.CompleteQueue();
    this.UpdateProgressBars();
  }

  private void OnResearchCanceled()
  {
    if (this.targetTech.IsComplete())
      return;
    this.toggle.ClearOnClick();
    this.toggle.onClick += new System.Action(this.OnResearchClicked);
    this.researchScreen.CancelResearch();
    Research.Instance.CancelResearch(this.targetTech);
  }

  public void QueueStateChanged(bool isSelected)
  {
    if (isSelected)
    {
      if (!this.targetTech.IsComplete())
      {
        this.toggle.isOn = true;
        this.BG.color = this.pendingColor;
        this.titleBG.color = this.pendingHeaderColor;
        this.toggle.ClearOnClick();
        this.toggle.onClick += new System.Action(this.OnResearchCanceled);
      }
      else
        this.toggle.isOn = false;
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
      foreach (Graphic componentsInChild in this.iconPanel.GetComponentsInChildren<Image>())
        componentsInChild.material = this.StandardUIMaterial;
    }
    else if (this.targetTech.IsComplete())
    {
      this.toggle.isOn = false;
      this.BG.color = this.completedColor;
      this.titleBG.color = this.completedHeaderColor;
      this.defaultColor = this.completedColor;
      this.toggle.ClearOnClick();
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
      foreach (Graphic componentsInChild in this.iconPanel.GetComponentsInChildren<Image>())
        componentsInChild.material = this.StandardUIMaterial;
    }
    else
    {
      this.toggle.isOn = false;
      this.BG.color = this.defaultColor;
      this.titleBG.color = this.incompleteHeaderColor;
      this.toggle.ClearOnClick();
      this.toggle.onClick += new System.Action(this.OnResearchClicked);
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.521568656f, 0.521568656f, 0.521568656f);
    }
  }

  public void UpdateFilterState(bool state) => this.filterLowlight.gameObject.SetActive(!state);

  public void SetPercentage(float percent)
  {
  }

  public void UpdateProgressBars()
  {
    foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
    {
      Transform child = keyValuePair.Value.transform.GetChild(0);
      float num1;
      float num2;
      if (this.targetTech.IsComplete())
      {
        num1 = 1f;
        LocText componentInChildren = child.GetComponentInChildren<LocText>();
        num2 = this.targetTech.costsByResearchTypeID[keyValuePair.Key];
        string str1 = num2.ToString();
        num2 = this.targetTech.costsByResearchTypeID[keyValuePair.Key];
        string str2 = num2.ToString();
        string str3 = $"{str1}/{str2}";
        componentInChildren.text = str3;
      }
      else
      {
        TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
        if (orAdd != null)
        {
          LocText componentInChildren = child.GetComponentInChildren<LocText>();
          num2 = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key];
          string str4 = num2.ToString();
          num2 = this.targetTech.costsByResearchTypeID[keyValuePair.Key];
          string str5 = num2.ToString();
          string str6 = $"{str4}/{str5}";
          componentInChildren.text = str6;
          num1 = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
        }
        else
          continue;
      }
      child.GetComponentsInChildren<Image>()[2].fillAmount = num1;
      child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
    }
  }

  private GameObject GetFreeIcon()
  {
    GameObject freeIcon = Util.KInstantiateUI(this.iconPrefab, this.iconPanel);
    freeIcon.SetActive(true);
    return freeIcon;
  }

  private Image GetFreeLine()
  {
    return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, this.gameObject);
  }

  public void ResearchCompleted(bool notify = true)
  {
    this.BG.color = this.completedColor;
    this.titleBG.color = this.completedHeaderColor;
    this.defaultColor = this.completedColor;
    if (notify)
    {
      this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = (object) this.targetTech.Id;
      ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, nameof (ResearchCompleted));
    }
    this.toggle.ClearOnClick();
    if (!notify)
      return;
    ResearchCompleteMessage researchCompleteMessage = new ResearchCompleteMessage(this.targetTech);
    MusicManager.instance.PlaySong("Stinger_ResearchComplete");
    Messenger.Instance.QueueMessage((Message) researchCompleteMessage);
  }
}
