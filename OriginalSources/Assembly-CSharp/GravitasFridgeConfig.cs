// Decompiled with JetBrains decompiler
// Type: GravitasFridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GravitasFridgeConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFRIDGE.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFRIDGE.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_fridge_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GravitasFridge", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 2, 2, decor, PermittedRotations.Unrotatable, noise: noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Steel);
    component.Temperature = 294.15f;
    LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_initial", (string) UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH3, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextResearchNote), true));
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
