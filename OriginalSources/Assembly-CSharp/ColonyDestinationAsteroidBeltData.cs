// Decompiled with JetBrains decompiler
// Type: ColonyDestinationAsteroidBeltData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColonyDestinationAsteroidBeltData
{
  private ProcGen.World startWorld;
  private ClusterLayout clusterLayout;
  private MutatedClusterLayout mutatedClusterLayout;
  private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();
  private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();
  public static List<Tuple<string, string, string>> survivalOptions = new List<Tuple<string, string, string>>()
  {
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
  };

  public float TargetScale { get; set; }

  public float Scale { get; set; }

  public int seed { get; private set; }

  public string startWorldPath => this.startWorld.filePath;

  public Sprite sprite { get; private set; }

  public int difficulty { get; private set; }

  public string startWorldName => (string) Strings.Get(this.startWorld.name);

  public string properName => this.clusterLayout == null ? "" : this.clusterLayout.name;

  public string beltPath
  {
    get
    {
      return this.clusterLayout == null ? WorldGenSettings.ClusterDefaultName : this.clusterLayout.filePath;
    }
  }

  public List<ProcGen.World> worlds { get; private set; }

  public ClusterLayout Layout
  {
    get
    {
      return this.mutatedClusterLayout != null ? this.mutatedClusterLayout.layout : this.clusterLayout;
    }
  }

  public ProcGen.World GetStartWorld => this.startWorld;

  public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
  {
    this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
    this.Scale = this.TargetScale = this.startWorld.iconScale;
    this.worlds = new List<ProcGen.World>();
    if (clusterPath != null)
      this.clusterLayout = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
    this.ReInitialize(seed);
  }

  public static Sprite GetUISprite(string filename)
  {
    if (filename.IsNullOrWhiteSpace())
      filename = DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone";
    KAnimFile anim;
    Assets.TryGetAnim((HashedString) filename, out anim);
    return (UnityEngine.Object) anim != (UnityEngine.Object) null ? Def.GetUISpriteFromMultiObjectAnim(anim) : Assets.GetSprite((HashedString) filename);
  }

  public void ReInitialize(int seed)
  {
    this.seed = seed;
    this.paramDescriptors.Clear();
    this.traitDescriptors.Clear();
    this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
    this.difficulty = this.clusterLayout.difficulty;
    this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(this.clusterLayout, seed, true, true);
    this.RemixClusterLayout();
  }

  public void RemixClusterLayout()
  {
    if (!WorldgenMixing.RefreshWorldMixing(this.mutatedClusterLayout, this.seed, true, true))
    {
      DebugUtil.LogWarningArgs((object) "World remix failed, using default cluster instead.");
      this.mutatedClusterLayout = new MutatedClusterLayout(this.clusterLayout);
    }
    this.worlds.Clear();
    for (int index = 0; index < this.Layout.worldPlacements.Count; ++index)
    {
      if (index != this.Layout.startWorldIndex)
        this.worlds.Add(SettingsCache.worlds.GetWorldData(this.Layout.worldPlacements[index].world));
    }
  }

  public List<AsteroidDescriptor> GetParamDescriptors()
  {
    if (this.paramDescriptors.Count == 0)
      this.paramDescriptors = this.GenerateParamDescriptors();
    return this.paramDescriptors;
  }

  public List<AsteroidDescriptor> GetTraitDescriptors()
  {
    if (this.traitDescriptors.Count == 0)
      this.traitDescriptors = this.GenerateTraitDescriptors();
    return this.traitDescriptors;
  }

  private List<AsteroidDescriptor> GenerateParamDescriptors()
  {
    List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();
    if (this.clusterLayout != null && DlcManager.FeatureClusterSpaceEnabled())
      paramDescriptors.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, (object) Strings.Get(this.clusterLayout.name)), (string) Strings.Get(this.clusterLayout.description), Color.white));
    paramDescriptors.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.PLANETNAME, (object) this.startWorldName), (string) null, Color.white));
    paramDescriptors.Add(new AsteroidDescriptor((string) Strings.Get(this.startWorld.description), (string) null, Color.white));
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      paramDescriptors.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.MOONNAMES), (string) null, Color.white));
      foreach (ProcGen.World world in this.worlds)
        paramDescriptors.Add(new AsteroidDescriptor($"{Strings.Get(world.name)}", (string) Strings.Get(world.description), Color.white));
    }
    int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
    Tuple<string, string, string> survivalOption = ColonyDestinationAsteroidBeltData.survivalOptions[index];
    paramDescriptors.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.TITLE, (object) survivalOption.first, (object) survivalOption.third), (string) null, Color.white));
    return paramDescriptors;
  }

  private List<AsteroidDescriptor> GenerateTraitDescriptors()
  {
    List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();
    List<ProcGen.World> worldList = new List<ProcGen.World>();
    worldList.Add(this.startWorld);
    worldList.AddRange((IEnumerable<ProcGen.World>) this.worlds);
    for (int index = 0; index < worldList.Count; ++index)
    {
      ProcGen.World singleWorld = worldList[index];
      if (DlcManager.IsExpansion1Active())
      {
        traitDescriptors.Add(new AsteroidDescriptor("", (string) null, Color.white));
        traitDescriptors.Add(new AsteroidDescriptor($"<b>{Strings.Get(singleWorld.name)}</b>", (string) null, Color.white));
      }
      List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld);
      foreach (WorldTrait worldTrait in worldTraits)
      {
        string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
        traitDescriptors.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", (object) Strings.Get(worldTrait.name), (object) worldTrait.colorHex), (string) Strings.Get(worldTrait.description), Util.ColorFromHex(worldTrait.colorHex), associatedIcon: associatedIcon));
      }
      if (worldTraits.Count == 0)
        traitDescriptors.Add(new AsteroidDescriptor((string) WORLD_TRAITS.NO_TRAITS.NAME, (string) WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, associatedIcon: "NoTraits"));
    }
    return traitDescriptors;
  }

  public List<AsteroidDescriptor> GenerateTraitDescriptors(
    ProcGen.World singleWorld,
    bool includeDefaultTrait = true)
  {
    List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();
    List<ProcGen.World> worldList = new List<ProcGen.World>();
    worldList.Add(this.startWorld);
    worldList.AddRange((IEnumerable<ProcGen.World>) this.worlds);
    for (int index = 0; index < worldList.Count; ++index)
    {
      if (worldList[index] == singleWorld)
      {
        List<WorldTrait> worldTraits = this.GetWorldTraits(worldList[index]);
        foreach (WorldTrait worldTrait in worldTraits)
        {
          string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
          traitDescriptors.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", (object) Strings.Get(worldTrait.name), (object) worldTrait.colorHex), (string) Strings.Get(worldTrait.description), Util.ColorFromHex(worldTrait.colorHex), associatedIcon: associatedIcon));
        }
        if (worldTraits.Count == 0 & includeDefaultTrait)
          traitDescriptors.Add(new AsteroidDescriptor((string) WORLD_TRAITS.NO_TRAITS.NAME, (string) WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, associatedIcon: "NoTraits"));
      }
    }
    return traitDescriptors;
  }

  public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
  {
    List<WorldTrait> worldTraits = new List<WorldTrait>();
    List<ProcGen.World> worldList = new List<ProcGen.World>();
    worldList.Add(this.startWorld);
    worldList.AddRange((IEnumerable<ProcGen.World>) this.worlds);
    for (int index = 0; index < worldList.Count; ++index)
    {
      if (worldList[index] == singleWorld)
      {
        ProcGen.World world = worldList[index];
        int seed = this.seed;
        if (seed > 0)
          seed += this.clusterLayout.worldPlacements.FindIndex((Predicate<WorldPlacement>) (x => x.world == world.filePath));
        foreach (string randomTrait in SettingsCache.GetRandomTraits(seed, world))
        {
          WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(randomTrait, true);
          worldTraits.Add(cachedWorldTrait);
        }
      }
    }
    return worldTraits;
  }
}
