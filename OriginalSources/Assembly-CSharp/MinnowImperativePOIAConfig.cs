// Decompiled with JetBrains decompiler
// Type: MinnowImperativePOIAConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class MinnowImperativePOIAConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "MinnowImperativePOIA";
  public const float RequiredDeliveryMass = 200f;

  public Tag RequiredDeliveryTag => SimHashes.Pearl.CreateTag();

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MinnowImperativePOIA", (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.NAME, (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.DESC, 30f, Assets.GetAnim((HashedString) "minnow_imperative_poi_a_kanim"), "off", Grid.SceneLayer.Creatures, 3, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE);
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    storage.capacityKg = 200f;
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = placedEntity.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = this.RequiredDeliveryTag;
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 200f;
    manualDeliveryKg.MinimumMass = 200f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
    manualDeliveryKg.enabled = false;
    placedEntity.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(placedEntity);
    MinnowImperativePOIStates.Def def = placedEntity.AddOrGetDef<MinnowImperativePOIStates.Def>();
    def.requestedTag = this.RequiredDeliveryTag;
    def.requiredMass = 200f;
    def.minnowPOIIdentity = MinnowImperativePOIStates.MinnowPOIIdentity.POI_A;
    string id = "MINNOW";
    Db.Get().Personalities.Get(id).Disabled = true;
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
