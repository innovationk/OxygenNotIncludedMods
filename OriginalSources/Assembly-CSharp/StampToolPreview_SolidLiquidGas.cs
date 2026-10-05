// Decompiled with JetBrains decompiler
// Type: StampToolPreview_SolidLiquidGas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TemplateClasses;
using UnityEngine;

#nullable disable
public class StampToolPreview_SolidLiquidGas : IStampToolPreviewPlugin
{
  public static Material solidMaterial;
  public static Material liquidMaterial;
  public static Material gasMaterial;

  public void Setup(StampToolPreviewContext context)
  {
    this.SetupMaterials(context);
    using (HashSetPool<int, StampToolPreview_SolidLiquidGas>.PooledHashSet pooledHashSet = PoolsFor<StampToolPreview_SolidLiquidGas>.AllocateHashSet<int>())
    {
      if (context.stampTemplate.buildings != null)
      {
        foreach (Prefab building in context.stampTemplate.buildings)
        {
          if (!building.IsNullOrDestroyed())
          {
            GameObject prefab = Assets.GetPrefab((Tag) building.id);
            if (!prefab.IsNullOrDestroyed())
            {
              Building component = prefab.GetComponent<Building>();
              if (!component.IsNullOrDestroyed() && component.Def.IsTilePiece)
                pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(building.location_x, building.location_y));
              MakeBaseSolid.Def def = prefab.GetDef<MakeBaseSolid.Def>();
              if (!def.IsNullOrDestroyed())
              {
                foreach (CellOffset solidOffset in def.solidOffsets)
                  pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(building.location_x + solidOffset.x, building.location_y + solidOffset.y));
              }
            }
          }
        }
      }
      if (context.stampTemplate.cells == null)
        return;
      for (int index = 0; index < context.stampTemplate.cells.Count; ++index)
      {
        TemplateClasses.Cell cell = context.stampTemplate.cells[index];
        if (!cell.IsNullOrDestroyed() && !pooledHashSet.Contains(StampToolPreview_SolidLiquidGas.CellHash(cell.location_x, cell.location_y)))
        {
          Element elementByHash = ElementLoader.FindElementByHash(cell.element);
          Element.State? nullable = elementByHash != null ? new Element.State?(elementByHash.state & Element.State.Solid) : new Element.State?();
          if (nullable.HasValue)
          {
            Material material;
            string str;
            switch (nullable.GetValueOrDefault())
            {
              case Element.State.Vacuum:
                material = StampToolPreview_SolidLiquidGas.gasMaterial;
                str = "Vacuum";
                break;
              case Element.State.Gas:
                material = StampToolPreview_SolidLiquidGas.gasMaterial;
                str = "Gas";
                break;
              case Element.State.Liquid:
                material = StampToolPreview_SolidLiquidGas.liquidMaterial;
                str = "Liquid";
                break;
              case Element.State.Solid:
                material = StampToolPreview_SolidLiquidGas.solidMaterial;
                str = "Solid";
                break;
              default:
                continue;
            }
            MeshRenderer meshRenderer;
            GameObject gameObject;
            StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f);
            gameObject.transform.SetParent(context.previewParent, false);
            gameObject.transform.localPosition = new Vector3((float) cell.location_x, (float) cell.location_y + Grid.HalfCellSizeInMeters);
            context.cleanupFn += (System.Action) (() =>
            {
              if (gameObject.IsNullOrDestroyed())
                return;
              UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
            });
            gameObject.name = $"TilePlacer ({str})";
            meshRenderer.material = material;
          }
        }
      }
    }
  }

  private void SetupMaterials(StampToolPreviewContext context)
  {
    if (StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
    {
      StampToolPreview_SolidLiquidGas.solidMaterial = StampToolPreviewUtil.MakeMaterial((Texture) Assets.GetTexture("stamptool_vis_solid"));
      StampToolPreview_SolidLiquidGas.solidMaterial.name = $"Solid ({StampToolPreview_SolidLiquidGas.solidMaterial.name})";
    }
    if (StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
    {
      StampToolPreview_SolidLiquidGas.liquidMaterial = StampToolPreviewUtil.MakeMaterial((Texture) Assets.GetTexture("stamptool_vis_liquid"));
      StampToolPreview_SolidLiquidGas.liquidMaterial.name = $"Liquid ({StampToolPreview_SolidLiquidGas.liquidMaterial.name})";
    }
    if (StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
    {
      StampToolPreview_SolidLiquidGas.gasMaterial = StampToolPreviewUtil.MakeMaterial((Texture) Assets.GetTexture("stamptool_vis_gas"));
      StampToolPreview_SolidLiquidGas.gasMaterial.name = $"Gas ({StampToolPreview_SolidLiquidGas.gasMaterial.name})";
    }
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      Color c = error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
      if (!StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
        StampToolPreview_SolidLiquidGas.solidMaterial.color = WithAlpha(c, 1f);
      if (!StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
        StampToolPreview_SolidLiquidGas.liquidMaterial.color = WithAlpha(c, 1f);
      if (StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
        return;
      StampToolPreview_SolidLiquidGas.gasMaterial.color = WithAlpha(c, 1f);
    });

    static Color WithAlpha(Color c, float a) => new Color(c.r, c.g, c.b, a);
  }

  private static int CellHash(int x, int y) => x + y * 10000;
}
