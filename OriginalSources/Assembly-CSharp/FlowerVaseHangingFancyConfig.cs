// Decompiled with JetBrains decompiler
// Type: FlowerVaseHangingFancyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class FlowerVaseHangingFancyConfig : IBuildingConfig
{
  public const string ID = "FlowerVaseHangingFancy";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] glasses = TUNING.MATERIALS.GLASSES;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = TUNING.BUILDINGS.DECOR.BONUS.TIER1.amount,
      radius = TUNING.BUILDINGS.DECOR.BONUS.TIER3.radius
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlowerVaseHangingFancy", 1, 2, "flowervase_hanging_kanim", 10, 10f, tieR1, glasses, 800f, BuildLocationRule.OnCeiling, decor, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingUse;
    buildingDef.GenerateOffsets(1, 1);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GLASS);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.DecorSeed);
    plantablePlot.plantLayer = Grid.SceneLayer.BuildingUse;
    plantablePlot.occupyingObjectVisualOffset = new Vector3(0.0f, -0.45f, 0.0f);
    go.AddOrGet<FlowerVase>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
