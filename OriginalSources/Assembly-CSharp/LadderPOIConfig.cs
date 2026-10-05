// Decompiled with JetBrains decompiler
// Type: LadderPOIConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LadderPOIConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    int width1 = 1;
    int height1 = 1;
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.DESC;
    int num1 = width1;
    int num2 = height1;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "ladder_poi_kanim");
    int width2 = num1;
    int height2 = num2;
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropLadder", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, width2, height2, decor, PermittedRotations.R90, noise: noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Polypropylene);
    component.Temperature = 294.15f;
    Ladder ladder = placedEntity.AddOrGet<Ladder>();
    ladder.upwardsMovementSpeedMultiplier = 1.5f;
    ladder.downwardsMovementSpeedMultiplier = 1.5f;
    placedEntity.AddOrGet<AnimTileable>();
    Object.DestroyImmediate((Object) placedEntity.AddOrGet<OccupyArea>());
    OccupyArea occupyArea = placedEntity.AddOrGet<OccupyArea>();
    occupyArea.SetCellOffsets(EntityTemplates.GenerateOffsets(width1, height1));
    occupyArea.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<Demolishable>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
