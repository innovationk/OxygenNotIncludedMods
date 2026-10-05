// Decompiled with JetBrains decompiler
// Type: StampToolPreview_Prefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using TemplateClasses;
using UnityEngine;

#nullable disable
public class StampToolPreview_Prefabs : IStampToolPreviewPlugin
{
  public void Setup(StampToolPreviewContext context)
  {
    if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
    {
      foreach (Prefab elementalOre in context.stampTemplate.elementalOres)
        StampToolPreview_Prefabs.SpawnPrefab(context, elementalOre);
    }
    if (!context.stampTemplate.otherEntities.IsNullOrDestroyed())
    {
      foreach (Prefab otherEntity in context.stampTemplate.otherEntities)
        StampToolPreview_Prefabs.SpawnPrefab(context, otherEntity);
    }
    if (!context.stampTemplate.backwallEntities.IsNullOrDestroyed())
    {
      foreach (Prefab backwallEntity in context.stampTemplate.backwallEntities)
        StampToolPreview_Prefabs.SpawnPrefab(context, backwallEntity);
    }
    if (!context.stampTemplate.buildings.IsNullOrDestroyed())
    {
      foreach (Prefab building in context.stampTemplate.buildings)
        StampToolPreview_Prefabs.SpawnPrefab(context, building);
    }
    if (context.stampTemplate.elementalOres.IsNullOrDestroyed())
      return;
    foreach (Prefab elementalOre in context.stampTemplate.elementalOres)
      StampToolPreview_Prefabs.SpawnPrefab(context, elementalOre);
  }

  public static void SpawnPrefab(StampToolPreviewContext context, Prefab prefabInfo)
  {
    GameObject prefab = Assets.TryGetPrefab((Tag) prefabInfo.id);
    if (prefab.IsNullOrDestroyed())
      return;
    if (!prefab.GetComponent<Building>().IsNullOrDestroyed())
    {
      Building component = prefab.GetComponent<Building>();
      if (component.Def.IsTilePiece)
        StampToolPreview_Prefabs.SpawnPrefab_Tile(context, prefabInfo, component);
      else
        StampToolPreview_Prefabs.SpawnPrefab_Building(context, prefabInfo, component);
    }
    else
      StampToolPreview_Prefabs.SpawnPrefab_Default(context, prefabInfo, prefab);
  }

