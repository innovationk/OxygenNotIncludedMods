// Decompiled with JetBrains decompiler
// Type: VistaConfigs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class VistaConfigs : IMultiEntityConfig
{
  public const Grid.SceneLayer SCENE_LAYER = Grid.SceneLayer.Background;
  public const string BEACH_VISTA_ID = "BeachVista";
  public const string REEF_VISTA_ID = "ReefVista";
  public const string KELP_VISTA_ID = "KelpVista";
  public const string ABYSS_VISTA_ID = "AbyssVista";

  public List<GameObject> CreatePrefabs()
  {
    return new List<GameObject>()
    {
      this.CreateSetPiece("BeachVista", "farmtile_kanim", 17, 10, "BeachVista", audioName: "beach_vista_lp"),
      this.CreateSetPiece("ReefVista", "farmtile_kanim", 17, 10, "ReefVista"),
      this.CreateSetPiece("KelpVista", "farmtile_kanim", 17, 10, "KelpVista"),
      this.CreateSetPiece("AbyssVista", "farmtile_kanim", 17, 10, "AbyssVista")
    };
  }

  private GameObject CreateSetPiece(
    string ID,
    string uiAnim,
    int width,
    int height,
    string vistaPrefabID,
    string spriteId = null,
    string audioName = null)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, (string) Strings.Get($"STRINGS.ENTITIES.VISTAS.{ID.ToUpperInvariant()}.NAME"), (string) Strings.Get($"STRINGS.ENTITIES.VISTAS.{ID.ToUpperInvariant()}.DESCRIPTION"), 100f, Assets.GetAnim((HashedString) uiAnim), "", Grid.SceneLayer.Background, width, height, DECOR.BONUS.TIER2);
    Vista vista = placedEntity.AddComponent<Vista>();
    vista.prefabName = vistaPrefabID;
    vista.sceneLayer = Grid.SceneLayer.Background;
    vista.width = width;
    vista.height = height;
    vista.audioName = audioName;
    placedEntity.GetComponent<KSelectable>().IsSelectable = false;
    if (audioName != null)
      placedEntity.AddComponent<LoopingSounds>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
