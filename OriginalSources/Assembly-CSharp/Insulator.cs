// Decompiled with JetBrains decompiler
// Type: Insulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
  [MyCmpReq]
  private Building building;
  [SerializeField]
  public CellOffset offset = CellOffset.none;

  protected override void OnSpawn()
  {
    SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
  }

  protected override void OnCleanUp()
  {
    SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), 1f);
  }
}
