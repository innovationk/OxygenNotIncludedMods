// Decompiled with JetBrains decompiler
// Type: StampToolPreview_Area
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class StampToolPreview_Area : IStampToolPreviewPlugin
{
  public static Material material;

  public void Setup(StampToolPreviewContext context)
  {
    if ((UnityEngine.Object) StampToolPreview_Area.material == (UnityEngine.Object) null)
    {
      StampToolPreview_Area.material = StampToolPreviewUtil.MakeMaterial((Texture) Assets.GetTexture("stamptool_vis_background"));
      StampToolPreview_Area.material.name = $"Area ({StampToolPreview_Area.material.name})";
    }
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      Color color = (error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK) with
      {
        a = 1f
      };
      StampToolPreview_Area.material.color = color;
    });
    for (int index = 0; index < context.stampTemplate.cells.Count; ++index)
    {
      TemplateClasses.Cell cell = context.stampTemplate.cells[index];
      MeshRenderer meshRenderer;
      GameObject gameObject;
      StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f);
      gameObject.name = "AreaPlacer";
      gameObject.transform.SetParent(context.previewParent, false);
      gameObject.transform.localPosition = new Vector3((float) cell.location_x, (float) cell.location_y + Grid.HalfCellSizeInMeters);
      context.cleanupFn += (System.Action) (() =>
      {
        if (gameObject.IsNullOrDestroyed())
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
      });
      meshRenderer.sharedMaterial = StampToolPreview_Area.material;
    }
  }
}
