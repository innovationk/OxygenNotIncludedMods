// Decompiled with JetBrains decompiler
// Type: HarvestablePOIClusterGridEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
  public string m_name;
  public string m_Anim;

  public override string Name => this.m_name;

  public override EntityLayer Layer => EntityLayer.POI;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "harvestable_space_poi_kanim"),
          initialAnim = this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim
        }
      };
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

  public void Init(AxialI location) => this.Location = location;

  public override Sprite GetUISprite()
  {
    Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimConfigs[0].animFile, this.AnimConfigs[0].initialAnim);
    return (Object) fromMultiObjectAnim == (Object) null ? base.GetUISprite() : fromMultiObjectAnim;
  }

  public override void onClustermapVisualizerAnimCreated(
    KBatchedAnimController controller,
    ClusterGridEntity.AnimConfig config)
  {
  }
}
