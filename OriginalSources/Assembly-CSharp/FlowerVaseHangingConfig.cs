// Decompiled with JetBrains decompiler
// Type: FlowerVaseHangingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class FlowerVaseHangingConfig : IBuildingConfig
{
  public const string ID = "FlowerVaseHanging";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlowerVaseHanging", 1, 2, "flowervase_hanging_basic_kanim", 10, 10f, tieR1, rawMetals, 800f, BuildLocationRule.OnCeiling, none2, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    buildingDef.GenerateOffsets(1, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.DecorSeed);
    plantablePlot.occupyingObjectVisualOffset = new Vector3(0.0f, -0.25f, 0.0f);
    go.AddOrGet<FlowerVase>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
