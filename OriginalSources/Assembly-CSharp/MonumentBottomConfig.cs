// Decompiled with JetBrains decompiler
// Type: MonumentBottomConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class MonumentBottomConfig : IBuildingConfig
{
  public const string ID = "MonumentBottom";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 7500f, 2500f };
    string[] construction_materials = new string[2]
    {
      SimHashes.Steel.ToString(),
      SimHashes.Obsidian.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues incomplete = TUNING.BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MonumentBottom", 5, 5, "monument_base_a_kanim", 1000, 60f, construction_mass, construction_materials, 9999f, BuildLocationRule.OnFloor, incomplete, noise);
    BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = (Tag) "MonumentBottom";
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.STATUE);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), (Tag) "MonumentMiddle", (AttachableBuilding) null)
    };
    go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Bottom;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
      monumentPart.part = MonumentPartResource.Part.Bottom;
      monumentPart.stateUISymbol = "base";
    });
  }
}
