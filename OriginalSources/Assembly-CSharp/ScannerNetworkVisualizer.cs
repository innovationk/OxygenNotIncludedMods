// Decompiled with JetBrains decompiler
// Type: ScannerNetworkVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
  public Vector2I OriginOffset = new Vector2I(0, 0);
  public int RangeMin;
  public int RangeMax;

  protected override void OnSpawn()
  {
    Components.ScannerVisualizers.Add(this.gameObject.GetMyWorldId(), this);
  }

  protected override void OnCleanUp()
  {
    Components.ScannerVisualizers.Remove(this.gameObject.GetMyWorldId(), this);
  }
}
