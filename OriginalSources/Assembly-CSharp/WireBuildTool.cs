// Decompiled with JetBrains decompiler
// Type: WireBuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class WireBuildTool : BaseUtilityBuildTool
{
  public static WireBuildTool Instance;

  public static void DestroyInstance() => WireBuildTool.Instance = (WireBuildTool) null;

  protected override void OnPrefabInit()
  {
    WireBuildTool.Instance = this;
    base.OnPrefabInit();
    this.viewMode = OverlayModes.Power.ID;
  }

  protected override void ApplyPathToConduitSystem()
  {
    if (this.path.Count < 2)
      return;
    for (int index = 1; index < this.path.Count; ++index)
    {
      if (this.path[index - 1].valid && this.path[index].valid)
      {
        int cell1 = this.path[index - 1].cell;
        int cell2 = this.path[index].cell;
        UtilityConnections cell3 = UtilityConnectionsExtensions.DirectionFromToCell(cell1, this.path[index].cell);
        if (cell3 != (UtilityConnections) 0)
        {
          UtilityConnections new_connection = cell3.InverseDirection();
          this.conduitMgr.AddConnection(cell3, cell1, false);
          this.conduitMgr.AddConnection(new_connection, cell2, false);
        }
      }
    }
  }
}
