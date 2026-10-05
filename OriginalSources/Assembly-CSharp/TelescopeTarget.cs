// Decompiled with JetBrains decompiler
// Type: TelescopeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
  private ClusterMapMeteorShower.Instance targetMeteorShower;

  public override string Name => (string) UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;

  public override EntityLayer Layer => EntityLayer.Telescope;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "telescope_target_kanim"),
          initialAnim = "idle"
        }
      };
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Visible;

  public void Init(AxialI location) => this.Location = location;

  public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
  {
    this.targetMeteorShower = meteorShower;
  }

  public override bool ShowName() => true;

  public override bool ShowProgressBar() => true;

  public override float GetProgress()
  {
    return this.targetMeteorShower != null ? this.targetMeteorShower.IdentifyingProgress : SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(this.Location);
  }
}
