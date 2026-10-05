// Decompiled with JetBrains decompiler
// Type: UnderwaterCritterCondoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterCritterCondoConfig : IBuildingConfig
{
  public const string ID = "UnderwaterCritterCondo";
  public static readonly Operational.Flag Submerged = new Operational.Flag(nameof (Submerged), Operational.Flag.Type.Requirement);
  private static string[] AllFGSymbols = new string[3]
  {
    "doorway_fg",
    "condo_fg",
    "doorway_squid_fg"
  };
  private static Dictionary<CritterCondo.CreatureFGLayerType, string> AnimFGLayersToSymbolName = new Dictionary<CritterCondo.CreatureFGLayerType, string>()
  {
    [CritterCondo.CreatureFGLayerType.SmallCreatureLayer] = UnderwaterCritterCondoConfig.AllFGSymbols[0],
    [CritterCondo.CreatureFGLayerType.LargeCreatureLayer] = UnderwaterCritterCondoConfig.AllFGSymbols[1],
    [CritterCondo.CreatureFGLayerType.SquidLayer] = UnderwaterCritterCondoConfig.AllFGSymbols[2]
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[1]{ 200f };
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = TUNING.BUILDINGS.DECOR.BONUS.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("UnderwaterCritterCondo", 3, 3, "underwater_critter_condo_kanim", 100, 120f, construction_mass, plastics, 1600f, BuildLocationRule.OnFloor, tieR3, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.Floodable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WATER);
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
  }

  private static StatusItem GetSubmergableStatusItem() => Db.Get().BuildingStatusItems.NotSubmerged;

  private static void DisableAllFGSymbols(KBatchedAnimController animController)
  {
    if ((UnityEngine.Object) animController == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < UnderwaterCritterCondoConfig.AllFGSymbols.Length; ++index)
    {
      string allFgSymbol = UnderwaterCritterCondoConfig.AllFGSymbols[index];
      animController.SetSymbolVisiblity((KAnimHashedString) allFgSymbol, false);
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<BuildingSubmergable>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    Effect resource = new Effect("InteractedWithUnderwaterCondo", (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, (string) STRINGS.CREATURES.MODIFIERS.UNDERWATERCRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false);
    resource.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME));
    Db.Get().effects.Add(resource);
    CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
    def.IsCritterCondoOperationalCb = (Func<CritterCondo.Instance, bool>) (condo_smi =>
    {
      if (!condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom())
        return false;
      Building component1 = condo_smi.GetComponent<Building>();
      for (int index = 0; index < component1.PlacementCells.Length; ++index)
      {
        if (!Grid.IsLiquid(component1.PlacementCells[index]))
          return false;
      }
      Operational component2 = condo_smi.GetComponent<Operational>();
      return !((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.IsOperational;
    });
    def.UpdateForegroundVisibilitySymbols = (Action<KBatchedAnimController, CritterCondo.CreatureFGLayerType>) ((foreground_controller, layer) =>
    {
      if (!((UnityEngine.Object) foreground_controller != (UnityEngine.Object) null))
        return;
      UnderwaterCritterCondoConfig.DisableAllFGSymbols(foreground_controller);
      foreground_controller.SetSymbolVisiblity((KAnimHashedString) UnderwaterCritterCondoConfig.AnimFGLayersToSymbolName[layer], true);
    });
    def.moveToStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.interactStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.condoTag = (Tag) "UnderwaterCritterCondo";
    def.effectId = resource.Id;
  }

  public override void ConfigurePost(BuildingDef def)
  {
  }
}
