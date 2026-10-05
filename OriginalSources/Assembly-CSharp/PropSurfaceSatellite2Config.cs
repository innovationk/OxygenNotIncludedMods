// Decompiled with JetBrains decompiler
// Type: PropSurfaceSatellite2Config
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropSurfaceSatellite2Config : IEntityConfig
{
  public static string ID = "PropSurfaceSatellite2";

  public GameObject CreatePrefab()
  {
    string id = PropSurfaceSatellite2Config.ID;
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE2.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE2.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "satellite2_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 4, 4, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
    setLocker.dropOffset = new Vector2I(0, 1);
    setLocker.numDataBanks = new int[2]{ 4, 9 };
    LoreBearerUtil.AddLoreTo(placedEntity);
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    SetLocker component = inst.GetComponent<SetLocker>();
    component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
    component.ChooseContents();
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    RadiationEmitter radiationEmitter = inst.AddOrGet<RadiationEmitter>();
    radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
    radiationEmitter.radiusProportionalToRads = false;
    radiationEmitter.emitRadiusX = (short) 12;
    radiationEmitter.emitRadiusY = (short) 12;
    radiationEmitter.emitRads = (float) (2400.0 / ((double) radiationEmitter.emitRadiusX / 6.0));
  }

  public void OnSpawn(GameObject inst)
  {
    RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
    if (!((Object) component != (Object) null))
      return;
    component.SetEmitting(true);
  }
}
