// Decompiled with JetBrains decompiler
// Type: SeedProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public SeedProducer.SeedInfo seedInfo;
  public float seedDropChanceMultiplier = 1f;
  public float seedDropChances = 0.1f;
  private bool droppedSeedAlready;
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((Action<SeedProducer, object>) ((component, data) =>
  {
    if (component.seedInfo.productionType == SeedProducer.ProductionType.HarvestOnly)
      return;
    component.DropSeed(data);
  }));
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((Action<SeedProducer, object>) ((component, data) => component.CropPicked(data)));

  public void Configure(
    string SeedID,
    SeedProducer.ProductionType productionType,
    int newSeedsProduced = 1)
  {
    this.seedInfo.seedId = SeedID;
    this.seedInfo.productionType = productionType;
    this.seedInfo.newSeedsProduced = newSeedsProduced;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
  }

  private GameObject ProduceSeed(string seedId, int units = 1, bool canMutate = true)
  {
    if (seedId == null || units <= 0)
      return (GameObject) null;
    Vector3 position = this.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f);
    GameObject prefab = Assets.GetPrefab(new Tag(seedId));
    GameObject gameObject = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Ore);
    MutantPlant component1 = this.GetComponent<MutantPlant>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
      bool flag = false;
      if (canMutate && (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.IsOriginal)
        flag = this.RollForMutation();
      if (flag)
        component2.Mutate();
      else
        component1.CopyMutationsTo(component2);
    }
    PrimaryElement component3 = this.gameObject.GetComponent<PrimaryElement>();
    PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
    component4.Temperature = component3.Temperature;
    component4.Units = (float) units;
    this.Trigger(472291861, (object) gameObject);
    gameObject.SetActive(true);
    string str = gameObject.GetProperName();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      str = component1.GetSubSpeciesInfo().GetNameWithMutations(str, component1.IsIdentified, false);
    PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) prefab).first, PopFXManager.Instance.sprite_Plus, str, gameObject.transform, Vector3.zero);
    return gameObject;
  }

  public void DropSeed(object data = null)
  {
    if (this.droppedSeedAlready || this.seedInfo.newSeedsProduced <= 0)
      return;
    GameObject gameObject = this.ProduceSeed(this.seedInfo.seedId, this.seedInfo.newSeedsProduced, false);
    Uprootable component = this.GetComponent<Uprootable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.worker != (UnityEngine.Object) null)
      gameObject.Trigger(580035959, (object) component.worker);
    this.Trigger(-1736624145, (object) gameObject);
    this.droppedSeedAlready = true;
  }

  public void CropDepleted(object data) => this.DropSeed();

  public void CropPicked(object data)
  {
    if (this.seedInfo.productionType != SeedProducer.ProductionType.Harvest && this.seedInfo.productionType != SeedProducer.ProductionType.HarvestOnly)
      return;
    this.CropPickedInternal(this.GetComponent<Harvestable>().completed_by);
  }

  public void SimulateCropPicked(WorkerBase worker) => this.CropPickedInternal(worker);

  private void CropPickedInternal(WorkerBase worker)
  {
    if (this.seedInfo.productionType != SeedProducer.ProductionType.Harvest && this.seedInfo.productionType != SeedProducer.ProductionType.HarvestOnly)
      return;
    float seedDropChances = this.seedDropChances;
    if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
      seedDropChances += worker.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
    float num = seedDropChances * this.seedDropChanceMultiplier;
    int units = (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num ? 1 : 0;
    if (units <= 0)
      return;
    this.ProduceSeed(this.seedInfo.seedId, units).Trigger(580035959, (object) worker);
  }

  public bool RollForMutation()
  {
    AttributeInstance attributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup((Component) this);
    int cell = Grid.PosToCell(this.gameObject);
    return (double) UnityEngine.Random.value < (double) Mathf.Clamp(Grid.IsValidCell(cell) ? Grid.Radiation[cell] : 0.0f, 0.0f, attributeInstance.GetTotalValue()) / (double) attributeInstance.GetTotalValue() * 0.800000011920929;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    int num = (UnityEngine.Object) Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != (UnityEngine.Object) null ? 1 : 0;
    switch (this.seedInfo.productionType)
    {
      case SeedProducer.ProductionType.Hidden:
      case SeedProducer.ProductionType.DigOnly:
      case SeedProducer.ProductionType.Crop:
        return (List<Descriptor>) null;
      case SeedProducer.ProductionType.Harvest:
      case SeedProducer.ProductionType.HarvestOnly:
        descriptors.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
        descriptors.Add(new Descriptor(string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, (object) GameUtil.GetFormattedPercent(this.seedDropChances * 100f * this.seedDropChanceMultiplier)), string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, (object) GameUtil.GetFormattedPercent(this.seedDropChances * 100f * this.seedDropChanceMultiplier))));
        break;
      case SeedProducer.ProductionType.Fruit:
        descriptors.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
        break;
      case SeedProducer.ProductionType.Sterile:
        descriptors.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.MUTANT_STERILE, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_STERILE));
        break;
      default:
        DebugUtil.Assert(false, "Seed producer type descriptor not specified");
        return (List<Descriptor>) null;
    }
    return descriptors;
  }

  [Serializable]
  public struct SeedInfo
  {
    public string seedId;
    public SeedProducer.ProductionType productionType;
    public int newSeedsProduced;
  }

  public enum ProductionType
  {
    Hidden,
    DigOnly,
    Harvest,
    Fruit,
    Sterile,
    Crop,
    HarvestOnly,
  }
}
