// Decompiled with JetBrains decompiler
// Type: POICeresTechUnlockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class POICeresTechUnlockConfig : IEntityConfig, IHasDlcRestrictions
{
  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "research_unlock_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("POICeresTechUnlock", name, desc, 100f, anim, "on", Grid.SceneLayer.Building, 3, 3, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas,
      GameTags.RoomProberBuilding,
      GameTags.LightSource
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<Demolishable>();
    POITechItemUnlockWorkable itemUnlockWorkable = placedEntity.AddOrGet<POITechItemUnlockWorkable>();
    itemUnlockWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_research_unlock_kanim")
    };
    itemUnlockWorkable.workTime = 5f;
    POITechItemUnlocks.Def def = placedEntity.AddOrGetDef<POITechItemUnlocks.Def>();
    def.POITechUnlockIDs = new List<string>()
    {
      "Campfire",
      "IceKettle",
      "WoodTile"
    };
    def.PopUpName = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
    def.animName = "ceres_remote_archive_kanim";
    def.loreUnlockId = "notes_welcometoceres";
    Light2D light2D = placedEntity.AddComponent<Light2D>();
    light2D.Color = LIGHT2D.POI_TECH_UNLOCK_COLOR;
    light2D.Range = 5f;
    light2D.Angle = 2.6f;
    light2D.Direction = LIGHT2D.POI_TECH_DIRECTION;
    light2D.Offset = LIGHT2D.POI_TECH_UNLOCK_OFFSET;
    light2D.overlayColour = LIGHT2D.POI_TECH_UNLOCK_OVERLAYCOLOR;
    light2D.shape = LightShape.Cone;
    light2D.drawOverlay = true;
    light2D.Lux = 1800;
    placedEntity.AddOrGet<Prioritizable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
