// Decompiled with JetBrains decompiler
// Type: Crop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
  [MyCmpReq]
  private KSelectable selectable;
  public Crop.CropVal cropVal;
  private AttributeInstance yield;
  public Vector3 cropSpawnOffset = new Vector3(0.0f, 0.75f, 0.0f);
  public string domesticatedDesc = "";
  private Storage planterStorage;
  private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>((Action<Crop, object>) ((component, data) => component.OnHarvest(data)));

  public string cropId => this.cropVal.cropId;

  public Storage PlanterStorage
  {
    get => this.planterStorage;
    set => this.planterStorage = value;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Crops.Add(this);
    this.yield = this.GetAttributes().Add(Db.Get().PlantAttributes.YieldAmount);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
  }

  public void Configure(Crop.CropVal cropval) => this.cropVal = cropval;

  public bool CanGrow() => this.cropVal.renewable;

  public void SpawnConfiguredFruit(object callbackParam)
  {
    this.SpawnAndGetConfiguredFruit(callbackParam, true);
  }

  public GameObject SpawnAndGetConfiguredFruit(object callbackParam, bool wasCropPicked)
  {
    GameObject configuredFruit = (GameObject) null;
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return (GameObject) null;
    Crop.CropVal cropVal = this.cropVal;
    if (!string.IsNullOrEmpty(cropVal.cropId))
    {
      configuredFruit = this.SpawnAndGetSomeFruit((Tag) cropVal.cropId, this.yield.GetTotalValue());
      if (wasCropPicked)
        this.Trigger(-1072826864, (object) this);
    }
    return configuredFruit;
  }

  public void SpawnSomeFruit(Tag cropID, float amount) => this.SpawnAndGetSomeFruit(cropID, amount);

  public GameObject SpawnAndGetSomeFruit(Tag cropID, float amount)
  {
    GameObject prefab = Assets.GetPrefab(cropID);
    GameObject someFruit = GameUtil.KInstantiate(prefab, this.transform.GetPosition() + this.cropSpawnOffset, Grid.SceneLayer.Ore);
    if ((UnityEngine.Object) someFruit != (UnityEngine.Object) null)
    {
      MutantPlant component1 = this.GetComponent<MutantPlant>();
      MutantPlant component2 = someFruit.GetComponent<MutantPlant>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.IsOriginal && (UnityEngine.Object) component2 != (UnityEngine.Object) null && this.GetComponent<SeedProducer>().RollForMutation())
        component2.Mutate();
      someFruit.SetActive(true);
      PrimaryElement component3 = someFruit.GetComponent<PrimaryElement>();
      component3.Units = amount;
      component3.Temperature = this.gameObject.GetComponent<PrimaryElement>().Temperature;
      this.Trigger(35625290, (object) someFruit);
      Edible component4 = someFruit.GetComponent<Edible>();
      string properName = someFruit.GetProperName();
      if ((bool) (UnityEngine.Object) component4)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component4.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", properName), (string) UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
      PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) prefab).first, PopFXManager.Instance.sprite_Plus, properName, this.transform, Vector3.zero);
      return someFruit;
    }
    DebugUtil.LogErrorArgs((UnityEngine.Object) this.gameObject, (object) "tried to spawn an invalid crop prefab:", (object) cropID);
    return (GameObject) null;
  }

  protected override void OnCleanUp()
  {
    Components.Crops.Remove(this);
    base.OnCleanUp();
  }

  private void OnHarvest(object obj)
  {
  }

  public List<Descriptor> RequirementDescriptors(GameObject go) => new List<Descriptor>();

  public List<Descriptor> InformationDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Tag tag = new Tag(this.cropVal.cropId);
    GameObject prefab = Assets.GetPrefab(tag);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
    {
      DebugUtil.LogWarningArgs((object) nameof (Crop), (object) this.gameObject.name, (object) "has an invalid crop prefab:", (object) tag);
      return descriptorList;
    }
    Edible component = prefab.GetComponent<Edible>();
    Klei.AI.Attribute yieldAmount = Db.Get().PlantAttributes.YieldAmount;
    float modifiedAttributeValue = go.GetComponent<Modifiers>().GetPreModifiedAttributeValue(yieldAmount);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      DebugUtil.Assert(GameTags.DisplayAsCalories.Contains(tag), "Trying to display crop info for an edible fruit which isn't displayed as calories!", tag.ToString());
      float caloriesPerUnit = component.FoodInfo.CaloriesPerUnit;
      float calories = caloriesPerUnit * modifiedAttributeValue;
      string formattedCalories = GameUtil.GetFormattedCalories(calories);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, (object) prefab.GetProperName(), (object) formattedCalories), string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, (object) "", (object) GameUtil.GetFormattedCalories(caloriesPerUnit), (object) GameUtil.GetFormattedCalories(calories)));
      descriptorList.Add(descriptor);
    }
    else
    {
      string str = !GameTags.DisplayAsUnits.Contains(tag) ? GameUtil.GetFormattedMass((float) this.cropVal.numProduced) : GameUtil.GetFormattedUnits((float) this.cropVal.numProduced, displaySuffix: false);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD_NONFOOD, (object) prefab.GetProperName(), (object) str), string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD_NONFOOD, (object) str));
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    foreach (Descriptor requirementDescriptor in this.RequirementDescriptors(go))
      descriptors.Add(requirementDescriptor);
    foreach (Descriptor informationDescriptor in this.InformationDescriptors(go))
      descriptors.Add(informationDescriptor);
    return descriptors;
  }

  [Serializable]
  public struct CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
  {
    public string cropId = crop_id;
    public float cropDuration = crop_duration;
    public int numProduced = num_produced;
    public bool renewable = renewable;
  }
}
