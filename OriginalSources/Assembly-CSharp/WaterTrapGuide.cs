// Decompiled with JetBrains decompiler
// Type: WaterTrapGuide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
  private int previousDepthAvailable = -1;
  public GameObject parent;
  public bool occupyTiles;
  private KBatchedAnimController parentController;
  private KBatchedAnimController guideController;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.parentController = this.parent.GetComponent<KBatchedAnimController>();
    this.guideController = this.GetComponent<KBatchedAnimController>();
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  private void RefreshTint() => this.guideController.TintColour = this.parentController.TintColour;

  public void RefreshPosition()
  {
    if (!((Object) this.guideController != (Object) null) || !this.guideController.IsMoving)
      return;
    this.guideController.SetDirty();
  }

  private void RefreshDepthAvailable()
  {
    int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell((KMonoBehaviour) this), this.parent);
    if (depthAvailable == this.previousDepthAvailable)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (depthAvailable == 0)
    {
      component.enabled = false;
    }
    else
    {
      component.enabled = true;
      component.Play(new HashedString("place_pipe" + depthAvailable.ToString()));
    }
    if (this.occupyTiles)
      WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
    this.previousDepthAvailable = depthAvailable;
  }

  public void RenderEveryTick(float dt)
  {
    this.RefreshPosition();
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  public static void OccupyArea(GameObject go, int depth_available)
  {
    int cell = Grid.PosToCell(go.transform.GetPosition());
    for (int index = 1; index <= 4; ++index)
    {
      int key = Grid.OffsetCell(cell, 0, -index);
      if (index <= depth_available)
        Grid.ObjectLayers[1][key] = go;
      else if (Grid.ObjectLayers[1].ContainsKey(key) && (Object) Grid.ObjectLayers[1][key] == (Object) go)
        Grid.ObjectLayers[1][key] = (GameObject) null;
    }
  }

  public static int GetDepthAvailable(int root_cell, GameObject pump)
  {
    int num1 = 4;
    int depthAvailable = 0;
    for (int index = 1; index <= num1; ++index)
    {
      int num2 = Grid.OffsetCell(root_cell, 0, -index);
      if (Grid.IsValidCell(num2) && !Grid.Solid[num2] && (!Grid.ObjectLayers[1].ContainsKey(num2) || (Object) Grid.ObjectLayers[1][num2] == (Object) null || (Object) Grid.ObjectLayers[1][num2] == (Object) pump))
        depthAvailable = index;
      else
        break;
    }
    return depthAvailable;
  }
}
