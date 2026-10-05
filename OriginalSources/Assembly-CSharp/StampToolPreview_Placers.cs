// Decompiled with JetBrains decompiler
// Type: StampToolPreview_Placers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StampToolPreview_Placers : IStampToolPreviewPlugin
{
  private List<GameObject> inUse = new List<GameObject>();
  private GameObjectPool pool;
  private Transform poolParent;

  public StampToolPreview_Placers(GameObject placerPrefab)
  {
    StampToolPreview_Placers toolPreviewPlacers = this;
    this.pool = new GameObjectPool((Func<GameObject>) (() =>
    {
      if ((UnityEngine.Object) toolPreviewPlacers.poolParent == (UnityEngine.Object) null)
        toolPreviewPlacers.poolParent = new GameObject("StampToolPreview::PlacerPool").transform;
      GameObject gameObject = Util.KInstantiate(placerPrefab, toolPreviewPlacers.poolParent.gameObject);
      gameObject.SetActive(false);
      return gameObject;
    }), (Action<GameObject>) (_ => { }));
  }

  public void Setup(StampToolPreviewContext context)
  {
    for (int index = 0; index < context.stampTemplate.cells.Count; ++index)
    {
      TemplateClasses.Cell cell = context.stampTemplate.cells[index];
      GameObject instance = this.pool.GetInstance();
      instance.transform.SetParent(context.previewParent.transform, false);
      instance.transform.localPosition = new Vector3((float) cell.location_x, (float) cell.location_y);
      instance.SetActive(true);
      this.inUse.Add(instance);
    }
    context.onErrorChangeFn += (Action<string>) (error =>
    {
      foreach (GameObject gameObject in this.inUse)
      {
        if (!gameObject.IsNullOrDestroyed())
          gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = error != null ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
      }
    });
    context.cleanupFn += (System.Action) (() =>
    {
      foreach (GameObject instance in this.inUse)
      {
        if (!instance.IsNullOrDestroyed())
        {
          instance.SetActive(false);
          instance.transform.SetParent(this.poolParent);
          this.pool.ReleaseInstance(instance);
        }
      }
      this.inUse.Clear();
    });
  }
}
