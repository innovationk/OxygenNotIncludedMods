// Decompiled with JetBrains decompiler
// Type: DisposableElectrobankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DisposableElectrobankConfig : IMultiEntityConfig
{
  public const string ID = "DisposableElectrobank_";
  public const float MASS = 20f;
  public static Dictionary<Tag, ComplexRecipe> recipes = new Dictionary<Tag, ComplexRecipe>();
  public const string ID_METAL_ORE = "DisposableElectrobank_RawMetal";
  public const string ID_URANIUM_ORE = "DisposableElectrobank_UraniumOre";

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    if (!DlcManager.IsContentSubscribed("DLC3_ID"))
      return prefabs;
    prefabs.Add(this.CreateDisposableElectrobank("DisposableElectrobank_RawMetal", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.DESC, 20f, SimHashes.Cuprite, "electrobank_popcan_kanim", DlcManager.DLC3));
    if (DlcManager.IsExpansion1Active())
    {
      GameObject disposableElectrobank = this.CreateDisposableElectrobank("DisposableElectrobank_UraniumOre", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.DESC, 10f, SimHashes.UraniumOre, "electrobank_uranium_kanim", DlcManager.EXPANSION1.Append<string>(DlcManager.DLC3));
      RadiationEmitter radiationEmitter = disposableElectrobank.AddOrGet<RadiationEmitter>();
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      radiationEmitter.radiusProportionalToRads = false;
      radiationEmitter.emitRadiusX = (short) 5;
      radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
      radiationEmitter.emitRads = 60f;
      radiationEmitter.emissionOffset = new Vector3(0.0f, 0.0f, 0.0f);
      prefabs.Add(disposableElectrobank);
      disposableElectrobank.GetComponent<Electrobank>().radioactivityTuning = radiationEmitter.emitRads;
    }
    prefabs.RemoveAll((Predicate<GameObject>) (t => (UnityEngine.Object) t == (UnityEngine.Object) null));
    return prefabs;
  }

  private GameObject CreateDisposableElectrobank(
    string id,
    LocString name,
    LocString description,
    float mass,
    SimHashes element,
    string animName,
    string[] requiredDlcIDs = null,
    string[] forbiddenDlcIds = null,
    string initialAnim = "object")
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(id, (string) name, (string) description, mass, true, Assets.GetAnim((HashedString) animName), initialAnim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, additionalTags: new List<Tag>()
    {
      GameTags.ChargedPortableBattery,
      GameTags.PedestalDisplayable,
      GameTags.DisposablePortableBattery
    });
    if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
      Assets.AddCountableTag(GameTags.ChargedPortableBattery);
    looseEntity.AddComponent<Electrobank>();
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    looseEntity.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER0);
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.requiredDlcIds = requiredDlcIDs;
    component.forbiddenDlcIds = forbiddenDlcIds;
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
