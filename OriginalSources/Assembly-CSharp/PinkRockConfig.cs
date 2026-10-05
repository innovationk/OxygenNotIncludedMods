// Decompiled with JetBrains decompiler
// Type: PinkRockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PinkRockConfig : IEntityConfig, IHasDlcRestrictions
{
  public string ID = "PinkRock";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string id = this.ID;
    string name = (string) STRINGS.CREATURES.SPECIES.PINKROCK.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.PINKROCK.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "pinkrock_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 25f, anim, "idle", Grid.SceneLayer.Building, 1, 1, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Experimental
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 235.15f;
    placedEntity.AddOrGet<Carvable>().dropItemPrefabId = "PinkRockCarved";
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    Light2D light2D = placedEntity.AddOrGet<Light2D>();
    light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
    light2D.Color = LIGHT2D.PINKROCK_COLOR;
    light2D.Range = 2f;
    light2D.Angle = 0.0f;
    light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
    light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
    light2D.shape = LightShape.Circle;
    light2D.drawOverlay = true;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
