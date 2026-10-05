// Decompiled with JetBrains decompiler
// Type: Db
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Db : EntityModifierSet
{
  private static Db _Instance;
  public TextAsset researchTreeFileVanilla;
  public TextAsset researchTreeFileExpansion1;
  public Diseases Diseases;
  public Database.Sicknesses Sicknesses;
  public Urges Urges;
  public AssignableSlots AssignableSlots;
  public StateMachineCategories StateMachineCategories;
  public Personalities Personalities;
  public Faces Faces;
  public Shirts Shirts;
  public Expressions Expressions;
  public Emotes Emotes;
  public Thoughts Thoughts;
  public CritterEmotions CritterEmotions;
  public Dreams Dreams;
  public BuildingStatusItems BuildingStatusItems;
  public MiscStatusItems MiscStatusItems;
  public CreatureStatusItems CreatureStatusItems;
  public RobotStatusItems RobotStatusItems;
  public StatusItemCategories StatusItemCategories;
  public Deaths Deaths;
  public Database.ChoreTypes ChoreTypes;
  public TechItems TechItems;
  public AccessorySlots AccessorySlots;
  public Accessories Accessories;
  public ScheduleBlockTypes ScheduleBlockTypes;
  public ScheduleGroups ScheduleGroups;
  public RoomTypeCategories RoomTypeCategories;
  public RoomTypes RoomTypes;
  public ArtifactDropRates ArtifactDropRates;
  public SpaceDestinationTypes SpaceDestinationTypes;
  public SkillPerks SkillPerks;
  public SkillGroups SkillGroups;
  public Skills Skills;
  public ColonyAchievements ColonyAchievements;
  public Quests Quests;
  public GameplayEvents GameplayEvents;
  public GameplaySeasons GameplaySeasons;
  public PlantMutations PlantMutations;
  public Spices Spices;
  public Techs Techs;
  public TechTreeTitles TechTreeTitles;
  public OrbitalTypeCategories OrbitalTypeCategories;
  public PermitResources Permits;
  public ArtableStatuses ArtableStatuses;
  public Stories Stories;

  public static string GetPath(string dlcId, string folder)
  {
    return !(dlcId == "") ? FileSystem.Normalize(System.IO.Path.Combine(Application.streamingAssetsPath, "dlc", DlcManager.GetContentDirectoryName(dlcId), folder)) : FileSystem.Normalize(System.IO.Path.Combine(Application.streamingAssetsPath, folder));
  }

  public static Db Get()
  {
    if ((UnityEngine.Object) Db._Instance == (UnityEngine.Object) null)
    {
      Db._Instance = Resources.Load<Db>(nameof (Db));
      Db._Instance.Initialize();
    }
    return Db._Instance;
  }

  public static BuildingFacades GetBuildingFacades() => Db.Get().Permits.BuildingFacades;

  public static ArtableStages GetArtableStages() => Db.Get().Permits.ArtableStages;

  public static EquippableFacades GetEquippableFacades() => Db.Get().Permits.EquippableFacades;

  public static StickerBombs GetStickerBombs() => Db.Get().Permits.StickerBombs;

  public static MonumentParts GetMonumentParts() => Db.Get().Permits.MonumentParts;

  public override void Initialize()
  {
    base.Initialize();
    this.Urges = new Urges();
    this.AssignableSlots = new AssignableSlots();
    this.StateMachineCategories = new StateMachineCategories();
    this.Personalities = new Personalities();
    this.Faces = new Faces();
    this.Shirts = new Shirts();
    this.Expressions = new Expressions(this.Root);
    this.Emotes = new Emotes(this.Root);
    Effect effect = this.effects.Get("SoreBack");
    effect.SetEmote(this.Emotes.Minion.SoreBack, effect.emoteCooldown);
    this.Thoughts = new Thoughts(this.Root);
    this.CritterEmotions = new CritterEmotions(this.Root);
    this.Dreams = new Dreams(this.Root);
    this.Deaths = new Deaths(this.Root);
    this.StatusItemCategories = new StatusItemCategories(this.Root);
    this.TechTreeTitles = new TechTreeTitles(this.Root);
    this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
    this.Techs = new Techs(this.Root);
    this.TechItems = new TechItems(this.Root);
    this.Techs.Init();
    this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
    this.TechItems.Init();
    this.Accessories = new Accessories(this.Root);
    this.AccessorySlots = new AccessorySlots(this.Root);
    this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
    this.ScheduleGroups = new ScheduleGroups(this.Root);
    this.RoomTypeCategories = new RoomTypeCategories(this.Root);
    this.RoomTypes = new RoomTypes(this.Root);
    this.ArtifactDropRates = new ArtifactDropRates(this.Root);
    this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
    this.Diseases = new Diseases(this.Root);
    this.Sicknesses = new Database.Sicknesses(this.Root);
    this.SkillPerks = new SkillPerks(this.Root);
    this.SkillGroups = new SkillGroups(this.Root);
    this.Skills = new Skills(this.Root);
    this.ColonyAchievements = new ColonyAchievements(this.Root);
    this.MiscStatusItems = new MiscStatusItems(this.Root);
    this.CreatureStatusItems = new CreatureStatusItems(this.Root);
    this.BuildingStatusItems = new BuildingStatusItems(this.Root);
    this.RobotStatusItems = new RobotStatusItems(this.Root);
    this.ChoreTypes = new Database.ChoreTypes(this.Root);
    this.Quests = new Quests(this.Root);
    this.GameplayEvents = new GameplayEvents(this.Root);
    this.GameplaySeasons = new GameplaySeasons(this.Root);
    this.Stories = new Stories(this.Root);
    if (DlcManager.FeaturePlantMutationsEnabled())
      this.PlantMutations = new PlantMutations(this.Root);
    this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
    this.ArtableStatuses = new ArtableStatuses(this.Root);
    this.Permits = new PermitResources(this.Root);
    Effect resource = new Effect("CenterOfAttention", (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0.0f, true, true, false);
    resource.Add(new AttributeModifier("StressDelta", -0.008333334f, (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME));
    this.effects.Add(resource);
    this.Spices = new Spices(this.Root);
    this.CollectResources((Resource) this.Root, this.ResourceTable);
  }

  public void PostProcess()
  {
    this.Techs.PostProcess();
    this.Permits.PostProcess();
  }

  private void CollectResources(Resource resource, List<Resource> resource_table)
  {
    if (resource.Guid != (ResourceGuid) null)
      resource_table.Add(resource);
    if (!(resource is ResourceSet resourceSet))
      return;
    for (int idx = 0; idx < resourceSet.Count; ++idx)
      this.CollectResources(resourceSet.GetResource(idx), resource_table);
  }

  public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
  {
    Resource resource1 = this.ResourceTable.FirstOrDefault<Resource>((Func<Resource, bool>) (s => s.Guid == guid));
    if (resource1 == null)
    {
      Debug.LogWarning((object) ("Could not find resource: " + guid?.ToString()));
      return default (ResourceType);
    }
    ResourceType resource2 = (ResourceType) resource1;
    if ((object) resource2 != null)
      return resource2;
    Debug.LogError((object) $"Resource type mismatch for resource: {resource1.Id}\nExpecting Type: {typeof (ResourceType).Name}\nGot Type: {resource1.GetType().Name}");
    return default (ResourceType);
  }

  public void ResetProblematicDbs()
  {
    this.Emotes.ResetProblematicReferences();
    this.SkillPerks.ResetProblematicReferences();
  }

  [Serializable]
  public class SlotInfo : Resource
  {
  }
}