  public static void SpawnPrefab_Tile(
    StampToolPreviewContext context,
    Prefab prefabInfo,
    Building buildingPrefab)
  {
    TextureAtlas textureAtlas = buildingPrefab.Def.BlockTilePlaceAtlas;
    if ((UnityEngine.Object) textureAtlas == (UnityEngine.Object) null)
      textureAtlas = buildingPrefab.Def.BlockTileAtlas;
    if ((UnityEngine.Object) textureAtlas == (UnityEngine.Object) null || textureAtlas.items == null || textureAtlas.items.Length < 0)
      return;
    GameObject gameObject;
    MeshRenderer meshRenderer;
    StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1.5f, new Vector4?(textureAtlas.items[0].uvBox));
    gameObject.name = $"TilePlacer {buildingPrefab.PrefabID()}";
    gameObject.transform.SetParent(context.previewParent.transform, false);
    gameObject.transform.SetLocalPosition((Vector3) new Vector2((float) prefabInfo.location_x, (float) prefabInfo.location_y + Grid.HalfCellSizeInMeters));
    Material material = StampToolPreviewUtil.MakeMaterial((Texture) textureAtlas.texture);
    material.name = $"Tile ({buildingPrefab.PrefabID()}) ({material.name})";
    meshRenderer.material = material;
    context.cleanupFn += (System.Action) (() =>
    {
      if (!gameObject.IsNullOrDestroyed())
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
      if (material.IsNullOrDestroyed())
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) material);
    });
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      if (meshRenderer.IsNullOrDestroyed())
        return;
      meshRenderer.material.color = error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
    });
  }

  public static void SpawnPrefab_Building(
    StampToolPreviewContext context,
    Prefab prefabInfo,
    Building buildingPrefab)
  {
    int layer = LayerMask.NameToLayer("Place");
    Building spawn = GameUtil.KInstantiate(!buildingPrefab.Def.BuildingPreview.IsNullOrDestroyed() ? buildingPrefab.Def.BuildingPreview : BuildingLoader.Instance.CreateBuildingPreview(buildingPrefab.Def), Vector3.zero, Grid.SceneLayer.Building, gameLayer: layer).GetComponent<Building>();
    context.cleanupFn += (System.Action) (() =>
    {
      if (spawn.IsNullOrDestroyed())
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) spawn.gameObject);
    });
    Rotatable component1 = spawn.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetOrientation(prefabInfo.rotationOrientation);
    KBatchedAnimController kanim = spawn.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) kanim != (UnityEngine.Object) null)
    {
      kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
      kanim.isMovable = true;
      kanim.Offset = buildingPrefab.Def.GetVisualizerOffset();
      kanim.name = kanim.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
      kanim.TintColour = (Color32) StampToolPreviewUtil.COLOR_OK;
      kanim.SetLayer(layer);
    }
    spawn.transform.SetParent(context.previewParent.transform, false);
    spawn.transform.SetLocalPosition((Vector3) new Vector2((float) prefabInfo.location_x, (float) prefabInfo.location_y));
    context.frameAfterSetupFn += (System.Action) (() =>
    {
      if (spawn.IsNullOrDestroyed())
        return;
      spawn.gameObject.SetActive(false);
      spawn.gameObject.SetActive(true);
      if (kanim.IsNullOrDestroyed())
        return;
      string anim_name1 = "";
      if ((prefabInfo.connections & 1) != 0)
        anim_name1 += "L";
      if ((prefabInfo.connections & 2) != 0)
        anim_name1 += "R";
      if ((prefabInfo.connections & 4) != 0)
        anim_name1 += "U";
      if ((prefabInfo.connections & 8) != 0)
        anim_name1 += "D";
      if (anim_name1 == "")
        anim_name1 = "None";
      if (!((UnityEngine.Object) kanim != (UnityEngine.Object) null) || !kanim.HasAnimation((HashedString) anim_name1))
        return;
      string anim_name2 = anim_name1 + "_place";
      kanim.Play((HashedString) (kanim.HasAnimation((HashedString) anim_name2) ? anim_name2 : anim_name1), KAnim.PlayMode.Loop);
    });
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      if (kanim.IsNullOrDestroyed())
        return;
      Color color = error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
      if (buildingPrefab.Def.SceneLayer == Grid.SceneLayer.Backwall)
        color.a = 0.2f;
      kanim.TintColour = (Color32) color;
    });
    BuildingFacade component2 = spawn.GetComponent<BuildingFacade>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || prefabInfo.facadeId.IsNullOrWhiteSpace())
      return;
    BuildingFacadeResource facade = Db.GetBuildingFacades().TryGet(prefabInfo.facadeId);
    if (facade == null || !facade.IsUnlocked())
      return;
    component2.ApplyBuildingFacade(facade);
  }

  public static void SpawnPrefab_Default(
    StampToolPreviewContext context,
    Prefab prefabInfo,
    GameObject prefab)
  {
    KBatchedAnimController component1 = prefab.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    string name = prefab.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
    int layer = LayerMask.NameToLayer("Place");
    GameObject spawn = new GameObject(name);
    spawn.SetActive(false);
    KBatchedAnimController kanim = spawn.AddComponent<KBatchedAnimController>();
    if (!component1.IsNullOrDestroyed())
    {
      kanim.AnimFiles = component1.AnimFiles;
      kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
      kanim.isMovable = true;
      kanim.name = name;
      kanim.TintColour = (Color32) StampToolPreviewUtil.COLOR_OK;
      kanim.SetLayer(layer);
    }
    spawn.transform.SetParent(context.previewParent.transform, false);
    OccupyArea component2 = prefab.GetComponent<OccupyArea>();
    int num1;
    if (component2.IsNullOrDestroyed() || component2._UnrotatedOccupiedCellsOffsets.Length == 0)
    {
      num1 = 0;
    }
    else
    {
      int num2 = int.MaxValue;
      int num3 = int.MinValue;
      foreach (CellOffset occupiedCellsOffset in component2._UnrotatedOccupiedCellsOffsets)
      {
        if (occupiedCellsOffset.x < num2)
          num2 = occupiedCellsOffset.x;
        if (occupiedCellsOffset.x > num3)
          num3 = occupiedCellsOffset.x;
      }
      num1 = num3 - num2 + 1;
    }
    if ((num1 == 0 ? 0 : (num1 % 2 == 0 ? 1 : 0)) != 0)
      spawn.transform.SetLocalPosition((Vector3) new Vector2((float) prefabInfo.location_x + Grid.HalfCellSizeInMeters, (float) prefabInfo.location_y));
    else
      spawn.transform.SetLocalPosition((Vector3) new Vector2((float) prefabInfo.location_x, (float) prefabInfo.location_y));
    context.frameAfterSetupFn += (System.Action) (() =>
    {
      if (spawn.IsNullOrDestroyed())
        return;
      spawn.gameObject.SetActive(false);
      spawn.gameObject.SetActive(true);
      if (kanim.IsNullOrDestroyed())
        return;
      kanim.Play((HashedString) "place", KAnim.PlayMode.Loop);
    });
    context.cleanupFn += (System.Action) (() =>
    {
      if (spawn.IsNullOrDestroyed())
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) spawn.gameObject);
    });
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      if (kanim.IsNullOrDestroyed())
        return;
      kanim.TintColour = (Color32) (error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
    });
  }
}
