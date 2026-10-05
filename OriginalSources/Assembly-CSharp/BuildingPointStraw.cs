// Decompiled with JetBrains decompiler
// Type: BuildingPointStraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BuildingPointStraw : KMonoBehaviour
{
  public const string SYMBOL_PREFIX = "straw";
  public const string ANIM_PREFIX = "on";
  public bool canControlAnimStates;
  public bool usesSymbols;
  public bool isInLiquid;
  public int maxDepth = 4;
  private int depthAvailable = -1;
  private HandleVector<int>.Handle partitionerEntry_solids;
  private HandleVector<int>.Handle partitionerEntry_buildings;
  private HandleVector<int>.Handle partitionerEntry_liquid;

  public int currentDepth => this.depthAvailable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshDepthAvailable();
    this.RegisterListenersToCellChanges();
  }

  protected override void OnCleanUp()
  {
    this.UnregisterListenersToCellChanges();
    base.OnCleanUp();
  }

  public int GetDepthOffset() => this.depthAvailable <= 0 ? -1 : -this.depthAvailable;

  public string GetSymbolSuffix()
  {
    return this.depthAvailable <= 0 ? "1" : this.depthAvailable.ToString();
  }

  public string GetAnimSuffix()
  {
    return this.depthAvailable <= 0 ? "_1" : "_" + this.depthAvailable.ToString();
  }

  public CellOffset GetBottomCellOffset() => new CellOffset(0, this.GetDepthOffset());

  public int GetStrawCell()
  {
    return Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.GetBottomCellOffset());
  }

  private int GetDepthAvailable()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int depthAvailable = 0;
    bool flag = false;
    for (int index = 1; index <= this.maxDepth; ++index)
    {
      int num = Grid.OffsetCell(cell, 0, -index);
      if (Grid.IsValidCell(num) && !Grid.Solid[num] && (!Grid.ObjectLayers[1].ContainsKey(num) || !((UnityEngine.Object) Grid.ObjectLayers[1][num] != (UnityEngine.Object) null) || !((UnityEngine.Object) Grid.ObjectLayers[1][num] != (UnityEngine.Object) this.gameObject)))
      {
        depthAvailable = index;
        if (Grid.IsLiquid(num))
        {
          if (!flag)
            flag = true;
          else
            break;
        }
      }
      else
        break;
    }
    return depthAvailable;
  }

  private void RefreshDepthAvailable()
  {
    int depthAvailable = this.GetDepthAvailable();
    bool flag1 = depthAvailable != this.depthAvailable;
    this.depthAvailable = depthAvailable;
    bool flag2 = Grid.IsLiquid(this.GetStrawCell());
    int num1 = this.isInLiquid != flag2 ? 1 : 0;
    this.isInLiquid = flag2;
    int num2 = flag1 ? 1 : 0;
    if ((num1 | num2) == 0)
      return;
    this.RefreshAnims();
    this.Trigger(360192579, (object) this);
  }

  private void RefreshAnims()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (this.usesSymbols)
    {
      for (int index = 1; index <= this.maxDepth; ++index)
      {
        string symbol = "straw" + index.ToString();
        bool is_visible = index <= this.depthAvailable;
        component.SetSymbolVisiblity((KAnimHashedString) symbol, is_visible);
      }
    }
    else
    {
      if (!this.canControlAnimStates)
        return;
      string anim_name = "on" + (this.depthAvailable > 0 ? "_" + this.depthAvailable.ToString() : "_1");
      component.Play((HashedString) anim_name);
    }
  }

  private void OnCellChanged(object data) => this.RefreshDepthAvailable();

  private void RegisterListenersToCellChanges()
  {
    CellOffset[] offsets = new CellOffset[this.maxDepth];
    for (int index = 0; index < this.maxDepth; ++index)
      offsets[index] = new CellOffset(0, -(index + 1));
    Extents extents = new Extents(Grid.PosToCell(this.transform.GetPosition()), offsets);
    this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("FishDeliveryPointStraw", (object) this.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnCellChanged));
    this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("FishDeliveryPointStraw", (object) this.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnCellChanged));
    this.partitionerEntry_liquid = GameScenePartitioner.Instance.Add("FishDeliveryPointStraw", (object) this.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnCellChanged));
  }

  private void UnregisterListenersToCellChanges()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry_liquid);
  }
}
