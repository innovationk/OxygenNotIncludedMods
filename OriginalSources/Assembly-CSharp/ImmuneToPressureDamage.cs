// Decompiled with JetBrains decompiler
// Type: ImmuneToPressureDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ImmuneToPressureDamage : KMonoBehaviour
{
  public CellOffset[] Cells = new CellOffset[1]
  {
    new CellOffset(0, 0)
  };

  protected override void OnPrefabInit()
  {
    foreach (CellOffset cell in this.Cells)
      SimMessages.SetCellProperties(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), cell), (byte) 8);
    base.OnPrefabInit();
  }

  protected override void OnCleanUp()
  {
    foreach (CellOffset cell in this.Cells)
      SimMessages.ClearCellProperties(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), cell), (byte) 8);
  }
}
