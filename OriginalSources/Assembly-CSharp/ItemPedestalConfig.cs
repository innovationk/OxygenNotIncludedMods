// Decompiled with JetBrains decompiler
// Type: ItemPedestalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ItemPedestalConfig : IBuildingConfig
{
  public const string ID = "ItemPedestal";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ItemPedestal", 1, 2, "pedestal_kanim", 10, 30f, tieR2, rawMinerals, 800f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.DefaultAnimState = "pedestal";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "small";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>((IEnumerable<Storage.StoredItemModifier>) new Storage.StoredItemModifier[2]
    {
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Preserve
    }));
    Prioritizable.AddRef(go);
    OrnamentReceptacle ornamentReceptacle = go.AddOrGet<OrnamentReceptacle>();
    ornamentReceptacle.AddDepositTag(GameTags.Ornament);
    ornamentReceptacle.AddDepositTag(GameTags.Suit);
    ornamentReceptacle.AddDepositTag(GameTags.Clothes);
    ornamentReceptacle.AddDepositTag(GameTags.Egg);
    ornamentReceptacle.AddDepositTag(GameTags.Seed);
    ornamentReceptacle.AddDepositTag(GameTags.Edible);
    ornamentReceptacle.AddDepositTag(GameTags.BionicUpgrade);
    ornamentReceptacle.AddDepositTag(GameTags.Solid);
    ornamentReceptacle.AddDepositTag(GameTags.Liquid);
    ornamentReceptacle.AddDepositTag(GameTags.Gas);
    ornamentReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
    ornamentReceptacle.occupyingObjectRelativePosition = new Vector3(0.0f, 1.2f, -1f);
    go.AddOrGet<DecorProvider>();
    go.AddOrGet<ItemPedestal>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
    go.GetComponent<KPrefabID>().AddTag(GameTags.OrnamentDisplayer);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
