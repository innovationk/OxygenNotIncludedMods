// Decompiled with JetBrains decompiler
// Type: LargeBackwallFarmConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LargeBackwallFarmConfig : IBuildingConfig
{
  public const string ID = "LargeBackwallFarm";

  public override string[] GetRequiredDlcIds()
  {
    return new string[1]{ "DLC5_ID" };
  }

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LargeBackwallFarm", 2, 2, "backwall_planter_box_kanim", 250, 60f, new float[2]
    {
      100f,
      50f
    }, new string[2]{ "Metal", "Glasses" }, 800f, BuildLocationRule.Anywhere, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.ObjectLayer = ObjectLayer.Backwall;
    buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
    buildingDef.DragBuild = true;
    buildingDef.Replaceable = false;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Repairable = false;
    buildingDef.ReplacementTags = new List<Tag>()
    {
      GameTags.Backwall
    };
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "medium";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    KPrefabID component = go.GetComponent<KPrefabID>();
    BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.occupyingObjectRelativePosition = new Vector3(0.49f, 0.0f, -0.5f);
    plantablePlot.AddDepositTag(GameTags.BackwallSeed);
    plantablePlot.AddAdditionalCriteria(new Func<GameObject, bool>(LargeBackwallFarmConfig.ForbiddenTags));
    plantablePlot.SetFertilizationFlags(true, false);
    plantablePlot.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    go.AddOrGet<LoopingSounds>();
    KPrefabID.PrefabFn prefabFn = new KPrefabID.PrefabFn(this.OnPrefabInit);
    component.prefabInitFn += prefabFn;
    go.AddComponent<ZoneTile>();
  }

  private void OnPrefabInit(GameObject instance)
  {
    instance.AddOrGet<PlantablePlot>().AddAdditionalCriteria(new Func<GameObject, bool>(LargeBackwallFarmConfig.ForbiddenTags));
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<PlantablePlot>().AddAdditionalCriteria(new Func<GameObject, bool>(LargeBackwallFarmConfig.ForbiddenTags));
  }

  private static bool ForbiddenTags(GameObject objInQuestion)
  {
    return !objInQuestion.GetComponent<KPrefabID>().HasTag(GameTags.DecorSeed);
  }
}
