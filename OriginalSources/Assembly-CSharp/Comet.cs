// Decompiled with JetBrains decompiler
// Type: Comet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
  public SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
  public float EXHAUST_RATE = 50f;
  public Vector2 spawnVelocity = new Vector2(12f, 15f);
  public Vector2 spawnAngle = new Vector2(-100f, -80f);
  public Vector2 massRange;
  public Vector2 temperatureRange;
  public SpawnFXHashes explosionEffectHash;
  public int splashRadius = 1;
  public int addTiles;
  public int addTilesMinHeight;
  public int addTilesMaxHeight;
  public int entityDamage = 1;
  public float totalTileDamage = 0.2f;
  protected float addTileMass;
  public int addDiseaseCount;
  public byte diseaseIdx = byte.MaxValue;
  public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);
  public Vector2I explosionOreCount = new Vector2I(0, 0);
  private float explosionMass;
  public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);
  public Vector2 explosionSpeedRange = new Vector2(8f, 14f);
  public float windowDamageMultiplier = 5f;
  public float bunkerDamageMultiplier;
  public string impactSound;
  public string flyingSound;
  public int flyingSoundID;
  private HashedString FLYING_SOUND_ID_PARAMETER = (HashedString) "meteorType";
  public bool affectedByDifficulty = true;
  public bool Targeted;
  [Serialize]
  protected Vector3 offsetPosition;
  [Serialize]
  protected Vector2 velocity;
  [Serialize]
  private float remainingTileDamage;
  private Vector3 previousPosition;
  private bool hasExploded;
  public bool canHitDuplicants;
  public string[] craterPrefabs;
  public string[] lootOnDestroyedByMissile;
  public bool destroyOnExplode = true;
  public bool spawnWithOffset;
  private float age;
  public System.Action OnImpact;
  public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();
  [MyCmpGet]
  private KBatchedAnimController anim;
  [MyCmpGet]
  private KSelectable selectable;
  public Tag typeID;
  private LoopingSounds loopingSounds;
  private List<GameObject> damagedEntities = new List<GameObject>();
  private List<int> destroyedCells = new List<int>();
  private const float MAX_DISTANCE_TEST = 6f;

  public float ExplosionMass => this.explosionMass;

  public float AddTileMass => this.addTileMass;

  public Vector3 TargetPosition => this.anim.PositionIncludingOffset;

  public Vector2 Velocity
  {
    get => this.velocity;
    set => this.velocity = value;
  }

  private float GetVolume(GameObject gameObject)
  {
    float volume = 1f;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.selectable != (UnityEngine.Object) null && this.selectable.IsSelected)
      volume = 1f;
    return volume;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.remainingTileDamage = this.totalTileDamage;
    this.loopingSounds = this.gameObject.GetComponent<LoopingSounds>();
    this.flyingSound = GlobalAssets.GetSound("Meteor_LP");
    this.RandomizeVelocity();
  }

  protected override void OnSpawn()
  {
    this.anim.Offset = this.offsetPosition;
    if (this.spawnWithOffset)
      this.SetupOffset();
    base.OnSpawn();
    this.RandomizeMassAndTemperature();
    this.StartLoopingSound();
    this.selectable.enabled = (double) this.offsetPosition.x == 0.0 && (double) this.offsetPosition.y == 0.0;
    this.typeID = this.GetComponent<KPrefabID>().PrefabTag;
    Components.Meteors.Add(this.gameObject.GetMyWorldId(), this);
  }

  protected override void OnCleanUp()
  {
    Components.Meteors.Remove(this.gameObject.GetMyWorldId(), this);
  }

  protected void SetupOffset()
  {
    Vector3 position1 = this.transform.GetPosition();
    Vector3 position2 = this.transform.GetPosition() with
    {
      z = 0.0f
    };
    Vector3 vector3_1 = new Vector3(this.velocity.x, this.velocity.y, 0.0f);
    WorldContainer myWorld = this.gameObject.GetMyWorld();
    float num1 = Mathf.Abs(((float) (myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y) / Mathf.Cos(Vector3.Angle(Vector3.up, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_2 = position2 - vector3_1.normalized * num1;
    float num2 = (float) (myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
    if (((double) vector3_2.x < (double) myWorld.WorldOffset.x * (double) Grid.CellSizeInMeters ? 0 : ((double) vector3_2.x <= (double) num2 ? 1 : 0)) == 0)
      num1 = Mathf.Abs(((double) vector3_1.x < 0.0 ? num2 - position2.x : position2.x - (float) myWorld.WorldOffset.x * Grid.CellSizeInMeters) / Mathf.Cos(Vector3.Angle((double) vector3_1.x < 0.0 ? Vector3.right : Vector3.left, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_3 = -vector3_1.normalized * num1;
    (position2 + vector3_3).z = position1.z;
    this.offsetPosition = vector3_3;
    this.anim.Offset = this.offsetPosition;
  }

  public virtual void RandomizeVelocity()
  {
    float num1 = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
    float f = (float) ((double) num1 * 3.1415927410125732 / 180.0);
    float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
    this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
    this.GetComponent<KBatchedAnimController>().Rotation = (float) (-(double) num1 - 90.0);
  }

  public void RandomizeMassAndTemperature()
  {
    float num1 = UnityEngine.Random.Range(this.massRange.x, this.massRange.y) * this.GetMassMultiplier();
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    component.Mass = num1;
    component.Temperature = UnityEngine.Random.Range(this.temperatureRange.x, this.temperatureRange.y);
    if (this.addTiles > 0)
    {
      float num2 = UnityEngine.Random.Range(0.95f, 0.98f);
      this.explosionMass = num1 * (1f - num2);
      this.addTileMass = num1 * num2;
    }
    else
    {
      this.explosionMass = num1;
      this.addTileMass = 0.0f;
    }
  }

  public float GetMassMultiplier()
  {
    float massMultiplier = 1f;
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
    if (this.affectedByDifficulty && currentQualitySetting != null)
    {
      switch (currentQualitySetting.id)
      {
        case "Infrequent":
          massMultiplier *= 1f;
          break;
        case "Intense":
          massMultiplier *= 0.8f;
          break;
        case "Doomed":
          massMultiplier *= 0.5f;
          break;
      }
    }
    return massMultiplier;
  }

  public int GetRandomNumOres()
  {
    return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
  }

  public float GetRandomTemperatureForOres()
  {
    return UnityEngine.Random.Range(this.explosionTemperatureRange.x, this.explosionTemperatureRange.y);
  }

  [ContextMenu("Explode")]
  private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
  {
    int world = (int) Grid.WorldIdx[cell];
    this.PlayImpactSound(pos);
    Vector3 pos1 = pos with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2)
    };
    if (this.explosionEffectHash != SpawnFXHashes.None)
      Game.Instance.SpawnFX(this.explosionEffectHash, pos1, 0.0f);
    Substance substance = element.substance;
    int randomNumOres = this.GetRandomNumOres();
    Vector2 vector2_1 = -this.velocity.normalized;
    Vector2 vector2_2 = new Vector2(vector2_1.y, -vector2_1.x);
    ListPool<ScenePartitionerEntry, Comet>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, Comet>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) pos.x - 3, (int) pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      GameObject gameObject = (partitionerEntry.obj as Pickupable).gameObject;
      if (!((UnityEngine.Object) gameObject.GetComponent<MinionIdentity>() != (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject.GetComponent<CreatureBrain>() != (UnityEngine.Object) null) && gameObject.GetDef<RobotAi.Def>() == null)
      {
        Vector2 initial_velocity = (((Vector2) (gameObject.transform.GetPosition() - pos)).normalized + new Vector2(0.0f, 0.55f)) * (0.5f * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y));
        if (GameComps.Fallers.Has((object) gameObject))
          GameComps.Fallers.Remove(gameObject);
        if (GameComps.Gravities.Has((object) gameObject))
          GameComps.Gravities.Remove(gameObject);
        GameComps.Fallers.Add(gameObject, initial_velocity);
      }
    }
    gathered_entries.Recycle();
    int num1 = this.splashRadius + 1;
    for (int y = -num1; y <= num1; ++y)
    {
      for (int x = -num1; x <= num1; ++x)
      {
        int cell1 = Grid.OffsetCell(cell, x, y);
        if (Grid.IsValidCellInWorld(cell1, world) && !this.destroyedCells.Contains(cell1))
        {
          float num2 = (float) ((1.0 - (double) Mathf.Abs(x) / (double) num1) * (1.0 - (double) Mathf.Abs(y) / (double) num1));
          if ((double) num2 > 0.0)
          {
            double num3 = (double) this.DamageTiles(cell1, prev_cell, (float) ((double) num2 * (double) this.totalTileDamage * 0.5));
          }
        }
      }
    }
    float mass = randomNumOres > 0 ? this.explosionMass / (float) randomNumOres : 1f;
    float temperatureForOres = this.GetRandomTemperatureForOres();
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    for (int index = 0; index < randomNumOres; ++index)
    {
      Vector2 normalized = (vector2_1 + vector2_2 * UnityEngine.Random.Range(-1f, 1f)).normalized;
      Vector3 initial_velocity = (Vector3) (normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y));
      Vector3 position = (Vector3) (normalized.normalized * 0.75f) + new Vector3(0.0f, 0.55f, 0.0f) + pos;
      GameObject go = substance.SpawnResource(position, mass, temperatureForOres, component.DiseaseIdx, component.DiseaseCount / (randomNumOres + this.addTiles));
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, (Vector2) initial_velocity);
    }
    if (this.addTiles > 0)
      this.DepositTiles(cell, element, world, prev_cell, temperatureForOres);
    this.SpawnCraterPrefabs();
    if (this.OnImpact == null)
      return;
    this.OnImpact();
  }

  protected virtual void DepositTiles(
    int cell,
    Element element,
    int world,
    int prev_cell,
    float temperature)
  {
    int depthOfElement = this.GetDepthOfElement(cell, element, world);
    float num1 = 1f;
    int addTilesMinHeight = this.addTilesMinHeight;
    float f = (float) (depthOfElement - addTilesMinHeight) / (float) (this.addTilesMaxHeight - this.addTilesMinHeight);
    if (!float.IsNaN(f))
      num1 -= f;
    int num2 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float) this.addTiles * num1), 1, this.addTiles));
    ListPool<int, Comet>.PooledList validCells = ListPool<int, Comet>.Allocate();
    FloodFill.BreadthCollect(prev_cell, new Func<int, FloodFill.BoundaryCheckResult>(BoundaryCondition), (List<int>) validCells, 11);
    float mass = num2 > 0 ? this.addTileMass / (float) this.addTiles : 1f;
    int disease_count = this.addDiseaseCount / num2;
    if (element.HasTag(GameTags.Unstable))
    {
      UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
      foreach (int cell1 in (List<int>) validCells)
      {
        if (num2 > 0)
        {
          component.Spawn(cell1, element, mass, temperature, byte.MaxValue, 0);
          --num2;
        }
        else
          break;
      }
    }
    else
    {
      foreach (int gameCell in (List<int>) validCells)
      {
        if (num2 > 0)
        {
          SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, this.diseaseIdx, disease_count);
          --num2;
        }
        else
          break;
      }
    }
    validCells.Recycle();

    FloodFill.BoundaryCheckResult BoundaryCondition(int cell)
    {
      return !Grid.IsValidCellInWorld(cell, world) || Grid.Solid[cell] ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue;
    }
  }

  protected virtual void SpawnCraterPrefabs()
  {
    if (this.craterPrefabs == null || this.craterPrefabs.Length == 0)
      return;
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) this.craterPrefabs[UnityEngine.Random.Range(0, this.craterPrefabs.Length)]), Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) this)));
    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -19.5f);
    gameObject.SetActive(true);
  }

  protected int GetDepthOfElement(int cell, Element element, int world)
  {
    int depthOfElement = 0;
    for (int cell1 = Grid.CellBelow(cell); Grid.IsValidCellInWorld(cell1, world) && Grid.Element[cell1] == element; cell1 = Grid.CellBelow(cell1))
      ++depthOfElement;
    return depthOfElement;
  }

  [ContextMenu("DamageTiles")]
  private float DamageTiles(int cell, int prev_cell, float input_damage)
  {
    GameObject tile_go = Grid.Objects[cell, 9];
    float num1 = 1f;
    bool flag = false;
    if ((UnityEngine.Object) tile_go != (UnityEngine.Object) null)
    {
      if (tile_go.GetComponent<KPrefabID>().HasTag(GameTags.Window))
        num1 = this.windowDamageMultiplier;
      else if (tile_go.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
      {
        num1 = this.bunkerDamageMultiplier;
        if ((UnityEngine.Object) tile_go.GetComponent<Door>() != (UnityEngine.Object) null)
          Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
      }
      SimCellOccupier component = tile_go.GetComponent<SimCellOccupier>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.doReplaceElement)
        flag = true;
    }
    Element element = !flag ? Grid.Element[cell] : tile_go.GetComponent<PrimaryElement>().Element;
    if ((double) element.strength == 0.0)
      return 0.0f;
    float amount = input_damage * num1 / element.strength;
    this.PlayTileDamageSound(element, Grid.CellToPos(cell), tile_go);
    if ((double) amount == 0.0)
      return 0.0f;
    float num2;
    if (flag)
    {
      BuildingHP component = tile_go.GetComponent<BuildingHP>();
      double a = (double) component.HitPoints / (double) component.MaxHitPoints;
      float f = amount * (float) component.MaxHitPoints;
      component.gameObject.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo()
      {
        damage = Mathf.RoundToInt(f),
        source = (string) BUILDINGS.DAMAGESOURCES.COMET,
        popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
      });
      double b = (double) amount;
      num2 = Mathf.Min((float) a, (float) b);
    }
    else
      num2 = WorldDamage.Instance.ApplyDamage(cell, amount, prev_cell, (string) BUILDINGS.DAMAGESOURCES.COMET, (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
    this.destroyedCells.Add(cell);
    float num3 = num2 / amount;
    return input_damage * (1f - num3);
  }

  private void DamageThings(Vector3 pos, int cell, int damage, GameObject ignoreObject = null)
  {
    if (damage == 0 || !Grid.IsValidCell(cell))
      return;
    GameObject building_go = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) building_go != (UnityEngine.Object) null && (UnityEngine.Object) building_go != (UnityEngine.Object) ignoreObject)
    {
      BuildingHP component1 = building_go.GetComponent<BuildingHP>();
      Building component2 = building_go.GetComponent<Building>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !this.damagedEntities.Contains(building_go))
      {
        float f = building_go.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? (float) damage * this.bunkerDamageMultiplier : (float) damage;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2.Def != (UnityEngine.Object) null)
          this.PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), building_go);
        component1.gameObject.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo()
        {
          damage = Mathf.RoundToInt(f),
          source = (string) BUILDINGS.DAMAGESOURCES.COMET,
          popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
        });
        this.damagedEntities.Add(building_go);
      }
    }
    ListPool<ScenePartitionerEntry, Comet>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, Comet>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) pos.x, (int) pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      Pickupable pickupable = partitionerEntry.obj as Pickupable;
      Health component = pickupable.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !this.damagedEntities.Contains(pickupable.gameObject))
      {
        float amount = pickupable.KPrefabID.HasTag(GameTags.Bunker) ? (float) damage * this.bunkerDamageMultiplier : (float) damage;
        component.Damage(amount);
        this.damagedEntities.Add(pickupable.gameObject);
      }
    }
    gathered_entries.Recycle();
  }

  public float GetDistanceFromImpact()
  {
    float num1 = this.velocity.x / this.velocity.y;
    Vector3 position = this.transform.GetPosition();
    float y = 0.0f;
    while ((double) y > -6.0)
    {
      float num2 = y - 1f;
      y = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
      Vector3 vector3 = new Vector3(y * num1, y, 0.0f);
      int cell = Grid.PosToCell(position + vector3);
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
        return vector3.magnitude;
    }
    return 6f;
  }

  public float GetSoundDistance() => this.GetDistanceFromImpact();

  private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
  {
    string sound = GlobalAssets.GetSound("MeteorDamage_" + (element.substance.GetMiningBreakSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal")));
    if (!(bool) (UnityEngine.Object) CameraController.Instance || !CameraController.Instance.IsAudibleSound(pos, (HashedString) sound))
      return;
    float volume = this.GetVolume(tile_go);
    KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
  }

  private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
  {
    if (!((UnityEngine.Object) def != (UnityEngine.Object) null))
      return;
    string str = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory)) ?? GlobalAssets.GetSound("MeteorDamage_Building_Metal");
    if (str == null || !(bool) (UnityEngine.Object) CameraController.Instance || !CameraController.Instance.IsAudibleSound(pos, (HashedString) str))
      return;
    float volume = this.GetVolume(building_go);
    KFMOD.PlayOneShot(str, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
  }

  public void Sim33ms(float dt)
  {
    if (this.hasExploded)
      return;
    if ((double) this.offsetPosition.y > 0.0)
    {
      this.offsetPosition += new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      this.anim.Offset = this.offsetPosition;
    }
    else
    {
      if (this.anim.Offset != Vector3.zero)
        this.anim.Offset = Vector3.zero;
      if (!this.selectable.enabled)
        this.selectable.enabled = true;
      Vector2 vector2_1 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * -0.1f;
      Vector2 vector2_2 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * 1.1f;
      Vector3 position = this.transform.GetPosition();
      Vector3 vector3 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      int cell1 = Grid.PosToCell(vector3);
      this.loopingSounds.UpdateVelocity(this.flyingSound, (Vector2) (vector3 - position));
      Element elementByHash = ElementLoader.FindElementByHash(this.EXHAUST_ELEMENT);
      if (this.EXHAUST_ELEMENT != SimHashes.Void && Grid.IsValidCell(cell1) && !Grid.Solid[cell1])
        SimMessages.EmitMass(cell1, elementByHash.idx, dt * this.EXHAUST_RATE, elementByHash.defaultValues.temperature, this.diseaseIdx, Mathf.RoundToInt((float) this.addDiseaseCount * dt));
      if ((double) vector3.x < (double) vector2_1.x || (double) vector2_2.x < (double) vector3.x || (double) vector3.y < (double) vector2_1.y)
        Util.KDestroyGameObject(this.gameObject);
      int cell2 = Grid.PosToCell((KMonoBehaviour) this);
      int cell3 = Grid.PosToCell(this.previousPosition);
      if (cell2 != cell3)
      {
        if (Grid.IsValidCell(cell2) && Grid.Solid[cell2])
        {
          PrimaryElement component = this.GetComponent<PrimaryElement>();
          this.remainingTileDamage = this.DamageTiles(cell2, cell3, this.remainingTileDamage);
          if ((double) this.remainingTileDamage <= 0.0)
          {
            this.Explode(position, cell2, cell3, component.Element);
            this.hasExploded = true;
            if (!this.destroyOnExplode)
              return;
            Util.KDestroyGameObject(this.gameObject);
            return;
          }
        }
        else
        {
          GameObject gameObject = (UnityEngine.Object) this.ignoreObstacleForDamage.Get() == (UnityEngine.Object) null ? (GameObject) null : this.ignoreObstacleForDamage.Get().gameObject;
          this.DamageThings(position, cell2, this.entityDamage, gameObject);
        }
      }
      if (this.canHitDuplicants && (double) this.age > 0.25 && (UnityEngine.Object) Grid.Objects[Grid.PosToCell(position), 0] != (UnityEngine.Object) null)
      {
        this.transform.position = Grid.CellToPos(Grid.PosToCell(position));
        this.Explode(position, cell2, cell3, this.GetComponent<PrimaryElement>().Element);
        if (!this.destroyOnExplode)
          return;
        Util.KDestroyGameObject(this.gameObject);
        return;
      }
      this.previousPosition = position;
      this.transform.SetPosition(vector3);
    }
    this.age += dt;
  }

  private void PlayImpactSound(Vector3 pos)
  {
    if (this.impactSound == null)
      this.impactSound = "Meteor_Large_Impact";
    this.loopingSounds.StopSound(this.flyingSound);
    string sound = GlobalAssets.GetSound(this.impactSound);
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
      return;
    float volume = this.GetVolume(this.gameObject);
    pos.z = 0.0f;
    EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
    int num = (int) instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
    KFMOD.EndOneShot(instance);
  }

  private void StartLoopingSound()
  {
    this.loopingSounds.StartSound(this.flyingSound);
    this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float) this.flyingSoundID);
  }

  public void Explode()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    Vector3 position = this.transform.GetPosition();
    int cell = Grid.PosToCell(position);
    this.Explode(position, cell, cell, component.Element);
    this.hasExploded = true;
    if (!this.destroyOnExplode)
      return;
    Util.KDestroyGameObject(this.gameObject);
  }
}
