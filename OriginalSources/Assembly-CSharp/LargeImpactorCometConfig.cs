// Decompiled with JetBrains decompiler
// Type: LargeImpactorCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LargeImpactorCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "LargeImpactorComet";
  private const SimHashes element = SimHashes.Regolith;
  private const int ADDED_CELLS = 6;

  public string[] GetRequiredDlcIds()
  {
    return new string[1]{ "DLC4_ID" };
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(LargeImpactorCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    LargeComet largeComet = entity.AddOrGet<LargeComet>();
    largeComet.impactSound = "Meteor_Large_Impact";
    largeComet.flyingSoundID = 2;
    largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_wind_kanim", "wind_loop"));
    largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_flame_inner_kanim", "flame_loop"));
    largeComet.mainAnimFile = new KeyValuePair<string, string>("asteroid_001_kanim", "idle");
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Regolith);
    primaryElement.Temperature = 20000f;
    KBatchedAnimController kbatchedAnimController = entity.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "asteroid_flame_outer_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "flame_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.animScale = 0.2f;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.AddTag(GameTags.Comet);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
    LargeComet largeComet = go.AddOrGet<LargeComet>();
    largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_wind_kanim", "wind_loop"));
    largeComet.additionalAnimFiles.Add(new KeyValuePair<string, string>("asteroid_flame_inner_kanim", "flame_loop"));
    largeComet.mainAnimFile = new KeyValuePair<string, string>("asteroid_001_kanim", "idle");
  }

  public void OnSpawn(GameObject go)
  {
  }
}
