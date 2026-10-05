// Decompiled with JetBrains decompiler
// Type: ResearchDestination
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
  public override string Name => (string) UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;

  public override EntityLayer Layer => EntityLayer.POI;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get => new List<ClusterGridEntity.AnimConfig>();
  }

  public override bool IsVisible => false;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

  public void Init(AxialI location) => this.m_location = location;
}
