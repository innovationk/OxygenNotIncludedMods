// Decompiled with JetBrains decompiler
// Type: OreSizeVisualizerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public struct OreSizeVisualizerData
{
  public PrimaryElement primaryElement;
  public OreSizeVisualizerComponents.TiersSetType tierSetType;
  public int absorbHandle;
  public int splitFromChunkHandle;

  public OreSizeVisualizerData(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.tierSetType = OreSizeVisualizerComponents.TiersSetType.Ores;
    this.absorbHandle = -1;
    this.splitFromChunkHandle = -1;
  }
}
