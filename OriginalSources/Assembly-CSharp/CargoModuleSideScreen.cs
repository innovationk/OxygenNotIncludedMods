// Decompiled with JetBrains decompiler
// Type: CargoModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CargoModuleSideScreen : SideScreenContent, ISimEveryTick
{
  private Clustercraft targetCraft;
  private Dictionary<IHexCellCollector, GameObject> modulePanels = new Dictionary<IHexCellCollector, GameObject>();
  public GameObject moduleContentContainer;
  public GameObject modulePanelPrefab;
  [SerializeField]
  private LayoutElement scrollRectLayout;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.ConsumeMouseScroll = true;
  }

  public override float GetSortKey() => 21f;

  public override bool IsValidForTarget(GameObject target)
  {
    return (Object) target.GetComponent<Clustercraft>() != (Object) null && this.GetCollectionModules(target.GetComponent<Clustercraft>()).Length != 0;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetCraft = target.GetComponent<Clustercraft>();
    this.RefreshModulePanel(this.targetCraft);
  }

  private IHexCellCollector[] GetCollectionModules(Clustercraft craft)
  {
    List<IHexCellCollector> hexCellCollectorList = new List<IHexCellCollector>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) craft.ModuleInterface.ClusterModules)
    {
      IHexCellCollector hexCellCollector = clusterModule.Get().GetComponent<IHexCellCollector>() ?? clusterModule.Get().GetSMI<IHexCellCollector>();
      if (hexCellCollector != null)
        hexCellCollectorList.Add(hexCellCollector);
    }
    return hexCellCollectorList.ToArray();
  }

  private void RefreshModulePanel(Clustercraft module)
  {
    this.ClearModules();
    foreach (IHexCellCollector collectionModule in this.GetCollectionModules(module))
    {
      GameObject gameObject = Util.KInstantiateUI(this.modulePanelPrefab, this.moduleContentContainer, true);
      this.modulePanels.Add(collectionModule, gameObject);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<Image>("icon").sprite = collectionModule.GetUISprite();
      component.GetReference<LocText>("label").SetText(collectionModule.GetProperName());
    }
    this.RefreshProgressBars();
    LayoutElement scrollRectLayout1 = this.scrollRectLayout;
    LayoutElement scrollRectLayout2 = this.scrollRectLayout;
    double num1 = (double) Mathf.Min((float) this.modulePanels.Count, 2.5f);
    double height = (double) this.modulePanelPrefab.GetComponent<RectTransform>().rect.height;
    double num2;
    float num3 = (float) (num2 = num1 * height);
    scrollRectLayout2.minHeight = (float) num2;
    double num4 = (double) num3;
    scrollRectLayout1.preferredHeight = (float) num4;
  }

  private void ClearModules()
  {
    foreach (KeyValuePair<IHexCellCollector, GameObject> modulePanel in this.modulePanels)
      Util.KDestroyGameObject(modulePanel.Value.gameObject);
    this.modulePanels.Clear();
  }

  private void RefreshProgressBars()
  {
    if (this.targetCraft.IsNullOrDestroyed() || (Object) ClusterMapSelectTool.Instance.GetSelected() == (Object) null || !this.IsValidForTarget(ClusterMapSelectTool.Instance.GetSelected().gameObject))
      return;
    foreach (KeyValuePair<IHexCellCollector, GameObject> modulePanel in this.modulePanels)
    {
      HierarchyReferences component = modulePanel.Value.GetComponent<HierarchyReferences>();
      GenericUIProgressBar reference1 = component.GetReference<GenericUIProgressBar>("gatheringProgressBar");
      float num1 = 4f;
      float num2 = modulePanel.Key.GetCapacity() - modulePanel.Key.GetMassStored();
      if (modulePanel.Key.CheckIsCollecting())
      {
        float num3 = modulePanel.Key.TimeInState() % num1;
        if ((double) num2 > 0.0)
        {
          reference1.SetFillPercentage(num3 / num1);
          reference1.label.SetText((string) STRINGS.UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_IN_PROGRESS);
        }
      }
      else if ((double) num2 == 0.0)
      {
        reference1.SetFillPercentage(0.0f);
        reference1.label.SetText((string) STRINGS.UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_FULL);
      }
      else
      {
        reference1.SetFillPercentage(0.0f);
        reference1.label.SetText((string) STRINGS.UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_STOPPED);
      }
      GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("capacityProgressBar");
      reference2.SetFillPercentage(modulePanel.Key.GetMassStored() / modulePanel.Key.GetCapacity());
      reference2.label.SetText(modulePanel.Key.GetCapacityBarText());
    }
  }

  public void SimEveryTick(float dt) => this.RefreshProgressBars();
}
