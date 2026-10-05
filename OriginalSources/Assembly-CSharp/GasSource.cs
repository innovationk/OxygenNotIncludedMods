// Decompiled with JetBrains decompiler
// Type: GasSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
  protected override CellOffset[] GetOffsetGroup() => OffsetGroups.LiquidSource;

  protected override IChunkManager GetChunkManager() => (IChunkManager) GasSourceManager.Instance;

  protected override void OnCleanUp() => base.OnCleanUp();
}
