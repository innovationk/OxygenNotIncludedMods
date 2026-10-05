// Decompiled with JetBrains decompiler
// Type: ModularLaunchpadPortLiquidUnloaderConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
  public const string ID = "ModularLaunchpadPortLiquidUnloader";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
  }
}
