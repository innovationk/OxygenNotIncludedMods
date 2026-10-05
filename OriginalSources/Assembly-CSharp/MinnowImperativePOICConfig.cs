// Decompiled with JetBrains decompiler
// Type: MinnowImperativePOICConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class MinnowImperativePOICConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "MinnowImperativePOIC";
  public const float RequiredDeliveryMass = 10f;

  public static Tag RequiredDeliveryTag => "Caviar".ToTag();

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MinnowImperativePOIC", (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_C.NAME, (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_C.DESC, 30f, Assets.GetAnim((HashedString) "minnow_imperative_poi_c_kanim"), "off", Grid.SceneLayer.Creatures, 3, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE);
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    storage.capacityKg = 10f;
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = placedEntity.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = MinnowImperativePOICConfig.RequiredDeliveryTag;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 10f;
    manualDeliveryKg.MinimumMass = 10f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
    manualDeliveryKg.enabled = false;
    placedEntity.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(placedEntity);
    MinnowImperativePOIStates.Def def = placedEntity.AddOrGetDef<MinnowImperativePOIStates.Def>();
    def.requestedTag = MinnowImperativePOICConfig.RequiredDeliveryTag;
    def.requiredMass = 10f;
    def.minnowPOIIdentity = MinnowImperativePOIStates.MinnowPOIIdentity.POI_C;
    placedEntity.GetComponent<KBatchedAnimController>().materialType = KAnimBatchGroup.MaterialType.DefaultInsideVistas;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
