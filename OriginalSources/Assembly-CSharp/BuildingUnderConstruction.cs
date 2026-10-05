// Decompiled with JetBrains decompiler
// Type: BuildingUnderConstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BuildingUnderConstruction : Building
{
  [MyCmpAdd]
  private KSelectable selectable;
  [MyCmpAdd]
  private SaveLoadRoot saveLoadRoot;
  [MyCmpAdd]
  private KPrefabID kPrefabID;

  protected override void OnPrefabInit()
  {
    this.transform.SetPosition(this.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(this.Def.SceneLayer)
    });
    this.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    Rotatable component2 = this.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      component1.Offset = this.Def.GetVisualizerOffset();
    KBoxCollider2D component3 = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
      component3.offset = component3.offset + new Vector2(visualizerOffset.x, visualizerOffset.y);
    }
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.Def.IsTilePiece)
      this.Def.RunOnArea(Grid.PosToCell(this.transform.GetPosition()), this.Orientation, (Action<int>) (c => TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer)));
    this.RegisterBlockTileRenderer(true);
  }

  protected override void OnCleanUp()
  {
    this.UnregisterBlockTileRenderer();
    base.OnCleanUp();
  }
}
