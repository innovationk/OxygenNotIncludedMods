// Decompiled with JetBrains decompiler
// Type: PartialLightBlocking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
  private const byte PartialLightBlockingProperties = 48 /*0x30*/;

  protected override void OnSpawn()
  {
    this.SetLightBlocking();
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    this.ClearLightBlocking();
    base.OnCleanUp();
  }

  public void SetLightBlocking()
  {
    foreach (int placementCell in this.GetComponent<Building>().PlacementCells)
      SimMessages.SetCellProperties(placementCell, (byte) 48 /*0x30*/);
  }

  public void ClearLightBlocking()
  {
    foreach (int placementCell in this.GetComponent<Building>().PlacementCells)
      SimMessages.ClearCellProperties(placementCell, (byte) 48 /*0x30*/);
  }
}
