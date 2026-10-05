// Decompiled with JetBrains decompiler
// Type: PacuCleanerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class PacuCleanerConfig : IEntityConfig
{
  public const string ID = "PacuCleaner";
  public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";
  public const string EGG_ID = "PacuCleanerEgg";
  public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;
  public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;
  public static SimHashes OUTPUT_ELEMENT = SimHashes.Water;
  public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
  public const int EGG_SORT_ORDER = 501;

  public static GameObject CreatePacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f, 223.15f, 298.15f), PacuTuning.PEN_SIZE_PER_CREATURE, true);
    if (!is_baby)
    {
      Storage storage = wildCreature.AddComponent<Storage>();
      storage.capacityKg = 10f;
      storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
      PassiveElementConsumer passiveElementConsumer = wildCreature.AddOrGet<PassiveElementConsumer>();
      passiveElementConsumer.elementToConsume = SimHashes.DirtyWater;
      passiveElementConsumer.consumptionRate = 0.2f;
      passiveElementConsumer.capacityKG = 10f;
      passiveElementConsumer.consumptionRadius = (byte) 3;
      passiveElementConsumer.showInStatusPanel = true;
      passiveElementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
      passiveElementConsumer.isRequired = false;
      passiveElementConsumer.storeOnConsume = true;
      passiveElementConsumer.showDescriptor = false;
      wildCreature.AddOrGet<UpdateElementConsumerPosition>();
      BubbleSpawner bubbleSpawner = wildCreature.AddComponent<BubbleSpawner>();
      bubbleSpawner.emitMass = 2f;
      bubbleSpawner.emitVariance = 0.5f;
      bubbleSpawner.element = PacuCleanerConfig.OUTPUT_ELEMENT;
      ElementConverter elementConverter = wildCreature.AddOrGet<ElementConverter>();
      elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
      {
        new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f)
      };
      elementConverter.outputElements = new ElementConverter.OutputElement[1]
      {
        new ElementConverter.OutputElement(0.2f, PacuCleanerConfig.OUTPUT_ELEMENT, 0.0f, true, true)
      };
    }
    return wildCreature;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, true), this as IHasDlcRestrictions, "PacuCleanerEgg", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, PacuTuning.EGG_SHELL_RATIO, "PacuCleanerBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_CLEANER, 501, true, true, 0.75f, false, false, PacuTuning.EGG_MASS);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    ElementConsumer component;
    if (!inst.TryGetComponent<ElementConsumer>(out component))
      return;
    component.EnableConsumption(true);
  }
}
