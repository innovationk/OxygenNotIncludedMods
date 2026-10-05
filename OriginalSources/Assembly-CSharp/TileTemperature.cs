// Decompiled with JetBrains decompiler
// Type: TileTemperature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private KSelectable selectable;

  protected override void OnPrefabInit()
  {
    this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
    this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
    base.OnPrefabInit();
  }

  protected override void OnSpawn() => base.OnSpawn();

  private static float OnGetTemperature(PrimaryElement primary_element)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if (!((Object) component != (Object) null) || !component.IsReady())
      return primary_element.InternalTemperature;
    int cell = Grid.PosToCell(primary_element.transform.GetPosition());
    return Grid.Temperature[cell];
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if ((Object) component != (Object) null && component.IsReady())
      Debug.LogWarning((object) "Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
    else
      primary_element.InternalTemperature = temperature;
  }
}
