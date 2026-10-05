// Decompiled with JetBrains decompiler
// Type: FoodRehydrator.DehydratedManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FoodRehydrator;

public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
{
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private Storage packages;
  private Storage water;
  private MeterController packagesMeter;
  private static string HASH_FOOD = "food";
  private KBatchedAnimController foodKBAC;
  private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>((Action<DehydratedManager, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private Tag chosenContent = GameTags.Dehydrated;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<DehydratedManager>(-905833192, DehydratedManager.OnCopySettingsDelegate);
  }

  public Tag ChosenContent
  {
    get => this.chosenContent;
    set
    {
      if (!(this.chosenContent != value))
        return;
      this.GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
      this.chosenContent = value;
      this.packages.DropUnlessHasTag(this.chosenContent);
      if (!(this.chosenContent != GameTags.Dehydrated))
        return;
      AccessabilityManager component = this.GetComponent<AccessabilityManager>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.CancelActiveWorkable();
    }
  }

  public void SetFabricatedFoodSymbol(Tag material)
  {
    this.foodKBAC.gameObject.SetActive(true);
    this.foodKBAC.SwapAnims(Assets.GetPrefab(material).GetComponent<KBatchedAnimController>().AnimFiles);
    this.foodKBAC.Play((HashedString) "object", KAnim.PlayMode.Loop);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Storage[] components = this.GetComponents<Storage>();
    Debug.Assert(components.Length == 2);
    this.packages = components[0];
    this.water = components[1];
    this.packagesMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target"
    });
    this.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
    this.SetupFoodSymbol();
    this.packagesMeter.SetPositionPercent((float) this.packages.items.Count / 5f);
  }

  public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
  {
    Debug.Assert(this.packages.items.Contains(package));
    this.packages.ConsumeIgnoringDisease(package);
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out float _, out disease_info, out aggregate_temperature);
    PrimaryElement component = food.GetComponent<PrimaryElement>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.AddDisease(disease_info.idx, disease_info.count, "rehydrating");
    component.SetMassTemperature(component.Mass, (float) ((double) component.Temperature * 0.125 + (double) aggregate_temperature * 0.875));
  }

  private void StorageChangeHandler(object obj)
  {
    if (!((UnityEngine.Object) ((GameObject) obj).GetComponent<DehydratedFoodPackage>() != (UnityEngine.Object) null))
      return;
    this.packagesMeter.SetPositionPercent((float) this.packages.items.Count / 5f);
  }

  private void SetupFoodSymbol()
  {
    GameObject gameObject = Util.NewGameObject(this.gameObject, "food_symbol");
    gameObject.SetActive(false);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    Vector3 column = (Vector3) component.GetSymbolTransform((HashedString) DehydratedManager.HASH_FOOD, out bool _).GetColumn(3) with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
    };
    gameObject.transform.SetPosition(column);
    this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
    this.foodKBAC.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "mushbar_kanim")
    };
    this.foodKBAC.initialAnim = "object";
    component.SetSymbolVisiblity((KAnimHashedString) DehydratedManager.HASH_FOOD, false);
    this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
    KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = new HashedString("food");
    kbatchedAnimTracker.offset = Vector3.zero;
  }

  public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
  {
    HashSet<Tag> resourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
    FewOptionSideScreen.IFewOptionSideScreen.Option[] options = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + resourcesFromTag.Count];
    options[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, (string) UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite((object) "icon_category_food"));
    int index = 1;
    foreach (Tag tag in resourcesFromTag)
    {
      options[index] = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite((object) tag));
      ++index;
    }
    return options;
  }

  public void OnOptionSelected(
    FewOptionSideScreen.IFewOptionSideScreen.Option option)
  {
    this.ChosenContent = option.tag;
  }

  public Tag GetSelectedOption() => this.chosenContent;

  protected void OnCopySettings(object data)
  {
    if (!(data is GameObject gameObject))
      return;
    DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.ChosenContent = component.ChosenContent;
  }
}
