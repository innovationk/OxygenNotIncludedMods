// Decompiled with JetBrains decompiler
// Type: ClusterFXEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
  [SerializeField]
  public string kAnimName;
  [SerializeField]
  public string animName;
  public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;
  public Vector3 animOffset;

  public override string Name => (string) UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;

  public override EntityLayer Layer => EntityLayer.FX;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) this.kAnimName),
          initialAnim = this.animName,
          playMode = this.animPlayMode,
          animOffset = this.animOffset
        }
      };
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Visible;

  public void Init(AxialI location, Vector3 animOffset)
  {
    this.Location = location;
    this.animOffset = animOffset;
  }
}
