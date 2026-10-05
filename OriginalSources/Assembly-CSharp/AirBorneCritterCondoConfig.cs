// Decompiled with JetBrains decompiler
// Type: AirBorneCritterCondoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class AirBorneCritterCondoConfig : IBuildingConfig
{
  public const string ID = "AirBorneCritterCondo";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[1]{ 200f };
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = TUNING.BUILDINGS.DECOR.BONUS.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AirBorneCritterCondo", 3, 3, "critter_condo_airborne_kanim", 100, 120f, construction_mass, plastics, 1600f, BuildLocationRule.OnCeiling, tieR3, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    Effect resource = new Effect("InteractedWithAirborneCondo", (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, (string) STRINGS.CREATURES.MODIFIERS.AIRBORNECRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false);
    resource.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME));
    Db.Get().effects.Add(resource);
    CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
    def.IsCritterCondoOperationalCb = (Func<CritterCondo.Instance, bool>) (condo_smi => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom());
    def.moveToStatusItem = new StatusItem("AIRBORNECRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.interactStatusItem = new StatusItem("AIRBORNECRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.condoTag = (Tag) "AirBorneCritterCondo";
    def.effectId = resource.Id;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
  }

  public override void ConfigurePost(BuildingDef def)
  {
  }
}
