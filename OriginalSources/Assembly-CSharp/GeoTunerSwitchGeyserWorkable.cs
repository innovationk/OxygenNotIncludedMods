// Decompiled with JetBrains decompiler
// Type: GeoTunerSwitchGeyserWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class GeoTunerSwitchGeyserWorkable : Workable
{
  private const string animName = "anim_use_remote_kanim";

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_remote_kanim")
    };
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(3f);
  }
}
