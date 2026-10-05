// Decompiled with JetBrains decompiler
// Type: PropSurfaceSatellite3Config
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PropSurfaceSatellite3Config : IEntityConfig
{
  public static string ID = "PropSurfaceSatellite3";

  public GameObject CreatePrefab()
  {
    string id = PropSurfaceSatellite3Config.ID;
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE3.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE3.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "satellite3_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 6, 6, decor, noise, additionalTags: new List<Tag>()
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
    inst.Subscribe(-372600542, (Action<object>) (locker => this.OnLockerLooted(inst)));
    RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetEmitting(true);
  }

  private void OnLockerLooted(GameObject inst)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) ArtifactSelector.Instance.GetUniqueArtifactID()), inst.transform.position);
    gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
    gameObject.SetActive(true);
  }
}
