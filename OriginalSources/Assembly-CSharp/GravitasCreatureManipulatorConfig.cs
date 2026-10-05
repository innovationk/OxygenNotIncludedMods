// Decompiled with JetBrains decompiler
// Type: GravitasCreatureManipulatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class GravitasCreatureManipulatorConfig : IBuildingConfig
{
  public const string ID = "GravitasCreatureManipulator";
  public const string CODEX_ENTRY_ID = "STORYTRAITCRITTERMANIPULATOR";
  public const string INITIAL_LORE_UNLOCK_ID = "story_trait_critter_manipulator_initial";
  public const string PARKING_LORE_UNLOCK_ID = "story_trait_critter_manipulator_parking";
  public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_critter_manipulator_complete";
  private const int HEIGHT = 4;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GravitasCreatureManipulator", 3, 4, "gravitas_critter_manipulator_kanim", 250, 120f, tieR5_1, refinedMetals, 3200f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.Floodable = false;
    buildingDef.Entombable = true;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "medium";
    buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    KPrefabID component1 = go.GetComponent<KPrefabID>();
    component1.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    PrimaryElement component2 = go.GetComponent<PrimaryElement>();
    component2.SetElement(SimHashes.Steel);
    component2.Temperature = 294.15f;
    BuildingTemplates.ExtendBuildingToGravitas(go);
    go.AddComponent<Storage>();
    Activatable activatable = go.AddComponent<Activatable>();
    activatable.synchronizeAnims = false;
    activatable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_remote_kanim")
    };
    activatable.SetWorkTime(30f);
    GravitasCreatureManipulator.Def def1 = go.AddOrGetDef<GravitasCreatureManipulator.Def>();
    def1.pickupOffset = new CellOffset(-1, 0);
    def1.dropOffset = new CellOffset(1, 0);
    def1.numSpeciesToUnlockMorphMode = 5;
    def1.workingDuration = 15f;
    def1.cooldownDuration = 540f;
    MakeBaseSolid.Def def2 = go.AddOrGetDef<MakeBaseSolid.Def>();
    def2.solidOffsets = new CellOffset[4];
    for (int y = 0; y < 4; ++y)
      def2.solidOffsets[y] = new CellOffset(0, y);
    component1.prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight));
    component1.prefabSpawnFn += new KPrefabID.PrefabFn(this.OnSpawn);
  }

  private void OnSpawn(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      if (kbatchedAnimController.name.Contains("_fg"))
      {
        kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
        kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
      }
    }
  }

  public static Option<string> GetBodyContentForSpeciesTag(Tag species)
  {
    Option<string> nameForSpeciesTag = GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species);
    Option<string> descriptionForSpeciesTag = GravitasCreatureManipulatorConfig.GetDescriptionForSpeciesTag(species);
    return nameForSpeciesTag.HasValue && descriptionForSpeciesTag.HasValue ? (Option<string>) GravitasCreatureManipulatorConfig.GetBodyContent(nameForSpeciesTag.Value, descriptionForSpeciesTag.Value) : (Option<string>) Option.None;
  }

  public static string GetBodyContentForUnknownSpecies()
  {
    return GravitasCreatureManipulatorConfig.GetBodyContent((string) CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN_TITLE, (string) CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN);
  }

  public static string GetBodyContent(string name, string desc)
  {
    return $"<size=125%><b>{name}</b></size><line-height=150%>\n</line-height>{desc}";
  }

  public static Option<string> GetNameForSpeciesTag(Tag species)
  {
    StringEntry result;
    return !Strings.TryGet("STRINGS.CREATURES.FAMILY_PLURAL." + species.ToString().ToUpper(), out result) ? (Option<string>) Option.None : Option.Some<string>((string) result);
  }

  public static Option<string> GetDescriptionForSpeciesTag(Tag species)
  {
    StringEntry result;
    return !Strings.TryGet("STRINGS.CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES." + species.ToString().ToUpper().Replace("SPECIES", ""), out result) ? (Option<string>) Option.None : Option.Some<string>((string) result);
  }

  public static class CRITTER_LORE_UNLOCK_ID
  {
    public static string For(Tag species)
    {
      return "story_trait_critter_manipulator_" + species.ToString().ToLower();
    }
  }
}
