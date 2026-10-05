// Decompiled with JetBrains decompiler
// Type: PumpingStationGuide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRender200ms
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

  public void RefreshPosition()
  {
    if (!((Object) this.guideController != (Object) null) || !this.guideController.IsMoving)
      return;
    this.guideController.SetDirty();
  }

  private void RefreshTint() => this.guideController.TintColour = this.parentController.TintColour;

  private void RefreshDepthAvailable()
  {
    int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell((KMonoBehaviour) this), this.parent);
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
      PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
    this.previousDepthAvailable = depthAvailable;
  }

  public void Render200ms(float dt)
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
      int key1 = Grid.OffsetCell(cell, 0, -index);
      int key2 = Grid.OffsetCell(cell, 1, -index);
      if (index <= depth_available)
      {
        Grid.ObjectLayers[1][key1] = go;
        Grid.ObjectLayers[1][key2] = go;
      }
      else
      {
        if (Grid.ObjectLayers[1].ContainsKey(key1) && (Object) Grid.ObjectLayers[1][key1] == (Object) go)
          Grid.ObjectLayers[1][key1] = (GameObject) null;
        if (Grid.ObjectLayers[1].ContainsKey(key2) && (Object) Grid.ObjectLayers[1][key2] == (Object) go)
          Grid.ObjectLayers[1][key2] = (GameObject) null;
      }
    }
  }

  public static int GetDepthAvailable(int root_cell, GameObject pump)
  {
    int num1 = 4;
    int depthAvailable = 0;
    for (int index = 1; index <= num1; ++index)
    {
      int num2 = Grid.OffsetCell(root_cell, 0, -index);
      int num3 = Grid.OffsetCell(root_cell, 1, -index);
      if (Grid.IsValidCell(num2) && !Grid.Solid[num2] && Grid.IsValidCell(num3) && !Grid.Solid[num3] && (!Grid.ObjectLayers[1].ContainsKey(num2) || (Object) Grid.ObjectLayers[1][num2] == (Object) null || (Object) Grid.ObjectLayers[1][num2] == (Object) pump) && (!Grid.ObjectLayers[1].ContainsKey(num3) || (Object) Grid.ObjectLayers[1][num3] == (Object) null || (Object) Grid.ObjectLayers[1][num3] == (Object) pump))
        depthAvailable = index;
      else
        break;
    }
    return depthAvailable;
  }
}
