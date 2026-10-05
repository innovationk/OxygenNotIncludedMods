// Decompiled with JetBrains decompiler
// Type: ArtableSelectionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ArtableSelectionSideScreen : SideScreenContent
{
  private Artable target;
  public KButton applyButton;
  public KButton clearButton;
  public GameObject stateButtonPrefab;
  private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();
  [SerializeField]
  private RectTransform scrollTransoform;
  private string selectedStage = "";
  private const int INVALID_SUBSCRIPTION = -1;
  private int workCompleteSub = -1;
  [SerializeField]
  private RectTransform buttonContainer;

  public override bool IsValidForTarget(GameObject target)
  {
    Artable component = target.GetComponent<Artable>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && !(component.CurrentStage == "Default");
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.applyButton.onClick += (System.Action) (() =>
    {
      this.target.SetUserChosenTargetState(this.selectedStage);
      SelectTool.Instance.Select((KSelectable) null, true);
    });
    this.clearButton.onClick += (System.Action) (() =>
    {
      this.selectedStage = "";
      this.target.SetDefault();
      SelectTool.Instance.Select((KSelectable) null, true);
    });
  }

  public override void SetTarget(GameObject target)
  {
    if (this.workCompleteSub != -1)
    {
      target.Unsubscribe(this.workCompleteSub);
      this.workCompleteSub = -1;
    }
    base.SetTarget(target);
    this.target = target.GetComponent<Artable>();
    this.workCompleteSub = target.Subscribe(-2011693419, new Action<object>(this.OnRefreshTarget));
    this.OnRefreshTarget();
  }

  public override void ClearTarget()
  {
    this.target.Unsubscribe(-2011693419);
    this.workCompleteSub = -1;
    base.ClearTarget();
  }

  private void OnRefreshTarget(object data = null)
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.GenerateStateButtons();
    this.selectedStage = this.target.CurrentStage;
    this.RefreshButtons();
  }

  public void GenerateStateButtons()
  {
    foreach (KeyValuePair<string, MultiToggle> button in this.buttons)
      Util.KDestroyGameObject(button.Value.gameObject);
    this.buttons.Clear();
    foreach (ArtableStage prefabStage in Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID()))
    {
      if (!(prefabStage.id == "Default"))
      {
        GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
        Sprite sprite = prefabStage.GetPermitPresentationInfo().sprite;
        MultiToggle component = gameObject.GetComponent<MultiToggle>();
        component.GetComponent<ToolTip>().SetSimpleTooltip(prefabStage.Name);
        component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
        this.buttons.Add(prefabStage.id, component);
      }
    }
  }

  private void RefreshButtons()
  {
    List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID());
    ArtableStage artableStage = prefabStages.Find((Predicate<ArtableStage>) (match => match.id == this.target.CurrentStage));
    int num = 0;
    foreach (KeyValuePair<string, MultiToggle> button in this.buttons)
    {
      KeyValuePair<string, MultiToggle> kvp = button;
      ArtableStage stage = prefabStages.Find((Predicate<ArtableStage>) (match => match.id == kvp.Key));
      if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
        kvp.Value.gameObject.SetActive(false);
      else if (!stage.IsUnlocked())
      {
        kvp.Value.gameObject.SetActive(false);
      }
      else
      {
        ++num;
        kvp.Value.gameObject.SetActive(true);
        kvp.Value.ChangeState(this.selectedStage == kvp.Key ? 1 : 0);
        kvp.Value.onClick += (System.Action) (() =>
        {
          this.selectedStage = stage.id;
          this.RefreshButtons();
        });
      }
    }
    this.scrollTransoform.GetComponent<LayoutElement>().preferredHeight = num > 3 ? 200f : 100f;
  }
}
