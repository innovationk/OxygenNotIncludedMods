// Decompiled with JetBrains decompiler
// Type: GravitasBathroomStallConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class GravitasBathroomStallConfig : IBuildingConfig
{
  public const string ID = "GravitasBathroomStall";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR0_1 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = tieR0_1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GravitasBathroomStall", 2, 2, "gravitas_toilet_kanim", 30, 30f, tieR4, rawMetals, 800f, BuildLocationRule.OnFloor, tieR0_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Granite);
    component.Temperature = 294.15f;
    BuildingTemplates.ExtendBuildingToGravitas(go);
    go.AddOrGet<Demolishable>();
    go.AddOrGetDef<GravitasBathroomStall.Def>();
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    base.ConfigureBuildingTemplate(go, prefab_tag);
    Activatable activatable = go.AddOrGet<Activatable>();
    activatable.SetWorkTime(5f);
    activatable.SetButtonTextOverride(new ButtonMenuTextOverride()
    {
      Text = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON,
      ToolTip = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_TOOLTIP,
      CancelText = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_CANCEL,
      CancelToolTip = UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.ACTIVATE_TOILET_BUTTON_CANCEL_TOOLTIP
    });
    activatable.Required = true;
    activatable.synchronizeAnims = true;
    activatable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_gravitas_toilet_kanim")
    };
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
