// Decompiled with JetBrains decompiler
// Type: HarvestModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
  private Clustercraft targetCraft;
  public GameObject moduleContentContainer;
  public GameObject modulePanelPrefab;

  private CraftModuleInterface craftModuleInterface
  {
    get => this.targetCraft.GetComponent<CraftModuleInterface>();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.ConsumeMouseScroll = true;
  }

  public override float GetSortKey() => 21f;

  public override bool IsValidForTarget(GameObject target)
  {
    return (Object) target.GetComponent<Clustercraft>() != (Object) null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetCraft = target.GetComponent<Clustercraft>();
    this.RefreshModulePanel((StateMachine.Instance) this.GetResourceHarvestModule(this.targetCraft));
  }

  private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
  {
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) craft.GetComponent<CraftModuleInterface>().ClusterModules)
    {
      GameObject gameObject = clusterModule.Get().gameObject;
      if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
        return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
    }
    return (ResourceHarvestModule.StatesInstance) null;
  }

  private void RefreshModulePanel(StateMachine.Instance module)
  {
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("icon").sprite = Def.GetUISprite((object) module.gameObject).first;
    component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
  }

  public void SimEveryTick(float dt)
  {
    if (this.targetCraft.IsNullOrDestroyed())
      return;
    HierarchyReferences component1 = this.GetComponent<HierarchyReferences>();
    ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
    if (resourceHarvestModule == null)
      return;
    GenericUIProgressBar reference1 = component1.GetReference<GenericUIProgressBar>("progressBar");
    float num1 = 4f;
    float num2 = resourceHarvestModule.timeinstate % num1;
    if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
    {
      reference1.SetFillPercentage(num2 / num1);
      reference1.label.SetText((string) STRINGS.UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
    }
    else
    {
      reference1.SetFillPercentage(0.0f);
      reference1.label.SetText((string) STRINGS.UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
    }
    GenericUIProgressBar reference2 = component1.GetReference<GenericUIProgressBar>("diamondProgressBar");
    Storage component2 = resourceHarvestModule.GetComponent<Storage>();
    reference2.SetFillPercentage(component2.MassStored() / component2.Capacity());
    reference2.label.SetText($"{ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name}: {GameUtil.GetFormattedMass(component2.MassStored())}");
  }
}
