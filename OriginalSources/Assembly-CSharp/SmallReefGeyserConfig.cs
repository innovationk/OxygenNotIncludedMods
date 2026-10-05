// Decompiled with JetBrains decompiler
// Type: SmallReefGeyserConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmallReefGeyserConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "SmallReefGeyser";
  private const float INHALE_RATE = 500f;
  private const float INHALE_TIME = 30f;
  private const float LIQUID_CAPACITY = 15000f;
  private const float EXHALE_TIME = 90f;
  private const float EXHALE_RATE = 166.666672f;
  public const float APPROXIMATE_EXHALE_TIME_PER_CYCLE = 450f;

  public GameObject CreatePrefab()
  {
    string id = SmallReefGeyserConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.GEYSER.SMALLREEFGEYSER.NAME;
    string name2 = (string) STRINGS.CREATURES.SPECIES.GEYSER.SMALLREEFGEYSER.NAME;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "geyser_reef_kanim");
    EffectorValues decor = tieR0;
    List<Tag> tagList = new List<Tag>()
    {
      GameTags.GeyserFeature
    };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name1, name2, 2000f, anim, "inactive", Grid.SceneLayer.Building, 3, 2, decor, noise, additionalTags: additionalTags);
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Katairite);
    component.Temperature = 305.15f;
    placedEntity.AddOrGet<LoopingSounds>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(placedEntity);
    defaultStorage.capacityKg = 15000f;
    defaultStorage.showInUI = true;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.storeOnConsume = true;
    elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
    elementConsumer.capacityKG = 15000f;
    elementConsumer.consumptionRate = 500f;
    elementConsumer.consumptionRadius = (byte) 1;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f);
    elementConsumer.overrideStatusItemString = (string) STRINGS.CREATURES.SPECIES.GEYSER.SMALLREEFGEYSER.LIQUID_CONSUMPTION;
    BreathingGeyser.Def def = placedEntity.AddOrGetDef<BreathingGeyser.Def>();
    def.inhaleRate = 500f;
    def.exhaleRate = 166.666672f;
    placedEntity.AddOrGet<Submergable>().GetStatusItem = new Func<StatusItem>(SmallReefGeyserConfig.GetSubmergableStatusItem);
    placedEntity.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.ReefGenerator, (AttachableBuilding) null)
    };
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<Submergable>().GetStatusItem = new Func<StatusItem>(SmallReefGeyserConfig.GetSubmergableStatusItem);
  }

  public void OnSpawn(GameObject inst)
  {
    inst.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "geotracker_target", false);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  private static StatusItem GetSubmergableStatusItem() => Db.Get().CreatureStatusItems.NotSubmerged;
}
