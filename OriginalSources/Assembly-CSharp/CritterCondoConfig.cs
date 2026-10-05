// Decompiled with JetBrains decompiler
// Type: CritterCondoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class CritterCondoConfig : IBuildingConfig
{
  public const string ID = "CritterCondo";
  public const float EFFECT_DURATION_IN_SECONDS = 600f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 200f, 10f };
    string[] construction_materials = new string[2]
    {
      "BuildableRaw",
      "BuildingFiber"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = TUNING.BUILDINGS.DECOR.BONUS.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterCondo", 3, 3, "critter_condo_kanim", 100, 120f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR3, noise);
    buildingDef.AudioCategory = "Metal";
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
    Effect resource = new Effect("InteractedWithCritterCondo", (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false);
    resource.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, (string) STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME));
    Db.Get().effects.Add(resource);
    CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
    def.IsCritterCondoOperationalCb = (Func<CritterCondo.Instance, bool>) (condo_smi => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom() && !condo_smi.GetComponent<Floodable>().IsFlooded);
    def.moveToStatusItem = new StatusItem("CRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.interactStatusItem = new StatusItem("CRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    def.effectId = resource.Id;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
  }

  public override void ConfigurePost(BuildingDef def)
  {
  }
}
