// Decompiled with JetBrains decompiler
// Type: UtilityNetworkGridNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
  public UtilityConnections connections;
  public int networkIdx;
  public const int InvalidNetworkIdx = -1;

  public bool Equals(UtilityNetworkGridNode other)
  {
    return this.connections == other.connections && this.networkIdx == other.networkIdx;
  }

  public override bool Equals(object obj) => ((UtilityNetworkGridNode) obj).Equals(this);

  public override int GetHashCode() => base.GetHashCode();

  public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y) => x.Equals(y);

  public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
  {
    return !x.Equals(y);
  }
}
