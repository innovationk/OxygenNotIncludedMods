// Decompiled with JetBrains decompiler
// Type: GravitasBathroomMirrorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GravitasBathroomMirrorConfig : IEntityConfig
{
  public const string ID = "GravitasBathroomMirror";

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMMIRROR.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMMIRROR.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_bathroom_mirror_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GravitasBathroomMirror", name, desc, 50f, anim, "on", Grid.SceneLayer.Building, 1, 1, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Glass);
    component.Temperature = 294.15f;
    LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_mirror", (string) UI.USERMENUACTIONS.READLORE.SEARCH_FLATOBJECT_SUCCESS.SEARCH1, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextJournalEntry), true));
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
