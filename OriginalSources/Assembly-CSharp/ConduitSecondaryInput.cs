// Decompiled with JetBrains decompiler
// Type: ConduitSecondaryInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
  [SerializeField]
  public ConduitPortInfo portInfo;

  public bool HasSecondaryConduitType(ConduitType type) => this.portInfo.conduitType == type;

  public CellOffset GetSecondaryConduitOffset(ConduitType type)
  {
    return this.portInfo.conduitType == type ? this.portInfo.offset : CellOffset.none;
  }
}
