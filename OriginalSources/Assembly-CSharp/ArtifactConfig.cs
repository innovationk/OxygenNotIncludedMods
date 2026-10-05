// Decompiled with JetBrains decompiler
// Type: ArtifactConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ArtifactConfig : IMultiEntityConfig
{
  public static float ARTIFACT_MASS = 25f;
  public static Dictionary<ArtifactType, List<string>> artifactItems = new Dictionary<ArtifactType, List<string>>();

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    ArtifactConfig.artifactItems.Add(ArtifactType.Terrestrial, new List<string>());
    ArtifactConfig.artifactItems.Add(ArtifactType.Space, new List<string>());
    ArtifactConfig.artifactItems.Add(ArtifactType.Any, new List<string>());
    prefabs.Add(ArtifactConfig.CreateArtifact("Sandstone", (string) UI.SPACEARTIFACTS.SANDSTONE.NAME, (string) UI.SPACEARTIFACTS.SANDSTONE.DESCRIPTION, "idle_layered_rock", "ui_layered_rock", TUNING.DECOR.SPACEARTIFACT.TIER0, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("Sink", (string) UI.SPACEARTIFACTS.SINK.NAME, (string) UI.SPACEARTIFACTS.SINK.DESCRIPTION, "idle_kitchen_sink", "ui_sink", TUNING.DECOR.SPACEARTIFACT.TIER0, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("RubiksCube", (string) UI.SPACEARTIFACTS.RUBIKSCUBE.NAME, (string) UI.SPACEARTIFACTS.RUBIKSCUBE.DESCRIPTION, "idle_rubiks_cube", "ui_rubiks_cube", TUNING.DECOR.SPACEARTIFACT.TIER0, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("OfficeMug", (string) UI.SPACEARTIFACTS.OFFICEMUG.NAME, (string) UI.SPACEARTIFACTS.OFFICEMUG.DESCRIPTION, "idle_coffee_mug", "ui_coffee_mug", TUNING.DECOR.SPACEARTIFACT.TIER0, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("Obelisk", (string) UI.SPACEARTIFACTS.OBELISK.NAME, (string) UI.SPACEARTIFACTS.OBELISK.DESCRIPTION, "idle_tallstone", "ui_tallstone", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("OkayXray", (string) UI.SPACEARTIFACTS.OKAYXRAY.NAME, (string) UI.SPACEARTIFACTS.OKAYXRAY.DESCRIPTION, "idle_xray", "ui_xray", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("Blender", (string) UI.SPACEARTIFACTS.BLENDER.NAME, (string) UI.SPACEARTIFACTS.BLENDER.DESCRIPTION, "idle_blender", "ui_blender", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("Moldavite", (string) UI.SPACEARTIFACTS.MOLDAVITE.NAME, (string) UI.SPACEARTIFACTS.MOLDAVITE.DESCRIPTION, "idle_moldavite", "ui_moldavite", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("VHS", (string) UI.SPACEARTIFACTS.VHS.NAME, (string) UI.SPACEARTIFACTS.VHS.DESCRIPTION, "idle_vhs", "ui_vhs", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("Saxophone", (string) UI.SPACEARTIFACTS.SAXOPHONE.NAME, (string) UI.SPACEARTIFACTS.SAXOPHONE.DESCRIPTION, "idle_saxophone", "ui_saxophone", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("ModernArt", (string) UI.SPACEARTIFACTS.MODERNART.NAME, (string) UI.SPACEARTIFACTS.MODERNART.DESCRIPTION, "idle_abstract_blocks", "ui_abstract_blocks", TUNING.DECOR.SPACEARTIFACT.TIER1, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("HoneyJar", (string) UI.SPACEARTIFACTS.HONEYJAR.NAME, (string) UI.SPACEARTIFACTS.HONEYJAR.DESCRIPTION, "idle_honey_jar", "ui_honey_jar", TUNING.DECOR.SPACEARTIFACT.TIER1, DlcManager.EXPANSION1, (string[]) null, "artifacts_2_kanim", artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("AmeliasWatch", (string) UI.SPACEARTIFACTS.AMELIASWATCH.NAME, (string) UI.SPACEARTIFACTS.AMELIASWATCH.DESCRIPTION, "idle_earnhart_watch", "ui_earnhart_watch", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("TeaPot", (string) UI.SPACEARTIFACTS.TEAPOT.NAME, (string) UI.SPACEARTIFACTS.TEAPOT.DESCRIPTION, "idle_teapot", "ui_teapot", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("BrickPhone", (string) UI.SPACEARTIFACTS.BRICKPHONE.NAME, (string) UI.SPACEARTIFACTS.BRICKPHONE.DESCRIPTION, "idle_brick_phone", "ui_brick_phone", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("RobotArm", (string) UI.SPACEARTIFACTS.ROBOTARM.NAME, (string) UI.SPACEARTIFACTS.ROBOTARM.DESCRIPTION, "idle_robot_arm", "ui_robot_arm", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("ShieldGenerator", (string) UI.SPACEARTIFACTS.SHIELDGENERATOR.NAME, (string) UI.SPACEARTIFACTS.SHIELDGENERATOR.DESCRIPTION, "idle_hologram_generator_loop", "ui_hologram_generator", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, postInitFn: (ArtifactConfig.PostInitFn) (go => go.AddOrGet<LoopingSounds>())));
    prefabs.Add(ArtifactConfig.CreateArtifact("BioluminescentRock", (string) UI.SPACEARTIFACTS.BIOLUMINESCENTROCK.NAME, (string) UI.SPACEARTIFACTS.BIOLUMINESCENTROCK.DESCRIPTION, "idle_bioluminescent_rock", "ui_bioluminescent_rock", TUNING.DECOR.SPACEARTIFACT.TIER2, (string[]) null, (string[]) null, postInitFn: (ArtifactConfig.PostInitFn) (go =>
    {
      Light2D light2D = go.AddOrGet<Light2D>();
      light2D.overlayColour = LIGHT2D.BIOLUMROCK_COLOR;
      light2D.Color = LIGHT2D.BIOLUMROCK_COLOR;
      light2D.Range = 2f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.BIOLUMROCK_DIRECTION;
      light2D.Offset = LIGHT2D.BIOLUMROCK_OFFSET;
      light2D.shape = LightShape.Cone;
      light2D.drawOverlay = true;
    }), artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("GrubStatue", (string) UI.SPACEARTIFACTS.GRUBSTATUE.NAME, (string) UI.SPACEARTIFACTS.GRUBSTATUE.DESCRIPTION, "idle_grub_statue", "ui_grub_statue", TUNING.DECOR.SPACEARTIFACT.TIER2, DlcManager.EXPANSION1, (string[]) null, "artifacts_2_kanim"));
    prefabs.Add(ArtifactConfig.CreateArtifact("Stethoscope", (string) UI.SPACEARTIFACTS.STETHOSCOPE.NAME, (string) UI.SPACEARTIFACTS.STETHOSCOPE.DESCRIPTION, "idle_stethocope", "ui_stethoscope", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("EggRock", (string) UI.SPACEARTIFACTS.EGGROCK.NAME, (string) UI.SPACEARTIFACTS.EGGROCK.DESCRIPTION, "idle_egg_rock_light", "ui_egg_rock_light", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("HatchFossil", (string) UI.SPACEARTIFACTS.HATCHFOSSIL.NAME, (string) UI.SPACEARTIFACTS.HATCHFOSSIL.DESCRIPTION, "idle_fossil_hatch", "ui_fossil_hatch", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("RockTornado", (string) UI.SPACEARTIFACTS.ROCKTORNADO.NAME, (string) UI.SPACEARTIFACTS.ROCKTORNADO.DESCRIPTION, "idle_whirlwind_rock", "ui_whirlwind_rock", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("PacuPercolator", (string) UI.SPACEARTIFACTS.PACUPERCOLATOR.NAME, (string) UI.SPACEARTIFACTS.PACUPERCOLATOR.DESCRIPTION, "idle_percolator", "ui_percolator", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("MagmaLamp", (string) UI.SPACEARTIFACTS.MAGMALAMP.NAME, (string) UI.SPACEARTIFACTS.MAGMALAMP.DESCRIPTION, "idle_lava_lamp", "ui_lava_lamp", TUNING.DECOR.SPACEARTIFACT.TIER3, (string[]) null, (string[]) null, postInitFn: (ArtifactConfig.PostInitFn) (go =>
    {
      Light2D light2D = go.AddOrGet<Light2D>();
      light2D.overlayColour = LIGHT2D.MAGMALAMP_COLOR;
      light2D.Color = LIGHT2D.MAGMALAMP_COLOR;
      light2D.Range = 2f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.MAGMALAMP_DIRECTION;
      light2D.Offset = LIGHT2D.MAGMALAMP_OFFSET;
      light2D.shape = LightShape.Cone;
      light2D.drawOverlay = true;
    })));
    prefabs.Add(ArtifactConfig.CreateArtifact("Oracle", (string) UI.SPACEARTIFACTS.ORACLE.NAME, (string) UI.SPACEARTIFACTS.ORACLE.DESCRIPTION, "idle_oracle", "ui_oracle", TUNING.DECOR.SPACEARTIFACT.TIER3, DlcManager.EXPANSION1, (string[]) null, "artifacts_2_kanim", artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("DNAModel", (string) UI.SPACEARTIFACTS.DNAMODEL.NAME, (string) UI.SPACEARTIFACTS.DNAMODEL.DESCRIPTION, "idle_dna", "ui_dna", TUNING.DECOR.SPACEARTIFACT.TIER4, (string[]) null, (string[]) null, artifact_type: ArtifactType.Terrestrial));
    prefabs.Add(ArtifactConfig.CreateArtifact("RainbowEggRock", (string) UI.SPACEARTIFACTS.RAINBOWEGGROCK.NAME, (string) UI.SPACEARTIFACTS.RAINBOWEGGROCK.DESCRIPTION, "idle_egg_rock_rainbow", "ui_egg_rock_rainbow", TUNING.DECOR.SPACEARTIFACT.TIER4, (string[]) null, (string[]) null));
    prefabs.Add(ArtifactConfig.CreateArtifact("PlasmaLamp", (string) UI.SPACEARTIFACTS.PLASMALAMP.NAME, (string) UI.SPACEARTIFACTS.PLASMALAMP.DESCRIPTION, "idle_plasma_lamp_loop", "ui_plasma_lamp", TUNING.DECOR.SPACEARTIFACT.TIER4, (string[]) null, (string[]) null, postInitFn: (ArtifactConfig.PostInitFn) (go =>
    {
      go.AddOrGet<LoopingSounds>();
      Light2D light2D = go.AddOrGet<Light2D>();
      light2D.overlayColour = LIGHT2D.PLASMALAMP_COLOR;
      light2D.Color = LIGHT2D.PLASMALAMP_COLOR;
      light2D.Range = 2f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.PLASMALAMP_DIRECTION;
      light2D.Offset = LIGHT2D.PLASMALAMP_OFFSET;
      light2D.shape = LightShape.Circle;
      light2D.drawOverlay = true;
    })));
    prefabs.Add(ArtifactConfig.CreateArtifact("MoodRing", (string) UI.SPACEARTIFACTS.MOODRING.NAME, (string) UI.SPACEARTIFACTS.MOODRING.DESCRIPTION, "idle_moodring", "ui_moodring", TUNING.DECOR.SPACEARTIFACT.TIER4, DlcManager.EXPANSION1, (string[]) null, "artifacts_2_kanim"));
    prefabs.Add(ArtifactConfig.CreateArtifact("SolarSystem", (string) UI.SPACEARTIFACTS.SOLARSYSTEM.NAME, (string) UI.SPACEARTIFACTS.SOLARSYSTEM.DESCRIPTION, "idle_solar_system_loop", "ui_solar_system", TUNING.DECOR.SPACEARTIFACT.TIER5, (string[]) null, (string[]) null, postInitFn: (ArtifactConfig.PostInitFn) (go => go.AddOrGet<LoopingSounds>()), artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("Moonmoonmoon", (string) UI.SPACEARTIFACTS.MOONMOONMOON.NAME, (string) UI.SPACEARTIFACTS.MOONMOONMOON.DESCRIPTION, "idle_moon", "ui_moon", TUNING.DECOR.SPACEARTIFACT.TIER5, (string[]) null, (string[]) null, artifact_type: ArtifactType.Space));
    prefabs.Add(ArtifactConfig.CreateArtifact("ReactorModel", (string) UI.SPACEARTIFACTS.REACTORMODEL.NAME, (string) UI.SPACEARTIFACTS.REACTORMODEL.DESCRIPTION, "idle_model", "ui_model", TUNING.DECOR.SPACEARTIFACT.TIER5, DlcManager.EXPANSION1, (string[]) null, "artifacts_2_kanim"));
    for (int index = prefabs.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) prefabs[index] == (UnityEngine.Object) null)
        prefabs.RemoveAt(index);
    }
    foreach (GameObject gameObject in prefabs)
    {
      SpaceArtifact component = gameObject.GetComponent<SpaceArtifact>();
      ArtifactType key = DlcManager.IsExpansion1Active() ? component.artifactType : ArtifactType.Any;
      ArtifactConfig.artifactItems[key].Add(gameObject.name);
    }
    return prefabs;
  }

  [Obsolete]
  public static GameObject CreateArtifact(
    string id,
    string name,
    string desc,
    string initial_anim,
    string ui_anim,
    ArtifactTier artifact_tier,
    string[] dlcIDs,
    string animFile = "artifacts_kanim",
    ArtifactConfig.PostInitFn postInitFn = null,
    SimHashes element = SimHashes.Creature,
    ArtifactType artifact_type = ArtifactType.Any)
  {
    DlcRestrictionsUtil.TemporaryHelperObject objectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIDs);
    return ArtifactConfig.CreateArtifact(id, name, desc, initial_anim, ui_anim, artifact_tier, objectFromAllowList.GetRequiredDlcIds(), objectFromAllowList.GetForbiddenDlcIds(), animFile, postInitFn, element, artifact_type);
  }

  public static GameObject CreateArtifact(
    string id,
    string name,
    string desc,
    string initial_anim,
    string ui_anim,
    ArtifactTier artifact_tier,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds,
    string animFile = "artifacts_kanim",
    ArtifactConfig.PostInitFn postInitFn = null,
    SimHashes element = SimHashes.Creature,
    ArtifactType artifact_type = ArtifactType.Any)
  {
    if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
      return (GameObject) null;
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("artifact_" + id.ToLower(), name, desc, ArtifactConfig.ARTIFACT_MASS, true, Assets.GetAnim((HashedString) animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, isPickupable: true, sortOrder: SORTORDER.ARTIFACTS, element: element, additionalTags: new List<Tag>()
    {
      GameTags.MiscPickupable
    });
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(artifact_tier.decorValues);
    decorProvider.overrideName = looseEntity.GetProperName();
    SpaceArtifact spaceArtifact = looseEntity.AddOrGet<SpaceArtifact>();
    spaceArtifact.SetUIAnim(ui_anim);
    spaceArtifact.SetArtifactTier(artifact_tier);
    spaceArtifact.uniqueAnimNameFragment = initial_anim;
    spaceArtifact.artifactType = artifact_type;
    looseEntity.AddOrGet<KSelectable>();
    looseEntity.GetComponent<Pickupable>().deleteOffGrid = false;
    looseEntity.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (instance => instance.GetComponent<SpaceArtifact>().SetArtifactTier(artifact_tier));
    looseEntity.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
    if (postInitFn != null)
      postInitFn(looseEntity);
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.requiredDlcIds = requiredDlcIds;
    component.forbiddenDlcIds = forbiddenDlcIds;
    component.AddTag(GameTags.PedestalDisplayable);
    component.AddTag(GameTags.Artifact);
    component.AddTag(GameTags.Ornament);
    looseEntity.AddOrGet<CodexEntryRedirector>().CodexID = "SPACEARTIFACT";
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public delegate void PostInitFn(GameObject gameObject);
}
